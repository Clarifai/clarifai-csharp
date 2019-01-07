using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Clarifai.API.Responses;
using Clarifai.DTOs.Models;
using Clarifai.DTOs.Models.Outputs;
using Clarifai.DTOs.Models.OutputsInfo;
using Clarifai.DTOs.Predictions;
using NUnit.Framework;

namespace Clarifai.IntegrationTests
{
    [TestFixture]
    public class ModelVersionIntTests : BaseIntTests
    {
        [Test]
        [Retry(3)]
        public async Task ShorthandGetModelVersionRequestShouldBeSuccessful()
        {
            string modelID = Client.PublicModels.ModerationModel.ModelID;
            var getModelResponse = await Client.GetModel<Concept>(modelID).ExecuteAsync();
            IModel<Concept> model = getModelResponse.Get();

            ClarifaiResponse<ModelVersion> response =
                await model.GetModelVersion(model.ModelVersion.ID)
                    .ExecuteAsync();

            AssertResponseSuccess(response);
            Assert.AreEqual(10000, response.Status.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, response.HttpCode);
            Assert.AreEqual(model.ModelVersion.ID, response.Get().ID);
            Assert.IsNotNull(response.Get().Status);
        }

        [Test]
        [Retry(3)]
        public async Task ShorthandGetModelVersionsRequestShouldBeSuccessful()
        {
            string modelID = Client.PublicModels.ModerationModel.ModelID;
            var getModelResponse = await Client.GetModel<Concept>(modelID).ExecuteAsync();
            IModel<Concept> model = getModelResponse.Get();

            ClarifaiResponse<List<ModelVersion>> response =
                await model.GetModelVersions().ExecuteAsync();
            AssertResponseSuccess(response);
            Assert.AreEqual(10000, response.Status.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, response.HttpCode);

            List<ModelVersion> modelVersions = response.Get();
            Assert.IsNotNull(modelVersions);
            Assert.AreNotEqual(0, modelVersions.Count);
        }

        [Test]
        [Retry(3)]
        public async Task ShorthandDeleteModelVersionRequestShouldBeSuccessful()
        {
            string modelID = GenerateRandomID();

            ClarifaiResponse<ConceptModel> createResponse
                = await Client.CreateModel(
                        modelID,
                        name: modelID,
                        concepts: new List<Concept> {new Concept("dog"), new Concept("cat")})
                    .ExecuteAsync();
            AssertResponseSuccess(createResponse);

            ConceptModel model = createResponse.Get();

            ClarifaiResponse<ModelVersion> getVersionResponse =
                await Client.GetModelVersion(modelID, model.ModelVersion.ID).ExecuteAsync();
            string modelVersionID = getVersionResponse.Get().ID;

            ClarifaiResponse<EmptyResponse> deleteVersionResponse =
                await model.DeleteModelVersion(modelVersionID).ExecuteAsync();
            AssertResponseSuccess(deleteVersionResponse);

            /*
             * The model version should not exist anymore.
             */
            ClarifaiResponse<ModelVersion> getVersionResponse2 =
                await Client.GetModelVersion(modelID, modelVersionID).ExecuteAsync();
            Assert.False(getVersionResponse2.IsSuccessful);
        }
    }
}
