using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Clarifai.API
{
    public enum RequestMethod { GET, POST, PATCH, DELETE }

    /// <summary>
    /// Executes HTTP REST requests to the Clarifai API endpoint.
    /// </summary>
    public interface IClarifaiHttpClient
    {
        string ApiKey { get; }

        Task<HttpResponseMessage> GetAsync(string url);
        Task<HttpResponseMessage> PostAsync(string url, JObject body);
        Task<HttpResponseMessage> PatchAsync(string url, JObject body);
        Task<HttpResponseMessage> DeleteAsync(string url, JObject body = null);
    }
}