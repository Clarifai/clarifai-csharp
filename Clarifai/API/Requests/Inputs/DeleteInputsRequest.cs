using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clarifai.DTOs.Models.Outputs;
using Clarifai.Internal.GRPC;
using Google.Protobuf;

namespace Clarifai.API.Requests.Inputs
{
    /// <summary>
    /// A request for deleting inputs.
    /// </summary>
    public class DeleteInputsRequest : ClarifaiRequest<EmptyResponse>
    {
        protected override RequestMethod Method => RequestMethod.DELETE;
        protected override string Url => "/v2/inputs/";

        private readonly IEnumerable<string> _inputIDs;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="inputIDs">the input IDs</param>
        public DeleteInputsRequest(IClarifaiHttpClient httpClient, params string[] inputIDs) :
            this(httpClient, inputIDs.ToList())
        { }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="inputIDs">the input IDs</param>
        public DeleteInputsRequest(IClarifaiHttpClient httpClient, IEnumerable<string> inputIDs)
            : base(httpClient)
        {
            _inputIDs = inputIDs;
        }

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
                Ids = { _inputIDs },
            });
        }
    }
}
