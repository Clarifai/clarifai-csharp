using System.Collections.Generic;
using System.Linq;
using Clarifai.API.Requests.Models;
using Clarifai.DTOs.Inputs;
using Clarifai.DTOs.Predictions;
using Newtonsoft.Json.Linq;

namespace Clarifai.API.Requests.Inputs
{
    /// <summary>
    /// A request for input modification.
    /// </summary>
    public class ModifyInputRequest : ClarifaiRequest<IClarifaiInput>
    {
        protected override RequestMethod Method => RequestMethod.PATCH;
        protected override string Url => "/v2/inputs";

        private readonly string _inputID;
        private readonly ModifyAction _action;
        private readonly IEnumerable<Concept> _positiveConcepts;
        private readonly IEnumerable<Concept> _negativeConcepts;

        /// <inheritdoc />
        public ModifyInputRequest(IClarifaiClient client, string inputID, ModifyAction action,
            IEnumerable<Concept> positiveConcepts, IEnumerable<Concept> negativeConcepts)
            : base(client)
        {
            _inputID = inputID;
            _action = action;
            _positiveConcepts = positiveConcepts;
            _negativeConcepts = negativeConcepts;
        }

        /// <inheritdoc />
        protected override IClarifaiInput Unmarshaller(dynamic jsonObject)
        {
            return ClarifaiInput.Deserialize(jsonObject.inputs[0]);
        }

        /// <inheritdoc />
        protected override JObject HttpRequestBody()
        {

            var concepts = new List<JObject>();
            if (_positiveConcepts != null)
            {
                concepts.AddRange(_positiveConcepts.Select(c => c.Serialize(true)));
            }
            if (_negativeConcepts != null)
            {
                concepts.AddRange(_negativeConcepts.Select(c => c.Serialize(false)));
            }
            return new JObject(
                new JProperty("action", _action.Serialize()),
                new JProperty("inputs", new JArray(new JObject(
                    new JProperty("id", _inputID),
                    new JProperty("data", new JObject(
                        new JProperty("concepts", concepts)))))));
        }
    }
}
