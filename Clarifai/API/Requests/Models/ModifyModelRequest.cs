using System.Collections.Generic;
using Clarifai.DTOs.Models;
using Clarifai.DTOs.Predictions;
using Newtonsoft.Json.Linq;

namespace Clarifai.API.Requests.Models
{
    /// <summary>
    /// Request for modifying a model.
    /// </summary>
    public class ModifyModelRequest : ClarifaiRequest<ConceptModel>
    {
        protected override RequestMethod Method => RequestMethod.PATCH;
        protected override string Url => "/v2/models";

        private readonly string _modelID;
        private readonly ModifyAction _action;
        private readonly string _name;
        private readonly IEnumerable<Concept> _concepts;
        private readonly bool? _areConceptsMutuallyExclusive;
        private readonly bool? _isEnvironmentClosed;
        private readonly string _language;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="client">the Clarifai client</param>
        /// <param name="modelID">the model ID</param>
        /// <param name="action">the modification action</param>
        /// <param name="name">the new model name</param>
        /// <param name="concepts">the concepts update the model with</param>
        /// <param name="areConceptsMutuallyExclusive">are concepts mutually exclusive</param>
        /// <param name="isEnvironmentClosed">is environment closed</param>
        /// <param name="language">the language</param>
        public ModifyModelRequest(IClarifaiClient client, string modelID,
            ModifyAction action = null, string name = null, IEnumerable<Concept> concepts = null,
            bool? areConceptsMutuallyExclusive = null, bool? isEnvironmentClosed = null,
            string language = null) : base(client)
        {
            _modelID = modelID;
            _action = action;
            _name = name;
            _concepts = concepts;
            _areConceptsMutuallyExclusive = areConceptsMutuallyExclusive;
            _isEnvironmentClosed = isEnvironmentClosed;
            _language = language;
        }

        /// <inheritdoc />
        protected override JObject HttpRequestBody()
        {
            var model = new ConceptModel(
                Client,
                _modelID,
                name: _name);
            var body = new JObject(
                new JProperty("models", new JArray(model.Serialize(_concepts,
                    _areConceptsMutuallyExclusive, _isEnvironmentClosed, _language))));

            ModifyAction action = _action ?? ModifyAction.Merge;
            body.Add("action", action.Serialize());

            return body;
        }

        /// <inheritdoc />
        protected override ConceptModel Unmarshaller(dynamic jsonObject)
        {
            return ConceptModel.Deserialize(Client, jsonObject.models[0]);
        }
    }
}
