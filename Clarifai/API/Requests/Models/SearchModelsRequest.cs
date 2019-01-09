using System.Collections.Generic;
using System.Threading.Tasks;
using Clarifai.DTOs.Models;
using Clarifai.Internal.GRPC;
using Google.Protobuf;
using Model = Clarifai.DTOs.Models.Model;

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
        protected override List<IModel> Unmarshaller(dynamic responseD)
        {
            MultiModelResponse response = responseD;

            var models = new List<IModel>();
            foreach (Internal.GRPC.Model model in response.Models)
            {
                ModelType modelType = ModelType.DetermineModelType(model.OutputInfo.TypeExt);
                models.Add(Model.GrpcDeserialize(HttpClient, modelType.Prediction, model));
            }
            return models;
        }

        /// <inheritdoc />
        protected override async Task<IMessage> GrpcRequestBody(V2.V2Client grpcClient)
        {
            var modelQuery = new ModelQuery
            {
                Name = _name
            };
            if (_modelType != null)
            {
                modelQuery = new ModelQuery(modelQuery)
                {
                    Type = _modelType.TypeExt
                };
            }
            return await grpcClient.PostModelsSearchesAsync(new PostModelsSearchesRequest
            {
                ModelQuery = modelQuery
            });
        }
    }
}
