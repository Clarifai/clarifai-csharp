using System.Collections.Generic;
using Clarifai.API;
using Clarifai.DTOs.Inputs;
using Clarifai.DTOs.Models;
using Clarifai.DTOs.Models.Outputs;
using Clarifai.Internal.GRPC;
using Model = Clarifai.Internal.GRPC.Model;

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

        public static WorkflowResult GrpcDeserialize(IClarifaiHttpClient httpClient,
            Internal.GRPC.WorkflowResult result)
        {
            var status = ClarifaiStatus.GrpcDeserialize(result.Status);
            var input = ClarifaiInput.GrpcDeserialize(result.Input);

            var predictions = new List<ClarifaiOutput>();
            foreach (Output output in result.Outputs)
            {
                Model model = output.Model;
                ModelType modelType = ModelType.DetermineModelType(model.OutputInfo.TypeExt);

                predictions.Add(ClarifaiOutput.GrpcDeserialize(httpClient, modelType, output));
            }

            return new WorkflowResult(status, input, predictions);
        }
    }
}
