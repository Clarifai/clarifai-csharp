using System.Collections.Generic;
using System.Threading.Tasks;
using Clarifai.API.Responses;
using Clarifai.DTOs.Predictions;
using NUnit.Framework;

namespace Clarifai.IntegrationTests
{
    [TestFixture]
    public class SearchConceptsIntTests : BaseIntTests
    {
        [Test]
        [Retry(3)]
        public async Task SearchConceptsShouldBeSuccessful()
        {
            ClarifaiResponse<List<Concept>> response = await Client.SearchConcepts("conc*")
                .ExecuteAsync();
            AssertResponseSuccess(response);
            Assert.IsNotNull(response.Get());
        }

        [Test]
        [Retry(3)]
        public async Task SearchConceptsWithLanguageShouldBeSuccessful()
        {
            ClarifaiResponse<List<Concept>> response = await Client.SearchConcepts("狗*",
                    language: "zh") // "zh" = Chinese
                .ExecuteAsync();
            AssertResponseSuccess(response);
            Assert.IsNotNull(response.Get());
        }
    }
}