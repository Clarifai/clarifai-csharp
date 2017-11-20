using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clarifai.API;
using Clarifai.API.Responses;
using Clarifai.DTOs.Inputs;
using Clarifai.DTOs.Models;
using Clarifai.DTOs.Models.Outputs;
using Clarifai.DTOs.Predictions;
using Clarifai.UnitTests.Fakes;
using NUnit.Framework;

namespace Clarifai.UnitTests
{
    [TestFixture]
    public class ModelUnitTests
    {
        [Test]
        public async Task GetModelResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                getResponse: @"
{
    ""status"": {

        ""code"": 10000,
        ""description"": ""Ok""

    },
    ""model"": {
        ""id"": ""some-model-id"",
        ""name"": ""some-model-name"",
        ""created_at"": ""2017-05-16T19:20:38.733764Z"",
        ""app_id"": ""main"",
        ""output_info"": {
            ""data"": {
                ""concepts"": [{
                    ""id"": ""some-concept-id"",
                    ""name"": ""safe"",
                    ""created_at"": ""2017-05-16T19:20:38.450157Z"",
                    ""language"": ""en"",
                    ""app_id"": ""main""
                }]
            },
            ""type"": ""concept"",
            ""type_ext"": ""concept""
        },
        ""model_version"": {
            ""id"": ""some-model-version-id"",
            ""created_at"": ""2017-05-16T19:20:38.733764Z"",
            ""status"": {
                ""code"": 21100,
                ""description"": ""Model trained successfully""
            },
            ""active_concept_count"": 5
        },
        ""display_name"": ""Moderation""
    }
}
");

            var client = new ClarifaiClient(httpClient);
            var response = await client.GetModel<Concept>("").ExecuteAsync();
            var model = (ConceptModel)response.Get();

            Assert.True(response.IsSuccessful);
            Assert.AreEqual("some-model-id", model.ModelID);
            Assert.AreEqual("some-model-name", model.Name);
            Assert.AreEqual("some-concept-id", model.OutputInfo.Concepts.First().ID);
            Assert.AreEqual("some-model-version-id", model.ModelVersion.ID);
        }

        [Test]
        public async Task GetModelInputsResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                getResponse: @"
{
    ""status"": {
        ""code"": 10000,
        ""description"": ""Ok""
    },
    ""inputs"": [{
        ""id"": ""@inputID"",
        ""data"": {
            ""image"": {
                ""url"": ""@imageURL""
            },
            ""concepts"": [{
                ""id"": ""@conceptID"",
                ""name"": ""@conceptName"",
                ""value"": 1,
                ""app_id"": ""@conceptAppID""
            }]
        },
        ""created_at"": ""2017-10-15T16:30:52.964888Z"",
        ""status"": {
            ""code"": 30000,
            ""description"": ""Download complete""
        }
    }]
}
");
            var client = new ClarifaiClient(httpClient);
            ClarifaiResponse<List<IClarifaiInput>> response = await client.GetModelInputs(
                "", "").ExecuteAsync();


            Assert.True(response.IsSuccessful);
            Assert.AreEqual("Ok", response.Status.Description);

            IClarifaiInput input = response.Get()[0];
            Assert.AreEqual("@inputID", input.ID);
            Assert.AreEqual("@conceptID", input.PositiveConcepts.First().ID);
        }

        [Test]
        public async Task DeleteAllModelsResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                deleteResponse: @"{""status"":{""code"":10000,""description"":""Ok""}}");
            var client = new ClarifaiClient(httpClient);
            ClarifaiResponse<EmptyResponse> response = await client.DeleteAllModels()
                .ExecuteAsync();

            Assert.True(response.IsSuccessful);
            Assert.AreEqual("Ok", response.Status.Description);
        }
    }
}
