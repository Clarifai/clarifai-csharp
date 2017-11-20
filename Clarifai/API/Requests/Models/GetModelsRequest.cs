using System.Collections.Generic;
using Clarifai.DTOs.Models;
using Newtonsoft.Json.Linq;

namespace Clarifai.API.Requests.Models
{
    /// <summary>
    /// A request for getting all models.
    /// </summary>
    public class GetModelsRequest : ClarifaiPaginatedRequest<List<IModel>>
    {
        protected override RequestMethod Method => RequestMethod.GET;
        protected override string Url => "/v2/models/";

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="client">the Clarifai client</param>
        public GetModelsRequest(IClarifaiClient client) : base(client)
        { }

        /// <inheritdoc />
        protected override JObject HttpRequestBody()
        {
            return new JObject();
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
