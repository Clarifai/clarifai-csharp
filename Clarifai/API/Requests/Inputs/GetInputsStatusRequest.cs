using System.Threading.Tasks;
using Clarifai.DTOs.Inputs;
using Clarifai.Internal.GRPC;
using Google.Protobuf;

namespace Clarifai.API.Requests.Inputs
{
    /// <summary>
    /// If you add inputs in bulk, they will process in the background. With this method you
    /// retrieve all inputs' status.
    /// </summary>
    public class GetInputsStatusRequest : ClarifaiRequest<ClarifaiInputsStatus>
    {
        protected override RequestMethod Method => RequestMethod.GET;
        protected override string Url => "/v2/inputs/status";

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        public GetInputsStatusRequest(IClarifaiHttpClient httpClient) : base(httpClient)
        { }

        /// <inheritdoc />
        protected override ClarifaiInputsStatus Unmarshaller(dynamic responseD)
        {
            SingleInputCountResponse response = responseD;
            return ClarifaiInputsStatus.GrpcDeserialize(response.Counts);
        }

        /// <inheritdoc />
        protected override async Task<IMessage> GrpcRequestBody(V2.V2Client grpcClient)
        {
            return await grpcClient.GetInputCountAsync(new GetInputCountRequest());
        }
    }
}
