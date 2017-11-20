using System.Threading.Tasks;
using Clarifai.API.Responses;

namespace Clarifai.API.Requests
{
    /// <summary>
    /// A Clarifai request.
    /// </summary>
    /// <typeparam name="T">the type of the response content</typeparam>
    interface IClarifaiRequest<T>
    {
        /// <summary>
        /// Executes the request asynchronously.
        /// </summary>
        /// <returns>a response task</returns>
        Task<ClarifaiResponse<T>> ExecuteAsync();
    }
}