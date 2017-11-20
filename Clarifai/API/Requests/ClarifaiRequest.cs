using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Clarifai.API.Responses;
using Clarifai.DTOs;
using Clarifai.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Clarifai.API.Requests
{
    /// <inheritdoc />
    public abstract class ClarifaiRequest<T> : IClarifaiRequest<T>
    {
        protected abstract RequestMethod Method { get; }
        protected abstract string Url { get; }

        private readonly IClarifaiClient _client;
        protected IClarifaiClient Client => _client;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="client">the Clarifai client</param>
        protected ClarifaiRequest(IClarifaiClient client)
        {
            _client = client;
        }

        /// <inheritdoc />
        public async Task<ClarifaiResponse<T>> ExecuteAsync()
        {
            HttpResponseMessage response;
            string responseContent;
            try
            {
                response = await HttpRequest();
                responseContent = await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                return new ClarifaiResponse<T>(
                    new ClarifaiStatus(ClarifaiStatus.StatusType.NetworkError, 404,
                        "HTTP request failed.", ex.Message),
                    HttpStatusCode.NotFound, "", default(T));
            }

            dynamic jsonObject;
            try
            {
                jsonObject = JsonConvert.DeserializeObject<dynamic>(responseContent);
            }
            catch (JsonException ex)
            {
                return new ClarifaiResponse<T>(
                    new ClarifaiStatus(ClarifaiStatus.StatusType.NetworkError,
                        (int) response.StatusCode, "Server provided a malformed JSON response.",
                        ex.Message),
                    response.StatusCode, responseContent, default(T));
            }

            dynamic statusJsonObject = jsonObject.status;
            ClarifaiStatus status = ClarifaiStatus.Deserialize(statusJsonObject,
                response.StatusCode);

            if (status.Type == ClarifaiStatus.StatusType.Successful ||
                status.Type == ClarifaiStatus.StatusType.MixedSuccess)
            {
                var deserializedResponse = Unmarshaller(jsonObject);
                return new ClarifaiResponse<T>(status, response.StatusCode, responseContent,
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
                return new ClarifaiResponse<T>(status, response.StatusCode, responseContent,
                    deserializedResponse);
            }
        }

        /// <summary>
        /// Runs the right HTTP request method and returns the response.
        /// </summary>
        /// <returns>the message response</returns>
        private async Task<HttpResponseMessage> HttpRequest()
        {
            switch (Method)
            {
                case RequestMethod.GET:
                {
                    return await _client.HttpClient.GetAsync(BuildUrl());
                }
                case RequestMethod.POST:
                {
                    return await _client.HttpClient.PostAsync(BuildUrl(), HttpRequestBody());
                }
                case RequestMethod.PATCH:
                {
                    return await _client.HttpClient.PatchAsync(BuildUrl(), HttpRequestBody());
                }
                case RequestMethod.DELETE:
                {
                    return await _client.HttpClient.DeleteAsync(BuildUrl(), HttpRequestBody());
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
