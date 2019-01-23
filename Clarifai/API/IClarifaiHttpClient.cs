using System.Net;
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
        HttpStatusCode LastResponseHttpStatusCode { get; }
        string LastResponseRawBody { get; }

        Task<string> GetAsync(string url);
        Task<string> PostAsync(string url, JObject body);
        Task<string> PatchAsync(string url, JObject body);
        Task<string> DeleteAsync(string url, JObject body = null);
    }
}