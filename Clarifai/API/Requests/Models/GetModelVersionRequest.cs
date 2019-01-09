using System.Threading.Tasks;
using Clarifai.Internal.GRPC;
using Google.Protobuf;
using ModelVersion = Clarifai.DTOs.Models.ModelVersion;

namespace Clarifai.API.Requests.Models
{
    /// <summary>
    /// Request for retrieving a specific model-version.
    /// </summary>
    public class GetModelVersionRequest : ClarifaiRequest<ModelVersion>
    {
        protected override RequestMethod Method => RequestMethod.GET;
        protected override string Url => $"/v2/models/{_modelID}/versions/{_versionID}";

        private readonly string _modelID;
        private readonly string _versionID;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="modelID">the model ID</param>
        /// <param name="versionID">the version ID</param>
        public GetModelVersionRequest(IClarifaiHttpClient httpClient, string modelID,
            string versionID)
            : base(httpClient)
        {
            _modelID = modelID;
            _versionID = versionID;
        }

        /// <inheritdoc />
        protected override ModelVersion Unmarshaller(dynamic responseD)
        {
            SingleModelVersionResponse response = responseD;
            return ModelVersion.GrpcDeserialize(response.ModelVersion);
        }

        /// <inheritdoc />
        protected override async Task<IMessage> GrpcRequestBody(V2.V2Client grpcClient)
        {
            return await grpcClient.GetModelVersionAsync(
                new Internal.GRPC.GetModelVersionRequest());
        }
    }
}
