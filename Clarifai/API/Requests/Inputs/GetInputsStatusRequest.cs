using Clarifai.DTOs.Inputs;
using Newtonsoft.Json.Linq;

namespace Clarifai.API.Requests.Inputs
{
    /// <summary>
    /// If you add inputs in bulk, they will process in the background. With this method you
    /// retrieve all inputs' status.
    /// </summary>
    public class GetInputsStatusRequest : ClarifaiRequest<ClarifaiInputsStatus>
    {
        protected override RequestMethod Method => RequestMethod.GET;
        protected override string Url => "/v2/inputs/status";

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        public GetInputsStatusRequest(IClarifaiHttpClient httpClient) : base(httpClient)
        { }

        /// <inheritdoc />
        protected override ClarifaiInputsStatus Unmarshaller(dynamic jsonObject)
        {
            return ClarifaiInputsStatus.Deserialize(jsonObject.counts);
        }

        /// <inheritdoc />
        protected override JObject HttpRequestBody()
        {
            return new JObject();
        }
    }
}
