using System.Collections.Generic;
using Clarifai.DTOs.Inputs;
using Clarifai.DTOs.Models;
using Clarifai.DTOs.Models.Outputs;

namespace Clarifai.DTOs.Workflows
{
    public class WorkflowResult
    {
        public ClarifaiStatus Status { get; }

        public ClarifaiInput Input { get; }

        public List<ClarifaiOutput> Predictions { get; }

        private WorkflowResult(ClarifaiStatus status, ClarifaiInput input,
            List<ClarifaiOutput> predictions)
        {
            Status = status;
            Input = input;
            Predictions = predictions;
        }

        public static WorkflowResult Deserialize(dynamic jsonObject)
        {
            var status = ClarifaiStatus.Deserialize(jsonObject.status);
            var input = ClarifaiInput.Deserialize(jsonObject.input);

            var predictions = new List<ClarifaiOutput>();
            foreach (dynamic output in jsonObject.outputs)
            {
                dynamic model = output.model;
                ModelType modelType = ModelType.DetermineModelType(
                    (string)model.output_info.type_ext);

                predictions.Add(ClarifaiOutput.Deserialize(modelType.Prediction.Name, output));
            }

            return new WorkflowResult(status, input, predictions);
        }
    }
}
