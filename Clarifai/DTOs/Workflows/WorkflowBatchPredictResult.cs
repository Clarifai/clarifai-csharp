using System.Collections.Generic;
using Clarifai.API;
using Clarifai.Internal.GRPC;

namespace Clarifai.DTOs.Workflows
{
    public class WorkflowBatchPredictResult
    {
        public Workflow Workflow { get; }

        public List<WorkflowResult> WorkflowResults { get; }

        private WorkflowBatchPredictResult(Workflow workflow, List<WorkflowResult> workflowResults)
        {
            Workflow = workflow;
            WorkflowResults = workflowResults;
        }

        public static WorkflowBatchPredictResult Deserialize(IClarifaiHttpClient httpClient,
            dynamic jsonObject)
        {
            Workflow workflow = null;
            if (jsonObject.workflow != null)
            {
                workflow = Workflows.Workflow.Deserialize(jsonObject.workflow);
            }
            var workflowResults = new List<WorkflowResult>();
            foreach (dynamic result in jsonObject.results)
            {
                workflowResults.Add(WorkflowResult.Deserialize(httpClient, result));
            }
            return new WorkflowBatchPredictResult(workflow, workflowResults);
        }

        public static WorkflowBatchPredictResult GrpcDeserialize(IClarifaiHttpClient httpClient,
            PostWorkflowResultsResponse response)
        {
            Workflow workflow = null;
            if (response.Workflow != null)
            {
                workflow = Workflows.Workflow.GrpcDeserialize(response.Workflow);
            }
            var workflowResults = new List<WorkflowResult>();
            foreach (Internal.GRPC.WorkflowResult result in response.Results)
            {
                workflowResults.Add(WorkflowResult.GrpcDeserialize(httpClient, result));
            }
            return new WorkflowBatchPredictResult(workflow, workflowResults);
        }
    }
}