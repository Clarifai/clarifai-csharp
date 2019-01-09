using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clarifai.DTOs.Inputs;
using Clarifai.DTOs.Workflows;
using Clarifai.Internal.GRPC;
using Google.Protobuf;

namespace Clarifai.API.Requests.Models
{
    /// <summary>
    /// Using workflows, you can predict using multiple models with one request.
    /// The latency that would otherwise be required for each model predict request is with
    /// workflow predict reduced to a latency of one request.
    /// </summary>
    public class WorkflowBatchPredictRequest : ClarifaiRequest<WorkflowBatchPredictResult>
    {
        protected override RequestMethod Method => RequestMethod.POST;
        protected override string Url => $"/v2/workflows/{_workflowID}/results";

        private readonly string _workflowID;
        private readonly IEnumerable<IClarifaiInput> _inputs;
        private readonly decimal? _minValue;
        private readonly int? _maxConcepts;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="workflowID">the workflow ID</param>
        /// <param name="inputs">the inputs to run predictions on</param>
        /// <param name="minValue">return only results that have at least this value</param>
        /// <param name="maxConcepts">the maximum number of concepts to return</param>
        public WorkflowBatchPredictRequest(IClarifaiHttpClient httpClient, string workflowID,
            IEnumerable<IClarifaiInput> inputs, decimal? minValue = null, int? maxConcepts = null)
            : base(httpClient)
        {
            _workflowID = workflowID;
            _inputs = inputs;
            _minValue = minValue;
            _maxConcepts = maxConcepts;
        }

        /// <inheritdoc />
        protected override async Task<IMessage> GrpcRequestBody(V2.V2Client grpcClient)
        {
            var request = new PostWorkflowResultsRequest
            {
                Inputs = {_inputs.Select(i => i.GrpcSerialize())}
            };
            if (_minValue != null || _maxConcepts != null)
            {
                var outputConfig = new OutputConfig();
                if (_minValue != null)
                {
                    outputConfig = new OutputConfig(outputConfig)
                    {
                        MinValue = (float) _minValue
                    };
                }
                if (_maxConcepts != null)
                {
                    outputConfig = new OutputConfig(outputConfig)
                    {
                        MaxConcepts = Convert.ToUInt32(_maxConcepts)
                    };
                }

                request = new PostWorkflowResultsRequest(request)
                {
                    OutputConfig = outputConfig
                };
            }
            return await grpcClient.PostWorkflowResultsAsync(request);
        }

        protected override WorkflowBatchPredictResult Unmarshaller(dynamic responseD)
        {
            PostWorkflowResultsResponse response = responseD;
            return WorkflowBatchPredictResult.GrpcDeserialize(HttpClient, response);
        }
    }
}
