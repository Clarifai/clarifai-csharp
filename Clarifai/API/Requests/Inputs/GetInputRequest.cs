using System.Threading.Tasks;
using Clarifai.DTOs.Inputs;
using Clarifai.Internal.GRPC;
using Google.Protobuf;

namespace Clarifai.API.Requests.Inputs
{
    /// <summary>
    /// A request for getting input.
    /// </summary>
    public class GetInputRequest : ClarifaiRequest<IClarifaiInput>
    {
        protected override RequestMethod Method => RequestMethod.GET;
        protected override string Url => "/v2/inputs/" + _inputID;

        private readonly string _inputID;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="inputID">the input ID</param>
        public GetInputRequest(IClarifaiHttpClient httpClient, string inputID) : base(httpClient)
        {
            _inputID = inputID;
        }

        /// <inheritdoc />
        protected override IClarifaiInput Unmarshaller(dynamic responseD)
        {
            SingleInputResponse response = responseD;
            return ClarifaiInput.GrpcDeserialize(response.Input);
        }

        /// <inheritdoc />
        protected override async Task<IMessage> GrpcRequestBody(V2.V2Client grpcClient)
        {
            return await grpcClient.GetInputAsync(new Internal.GRPC.GetInputRequest());
        }
    }
}
