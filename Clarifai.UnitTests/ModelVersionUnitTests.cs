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
        public async Task GetModelVersionShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                getResponse: @"
{
  ""status"": {
    ""code"": 10000,
    ""description"": ""Ok""
  },
  ""model_version"": {
    ""id"": ""@modelVersionID"",
    ""created_at"": ""2017-10-31T16:30:31.226185Z"",
    ""status"": {
      ""code"": 21100,
      ""description"": ""Model trained successfully""
    },
    ""active_concept_count"": 5,
    ""train_stats"": {}
  }
}
");
            var client = new ClarifaiClient(httpClient);
            ClarifaiResponse<ModelVersion> response = await client.GetModelVersion(
                    "@modelID", "@modelVersionID")
                .ExecuteAsync();

            Assert.AreEqual(
                "/v2/models/@modelID/versions/@modelVersionID", httpClient.RequestedUrl);

            Assert.True(response.IsSuccessful);
            Assert.AreEqual("Ok", response.Status.Description);

            ModelVersion modelVersion = response.Get();
            Assert.AreEqual("@modelVersionID", modelVersion.ID);
            Assert.AreEqual(5, modelVersion.ActiveConceptCount);
        }

        [Test]
        public async Task GetModelVersionsShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                getResponse: @"
{
  ""status"": {
    ""code"": 10000,
    ""description"": ""Ok""
  },
  ""model_versions"": [
    {
      ""id"": ""@modelVersionID1"",
      ""created_at"": ""2017-10-31T16:30:31.226185Z"",
      ""status"": {
        ""code"": 21100,
        ""description"": ""Model trained successfully""
      },
      ""active_concept_count"": 5,
      ""train_stats"": {}
    },
    {
      ""id"": ""@modelVersionID2"",
      ""created_at"": ""2017-05-16T19:20:38.733764Z"",
      ""status"": {
        ""code"": 21100,
        ""description"": ""Model trained successfully""
      },
      ""active_concept_count"": 5,
      ""train_stats"": {}
    }
  ]
}
");
            var client = new ClarifaiClient(httpClient);
            ClarifaiResponse<List<ModelVersion>> response = await client.GetModelVersions(
                    "@modelID")
                .ExecuteAsync();

            Assert.AreEqual("/v2/models/@modelID/versions", httpClient.RequestedUrl);

            Assert.True(response.IsSuccessful);
            Assert.AreEqual("Ok", response.Status.Description);

            List<ModelVersion> modelVersions = response.Get();
            Assert.AreEqual(2, modelVersions.Count);

            ModelVersion modelVersion1 = modelVersions[0];
            Assert.AreEqual("@modelVersionID1", modelVersion1.ID);
            Assert.AreEqual(5, modelVersion1.ActiveConceptCount);

            ModelVersion modelVersion2 = modelVersions[1];
            Assert.AreEqual("@modelVersionID2", modelVersion2.ID);
            Assert.AreEqual(5, modelVersion2.ActiveConceptCount);
        }

        [Test]
        public async Task DeleteModelVersionShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                deleteResponse: @"{""status"":{""code"":10000,""description"":""Ok""}}");
            var client = new ClarifaiClient(httpClient);
            ClarifaiResponse<EmptyResponse> response = await client.DeleteModelVersion(
                    "@modelID", "@versionID")
                .ExecuteAsync();

            Assert.AreEqual("/v2/models/@modelID/versions/@versionID", httpClient.RequestedUrl);

            Assert.True(response.IsSuccessful);
            Assert.AreEqual("Ok", response.Status.Description);
        }
    }
}
