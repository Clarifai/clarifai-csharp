using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Clarifai.API;
using Clarifai.API.Requests.Models;
using Clarifai.API.Responses;
using Clarifai.DTOs;
using Clarifai.DTOs.Inputs;
using Clarifai.DTOs.Models;
using Clarifai.DTOs.Models.Outputs;
using Clarifai.DTOs.Models.OutputsInfo;
using Clarifai.DTOs.Predictions;
using NUnit.Framework;

namespace Clarifai.IntegrationTests
{
    [TestFixture]
    public class ModelIntTests : BaseIntTests
    {
        [Test]
        [Retry(3)]
        public async Task GettingModelShouldBeSuccessful()
        {
            ClarifaiResponse<IModel<Concept>> response =
                await Client.GetModel<Concept>(Client.PublicModels.ModerationModel.ModelID)
                    .ExecuteAsync();

            AssertResponseSuccess(response);
            Assert.AreEqual(10000, response.Status.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, response.HttpCode);
            Assert.NotNull(response.RawBody);

            Assert.NotNull(response.Get().ModelID);
        }

        [Test]
        [Retry(3)]
        public async Task CreatingModifyingAndDeletingModelShouldBeSuccessful()
        {
            string modelID = GenerateRandomID();

            string originalName = "original-name";
            string newName = "new-name";

            var originalConcepts = new List<Concept>
            {
                new Concept("dog"),
                new Concept("cat"),
            };
            var newConcepts = new List<Concept>
            {
                new Concept("horse"),
                new Concept("bird"),
            };

            try
            {
                /*
                 * Create a new model.
                 */
                ClarifaiResponse<ConceptModel> createResponse =
                    await Client.CreateModel(
                        modelID,
                        name: originalName,
                        concepts: originalConcepts,
                        areConceptsMutuallyExclusive: false,
                        isEnvironmentClosed: false)
                    .ExecuteAsync();

                AssertResponseSuccess(createResponse);
                ConceptModel createdModel = createResponse.Get();
                Assert.AreEqual(modelID, createdModel.ModelID);
                Assert.AreEqual(originalName, createdModel.Name);
                Assert.AreEqual(false, createdModel.OutputInfo.AreConceptsMutuallyExclusive);
                Assert.AreEqual(false, createdModel.OutputInfo.IsEnvironmentClosed);
                Assert.AreEqual(HttpStatusCode.OK, createResponse.HttpCode);

                /*
                 * Modify the model.
                 */
                ClarifaiResponse<ConceptModel> modifyResponse =
                    await Client.ModifyModel(
                        modelID,
                        ModifyAction.Overwrite,
                        name: newName,
                        concepts: newConcepts,
                        areConceptsMutuallyExclusive: true,
                        isEnvironmentClosed: true)
                    .ExecuteAsync();

                AssertResponseSuccess(modifyResponse);
                ConceptModel modifiedModel = modifyResponse.Get();
                Assert.AreEqual(modelID, modifiedModel.ModelID);
                Assert.AreEqual(newName, modifiedModel.Name);
                Assert.AreEqual(true, modifiedModel.OutputInfo.AreConceptsMutuallyExclusive);
                Assert.AreEqual(true, modifiedModel.OutputInfo.IsEnvironmentClosed);
                Assert.AreEqual(HttpStatusCode.OK, modifyResponse.HttpCode);

                /*
                 * Get the model to ensure the fields were changed.
                 */
                ClarifaiResponse<IModel<Concept>> getResponse =
                    await Client.GetModel<Concept>(modelID)
                    .ExecuteAsync();
                AssertResponseSuccess(getResponse);
                IModel<Concept> retrievedModel = getResponse.Get();
                Assert.AreEqual(modelID, retrievedModel.ModelID);
                Assert.AreEqual(newName, retrievedModel.Name);
                Assert.AreEqual(HttpStatusCode.OK, getResponse.HttpCode);
                Assert.AreEqual(2, newConcepts.Count);
                Assert.IsTrue(newConcepts.Any(c => c.ID == newConcepts[0].ID));
                Assert.IsTrue(newConcepts.Any(c => c.ID == newConcepts[1].ID));
            }
            finally
            {
                /*
                 * Delete the model.
                 */
                ClarifaiResponse<EmptyResponse> deleteResponse =
                    await Client.DeleteModel(modelID).ExecuteAsync();
                AssertResponseSuccess(deleteResponse);
                Assert.AreEqual(HttpStatusCode.OK, deleteResponse.HttpCode);
            }
        }

