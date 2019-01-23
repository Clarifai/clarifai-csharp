using System.Net;
using System.Threading.Tasks;
using Clarifai.API;
using Newtonsoft.Json.Linq;

namespace Clarifai.UnitTests.Fakes
{
    /// <summary>
    /// A fake Clarifai HTTP client used for testing.
    /// </summary>
    public class FkClarifaiHttpClient : IClarifaiHttpClient
    {
        public string ApiKey => "fake-api-key";

        public HttpStatusCode LastResponseHttpStatusCode { get; }
        public string LastResponseRawBody { get; private set; }

        /// <summary>
        /// The body of the last POST request.
        /// </summary>
        public JObject PostedBody { get; private set; }

        /// <summary>
        /// The body of the last PATCH request.
        /// </summary>
        public JObject PatchedBody { get; private set; }

        /// <summary>
        /// The body of the last DELETE request.
        /// </summary>
        public JObject DeletedBody { get; private set; }

        /// <summary>
        /// The last URL against which the request was executed.
        /// </summary>
        public string RequestedUrl { get; private set; }

        private readonly string _getResponse;
        private readonly string _postResponse;
        private readonly string _patchResponse;
        private readonly string _deleteResponse;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="getResponse">the response GET request should return</param>
        /// <param name="postResponse">the response POST request should return</param>
        /// <param name="patchResponse">the response PATCH request should return</param>
        /// <param name="deleteResponse">the response DELETE request should return</param>
        /// <param name="lastResponseHttpStatusCode">the response HTTP status code</param>
        public FkClarifaiHttpClient(string getResponse = null, string postResponse = null,
            string patchResponse = null, string deleteResponse = null,
            HttpStatusCode lastResponseHttpStatusCode = HttpStatusCode.OK)
        {
            _getResponse = getResponse;
            _postResponse = postResponse;
            _patchResponse = patchResponse;
            _deleteResponse = deleteResponse;

            LastResponseHttpStatusCode = lastResponseHttpStatusCode;
        }

        public async Task<string> GetAsync(string url)
        {
            RequestedUrl = url;
            LastResponseRawBody = _getResponse;
            return await Task.FromResult(_getResponse);
        }

        public async Task<string> PostAsync(string url, JObject body)
        {
            RequestedUrl = url;
            PostedBody = body;
            LastResponseRawBody = _postResponse;
            return await Task.FromResult(_postResponse);
        }

        public async Task<string> PatchAsync(string url, JObject body)
        {
            RequestedUrl = url;
            PatchedBody = body;
            LastResponseRawBody = _patchResponse;
            return await Task.FromResult(_patchResponse);
        }

        public async Task<string> DeleteAsync(string url, JObject body = null)
        {
            RequestedUrl = url;
            DeletedBody = body;
            LastResponseRawBody = _deleteResponse;
            return await Task.FromResult(_deleteResponse);
        }
    }
}
