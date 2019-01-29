using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Clarifai.API.Responses;
using Clarifai.DTOs.Inputs;
using Clarifai.DTOs.Models;
using Clarifai.DTOs.Predictions;
using NUnit.Framework;

namespace Clarifai.IntegrationTests
{
    [TestFixture]
    public class ModelEvaluationIntTests : BaseIntTests
    {
        [Test]
        [Retry(3)]
        [Ignore("Skipped because the evaluation time varies.")]
        public async Task ModelEvaluationRequestShouldBeSuccessful()
        {
            /*
             * Create a model.
             */
            string modelID = GenerateRandomID();
            ClarifaiResponse<ConceptModel> createModelResponse = await Client.CreateModel(
                    modelID,
                    concepts: new List<Concept>
                    {
                        new Concept("celeb"),
                        new Concept("cat"),
                    })
                .ExecuteAsync();
            AssertResponseSuccess(createModelResponse);

            try
            {
                /*
                 * Add inputs associated with concepts.
                 */
                ClarifaiResponse<List<IClarifaiInput>> addInputsResponse = await Client.AddInputs(
                    new ClarifaiURLImage(CELEB1,
                        positiveConcepts: new List<Concept> {new Concept("celeb")},
                        allowDuplicateUrl: true),
                    new ClarifaiURLImage(CAT1,
                        positiveConcepts: new List<Concept> {new Concept("cat")},
                        allowDuplicateUrl: true)
                ).ExecuteAsync();
                AssertResponseSuccess(addInputsResponse);

                /*
                 * Train the model.
                 */
                await Client.TrainModel<Concept>(modelID).ExecuteAsync();

                /*
                 * Wait until the model has finished training.
                 */
                int count = 0;
                string modelVersionID;
                while (true)
                {
                    count++;
                    if (count == 20)
                    {
                        Assert.Fail("Model training has not finished in the allotted time.");
                    }

                    ClarifaiResponse<IModel<Concept>> response =
                        await Client.GetModel<Concept>(modelID).ExecuteAsync();
                    modelVersionID = response.Get().ModelVersion.ID;

                    ModelTrainingStatus status = response.Get().ModelVersion.Status;
                    if (status.IsTerminalEvent())
                    {
                        if (status.StatusCode == ModelTrainingStatus.Trained.StatusCode)
                        {
                            break;
                        }

                        Assert.Fail("The model was not trained successfully: " + status.StatusCode);
                    }

                    Thread.Sleep(1000);
                }

                /*
                 * Start model evaluation.
                 */
                ClarifaiResponse<ModelVersion> evalResponse = await Client.ModelEvaluation(
                        modelID, modelVersionID)
                    .ExecuteAsync();
                AssertResponseSuccess(evalResponse);
                Assert.NotNull(evalResponse.Get().ModelMetricsStatus);
            }
            finally
            {
                await Client.DeleteModel(modelID).ExecuteAsync();
            }
        }
    }
}
