using System;
using System.Collections.Generic;
using Clarifai.DTOs.Models;

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
        /// <param name="httpClient">the HTTP client</param>
        public GetModelsRequest(IClarifaiHttpClient httpClient) : base(httpClient)
        { }

        /// <inheritdoc />
        protected override List<IModel> Unmarshaller(dynamic jsonObject)
        {
            var models = new List<IModel>();
            foreach (dynamic model in jsonObject.models)
            {
                string typeExt = (string)model.output_info.type_ext;
                ModelType modelType = ModelType.DetermineModelType(typeExt);
                if (modelType == null)
                {
                    Console.Error.WriteLine(
                        $"Warning: Unknown model type '{typeExt}', skipping. Please upgrade the " +
                        $"to the latest version of the library.");
                    continue;
                }
                models.Add(Model.Deserialize(HttpClient, modelType.Prediction, model));
            }
            return models;
        }
    }
}
