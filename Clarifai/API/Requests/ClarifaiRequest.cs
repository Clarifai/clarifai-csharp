using System;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Clarifai.API.Responses;
using Clarifai.DTOs;
using Clarifai.Internal.GRPC;
using Clarifai.Internal.GRPC.Status;
using Google.Protobuf;

namespace Clarifai.API.Requests
{
    /// <inheritdoc />
    public abstract class ClarifaiRequest<T> : IClarifaiRequest<T>
    {
        protected abstract RequestMethod Method { get; }
        protected abstract string Url { get; }

        protected IClarifaiHttpClient HttpClient { get; }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        protected ClarifaiRequest(IClarifaiHttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        /// <inheritdoc />
        public async Task<ClarifaiResponse<T>> ExecuteAsync()
        {
            IMessage message;
            try
            {
                message = await GrpcRequest();
            }
            catch (InvalidJsonException ex)
            {
                return new ClarifaiResponse<T>(
                    new ClarifaiStatus(ClarifaiStatus.StatusType.NetworkError, 404,
                        "HTTP request failed.", ex.Message),
                    HttpStatusCode.NotFound, "", default(T));
            }

            PropertyInfo method = message.GetType().GetRuntimeProperty("Status");
            object result = method.GetValue(message);
            Status statusJsonObject = (Status) result;

            ClarifaiStatus status = ClarifaiStatus.GrpcDeserialize(statusJsonObject,
                HttpClient.LastResponseHttpStatusCode);

            T deserializedResponse;
            if (status.Type == ClarifaiStatus.StatusType.Successful ||
                status.Type == ClarifaiStatus.StatusType.MixedSuccess)
            {
                deserializedResponse = Unmarshaller(message);
            }
            else
            {
                // If response unsuccessful, try deserializing the response object anyway.
                try
                {
                    deserializedResponse = Unmarshaller(message);
                }
                catch (Exception)
                {
                    deserializedResponse = default(T);
                }
            }

            return new ClarifaiResponse<T>(status, HttpClient.LastResponseHttpStatusCode,
                HttpClient.LastResponseRawBody, deserializedResponse);
        }

        /// <summary>
        /// Runs the right gRPC request method using a JSON channel, and returns the response.
        /// </summary>
        /// <returns>the message response</returns>
        private async Task<IMessage> GrpcRequest()
        {
            JsonParser.Settings jsonParserSettings = JsonParser.Settings.Default
                .WithIgnoreUnknownFields(true);
            var jsonParser = new JsonParser(jsonParserSettings);

            JsonFormatter.Settings jsonFormatterSettings = JsonFormatter.Settings.Default;
            var jsonFormatter = new JsonFormatter(jsonFormatterSettings);

            var jsonInvoker = new JsonCallInvoker(
                BuildUrl(), Method, HttpClient, jsonFormatter, jsonParser);
            var client = new V2.V2Client(jsonInvoker);
            return await GrpcRequestBody(client);
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
        protected abstract Task<IMessage> GrpcRequestBody(V2.V2Client grpcClient);

        /// <summary>
        /// Unmarshalls (or deserializes) the Protobuf object into a concrete output object.
        /// </summary>
        /// <param name="response">the Protobuf object</param>
        /// <returns>the request output object</returns>
        protected abstract T Unmarshaller(dynamic response);
    }
}
