using System.Collections.Generic;
using System.Threading.Tasks;
using Clarifai.DTOs.Inputs;
using Clarifai.Internal.GRPC;
using Google.Protobuf;

namespace Clarifai.API.Requests.Inputs
{
    /// <summary>
    /// A request for getting inputs.
    /// </summary>
    public class GetInputsRequest : ClarifaiPaginatedRequest<List<IClarifaiInput>>
    {
        protected override RequestMethod Method => RequestMethod.GET;
        protected override string Url => "/v2/inputs";

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        public GetInputsRequest(IClarifaiHttpClient httpClient) : base(httpClient)
        { }

        /// <inheritdoc />
        protected override List<IClarifaiInput> Unmarshaller(dynamic responseD)
        {
            MultiInputResponse response = responseD;

            var inputs = new List<IClarifaiInput>();
            foreach (var input in response.Inputs)
            {
                inputs.Add(ClarifaiInput.GrpcDeserialize(input));
            }
            return inputs;
        }

        /// <inheritdoc />
        protected override async Task<IMessage> GrpcRequestBody(V2.V2Client grpcClient)
        {
            return await grpcClient.ListInputsAsync(new ListInputsRequest());
        }
    }
}
