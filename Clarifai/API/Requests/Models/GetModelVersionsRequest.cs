using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clarifai.Internal.GRPC;
using Google.Protobuf;
using ModelVersion = Clarifai.DTOs.Models.ModelVersion;

namespace Clarifai.API.Requests.Models
{
    /// <summary>
    /// Request for retrieving all model-versions for a certain modelID.
    /// </summary>
    public class GetModelVersionsRequest : ClarifaiPaginatedRequest<List<ModelVersion>>
    {
        protected override RequestMethod Method => RequestMethod.GET;
        protected override string Url => $"/v2/models/{_modelID}/versions";

        private readonly string _modelID;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="modelID">the model ID</param>
        public GetModelVersionsRequest(IClarifaiHttpClient httpClient, string modelID)
            : base(httpClient)
        {
            _modelID = modelID;
        }

        /// <inheritdoc />
        protected override List<ModelVersion> Unmarshaller(dynamic responseD)
        {
            MultiModelVersionResponse response = responseD;
            return response.ModelVersions.Select(ModelVersion.GrpcDeserialize).ToList();
        }

        /// <inheritdoc />
        protected override async Task<IMessage> GrpcRequestBody(V2.V2Client grpcClient)
        {
            return await grpcClient.ListModelVersionsAsync(new ListModelVersionsRequest());
        }
    }
}
