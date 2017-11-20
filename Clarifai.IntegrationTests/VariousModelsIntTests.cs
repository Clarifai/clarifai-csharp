using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Clarifai.API.Responses;
using Clarifai.DTOs.Inputs;
using Clarifai.DTOs.Models;
using Clarifai.DTOs.Models.Outputs;
using Clarifai.DTOs.Predictions;
using NUnit.Framework;

namespace Clarifai.IntegrationTests
{
    [TestFixture]
    public class VariousModelsIntTests : BaseIntTests
    {
        [Test]
        [Retry(3)]
        public async Task GetColorModelShouldBeSuccessful()
        {
            ClarifaiResponse<IModel<Color>> response =
                await Client.GetModel<Color>(Client.PublicModels.ColorModel.ModelID)
                    .ExecuteAsync();

            Assert.True(response.IsSuccessful);
            Assert.AreEqual(10000, response.Status.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, response.HttpCode);
            Assert.NotNull(response.RawBody);

            ColorModel colorModel = (ColorModel) response.Get();
            Assert.NotNull(colorModel.ModelID);
            Assert.NotNull(colorModel.OutputInfo.Concepts);
        }

        [Test]
        [Retry(3)]
        public async Task PredictOnColorModelShouldBeSuccessful()
        {
            string modelID = Client.PublicModels.ColorModel.ModelID;

            ClarifaiResponse<List<ClarifaiOutput<Color>>> predictResponse =
                await Client.Predict<Color>(
                        modelID,
                        new List<IClarifaiInput> {new ClarifaiURLImage(APPAREL1)})
                    .ExecuteAsync();
            Assert.True(predictResponse.IsSuccessful);
            Color color = predictResponse.Get()[0].Data[0];
            Assert.NotNull(color.Name);
        }

        [Test]
        [Retry(3)]
        public async Task GetDemographicsModelShouldBeSuccessful()
        {
            ClarifaiResponse<IModel<Demographics>> response =
                await Client.GetModel<Demographics>(
                        Client.PublicModels.DemographicsModel.ModelID)
                    .ExecuteAsync();

            Assert.True(response.IsSuccessful);
            Assert.AreEqual(10000, response.Status.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, response.HttpCode);
            Assert.NotNull(response.RawBody);

            DemographicsModel demographicsModel = (DemographicsModel) response.Get();
            Assert.NotNull(demographicsModel.ModelID);
        }

        [Test]
        [Retry(3)]
        public async Task PredictOnDemographicsModelShouldBeSuccessful()
        {
            string modelID = Client.PublicModels.DemographicsModel.ModelID;

            ClarifaiResponse<List<ClarifaiOutput<Demographics>>> predictResponse =
                await Client.Predict<Demographics>(
                        modelID,
                        new List<IClarifaiInput> {new ClarifaiURLImage(CELEB1)})
                    .ExecuteAsync();
            Assert.True(predictResponse.IsSuccessful);

            Demographics demographics = predictResponse.Get()[0].Data[0];
            Assert.NotNull(demographics.Crop);
            Assert.NotNull(demographics.AgeAppearanceConcepts);
            Assert.NotNull(demographics.GenderAppearanceConcepts);
            Assert.NotNull(demographics.MulticulturalAppearanceConcepts);
        }

        [Test]
        [Retry(3)]
        public async Task GetEmbeddingModelShouldBeSuccessful()
        {
            ClarifaiResponse<IModel<Embedding>> response =
                await Client.GetModel<Embedding>(
                        Client.PublicModels.GeneralEmbeddingModel.ModelID)
                    .ExecuteAsync();

            Assert.True(response.IsSuccessful);
            Assert.AreEqual(10000, response.Status.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, response.HttpCode);
            Assert.NotNull(response.RawBody);

            EmbeddingModel embeddingModel = (EmbeddingModel) response.Get();
            Assert.NotNull(embeddingModel.ModelID);
        }

