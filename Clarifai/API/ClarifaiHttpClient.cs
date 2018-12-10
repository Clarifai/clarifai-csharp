using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Clarifai.Exceptions;
using Clarifai.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Clarifai.API
{
    /// <inheritdoc />
    public class ClarifaiHttpClient : IClarifaiHttpClient
    {
        public string CurrentVersion => "1.3.0";

        public string ApiKey { get; }

        private readonly string _baseUrl;
        private readonly HttpClient _httpClient = new HttpClient();

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="apiKey">the API key</param>
        /// <param name="baseUrl">Clarifai base URL</param>
        public ClarifaiHttpClient(string apiKey, string baseUrl = @"https://api.clarifai.com")
        {
            ApiKey = apiKey;
            _baseUrl = baseUrl;

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Key", apiKey);
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add(
                "X-Clarifai-Client", string.Format("c#:{0}", CurrentVersion));
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await _httpClient.GetAsync(FullUrl(url));
        }

        public async Task<HttpResponseMessage> PostAsync(string url, JObject body)
        {
            var content = new StringContent(SerializedBody(body), Encoding.UTF8,
                "application/json");
            return await _httpClient.PostAsync(FullUrl(url), content);
        }

        public async Task<HttpResponseMessage> PatchAsync(string url, JObject body)
        {
            var content = new StringContent(SerializedBody(body), Encoding.UTF8,
                "application/json");
            return await _httpClient.PatchAsync(FullUrl(url), content);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string url, JObject body = null)
        {
            if (body != null)
            {
                var content = new StringContent(SerializedBody(body), Encoding.UTF8,
                    "application/json");
                return await _httpClient.DeleteAsync(FullUrl(url), content);
            }
            else
            {
                return await _httpClient.DeleteAsync(FullUrl(url));
            }
        }

        private string SerializedBody(JObject body)
        {
            return JsonConvert.SerializeObject(
                body,
                Formatting.None,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        private string FullUrl(string url)
        {
            if (url.Substring(0, 1) != "/")
            {
                url = "/" + url;
            }
            return _baseUrl + url;
        }
    }
}
