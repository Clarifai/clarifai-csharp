using System.Collections.Generic;
using Clarifai.DTOs.Inputs;
using Newtonsoft.Json.Linq;

namespace Clarifai.API.Requests.Inputs
{
    /// <summary>
    /// A request for getting inputs.
    /// </summary>
    public class GetInputsRequest : ClarifaiPaginatedRequest<List<IClarifaiInput>>
    {
        protected override RequestMethod Method => RequestMethod.GET;
        protected override string Url => "/v2/inputs";

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        public GetInputsRequest(IClarifaiHttpClient httpClient) : base(httpClient)
        { }

        /// <inheritdoc />
        protected override List<IClarifaiInput> Unmarshaller(dynamic jsonObject)
        {
            var inputs = new List<IClarifaiInput>();
            foreach (var input in jsonObject.inputs)
            {
                inputs.Add(ClarifaiURLImage.Deserialize(input));
            }
            return inputs;
        }

        /// <inheritdoc />
        protected override JObject HttpRequestBody()
        {
            return new JObject();
        }
    }
}
