using System.Collections.Generic;
using System.Threading.Tasks;
using Clarifai.API.Responses;
using Clarifai.DTOs.Models;
using NUnit.Framework;

namespace Clarifai.IntegrationTests
{
    [TestFixture]
    public class SearchModelsIntTests : BaseIntTests
    {
        [Test]
        [Retry(3)]
        public async Task SearchModelsByNameShouldBeSuccessful()
        {
            ClarifaiResponse<List<IModel>> response = await Client.SearchModels("celeb*")
                .ExecuteAsync();

            Assert.NotNull(response.Get());
            Assert.True(response.Get().Count > 0);
        }

        [Test]
        [Retry(3)]
        public async Task SearchModelsByTypeShouldBeSuccessful()
        {
            ClarifaiResponse<List<IModel>> response = await Client.SearchModels("*",
                    ModelType.Focus)
                .ExecuteAsync();

            Assert.NotNull(response.Get());
            Assert.True(response.Get().Count > 0);
        }
    }
}
