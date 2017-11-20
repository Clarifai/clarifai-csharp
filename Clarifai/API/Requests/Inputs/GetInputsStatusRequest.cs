using Clarifai.DTOs.Inputs;
using Newtonsoft.Json.Linq;

namespace Clarifai.API.Requests.Inputs
{
    /// <summary>
    /// A request for getting input.
    /// </summary>
    public class GetInputsStatusRequest : ClarifaiRequest<ClarifaiInputsStatus>
    {
        protected override RequestMethod Method => RequestMethod.GET;
        protected override string Url => "/v2/inputs/status";

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="client">the Clarifai client</param>
        public GetInputsStatusRequest(IClarifaiClient client) : base(client)
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
