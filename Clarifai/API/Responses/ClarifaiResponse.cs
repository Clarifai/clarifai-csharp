using System.Net;
using Clarifai.DTOs;

namespace Clarifai.API.Responses
{
    /// <summary>
    /// A Clarifai response
    /// </summary>
    /// <typeparam name="T">the type of the response</typeparam>
    public class ClarifaiResponse<T>
    {
        /// <summary>
        /// The response status.
        /// </summary>
        public ClarifaiStatus Status { get; }

        /// <summary>
        /// The response HTTP status code.
        /// </summary>
        public HttpStatusCode HttpCode { get; }

        /// <summary>
        /// Was the request successful.
        /// </summary>
        public bool IsSuccessful => ClarifaiStatus.StatusType.Successful == Status.Type;

        /// <summary>
        /// Raw response string.
        /// </summary>
        public string RawBody { get; }

        private readonly T _deserialized;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="status">the response status</param>
        /// <param name="httpCode">the HTTP code</param>
        /// <param name="rawBody">the raw body</param>
        /// <param name="deserialized">the deserialized object</param>
        public ClarifaiResponse(ClarifaiStatus status, HttpStatusCode httpCode, string rawBody,
            T deserialized)
        {
            Status = status;
            HttpCode = httpCode;
            RawBody = rawBody;
            _deserialized = deserialized;
        }

        /// <summary>
        /// Returns a deserialized object contained in the response.
        /// </summary>
        /// <returns>the deserialized object</returns>
        public T Get() => _deserialized;
    }
}
