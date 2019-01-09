using System.Threading.Tasks;
using Clarifai.DTOs.Models.Outputs;
using Clarifai.Internal.GRPC;
using Google.Protobuf;

namespace Clarifai.API.Requests.Inputs
{
    /// <summary>
    /// Deletes all inputs.
    /// </summary>
    public class DeleteAllInputsRequest : ClarifaiRequest<EmptyResponse>
    {
        protected override RequestMethod Method => RequestMethod.DELETE;
        protected override string Url => "/v2/inputs/";

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        public DeleteAllInputsRequest(IClarifaiHttpClient httpClient) : base(httpClient)
        { }

        /// <inheritdoc />
        protected override EmptyResponse Unmarshaller(dynamic responseD)
        {
            return new EmptyResponse();
        }

        /// <inheritdoc />
        protected override async Task<IMessage> GrpcRequestBody(V2.V2Client grpcClient)
        {
            return await grpcClient.DeleteInputsAsync(new Internal.GRPC.DeleteInputsRequest
            {
                DeleteAll = true
            });
        }
    }
}
