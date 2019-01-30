using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Clarifai.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Clarifai.API
{
    /// <inheritdoc />
    public class ClarifaiHttpClient : IClarifaiHttpClient
    {
        public string CurrentVersion => "1.3.0";

        public HttpStatusCode LastResponseHttpStatusCode { get; private set; }
        public string LastResponseRawBody { get; private set; }

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

        public async Task<string> GetAsync(string url)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(FullUrl(url));

            string responseBody = await response.Content.ReadAsStringAsync();
            LastResponseRawBody = responseBody;
            LastResponseHttpStatusCode = response.StatusCode;

            return responseBody;
        }

        public async Task<string> PostAsync(string url, JObject body)
        {
            HttpResponseMessage response = await _httpClient.PostAsync(
                FullUrl(url), PrepareRequestContent(body));

            string responseBody = await response.Content.ReadAsStringAsync();
            LastResponseRawBody = responseBody;
            LastResponseHttpStatusCode = response.StatusCode;

            return responseBody;
        }

        public async Task<string> PatchAsync(string url, JObject body)
        {
            HttpResponseMessage response = await _httpClient.PatchAsync(
                FullUrl(url), PrepareRequestContent(body));

            string responseBody = await response.Content.ReadAsStringAsync();
            LastResponseRawBody = responseBody;
            LastResponseHttpStatusCode = response.StatusCode;

            return responseBody;
        }

        public async Task<string> DeleteAsync(string url, JObject body = null)
        {
            HttpResponseMessage response;
            if (body != null)
            {
                response = await _httpClient.DeleteAsync(FullUrl(url), PrepareRequestContent(body));
            }
            else
            {
                response = await _httpClient.DeleteAsync(FullUrl(url));
            }

            string responseBody = await response.Content.ReadAsStringAsync();
            LastResponseRawBody = responseBody;
            LastResponseHttpStatusCode = response.StatusCode;

            return responseBody;
        }

        private StringContent PrepareRequestContent(JObject body)
        {
            return new StringContent(SerializedBody(body), Encoding.UTF8,
                "application/json");
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
