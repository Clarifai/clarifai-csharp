using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Clarifai.API.Responses;
using Clarifai.DTOs;
using Clarifai.DTOs.Inputs;
using Clarifai.DTOs.Models.Outputs;
using Clarifai.DTOs.Predictions;
using NUnit.Framework;

namespace Clarifai.IntegrationTests
{
    [TestFixture]
    public class PredictIntTests : BaseIntTests
    {
        [Test]
        [Retry(3)]
        public async Task ColorPredictForOneImageShouldBeSuccessful()
        {
            var response = await Client.Predict<Color>(
                    Client.PublicModels.ColorModel.ModelID,
                    new ClarifaiURLImage(CELEB1))
                .ExecuteAsync();

            Assert.True(response.IsSuccessful);
            Assert.AreEqual(10000, response.Status.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, response.HttpCode);
            Assert.NotNull(response.RawBody);

            Assert.NotNull(response.Get().Data[0]);
        }

        [Test]
        [Retry(3)]
        public async Task ConceptPredictForOneImageShouldBeSuccessful()
        {
            var response = await Client.Predict<Concept>(
                    Client.PublicModels.GeneralModel.ModelID,
                    new ClarifaiURLImage(CELEB1))
                .ExecuteAsync();

            Assert.True(response.IsSuccessful);
            Assert.AreEqual(10000, response.Status.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, response.HttpCode);
            Assert.NotNull(response.RawBody);

            Assert.NotNull(response.Get().Data[0]);
        }

        [Test]
        [Retry(3)]
        public async Task ConceptPredictForOneImageWithCropShouldBeSuccessful()
        {
            var response = await Client.Predict<Concept>(
                    Client.PublicModels.GeneralModel.ModelID,
                    new ClarifaiURLImage(
                        CELEB1,
                        crop: new Crop(0.1M, 0.2M, 0.3M, 0.4M)))
                .ExecuteAsync();

            Assert.True(response.IsSuccessful);
            Assert.AreEqual(10000, response.Status.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, response.HttpCode);
            Assert.NotNull(response.RawBody);
            Assert.NotNull(response.Get().Data[0]);
        }

        [Test]
        [Retry(3)]
        public async Task ConceptPredictForSeveralImagesShouldBeSuccessful()
        {
            var response = await Client.Predict<Concept>(
                    Client.PublicModels.GeneralModel.ModelID,
                    new List<IClarifaiInput>
                    {
                        new ClarifaiURLImage(CELEB1),
                        new ClarifaiURLImage(APPAREL1),
                        new ClarifaiURLImage(CAT1)
                    })
                .ExecuteAsync();

            Assert.True(response.IsSuccessful);
            Assert.AreEqual(10000, response.Status.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, response.HttpCode);
            Assert.NotNull(response.RawBody);

            Assert.NotNull(response.Get()[0].Data[0]);
            Assert.NotNull(response.Get()[1].Data[0]);
            Assert.NotNull(response.Get()[2].Data[0]);
        }

        [Test]
        [Retry(3)]
        public async Task ConceptPredictForSeveralImagesUsingListShouldBeSuccessful()
        {
            var response = await Client.Predict<Concept>(
                Client.PublicModels.GeneralModel.ModelID,
                new List<IClarifaiInput>
                {
                    new ClarifaiURLImage(CELEB1),
                    new ClarifaiURLImage(APPAREL1),
                    new ClarifaiURLImage(CAT1),
                })
                .ExecuteAsync();

            Assert.True(response.IsSuccessful);
            Assert.AreEqual(10000, response.Status.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, response.HttpCode);
            Assert.NotNull(response.RawBody);

            Assert.NotNull(response.Get()[0].Data[0]);
            Assert.NotNull(response.Get()[1].Data[0]);
            Assert.NotNull(response.Get()[2].Data[0]);
        }

        [Test]
        [Retry(3)]
        public async Task ConceptPredictForOneFileImageShouldBeSuccessful()
        {
            byte[] bytes = ReadResource(BALLOONS_IMAGE_FILE);

            var response = await Client.Predict<Concept>(
                    Client.PublicModels.GeneralModel.ModelID,
                    new ClarifaiFileImage(bytes))
                .ExecuteAsync();

            Assert.True(response.IsSuccessful);
            Assert.AreEqual(10000, response.Status.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, response.HttpCode);
            Assert.NotNull(response.RawBody);

            Assert.NotNull(response.Get().Data[0]);
        }

        [Test]
        [Retry(3)]
        public async Task ConceptPredictForOneFileVideoShouldBeSuccessful()
        {
            byte[] bytes = ReadResource(BEER_VIDEO_FILE);

            var response = await Client.Predict<Frame>(
                    Client.PublicModels.GeneralVideoModel.ModelID,
                    new ClarifaiFileVideo(bytes))
                .ExecuteAsync();

            Assert.True(response.IsSuccessful);
            Assert.AreEqual(10000, response.Status.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, response.HttpCode);
            Assert.NotNull(response.RawBody);

            Assert.NotNull(response.Get().Data[0].Concepts[0]);
        }

