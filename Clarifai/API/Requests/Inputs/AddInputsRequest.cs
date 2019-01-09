using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clarifai.DTOs.Inputs;
using Clarifai.Internal.GRPC;
using Google.Protobuf;

namespace Clarifai.API.Requests.Inputs
{
    /// <summary>
    /// A request for adding inputs.
    /// </summary>
    public class AddInputsRequest : ClarifaiRequest<List<IClarifaiInput>>
    {
        protected override RequestMethod Method => RequestMethod.POST;
        protected override string Url => "/v2/inputs/";

        private readonly IEnumerable<IClarifaiInput> _inputs;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="inputs">the inputs</param>
        public AddInputsRequest(IClarifaiHttpClient httpClient, params IClarifaiInput[] inputs) :
            this(httpClient, inputs.ToList())
        { }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="inputs">the inputs</param>
        public AddInputsRequest(IClarifaiHttpClient httpClient, IEnumerable<IClarifaiInput> inputs)
            : base(httpClient)
        {
            _inputs = inputs;
        }

        /// <inheritdoc />
        protected override List<IClarifaiInput> Unmarshaller(dynamic responseD)
        {
            MultiInputResponse response = responseD;

            var inputs = new List<IClarifaiInput>();
            foreach (Input input in response.Inputs)
            {
                inputs.Add(ClarifaiInput.GrpcDeserialize(input));
            }
            return inputs;
        }

        /// <inheritdoc />
        protected override async Task<IMessage> GrpcRequestBody(V2.V2Client grpcClient)
        {
            return await grpcClient.PostInputsAsync(new PostInputsRequest
            {
                Inputs = {_inputs.Select(i => i.GrpcSerialize())}
            });
        }
    }
}
