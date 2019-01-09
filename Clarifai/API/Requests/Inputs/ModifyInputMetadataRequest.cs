using System.Threading.Tasks;
using Clarifai.API.Requests.Models;
using Clarifai.DTOs.Inputs;
using Clarifai.Internal.GRPC;
using Google.Protobuf;
using Newtonsoft.Json.Linq;

namespace Clarifai.API.Requests.Inputs
{
    /// <summary>
    /// A request for adding the given metadata to this input's metadata.
    /// </summary>
    public class ModifyInputMetadataRequest : ClarifaiRequest<IClarifaiInput>
    {
        protected override RequestMethod Method => RequestMethod.PATCH;
        protected override string Url => "/v2/inputs";

        private readonly string _inputID;
        private readonly JObject _metadata;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="inputID">the input ID</param>
        /// <param name="metadata">the input's new metadata</param>
        public ModifyInputMetadataRequest(IClarifaiHttpClient httpClient, string inputID,
            JObject metadata)
            : base(httpClient)
        {
            _inputID = inputID;
            _metadata = metadata;
        }

        /// <inheritdoc />
        protected override IClarifaiInput Unmarshaller(dynamic responseD)
        {
            MultiInputResponse response = responseD;
            return ClarifaiInput.GrpcDeserialize(response.Inputs[0]);
        }

        /// <inheritdoc />
        protected override async Task<IMessage> GrpcRequestBody(V2.V2Client grpcClient)
        {
            return await grpcClient.PatchInputsAsync(new PatchInputsRequest
            {
                Action = ModifyAction.Overwrite.GrpcSerialize(),
                Inputs = {new Input
                {
                    Id = _inputID,
                    Data = new Data
                    {
                        Metadata = StructHelper.JObjectToStruct(_metadata)
                    }
                }}
            });
        }
    }
}
