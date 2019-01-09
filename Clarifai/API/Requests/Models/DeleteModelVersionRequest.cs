using System.Threading.Tasks;
using Clarifai.DTOs.Models.Outputs;
using Clarifai.Internal.GRPC;
using Google.Protobuf;

namespace Clarifai.API.Requests.Models
{
    /// <summary>
    /// A request to delete a model version with.
    /// </summary>
    public class DeleteModelVersionRequest : ClarifaiRequest<EmptyResponse>
    {
        protected override RequestMethod Method => RequestMethod.DELETE;
        protected override string Url => $"/v2/models/{_modelID}/versions/{_versionID}";

        private readonly string _modelID;
        private readonly string _versionID;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="modelID">the model ID</param>
        /// <param name="versionID">the version ID of the model</param>
        public DeleteModelVersionRequest(IClarifaiHttpClient httpClient, string modelID,
            string versionID) : base(httpClient)
        {
            _modelID = modelID;
            _versionID = versionID;
        }

        /// <inheritdoc />
        protected override EmptyResponse Unmarshaller(dynamic responseD)
        {
            return new EmptyResponse();
        }

        /// <inheritdoc />
        protected override async Task<IMessage> GrpcRequestBody(V2.V2Client grpcClient)
        {
            return await grpcClient.DeleteModelVersionAsync(
                new Internal.GRPC.DeleteModelVersionRequest());
        }
    }
}