        [Test]
        [Retry(3)]
        public async Task PredictForOneVideoFileWithSampleMsShouldBeSuccessful()
        {
            byte[] bytes = ReadResource(BEER_VIDEO_FILE);

            var response = await Client.Predict<Frame>(
                    Client.PublicModels.GeneralVideoModel.ModelID,
                    new ClarifaiFileVideo(bytes),
                    sampleMs: 2000)
                .ExecuteAsync();

            Assert.True(response.IsSuccessful);
            Assert.AreEqual(10000, response.Status.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, response.HttpCode);
            Assert.NotNull(response.RawBody);

            Assert.NotNull(response.Get().Data[0].Concepts[0]);
        }

        [Test]
        [Retry(3)]
        public async Task ShorthandPredictForOneVideoFileWithSampleMsShouldBeSuccessful()
        {
            byte[] bytes = ReadResource(BEER_VIDEO_FILE);

            var response = await Client.PublicModels.GeneralVideoModel.Predict(
                    new ClarifaiFileVideo(bytes),
                    sampleMs: 2000)
                .ExecuteAsync();

            Assert.True(response.IsSuccessful);
            Assert.AreEqual(10000, response.Status.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, response.HttpCode);
            Assert.NotNull(response.RawBody);

            Assert.NotNull(response.Get().Data[0].Concepts[0]);
        }

        [Test]
        [Retry(3)]
        public async Task ConceptPredictWithModelVersionForOneImageShouldBeSuccessful()
        {
            string modelID = Client.PublicModels.GeneralModel.ModelID;
            var getModelResponse = await Client.GetModel<Concept>(modelID).ExecuteAsync();
            string modelVersionID = getModelResponse.Get().ModelVersion.ID;

            var response = await Client.Predict<Concept>(
                    modelID,
                    new ClarifaiURLImage(CELEB1),
                    modelVersionID: modelVersionID)
                .ExecuteAsync();

            Assert.True(response.IsSuccessful);
            Assert.AreEqual(10000, response.Status.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, response.HttpCode);
            Assert.NotNull(response.RawBody);

            Assert.NotNull(response.Get().Data[0]);
        }

        [Test]
        [Retry(3)]
        public async Task ShorthandConceptPredictImageShouldBeSuccessful()
        {
            ClarifaiResponse<ClarifaiOutput<Concept>> response =
                await Client.PublicModels.GeneralModel.Predict(new ClarifaiURLImage(CELEB1))
                    .ExecuteAsync();

            Assert.True(response.IsSuccessful);
            Assert.AreEqual(10000, response.Status.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, response.HttpCode);
            Assert.NotNull(response.RawBody);

            Assert.NotNull(response.Get().Data[0]);
        }

        [Test]
        [Retry(3)]
        public async Task ShorthandConceptPredictWithMinValueShouldBeSuccessful()
        {
            ClarifaiResponse<ClarifaiOutput<Concept>> response =
                await Client.PublicModels.GeneralModel.Predict(
                        new ClarifaiURLImage(CELEB1), minValue: 0.95M)
                    .ExecuteAsync();

            Assert.True(response.IsSuccessful);
            Assert.AreEqual(10000, response.Status.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, response.HttpCode);
            Assert.NotNull(response.RawBody);

            List<Concept> concepts = response.Get().Data;
            Assert.True(concepts.TrueForAll(c => c.Value >= 0.95M));
        }

        [Test]
        [Retry(3)]
        public async Task ShorthandConceptPredictWithMaxConceptsShouldBeSuccessful()
        {
            ClarifaiResponse<ClarifaiOutput<Concept>> response =
                await Client.PublicModels.GeneralModel.Predict(
                        new ClarifaiURLImage(CELEB1), maxConcepts: 3)
                    .ExecuteAsync();

            Assert.True(response.IsSuccessful);
            Assert.AreEqual(10000, response.Status.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, response.HttpCode);
            Assert.NotNull(response.RawBody);

            List<Concept> concepts = response.Get().Data;
            Assert.True(concepts.Count <= 3);
        }

        [Test]
        [Retry(3)]
        public async Task ShorthandConceptPredictWithSelectConceptsShouldBeSuccessful()
        {
            string catConceptId = "ai_mFqxrph2";
            string dogConceptId = "ai_8S2Vq3cR";
            ClarifaiResponse<ClarifaiOutput<Concept>> response =
                await Client.PublicModels.GeneralModel.Predict(
                        new ClarifaiURLImage(CELEB1),
                        selectConcepts: new List<Concept>
                        {
                            new Concept(catConceptId), new Concept(dogConceptId)
                        })
                    .ExecuteAsync();

            Assert.True(response.IsSuccessful);
            Assert.AreEqual(10000, response.Status.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, response.HttpCode);
            Assert.NotNull(response.RawBody);

            List<string> conceptNames = response.Get().Data.Select(c => c.ID).ToList();
            Assert.AreEqual(2, conceptNames.Count);
            CollectionAssert.Contains(conceptNames, catConceptId);
            CollectionAssert.Contains(conceptNames, dogConceptId);
        }
    }
}