        [Test]
        [Retry(3)]
        public async Task PredictOnEmbeddingModelShouldBeSuccessful()
        {
            string modelID = Client.PublicModels.GeneralEmbeddingModel.ModelID;

            ClarifaiResponse<List<ClarifaiOutput<Embedding>>> predictResponse =
                await Client.Predict<Embedding>(
                        modelID,
                        new List<IClarifaiInput> {new ClarifaiURLImage(CELEB1)})
                    .ExecuteAsync();
            Assert.True(predictResponse.IsSuccessful);
            Embedding embedding = predictResponse.Get()[0].Data[0];
            Assert.NotNull(embedding.Vector);
        }

        [Test]
        [Retry(3)]
        public async Task GetFaceConceptsModelShouldBeSuccessful()
        {
            ClarifaiResponse<IModel<FaceConcepts>> response =
                await Client.GetModel<FaceConcepts>(
                        Client.PublicModels.CelebrityModel.ModelID)
                    .ExecuteAsync();

            Assert.True(response.IsSuccessful);
            Assert.AreEqual(10000, response.Status.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, response.HttpCode);
            Assert.NotNull(response.RawBody);

            FaceConceptsModel faceConceptsModel = (FaceConceptsModel) response.Get();
            Assert.NotNull(faceConceptsModel.ModelID);
        }

        [Test]
        [Retry(3)]
        public async Task PredictOnFaceConceptsModelShouldBeSuccessful()
        {
            string modelID = Client.PublicModels.CelebrityModel.ModelID;

            ClarifaiResponse<List<ClarifaiOutput<FaceConcepts>>> predictResponse =
                await Client.Predict<FaceConcepts>(
                        modelID,
                        new List<IClarifaiInput> {new ClarifaiURLImage(FACE1)})
                    .ExecuteAsync();
            Assert.True(predictResponse.IsSuccessful);
            FaceConcepts faceConcepts = predictResponse.Get()[0].Data[0];
            Assert.NotNull(faceConcepts.Crop);
            Assert.NotNull(faceConcepts.Concepts);
        }

        [Test]
        [Retry(3)]
        public async Task GetFaceDetectionModelShouldBeSuccessful()
        {
            ClarifaiResponse<IModel<FaceDetection>> response =
                await Client.GetModel<FaceDetection>(
                        Client.PublicModels.FaceDetectionModel.ModelID)
                    .ExecuteAsync();

            Assert.True(response.IsSuccessful);
            Assert.AreEqual(10000, response.Status.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, response.HttpCode);
            Assert.NotNull(response.RawBody);

            FaceDetectionModel faceDetectionModel = (FaceDetectionModel) response.Get();
            Assert.NotNull(faceDetectionModel.ModelID);
        }

        [Test]
        [Retry(3)]
        public async Task PredictOnFaceDetectionModelShouldBeSuccessful()
        {
            string modelID = Client.PublicModels.FaceDetectionModel.ModelID;

            ClarifaiResponse<List<ClarifaiOutput<FaceDetection>>> predictResponse =
                await Client.Predict<FaceDetection>(
                        modelID,
                        new List<IClarifaiInput> {new ClarifaiURLImage(FACE1)})
                    .ExecuteAsync();
            Assert.True(predictResponse.IsSuccessful);
            FaceDetection faceDetection = predictResponse.Get()[0].Data[0];
            Assert.NotNull(faceDetection.Crop);
        }

        [Test]
        [Retry(3)]
        public async Task GetFaceEmbeddingModelShouldBeSuccessful()
        {
            ClarifaiResponse<IModel<FaceEmbedding>> response =
                await Client.GetModel<FaceEmbedding>(
                        Client.PublicModels.FaceEmbeddingModel.ModelID)
                    .ExecuteAsync();

            Assert.True(response.IsSuccessful);
            Assert.AreEqual(10000, response.Status.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, response.HttpCode);
            Assert.NotNull(response.RawBody);

            FaceEmbeddingModel faceEmbeddingModel = (FaceEmbeddingModel) response.Get();
            Assert.NotNull(faceEmbeddingModel.ModelID);
        }

        [Test]
        [Retry(3)]
        public async Task PredictOnFaceEmbeddingModelShouldBeSuccessful()
        {
            string modelID = Client.PublicModels.FaceEmbeddingModel.ModelID;

            ClarifaiResponse<List<ClarifaiOutput<FaceEmbedding>>> predictResponse =
                await Client.Predict<FaceEmbedding>(
                        modelID,
                        new List<IClarifaiInput> {new ClarifaiURLImage(CELEB1)})
                    .ExecuteAsync();

            Assert.True(predictResponse.IsSuccessful);

            FaceEmbedding faceEmbedding = predictResponse.Get()[0].Data[0];
            Assert.NotNull(faceEmbedding.Crop);
            Assert.NotNull(faceEmbedding.Embeddings[0].Vector);
        }

