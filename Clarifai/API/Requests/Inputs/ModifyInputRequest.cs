using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clarifai.API.Requests.Models;
using Clarifai.DTOs.Feedbacks;
using Clarifai.DTOs.Inputs;
using Clarifai.Internal.GRPC;
using Google.Protobuf;
using Concept = Clarifai.DTOs.Predictions.Concept;

namespace Clarifai.API.Requests.Inputs
{
    /// <summary>
    /// A request for input modification.
    /// </summary>
    public class ModifyInputRequest : ClarifaiRequest<IClarifaiInput>
    {
        protected override RequestMethod Method => RequestMethod.PATCH;
        protected override string Url => "/v2/inputs";

        private readonly string _inputID;
        private readonly ModifyAction _action;
        private readonly IEnumerable<Concept> _positiveConcepts;
        private readonly IEnumerable<Concept> _negativeConcepts;
        private readonly IEnumerable<RegionFeedback> _regionFeedbacks;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="inputID">the input ID</param>
        /// <param name="action">the action</param>
        /// <param name="positiveConcepts">positive concepts</param>
        /// <param name="negativeConcepts">negative concepts</param>
        /// <param name="regionFeedbacks">region feedbacks</param>
        public ModifyInputRequest(IClarifaiHttpClient httpClient, string inputID, ModifyAction action,
            IEnumerable<Concept> positiveConcepts, IEnumerable<Concept> negativeConcepts,
            IEnumerable<RegionFeedback> regionFeedbacks = null)
            : base(httpClient)
        {
            _inputID = inputID;
            _action = action;
            _positiveConcepts = positiveConcepts;
            _negativeConcepts = negativeConcepts;
            _regionFeedbacks = regionFeedbacks;
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
            var concepts = new List<Internal.GRPC.Concept>();
            if (_positiveConcepts != null)
            {
                concepts.AddRange(_positiveConcepts.Select(c => c.GrpcSerialize(true)));
            }
            if (_negativeConcepts != null)
            {
                concepts.AddRange(_negativeConcepts.Select(c => c.GrpcSerialize(false)));
            }
            var data = new Data
            {
                Concepts = { concepts }
            };
            if (_regionFeedbacks !=  null && _regionFeedbacks.Any())
            {
                data = new Data(data)
                {
                    Regions = { _regionFeedbacks.Select(r => r.GrpcSerialize())}
                };
            }
            return await grpcClient.PatchInputsAsync(new PatchInputsRequest
            {
                Action = _action.GrpcSerialize(),
                Inputs = {new Input
                {
                    Id = _inputID,
                    Data = data
                }}
            });
        }
    }
}
