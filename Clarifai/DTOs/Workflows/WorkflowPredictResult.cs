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

        public static WorkflowPredictResult Deserialize(dynamic jsonObject)
        {
            return new WorkflowPredictResult(
                jsonObject.workflow != null
                    ? Workflows.Workflow.Deserialize(jsonObject.workflow)
                    : null,
                Workflows.WorkflowResult.Deserialize(jsonObject.results[0]));
        }
    }
}
