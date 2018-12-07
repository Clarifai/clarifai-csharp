using System.Collections.Generic;
using System.Threading.Tasks;
using Clarifai.API.Responses;
using Clarifai.DTOs.Inputs;
using Clarifai.DTOs.Workflows;
using NUnit.Framework;

namespace Clarifai.IntegrationTests
{
    [TestFixture]
    public class WorkflowIntTests : BaseIntTests
    {
        [Test]
        [Retry(3)]
        public async Task WorkflowPredictURLImageShouldBeSuccessful()
        {
            var response = await Client.WorkflowPredict(
                    "food-and-general",
                    new ClarifaiURLImage(CELEB1))
                .ExecuteAsync();

            WorkflowResult result = response.Get().WorkflowResult;
            Assert.AreEqual(2, result.Predictions.Count);
            Assert.NotNull(result.Predictions[0].Data);
            Assert.NotNull(result.Predictions[1].Data);
        }

        [Test]
        [Retry(3)]
        public async Task WorkflowPredictFileImageShouldBeSuccessful()
        {
            var response = await Client.WorkflowPredict(
                    "food-and-general",
                    new ClarifaiFileImage(ReadResource(BALLOONS_IMAGE_FILE)))
                .ExecuteAsync();

            Assert.True(response.IsSuccessful);

            WorkflowResult result = response.Get().WorkflowResult;
            Assert.AreEqual(2, result.Predictions.Count);
            Assert.NotNull(result.Predictions[0].Data);
            Assert.NotNull(result.Predictions[1].Data);

            ClarifaiFileImage fileImage = (ClarifaiFileImage) result.Input;
            Assert.NotNull(fileImage.Bytes);
        }

        [Test]
        [Retry(3)]
        public async Task WorkflowBatchPredictShouldBeSuccessful()
        {
            ClarifaiResponse<WorkflowBatchPredictResult> response =
                await Client.WorkflowPredict(
                        "food-and-general",
                        new List<IClarifaiInput>
                        {
                            new ClarifaiURLImage(CELEB1),
                        })
                    .ExecuteAsync();

            List<WorkflowResult> workflowResults = response.Get().WorkflowResults;
            Assert.AreEqual(1, workflowResults.Count);

            WorkflowResult result = workflowResults[0];
            Assert.AreEqual(2, result.Predictions.Count);
            Assert.NotNull(result.Predictions[0].Data);
            Assert.NotNull(result.Predictions[1].Data);
        }
    }
}
