using System.Collections.Generic;

namespace Clarifai.DTOs.Workflows
{
    public class WorkflowPredictResult
    {
        public Workflow Workflow { get; }

        public List<WorkflowResult> WorkflowResults { get; }

        private WorkflowPredictResult(Workflow workflow, List<WorkflowResult> workflowResults)
        {
            Workflow = workflow;
            WorkflowResults = workflowResults;
        }

        public static WorkflowPredictResult Deserialize(dynamic jsonObject)
        {
            Workflow workflow = Workflows.Workflow.Deserialize(jsonObject.workflow);
            var workflowResults = new List<WorkflowResult>();
            foreach (dynamic result in jsonObject.results)
            {
                workflowResults.Add(WorkflowResult.Deserialize(result));
            }
            return new WorkflowPredictResult(workflow, workflowResults);
        }
    }
}
