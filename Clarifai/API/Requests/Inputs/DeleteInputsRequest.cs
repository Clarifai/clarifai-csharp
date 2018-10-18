using System.Collections.Generic;
using System.Linq;
using Clarifai.DTOs.Models.Outputs;
using Newtonsoft.Json.Linq;

namespace Clarifai.API.Requests.Inputs
{
    /// <summary>
    /// A request for deleting inputs.
    /// </summary>
    public class DeleteInputsRequest : ClarifaiRequest<EmptyResponse>
    {
        protected override RequestMethod Method => RequestMethod.DELETE;
        protected override string Url => "/v2/inputs/";

        private readonly IEnumerable<string> _inputIDs;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="inputIDs">the input IDs</param>
        public DeleteInputsRequest(IClarifaiHttpClient httpClient, params string[] inputIDs) :
            this(httpClient, inputIDs.ToList())
        { }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="inputIDs">the input IDs</param>
        public DeleteInputsRequest(IClarifaiHttpClient httpClient, IEnumerable<string> inputIDs)
            : base(httpClient)
        {
            _inputIDs = inputIDs;
        }

        /// <inheritdoc />
        protected override EmptyResponse Unmarshaller(dynamic jsonObject)
        {
            return new EmptyResponse();
        }

        /// <inheritdoc />
        protected override JObject HttpRequestBody()
        {
            return new JObject(
                new JProperty("ids", new JArray(_inputIDs)));
        }
    }
}
