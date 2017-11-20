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
        public async Task WorkflowPredictShouldBeSuccessful()
        {
            ClarifaiResponse<WorkflowPredictResult> response =
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
