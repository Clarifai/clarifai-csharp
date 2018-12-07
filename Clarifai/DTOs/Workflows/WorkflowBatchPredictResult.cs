using System.Collections.Generic;
using Clarifai.API;

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
    }
}