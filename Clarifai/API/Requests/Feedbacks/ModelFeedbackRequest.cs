using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clarifai.DTOs.Feedbacks;
using Clarifai.DTOs.Models.Outputs;
using Clarifai.Internal.GRPC;
using Google.Protobuf;

namespace Clarifai.API.Requests.Feedbacks
{
    /// <summary>
    /// Used to send a feedback of prediction success to the model.
    /// </summary>
    public class ModelFeedbackRequest : ClarifaiRequest<EmptyResponse>
    {
        protected override RequestMethod Method => RequestMethod.POST;
        protected override string Url
        {
            get
            {
                if (_modelVersionID == null)
                {
                    return $"/v2/models/{_modelID}/feedback";
                }
                else
                {
                    return $"/v2/models/{_modelID}/versions/{_modelVersionID}/feedback";
                }
            }
        }

        private readonly string _modelID;
        private readonly string _imageUrl;
        private readonly string _inputID;
        private readonly string _outputID;
        private readonly IEnumerable<ConceptFeedback> _concepts;
        private readonly IEnumerable<RegionFeedback> _regions;
        private readonly string _modelVersionID;
        private readonly string _endUserID;
        private readonly string _sessionID;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="modelID">the model ID</param>
        /// <param name="imageUrl">the image URL</param>
        /// <param name="inputID">the input ID</param>
        /// <param name="outputID">the output ID</param>
        /// <param name="endUserID">the end user ID</param>
        /// <param name="sessionID">the session ID</param>
        /// <param name="concepts">the concepts</param>
        /// <param name="regions">the regions</param>
        /// <param name="modelVersionID">the model version ID</param>
        public ModelFeedbackRequest(IClarifaiHttpClient httpClient, string modelID, string imageUrl,
            string inputID, string outputID, string endUserID, string sessionID,
            IEnumerable<ConceptFeedback> concepts = null,
            IEnumerable<RegionFeedback> regions = null, string modelVersionID = null)
            : base(httpClient)
        {
            _modelID = modelID;
            _imageUrl = imageUrl;
            _inputID = inputID;
            _outputID = outputID;
            _endUserID = endUserID;
            _sessionID = sessionID;
            _concepts = concepts;
            _regions = regions;
            _modelVersionID = modelVersionID;
        }

        /// <inheritdoc />
        protected override EmptyResponse Unmarshaller(dynamic responseD)
        {
            return new EmptyResponse();
        }

        /// <inheritdoc />
        protected override async Task<IMessage> GrpcRequestBody(V2.V2Client grpcClient)
        {
            var data = new Data
            {
                Image = new Image
                {
                    Url = _imageUrl
                },
            };

            if (_concepts != null)
            {
                data = new Data(data)
                {
                    Concepts = {_concepts.Select(c => c.GrpcSerialize())},
                };
            }

            if (_regions != null)
            {
                data = new Data(data)
                {
                    Regions = {_regions.Select(r => r.GrpcSerialize())}
                };
            }

            return await grpcClient.PostModelFeedbackAsync(new PostModelFeedbackRequest
            {
                Input = new Input
                {
                    Id = _inputID,
                    Data = data,
                    FeedbackInfo = new FeedbackInfo
                    {
                        EventType = EventType.Annotation,
                        OutputId = _outputID,
                        EndUserId = _endUserID,
                        SessionId = _sessionID,
                    }
                }
            });
        }
    }
}
