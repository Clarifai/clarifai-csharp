using System.Collections.Generic;
using System.Linq;
using Clarifai.DTOs.Inputs;
using Newtonsoft.Json.Linq;

namespace Clarifai.API.Requests.Inputs
{
    /// <summary>
    /// A request for adding inputs.
    /// </summary>
    public class AddInputsRequest : ClarifaiRequest<List<IClarifaiInput>>
    {
        protected override RequestMethod Method => RequestMethod.POST;
        protected override string Url => "/v2/inputs/";

        private readonly IEnumerable<IClarifaiInput> _inputs;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="client">the Clarifai client</param>
        /// <param name="inputs">the inputs</param>
        public AddInputsRequest(IClarifaiClient client, params IClarifaiInput[] inputs) :
            this(client, inputs.ToList())
        { }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="client">the Clarifai client</param>
        /// <param name="inputs">the inputs</param>
        public AddInputsRequest(IClarifaiClient client, IEnumerable<IClarifaiInput> inputs)
            : base(client)
        {
            _inputs = inputs;
        }

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
            return new JObject(
                new JProperty("inputs",
                    new JArray(_inputs.Select(i => i.Serialize()))));
        }
    }
}
