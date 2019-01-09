using Clarifai.API;
using Clarifai.Internal.GRPC;

namespace Clarifai.DTOs.Workflows
{
    public class WorkflowPredictResult
    {
        public Workflow Workflow { get; }

        public WorkflowResult WorkflowResult { get; }

        private WorkflowPredictResult(Workflow workflow, WorkflowResult workflowResult)
        {
            Workflow = workflow;
            WorkflowResult = workflowResult;
        }

        /// <summary>
        /// Deserializes the object out of a JSON dynamic object.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="jsonObject">the JSON object</param>
        /// <returns>the deserialized object</returns>
        public static WorkflowPredictResult Deserialize(IClarifaiHttpClient httpClient,
            dynamic jsonObject)
        {
            return new WorkflowPredictResult(
                jsonObject.workflow != null
                    ? Workflows.Workflow.Deserialize(jsonObject.workflow)
                    : null,
                Workflows.WorkflowResult.Deserialize(httpClient, jsonObject.results[0]));
        }

        public static WorkflowPredictResult GrpcDeserialize(IClarifaiHttpClient httpClient,
            PostWorkflowResultsResponse response)
        {
            return new WorkflowPredictResult(
                response.Workflow != null
                    ? Workflows.Workflow.GrpcDeserialize(response.Workflow)
                    : null,
                Workflows.WorkflowResult.GrpcDeserialize(httpClient, response.Results[0]));
        }
    }
}
