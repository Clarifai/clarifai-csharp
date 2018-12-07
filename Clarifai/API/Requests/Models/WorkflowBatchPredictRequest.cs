using System.Collections.Generic;
using System.Linq;
using Clarifai.DTOs.Inputs;
using Clarifai.DTOs.Workflows;
using Newtonsoft.Json.Linq;

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
        protected override JObject HttpRequestBody()
        {
            var body = new JObject(
                new JProperty("inputs", new JArray(_inputs.Select(i => i.Serialize()))));
            var outputConfig = new JObject();
            if (_minValue != null)
            {
                outputConfig["min_value"] = _minValue;
            }
            if (_maxConcepts != null)
            {
                outputConfig["max_concepts"] = _maxConcepts;
            }
            if (outputConfig.Count > 0)
            {
                body.Add("output_config", outputConfig);
            }
            return body;
        }

        /// <inheritdoc />
        protected override WorkflowBatchPredictResult Unmarshaller(dynamic jsonObject)
        {
            return WorkflowBatchPredictResult.Deserialize(HttpClient, jsonObject);
        }
    }
}
