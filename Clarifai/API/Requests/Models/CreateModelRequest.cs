using System.Collections.Generic;
using Clarifai.DTOs.Models;
using Clarifai.DTOs.Models.OutputsInfo;
using Clarifai.DTOs.Predictions;
using Newtonsoft.Json.Linq;

namespace Clarifai.API.Requests.Models
{
    /// <summary>
    /// A request for creating new models.
    /// </summary>
    public class CreateModelRequest : ClarifaiRequest<ConceptModel>
    {
        protected override RequestMethod Method => RequestMethod.POST;
        protected override string Url => "/v2/models/";

        private readonly string _modelId;
        private readonly string _name;
        private readonly IEnumerable<Concept> _concepts;
        private readonly bool? _areConceptsMutuallyExclusive;
        private readonly bool? _isEnvironmentClosed;
        private readonly string _language;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the Clarifai client</param>
        /// <param name="modelID">the model ID</param>
        /// <param name="name">the model name</param>
        /// <param name="concepts">the concepts to include in the model</param>
        /// <param name="areConceptsMutuallyExclusive">are concepts mutually exclusive</param>
        /// <param name="isEnvironmentClosed">is environment closed</param>
        /// <param name="language">the language</param>
        public CreateModelRequest(IClarifaiHttpClient httpClient, string modelID,
            string name = null, IEnumerable<Concept> concepts = null,
            bool? areConceptsMutuallyExclusive = null, bool? isEnvironmentClosed = null,
            string language = null) : base(httpClient)
        {
            _modelId = modelID;
            _name = name;
            _concepts = concepts;
            _areConceptsMutuallyExclusive = areConceptsMutuallyExclusive;
            _isEnvironmentClosed = isEnvironmentClosed;
            _language = language;
        }

        /// <inheritdoc />
        protected override ConceptModel Unmarshaller(dynamic jsonObject)
        {
            return ConceptModel.Deserialize(HttpClient, jsonObject.model);
        }

        /// <inheritdoc />
        protected override JObject HttpRequestBody()
        {
            var model = new ConceptModel(HttpClient, _modelId, name: _name);
            return new JObject(
                new JProperty("model", model.Serialize(_concepts, _areConceptsMutuallyExclusive,
                    _isEnvironmentClosed, _language)));
        }
    }
}
