using System.Net.Http;
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
        public FkClarifaiHttpClient(string getResponse = null, string postResponse = null,
            string patchResponse = null, string deleteResponse = null)
        {
            _getResponse = getResponse;
            _postResponse = postResponse;
            _patchResponse = patchResponse;
            _deleteResponse = deleteResponse;
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await Task.FromResult(new HttpResponseMessage
            {
                Content = new StringContent(_getResponse)
            });
        }

        public async Task<HttpResponseMessage> PostAsync(string url, JObject body)
        {
            PostedBody = body;
            return await Task.FromResult(new HttpResponseMessage
            {
                Content = new StringContent(_postResponse)
            });
        }

        public async Task<HttpResponseMessage> PatchAsync(string url, JObject body)
        {
            PatchedBody = body;
            return await Task.FromResult(new HttpResponseMessage
            {
                Content = new StringContent(_patchResponse)
            });
        }

        public async Task<HttpResponseMessage> DeleteAsync(string url, JObject body = null)
        {
            DeletedBody = body;
            return await Task.FromResult(new HttpResponseMessage
            {
                Content = new StringContent(_deleteResponse)
            });
        }
    }
}
