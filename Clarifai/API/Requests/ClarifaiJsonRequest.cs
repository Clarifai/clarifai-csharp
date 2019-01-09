using System;
using System.Dynamic;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Clarifai.API.Responses;
using Clarifai.DTOs;
using Clarifai.DTOs.Inputs;
using Clarifai.Exceptions;
using Clarifai.Internal.GRPC;
using Clarifai.Internal.GRPC.Status;
using Google.Protobuf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Clarifai.API.Requests
{
    /// <inheritdoc />
    public abstract class ClarifaiJsonRequest<T> : IClarifaiRequest<T>
    {
        protected abstract RequestMethod Method { get; }
        protected abstract string Url { get; }

        protected IClarifaiHttpClient HttpClient { get; }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        protected ClarifaiJsonRequest(IClarifaiHttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        /// <inheritdoc />
        public async Task<ClarifaiResponse<T>> ExecuteAsync()
        {
            string responseContent;
            try
            {
                responseContent = await HttpRequest();
            }
            catch (HttpRequestException ex)
            {
                return new ClarifaiResponse<T>(
                    new ClarifaiStatus(ClarifaiStatus.StatusType.NetworkError, 404,
                        "HTTP request failed.", ex.Message),
                    HttpStatusCode.NotFound, "", default(T));
            }

            HttpStatusCode statusCode = HttpClient.LastResponseHttpStatusCode;

            dynamic jsonObject;
            try
            {
                jsonObject = JsonConvert.DeserializeObject<dynamic>(responseContent);
            }
            catch (JsonException ex)
            {
                return new ClarifaiResponse<T>(
                    new ClarifaiStatus(ClarifaiStatus.StatusType.NetworkError,
                        (int) statusCode, "Server provided a malformed JSON response.",
                        ex.Message),
                    statusCode, responseContent, default(T));
            }

            dynamic statusJsonObject = jsonObject.status;
            // If there is no status object present, assume it's successful.
            if (statusJsonObject == null)
            {
                statusJsonObject = new ExpandoObject();
                statusJsonObject.code = 10000;
                statusJsonObject.description = "Ok";
                statusJsonObject.details = null;
            }

            ClarifaiStatus status = ClarifaiStatus.Deserialize(statusJsonObject,
                statusCode);

            if (status.Type == ClarifaiStatus.StatusType.Successful ||
                status.Type == ClarifaiStatus.StatusType.MixedSuccess)
            {
                var deserializedResponse = Unmarshaller(jsonObject);
                return new ClarifaiResponse<T>(status, statusCode, responseContent,
                    deserializedResponse);
            }
            else
            {
                T deserializedResponse;
                try
                {
                    deserializedResponse = Unmarshaller(jsonObject);
                }
                catch (Exception)
                {
                    deserializedResponse = default(T);
                }
                return new ClarifaiResponse<T>(status, statusCode, responseContent,
                    deserializedResponse);
            }
        }

        /// <summary>
        /// Runs the right HTTP request method and returns the response.
        /// </summary>
        /// <returns>the message response</returns>
        private async Task<string> HttpRequest()
        {
            switch (Method)
            {
                case RequestMethod.GET:
                {
                    return await HttpClient.GetAsync(BuildUrl());
                }
                case RequestMethod.POST:
                {
                    return await HttpClient.PostAsync(BuildUrl(), HttpRequestBody());
                }
                case RequestMethod.PATCH:
                {
                    return await HttpClient.PatchAsync(BuildUrl(), HttpRequestBody());
                }
                case RequestMethod.DELETE:
                {
                    return await HttpClient.DeleteAsync(BuildUrl(), HttpRequestBody());
                }
                default:
                {
                    throw new ClarifaiException("Unknown RequestMethod: " + Method);
                }
            }
        }

        /// <summary>
        /// The URL to run the request against. Must not include base.
        /// </summary>
        /// <returns>the URL</returns>
        protected virtual string BuildUrl()
        {
            return Url;
        }

        /// <summary>
        /// The body to be sent in the request.
        /// </summary>
        /// <returns>the request body</returns>
        protected abstract JObject HttpRequestBody();

        /// <summary>
        /// Unmarshalls (or deserializes) the JSON object into a concrete output object.
        /// </summary>
        /// <param name="jsonObject">the JSON object</param>
        /// <returns>the request output object</returns>
        protected abstract T Unmarshaller(dynamic jsonObject);
    }
}
