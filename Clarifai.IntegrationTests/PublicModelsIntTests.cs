using System.Collections.Generic;
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
    public class PublicModelsIntTests : BaseIntTests
    {
        [Test]
        [Retry(3)]
        public async Task AllConceptModelsShouldPredictSuccessfully()
        {
            List<Model> conceptModels = new List<Model>
            {
                Client.PublicModels.ApparelModel,
                Client.PublicModels.FoodModel,
                Client.PublicModels.GeneralModel,
                Client.PublicModels.LandscapeQualityModel,
                Client.PublicModels.ModerationModel,
                Client.PublicModels.NsfwModel,
                Client.PublicModels.PortraitQualityModel,
                Client.PublicModels.TexturesAndPatternsModel,
                Client.PublicModels.TravelModel,
                Client.PublicModels.WeddingModel,
            };
            foreach (Model model in conceptModels)
            {
                ClarifaiResponse<List<ClarifaiOutput<Concept>>> response =
                    await Client.BatchPredict<Concept>(
                            model.ModelID,
                            new List<IClarifaiInput> {new ClarifaiURLImage(CAT1)})
                        .ExecuteAsync();
                Assert.True(response.IsSuccessful, $"BatchPredict on {model.Name} not successful.");
            }
        }

        [Test]
        [Retry(3)]
        public async Task AllVideoModelsShouldPredictSuccessfully()
        {
            List<Model> videoModels = new List<Model>
            {
                Client.PublicModels.ApparelVideoModel,
                Client.PublicModels.FoodVideoModel,
                Client.PublicModels.GeneralVideoModel,
                Client.PublicModels.NsfwVideoModel,
                Client.PublicModels.TravelVideoModel,
                Client.PublicModels.WeddingVideoModel,
            };
            foreach (Model model in videoModels)
            {
                ClarifaiResponse<List<ClarifaiOutput<Frame>>> response =
                    await Client.BatchPredict<Frame>(
                            model.ModelID,
                            new List<IClarifaiInput> {new ClarifaiURLVideo(GIF1)})
                        .ExecuteAsync();
                Assert.True(response.IsSuccessful, $"BatchPredict on {model.Name} not successful.");
            }
        }
    }
}
