using System.Net.Http;
using System.Threading.Tasks;

namespace Clarifai.Extensions
{
    /// <summary>
    /// Extensions for easier HttpClient usage.
    /// </summary>
    public static class HttpClientExtensions
    {
        /// <summary>
        /// HttpClient doesn't support PATCH methods by default.
        /// Idea from: https://stackoverflow.com/a/29772349/365837
        /// </summary>
        /// <param name="client">the instance of HttpClient</param>
        /// <param name="url">the url</param>
        /// <param name="content">the content</param>
        /// <returns>a message task</returns>
        public static async Task<HttpResponseMessage> PatchAsync(
            this HttpClient client, string url, HttpContent content)
        {
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), url)
            {
                Content = content
            };
            return await client.SendAsync(request);
        }

        /// <summary>
        /// HttpClient doesn't support DELETE methods that have body, by default.
        /// Idea from: https://stackoverflow.com/questions/28054515/how-to-send-delete-with-json-to-the-rest-api-using-httpclient
        /// </summary>
        /// <param name="client">the instance of HttpClient</param>
        /// <param name="url">the url</param>
        /// <param name="content">the content</param>
        /// <returns>a message task</returns>
        public static async Task<HttpResponseMessage> DeleteAsync(
            this HttpClient client, string url, HttpContent content)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = content
            };
            return await client.SendAsync(request);
        }
    }
}
