using System.Collections.Generic;
using Clarifai.API;
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

        /// <summary>
        /// Deserializes the object out of a JSON dynamic object.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="jsonObject">the JSON object</param>
        /// <returns>the deserialized object</returns>
        public static WorkflowResult Deserialize(IClarifaiHttpClient httpClient, dynamic jsonObject)
        {
            var status = ClarifaiStatus.Deserialize(jsonObject.status);
            var input = ClarifaiInput.Deserialize(jsonObject.input);

            var predictions = new List<ClarifaiOutput>();
            foreach (dynamic output in jsonObject.outputs)
            {
                dynamic model = output.model;
                ModelType modelType = ModelType.DetermineModelType(
                    (string)model.output_info.type_ext);

                predictions.Add(ClarifaiOutput.Deserialize(httpClient, modelType, output));
            }

            return new WorkflowResult(status, input, predictions);
        }
    }
}
