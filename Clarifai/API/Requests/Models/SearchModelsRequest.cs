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
        /// <param name="client">the Clarifai client</param>
        /// <param name="name">
        /// the model name - use "*" for any name, or search by part of a name like "celeb*"
        /// </param>
        /// <param name="modelType">the model type</param>
        public SearchModelsRequest(IClarifaiClient client, string name, ModelType modelType = null)
            : base(client)
        {
            _name = name;
            _modelType = modelType;
        }

        /// <inheritdoc />
        protected override JObject HttpRequestBody()
        {
            var query = new JObject(
                new JProperty("name", _name));
            if (_modelType != null)
            {
                query["type"] = _modelType.TypeExt;
            }
            return new JObject(
                new JProperty("model_query", query));
        }

        /// <inheritdoc />
        protected override List<IModel> Unmarshaller(dynamic jsonObject)
        {
            var models = new List<IModel>();
            foreach (dynamic model in jsonObject.models)
            {
                ModelType modelType = ModelType.DetermineModelType(
                    (string)model.output_info.type_ext);
                models.Add(Model.Deserialize(Client, modelType.Prediction, model));
            }
            return models;
        }
    }
}