        [Test]
        [Retry(3)]
        [Ignore("Skipped because the training time varies.")]
        public async Task ShorthandTrainingModelShouldBeSuccessful()
        {
            /*
             * Create a model.
             */
            string modelID = GenerateRandomID();
            var createModelResponse = await Client.CreateModel(
                    modelID,
                    concepts: new List<Concept>
                    {
                        new Concept("celeb"),
                        new Concept("cat"),
                    })
                .ExecuteAsync();
            ConceptModel model = createModelResponse.Get();

            /*
             * Add inputs associated with concepts.
             */
            ClarifaiResponse<List<IClarifaiInput>> addInputsResponse = await Client.AddInputs(
                new ClarifaiURLImage(CELEB1, positiveConcepts: new List<Concept>{new Concept("celeb")},
                    allowDuplicateUrl: true),
                new ClarifaiURLImage(CAT1, positiveConcepts: new List<Concept>{new Concept("cat")},
                    allowDuplicateUrl: true)
            ).ExecuteAsync();
            if (addInputsResponse.Status.Type != ClarifaiStatus.StatusType.Successful)
            {
                Assert.Fail("Adding inputs not successful: " + addInputsResponse.Status.Type);
            }

            /*
             * Train the model.
             */
            await model.TrainModel().ExecuteAsync();

            /*
             * Wait until the model has finished training.
             */
            int count = 0;
            while (true)
            {
                count++;
                if (count == 20)
                {
                    Assert.Fail("Model training has not finished in the allotted time.");
                }

                ClarifaiResponse<IModel<Concept>> response =
                    await Client.GetModel<Concept>(modelID).ExecuteAsync();
                ModelTrainingStatus status = response.Get().ModelVersion.Status;
                if (status.IsTerminalEvent())
                {
                    if (status.StatusCode == ModelTrainingStatus.Trained.StatusCode)
                    {
                        return;
                    }
                    Assert.Fail("The model was not trained successfully: " + status.StatusCode);
                }
                Thread.Sleep(1000);
            }
        }

        [Test]
        [Retry(3)]
        public async Task GetModelsShouldBeSuccessful()
        {
            ClarifaiResponse<List<IModel>> response = await Client.GetModels().ExecuteAsync();
            List<IModel> models = response.Get();
            Assert.IsNotNull(models);
            Assert.AreNotEqual(0, models.Count);
            foreach (IModel model in models)
            {
                Assert.IsNotNull(model.Name);
                Assert.IsNotNull(model.OutputInfo.Type);
                Assert.IsNotNull(model.OutputInfo.TypeExt);
            }
        }

        [Test]
        [Retry(3)]
        public async Task PaginatedGetModelsShouldBeSuccessful()
        {
            ClarifaiResponse<List<IModel>> response = await Client.GetModels()
                .Page(1)
                .PerPage(3)
                .ExecuteAsync();
            List<IModel> models = response.Get();
            Assert.IsNotNull(models);
            Assert.True(models.Count <= 3);
            foreach (IModel model in models)
            {
                Assert.IsNotNull(model);
            }
        }

        [Test]
        [Retry(3)]
        [Ignore("Temporarily disabled until a backend GET /model/../inputs issue fixed")]
        public async Task ShorthandGetModelInputsShouldBeSuccessful()
        {
            /*
             * Create a model.
             */
            string modelID = GenerateRandomID();
            ClarifaiResponse<ConceptModel> createModelResponse = await Client.CreateModel(
                modelID,
                concepts: new List<Concept> { new Concept("cat") })
            .ExecuteAsync();

            AssertResponseSuccess(createModelResponse);

            try
            {
                ConceptModel model = createModelResponse.Get();
                var getModelInputsResponse = await model.GetModelInputs().ExecuteAsync();

                Assert.True(getModelInputsResponse.IsSuccessful);
                Assert.NotNull(getModelInputsResponse.Get());

            }
            finally
            {
                /*
                 * Delete the model.
                 */
                var deleteResponse = await Client.DeleteModel(modelID)
                    .ExecuteAsync();
                Assert.True(deleteResponse.IsSuccessful);
            }
        }

        [Test]
        [Retry(3)]
        public async Task DeleteAllModelsRequestShouldBeSuccessful()
        {
            ClarifaiResponse<EmptyResponse> response = await Client.DeleteAllModels()
                .ExecuteAsync();
            AssertResponseSuccess(response);
        }
    }
}
