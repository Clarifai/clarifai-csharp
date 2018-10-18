using Clarifai.DTOs.Models.Outputs;
using Newtonsoft.Json.Linq;

namespace Clarifai.API.Requests.Inputs
{
    /// <summary>
    /// Deletes all inputs.
    /// </summary>
    public class DeleteAllInputsRequest : ClarifaiRequest<EmptyResponse>
    {
        protected override RequestMethod Method => RequestMethod.DELETE;
        protected override string Url => "/v2/inputs/";

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        public DeleteAllInputsRequest(IClarifaiHttpClient httpClient) : base(httpClient)
        { }

        /// <inheritdoc />
        protected override EmptyResponse Unmarshaller(dynamic jsonObject)
        {
            return new EmptyResponse();
        }

        /// <inheritdoc />
        protected override JObject HttpRequestBody()
        {
            return new JObject(
                new JProperty("delete_all", true));
        }
    }
}
