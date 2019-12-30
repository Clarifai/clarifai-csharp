using System.Collections.Generic;
using Clarifai.DTOs.Models;
using Newtonsoft.Json.Linq;

namespace Clarifai.API.Requests.Models
{
    /// <summary>
    /// Search all the models by name and type of the model.
    /// </summary>
    public class SearchModelsRequest : ClarifaiPaginatedRequest<List<IModel>>
    {
        protected override RequestMethod Method => RequestMethod.POST;
        protected override string Url => "/v2/models/searches";

        private readonly string _name;
        private readonly ModelType _modelType;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="name">
        /// the model name - use "*" for any name, or search by part of a name like "celeb*"
        /// </param>
        /// <param name="modelType">the model type</param>
        public SearchModelsRequest(IClarifaiHttpClient httpClient, string name,
            ModelType modelType = null)
            : base(httpClient)
        {
            _name = name;
            _modelType = modelType;
        }

        /// <inheritdoc />
        protected override JObject PaginatedHttpRequestBody()
        {
            JObject body = base.PaginatedHttpRequestBody();
            var query = new JObject(
                new JProperty("name", _name));
            if (_modelType != null)
            {
                query["type"] = _modelType.TypeExt;
            }

            body["model_query"] = query;
            return body;
        }

        /// <inheritdoc />
        protected override List<IModel> Unmarshaller(dynamic jsonObject)
        {
            var models = new List<IModel>();
            foreach (dynamic model in jsonObject.models)
            {
                ModelType modelType = ModelType.DetermineModelType(
                    (string)model.output_info.type_ext);
                if (modelType == null)
                {
                    continue;
                }
                models.Add(Model.Deserialize(HttpClient, modelType.Prediction, model));
            }
            return models;
        }
    }
}
