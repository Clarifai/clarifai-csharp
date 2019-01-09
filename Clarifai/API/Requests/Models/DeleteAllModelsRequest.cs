using System.Threading.Tasks;
using Clarifai.DTOs.Models.Outputs;
using Clarifai.Internal.GRPC;
using Google.Protobuf;

namespace Clarifai.API.Requests.Models
{
    /// <summary>
    /// Deletes all custom models.
    /// </summary>
    public class DeleteAllModelsRequest : ClarifaiRequest<EmptyResponse>
    {
        protected override RequestMethod Method => RequestMethod.DELETE;
        protected override string Url => "/v2/models/";

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        public DeleteAllModelsRequest(IClarifaiHttpClient httpClient) : base(httpClient)
        { }

        /// <inheritdoc />
        protected override EmptyResponse Unmarshaller(dynamic responseD)
        {
            return new EmptyResponse();
        }

        /// <inheritdoc />
        protected override async Task<IMessage> GrpcRequestBody(V2.V2Client grpcClient)
        {
            return await grpcClient.DeleteModelsAsync(new Internal.GRPC.DeleteModelsRequest
            {
                DeleteAll = true
            });
        }
    }
}
