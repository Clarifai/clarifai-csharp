using System.Threading.Tasks;
using Clarifai.API;
using Clarifai.API.Responses;
using Clarifai.DTOs.Models;
using Clarifai.UnitTests.Fakes;
using NUnit.Framework;

namespace Clarifai.UnitTests
{
    [TestFixture]
    public class ModelEvaluationUnitTests
    {
        [Test]
        public async Task ModelEvaluationResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                postResponse: @"
{
  ""status"": {
    ""code"": 10000,
    ""description"": ""Ok""
  },
  ""model_version"": {
    ""id"": ""@modelVersionID"",
    ""created_at"": ""2017-01-01T00:00:00.000000Z"",
    ""status"": {
      ""code"": 21100,
      ""description"": ""Model trained successfully""
    },
    ""active_concept_count"": 2,
    ""metrics"": {
      ""status"": {
        ""code"": 21303,
        ""description"": ""Model is queued for evaluation.""
      }
    },
    ""total_input_count"": 30
  }
}
");
            var client = new ClarifaiClient(httpClient);
            ClarifaiResponse<ModelVersion> response = await client.ModelEvaluation(
                "@modelID", "@modelVersionID").ExecuteAsync();

            Assert.AreEqual(
                "/v2/models/@modelID/versions/@modelVersionID/metrics", httpClient.RequestedUrl);

            Assert.True(response.IsSuccessful);
            Assert.AreEqual("Ok", response.Status.Description);

            ModelVersion modelVersion = response.Get();
            Assert.AreEqual("@modelVersionID", modelVersion.ID);
            Assert.AreEqual(21100, modelVersion.Status.StatusCode);
            Assert.AreEqual("Model trained successfully", modelVersion.Status.Description);
            Assert.AreEqual(21303, modelVersion.ModelMetricsStatus.StatusCode);
            Assert.AreEqual("Model is queued for evaluation.",
                modelVersion.ModelMetricsStatus.Description);
            Assert.AreEqual(2, modelVersion.ActiveConceptCount);
            Assert.AreEqual(30, modelVersion.TotalInputCount);
        }
    }
}
