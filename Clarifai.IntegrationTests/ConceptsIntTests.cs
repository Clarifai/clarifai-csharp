using System.Collections.Generic;
using System.Threading.Tasks;
using Clarifai.API.Responses;
using Clarifai.DTOs.Predictions;
using NUnit.Framework;

namespace Clarifai.IntegrationTests
{
    [TestFixture]
    public class ConceptsIntTests : BaseIntTests
    {
        [Test]
        [Retry(3)]
        public async Task ModifyConceptRequestShouldUpdateConceptName()
        {
            string conceptID = "someConceptID";
            string originalConceptName = "originalConceptName";
            string newConceptName = "newConceptName";

            try
            {
                // Create a concept. It's fine if it already exists.
                await Client.AddConcepts(new Concept(conceptID, originalConceptName))
                    .ExecuteAsync();

                // Modify the concept.
                ClarifaiResponse<List<Concept>> modifyResponse = await Client.ModifyConcepts(
                        new Concept(conceptID, newConceptName))
                    .ExecuteAsync();

                Assert.True(modifyResponse.IsSuccessful);
                Assert.AreEqual(newConceptName, modifyResponse.Get()[0].Name);

                // Get the modified concept again to make sure the name was really updated.
                ClarifaiResponse<Concept> getResponse = await Client.GetConcept(conceptID)
                    .ExecuteAsync();
                Assert.AreEqual(newConceptName, getResponse.Get().Name);
            }
            finally
            {
                // Revert the concept's name back to the original name.
                await Client.ModifyConcepts(new Concept(conceptID, originalConceptName))
                    .ExecuteAsync();
            }
        }

        [Test]
        [Retry(3)]
        public async Task GetConceptsShouldBeSuccessful()
        {
            ClarifaiResponse<List<Concept>> response = await Client.GetConcepts().ExecuteAsync();
            Assert.True(response.IsSuccessful);
            Assert.NotNull(response.Get());
        }

        [Test]
        [Retry(3)]
        public async Task SearchConceptsShouldBeSuccessful()
        {
            ClarifaiResponse<List<Concept>> response = await Client.SearchConcepts("conc*")
                .ExecuteAsync();
            Assert.True(response.IsSuccessful);
            Assert.IsNotNull(response.Get());
        }

        [Test]
        [Retry(3)]
        public async Task SearchConceptsWithLanguageShouldBeSuccessful()
        {
            ClarifaiResponse<List<Concept>> response = await Client.SearchConcepts("狗*",
                    language: "zh") // "zh" = Chinese
                .ExecuteAsync();
            Assert.True(response.IsSuccessful);
            Assert.IsNotNull(response.Get());
        }
    }
}
