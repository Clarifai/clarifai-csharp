using System.Collections.Generic;
using System.Threading.Tasks;
using Clarifai.API;
using Clarifai.API.Responses;
using Clarifai.DTOs.Models;
using Clarifai.DTOs.Models.Outputs;
using Clarifai.UnitTests.Fakes;
using NUnit.Framework;

namespace Clarifai.UnitTests
{
    [TestFixture]
    public class ModelVersionUnitTests
    {
        [Test]
        public async Task DeleteModelVersionShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                deleteResponse: @"{""status"":{""code"":10000,""description"":""Ok""}}");
            var client = new ClarifaiClient(httpClient);
            ClarifaiResponse<EmptyResponse> response = await client.DeleteModelVersion(
                    "@modelID", "@versionID")
                .ExecuteAsync();

            Assert.True(response.IsSuccessful);
            Assert.AreEqual("Ok", response.Status.Description);
        }

        [Test]
        public async Task GetModelVersionResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                getResponse: @"
{
    ""status"": {
        ""code"": 10000,
        ""description"": ""Ok""
    },
    ""model_version"": {
        ""id"": ""@versionID"",
        ""created_at"": ""2017-01-01T00:00:00.000000Z"",
        ""status"": {
            ""code"": 21100,
            ""description"": ""Model trained successfully""
        },
        ""active_concept_count"": 2,
        ""metrics"": {
            ""status"": {
                ""code"": 21303,
                ""description"": ""@modelDESCRIPTION""
            }
        },
        ""total_input_count"": 30
    }
}
");
            var client = new ClarifaiClient(httpClient);
            ClarifaiResponse<ModelVersion> response = await client.GetModelVersion("@modelID","@versionID").ExecuteAsync();

            Assert.True(response.IsSuccessful);
            Assert.AreEqual(response.Get().ID, "@versionID");
            Assert.AreEqual(response.Get().ActiveConceptCount, 2);
            Assert.AreEqual(response.Get().ModelMetricsStatus.Description, "@modelDESCRIPTION");
        }


        [Test]
        public async Task GetModelVersionsResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                getResponse: @"
{
    ""status"": {
        ""code"": 10000,
        ""description"": ""Ok""
    },
    ""model_versions"": [{
        ""id"": ""@versionID1"",
        ""created_at"": ""2017-01-01T00:00:00.000000Z"",
        ""status"": {
            ""code"": 21100,
            ""description"": ""Model trained successfully""
        },
        ""active_concept_count"": 3,
        ""metrics"": {
            ""status"": {
                ""code"": 21303,
                ""description"": ""@modelDESCRIPTION""
            }
        },
        ""total_input_count"": 30
    },
    {
        ""id"": ""@versionID2"",
        ""created_at"": ""2017-01-01T00:00:00.000000Z"",
        ""status"": {
            ""code"": 21100,
            ""description"": ""Model trained successfully""
        },
        ""active_concept_count"": 3,
        ""metrics"": {
            ""status"": {
                ""code"": 21303,
                ""description"": ""@modelDESCRIPTION""
            }
        },
        ""total_input_count"": 30  
    }]
}
");
            var client = new ClarifaiClient(httpClient);
            ClarifaiResponse<List<ModelVersion>> response = await client.GetModelVersions("@modelID").ExecuteAsync();
            List<ModelVersion> modelVersions = response.Get();

            Assert.True(response.IsSuccessful);
            Assert.AreEqual(modelVersions[0].ID, "@versionID1");
            Assert.AreEqual(modelVersions[0].ActiveConceptCount, 3);
            Assert.AreEqual(modelVersions[0].TotalInputCount, 30);
            Assert.AreEqual(modelVersions[1].ID, "@versionID2");
            Assert.AreEqual(modelVersions[0].ModelMetricsStatus.Description, "@modelDESCRIPTION");
            Assert.AreEqual(modelVersions[0].ModelMetricsStatus.StatusCode, 21303);
        }
    }
}