        [Test]
        [Retry(3)]
        public async Task GetFocusModelShouldBeSuccessful()
        {
            ClarifaiResponse<IModel<Focus>> response =
                await Client.GetModel<Focus>(Client.PublicModels.FocusModel.ModelID)
                    .ExecuteAsync();

            Assert.True(response.IsSuccessful);
            Assert.AreEqual(10000, response.Status.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, response.HttpCode);
            Assert.NotNull(response.RawBody);

            FocusModel focusModel = (FocusModel) response.Get();
            Assert.NotNull(focusModel.ModelID);
        }

        [Test]
        [Retry(3)]
        public async Task PredictOnFocusModelShouldBeSuccessful()
        {
            string modelID = Client.PublicModels.FocusModel.ModelID;

            ClarifaiResponse<List<ClarifaiOutput<Focus>>> predictResponse =
                await Client.Predict<Focus>(
                        modelID,
                        new List<IClarifaiInput> {new ClarifaiURLImage(APPAREL1)})
                    .ExecuteAsync();
            Assert.True(predictResponse.IsSuccessful);
            Focus focus = predictResponse.Get()[0].Data[0];
            Assert.NotNull(focus.Crop);
        }

        [Test]
        [Retry(3)]
        public async Task GetLogoModelShouldBeSuccessful()
        {
            ClarifaiResponse<IModel<Logo>> response =
                await Client.GetModel<Logo>(Client.PublicModels.LogoModel.ModelID)
                    .ExecuteAsync();

            Assert.True(response.IsSuccessful);
            Assert.AreEqual(10000, response.Status.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, response.HttpCode);
            Assert.NotNull(response.RawBody);

            LogoModel logoModel = (LogoModel) response.Get();
            Assert.NotNull(logoModel.ModelID);
            Assert.NotNull(logoModel.OutputInfo.Concepts);
        }

        [Test]
        [Retry(3)]
        public async Task PredictOnLogoModelShouldBeSuccessful()
        {
            string modelID = Client.PublicModels.LogoModel.ModelID;

            ClarifaiResponse<List<ClarifaiOutput<Logo>>> predictResponse =
                await Client.Predict<Logo>(
                        modelID, new List<IClarifaiInput> {new ClarifaiURLImage(APPAREL1)})
                    .ExecuteAsync();
            Assert.True(predictResponse.IsSuccessful);
            Logo logo = predictResponse.Get()[0].Data[0];
            Assert.NotNull(logo.Crop);
            Assert.NotNull(logo.Concepts);
        }

        [Test]
        [Retry(3)]
        public async Task GetVideoModelShouldBeSuccessful()
        {
            ClarifaiResponse<IModel<Frame>> response =
                await Client.GetModel<Frame>(
                        Client.PublicModels.NsfwVideoModel.ModelID)
                    .ExecuteAsync();

            Assert.True(response.IsSuccessful);
            Assert.AreEqual(10000, response.Status.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, response.HttpCode);
            Assert.NotNull(response.RawBody);

            VideoModel videoModel = (VideoModel)response.Get();
            Assert.NotNull(videoModel.ModelID);
            Assert.NotNull(videoModel.OutputInfo.Concepts);
        }

        [Test]
        [Retry(3)]
        public async Task PredictOnVideoModelShouldBeSuccessful()
        {
            string modelID = Client.PublicModels.NsfwVideoModel.ModelID;

            ClarifaiResponse<List<ClarifaiOutput<Frame>>> predictResponse =
                await Client.Predict<Frame>(
                        modelID, new List<IClarifaiInput> {new ClarifaiURLVideo(GIF1)})
                    .ExecuteAsync();

            Assert.True(predictResponse.IsSuccessful);

            Frame video = predictResponse.Get()[0].Data[0];
            Assert.NotNull(video.Concepts);
        }
    }
}
