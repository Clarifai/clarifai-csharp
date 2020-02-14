using System.Collections.Generic;
using System.Threading.Tasks;
using Clarifai.API;
using Clarifai.DTOs.Models;
using Clarifai.UnitTests.Fakes;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Clarifai.UnitTests
{
    [TestFixture]
    public class SearchModelsUnitTests
    {
        [Test]
        public async Task SearchModelsByNameRequestAndResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                postResponse: @"
{
    ""status"": {
        ""code"": 10000,
        ""description"": ""Ok""
    },
    ""models"": [{
        ""id"": ""@modelID"",
        ""name"": ""moderation"",
        ""created_at"": ""2016-10-25T19:30:38.541073Z"",
        ""app_id"": ""main"",
        ""output_info"": {
            ""message"": ""Show output_info with: GET /models/{model_id}/output_info"",
            ""type"": ""concept"",
            ""type_ext"": ""concept""
        },
        ""model_version"": {
            ""id"": ""@modelVersionID"",
            ""created_at"": ""2016-10-25T19:30:38.541073Z"",
            ""status"": {
                ""code"": 21100,
                ""description"": ""Model trained successfully""
            },
            ""active_concept_count"": 10554
        },
        ""display_name"": ""Moderation""
    }]
}
");

            var client = new ClarifaiClient(httpClient);
            var response = await client.SearchModels("moder*").ExecuteAsync();

            var expectedRequestBody = JObject.Parse(@"
{
    ""model_query"": {
      ""name"": ""moder*""
    }
}
");
            Assert.True(JToken.DeepEquals(expectedRequestBody, httpClient.PostedBody));

            List<IModel> models = response.Get();

            Assert.True(response.IsSuccessful);
            Assert.AreEqual(1, models.Count);
            Assert.AreEqual("@modelID", models[0].ModelID);
            Assert.AreEqual("@modelVersionID", models[0].ModelVersion.ID);
        }

        [Test]
        public async Task SearchModelsByNameAndTypeRequestAndResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                postResponse: @"
{
    ""status"": {
        ""code"": 10000,
        ""description"": ""Ok""
    },
    ""models"": [{
        ""id"": ""@modelID"",
        ""name"": ""color"",
        ""created_at"": ""2017-03-06T22:57:00.660603Z"",
        ""app_id"": ""main"",
        ""output_info"": {
            ""message"": ""Show output_info with: GET /models/{model_id}/output_info"",
            ""type"": ""color"",
            ""type_ext"": ""color""
        },
        ""model_version"": {
            ""id"": ""@modelVersionID"",
            ""created_at"": ""2017-03-06T22:57:00.684652Z"",
            ""status"": {
                ""code"": 21100,
                ""description"": ""Model trained successfully""
            }
        },
        ""display_name"": ""Color""
    }]
}
");

            var client = new ClarifaiClient(httpClient);
            var response = await client.SearchModels("*", ModelType.Color).ExecuteAsync();


            var expectedRequestBody = JObject.Parse(@"
{
    ""model_query"": {
      ""name"": ""*"",
      ""type"": ""color""
    }
}
");
            Assert.True(JToken.DeepEquals(expectedRequestBody, httpClient.PostedBody));

            List<IModel> models = response.Get();

            Assert.True(response.IsSuccessful);
            Assert.AreEqual(1, models.Count);
            Assert.AreEqual("@modelID", models[0].ModelID);
            Assert.AreEqual("@modelVersionID", models[0].ModelVersion.ID);
        }
    }
}
