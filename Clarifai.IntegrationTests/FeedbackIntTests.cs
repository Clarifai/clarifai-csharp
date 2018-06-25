using System.Collections.Generic;
using System.Threading.Tasks;
using Clarifai.API.Responses;
using Clarifai.DTOs;
using Clarifai.DTOs.Feedbacks;
using Clarifai.DTOs.Models.Outputs;
using NUnit.Framework;

namespace Clarifai.IntegrationTests
{
    [TestFixture]
    public class FeedbackIntTests : BaseIntTests
    {
        [Test]
        [Retry(3)]
        public async Task ImageModelFeedbackWithConceptsShouldBeSuccessful()
        {
            ClarifaiResponse<EmptyResponse> response = await Client.ModelFeedback(
                modelID: Client.PublicModels.GeneralModel.ModelID,
                imageUrl: CAT1,
                inputID: "@inputID",
                outputID: "@outputID",
                endUserID: "@endUserID",
                sessionID: "@sessionID",
                concepts: new List<ConceptFeedback>
                {
                    new ConceptFeedback("cat", true), new ConceptFeedback("dog", false)
                }
            ).ExecuteAsync();

            Assert.True(response.IsSuccessful);
        }

        [Test]
        [Retry(3)]
        public async Task ImageModelFeedbackWithRegionsShouldBeSuccessful()
        {
            ClarifaiResponse<EmptyResponse> response = await Client.ModelFeedback(
                modelID: Client.PublicModels.CelebrityModel.ModelID,
                imageUrl: CELEB1,
                inputID: "@inputID",
                outputID: "@outputID",
                endUserID: "@endUserID",
                sessionID: "@sessionID",
                concepts: new List<ConceptFeedback>
                {
                    new ConceptFeedback("freeman", true), new ConceptFeedback("eminem", false)
                },
                regions: new List<RegionFeedback>
                {
                    new RegionFeedback(
                        new Crop(0.1m, 0.1m, 0.2m, 0.2m),
                        Feedback.NotDetected,
                        new List<ConceptFeedback>
                        {
                            new ConceptFeedback("freeman", true),
                            new ConceptFeedback("eminem", false)
                        })
                }
            ).ExecuteAsync();

            Assert.True(response.IsSuccessful);
        }

        [Test]
        [Retry(3)]
        public async Task SearchesFeedbackShouldBeSuccessful()
        {
            ClarifaiResponse<EmptyResponse> response = await Client.SearchesFeedback("@inputID",
                    "@searchID", "@endUserID", "@sessionID")
                .ExecuteAsync();

            Assert.True(response.IsSuccessful);
        }
    }
}
