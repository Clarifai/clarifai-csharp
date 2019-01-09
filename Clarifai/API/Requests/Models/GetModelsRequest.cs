using System.Collections.Generic;
using System.Threading.Tasks;
using Clarifai.DTOs.Models;
using Clarifai.Internal.GRPC;
using Google.Protobuf;
using Model = Clarifai.DTOs.Models.Model;

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
            return await grpcClient.ListModelsAsync(new ListModelsRequest());
        }
    }
}
