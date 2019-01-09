using System.Collections.Generic;
using System.Threading.Tasks;
using Clarifai.DTOs.Inputs;
using Clarifai.Internal.GRPC;
using Google.Protobuf;

namespace Clarifai.API.Requests.Models
{
    /// <summary>
    /// Returns the model's inputs.
    /// </summary>
    public class GetModelInputsRequest : ClarifaiPaginatedRequest<List<IClarifaiInput>>
    {
        protected override RequestMethod Method => RequestMethod.GET;
        protected override string Url
        {
            get
            {
                if (_versionID == null)
                {
                    return $"/v2/models/{_modelID}/inputs";
                }
                else
                {
                    return $"/v2/models/{_modelID}/versions/{_versionID}/inputs";
                }
            }
        }

        private readonly string _modelID;
        private readonly string _versionID;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="modelID">the model ID</param>
        /// <param name="versionID">the model's version ID</param>
        public GetModelInputsRequest(IClarifaiHttpClient httpClient, string modelID,
            string versionID = null) : base(httpClient)
        {
            _modelID = modelID;
            _versionID = versionID;
        }

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
            return await grpcClient.ListModelInputsAsync(new ListModelInputsRequest());
        }
    }
}
