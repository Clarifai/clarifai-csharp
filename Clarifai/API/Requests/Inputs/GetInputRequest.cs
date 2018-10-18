using Clarifai.DTOs.Inputs;
using Newtonsoft.Json.Linq;

namespace Clarifai.API.Requests.Inputs
{
    /// <summary>
    /// A request for getting input.
    /// </summary>
    public class GetInputRequest : ClarifaiRequest<IClarifaiInput>
    {
        protected override RequestMethod Method => RequestMethod.GET;
        protected override string Url => "/v2/inputs/" + _inputID;

        private readonly string _inputID;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="inputID">the input ID</param>
        public GetInputRequest(IClarifaiHttpClient httpClient, string inputID) : base(httpClient)
        {
            _inputID = inputID;
        }

        /// <inheritdoc />
        protected override IClarifaiInput Unmarshaller(dynamic jsonObject)
        {
            return ClarifaiURLImage.Deserialize(jsonObject.input);
        }

        /// <inheritdoc />
        protected override JObject HttpRequestBody()
        {
            return new JObject();
        }
    }
}
