using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Clarifai.API;
using Clarifai.DTOs.Predictions;
using Clarifai.UnitTests.Fakes;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Clarifai.UnitTests
{
    [TestFixture]
    public class SearchConceptsUnitTests
    {
        [Test]
        public async Task SearchConceptsRequestAndResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                postResponse: @"
{
  ""status"": {
    ""code"": 10000,
    ""description"": ""Ok""
  },
  ""concepts"": [
    {
      ""id"": ""ai_BhC2K0NW"",
      ""name"": ""concealer"",
      ""value"": 1,
      ""created_at"": ""2016-03-17T11:43:01.223962Z"",
      ""language"": ""en"",
      ""app_id"": ""main"",
      ""definition"": ""concealer""
    },
    {
      ""id"": ""ai_4PSs2Vfk"",
      ""name"": ""concentrate"",
      ""value"": 1,
      ""created_at"": ""2016-03-17T11:43:01.223962Z"",
      ""language"": ""en"",
      ""app_id"": ""main"",
      ""definition"": ""direct one's attention on something""
    }
  ]
}
");

            var client = new ClarifaiClient(httpClient);
            var response = await client.SearchConcepts("conc*")
                .ExecuteAsync();

            Assert.AreEqual("/v2/concepts/searches/", httpClient.RequestedUrl);
            var expectedRequestBody = JObject.Parse(@"
{
  ""concept_query"": {
    ""name"": ""conc*""
  }
}
");

            Assert.True(JToken.DeepEquals(expectedRequestBody, httpClient.PostedBody));

            Assert.True(response.IsSuccessful);

            List<Concept> concepts = response.Get();

            Assert.AreEqual("ai_BhC2K0NW", concepts[0].ID);
            Assert.AreEqual("concealer", concepts[0].Name);

            Assert.AreEqual("ai_4PSs2Vfk", concepts[1].ID);
            Assert.AreEqual("concentrate", concepts[1].Name);
        }

        [Test]
        public async Task SearchConceptsWithLanguageRequestAndResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                postResponse: @"
{
  ""status"": {
    ""code"": 10000,
    ""description"": ""Ok""
  },
  ""concepts"": [
    {
      ""id"": ""ai_8S2Vq3cR"",
      ""name"": ""狗"",
      ""value"": 1,
      ""created_at"": ""2016-03-17T11:43:01.223962Z"",
      ""language"": ""zh"",
      ""app_id"": ""main""
    },
    {
      ""id"": ""ai_JnCvnGd0"",
      ""name"": ""狗仔队"",
      ""value"": 1,
      ""created_at"": ""2016-03-17T11:43:01.223962Z"",
      ""language"": ""zh"",
      ""app_id"": ""main""
    },
    {
      ""id"": ""ai_5xTpcQTX"",
      ""name"": ""狗窝"",
      ""value"": 1,
      ""created_at"": ""2016-03-17T11:43:01.223962Z"",
      ""language"": ""zh"",
      ""app_id"": ""main""
    }
  ]
}
");

            var client = new ClarifaiClient(httpClient);
            var response = await client.SearchConcepts("狗*", language: "zh")
                .ExecuteAsync();

            Assert.AreEqual("/v2/concepts/searches/", httpClient.RequestedUrl);
            var expectedRequestBody = JObject.Parse(@"
{
  ""concept_query"": {
    ""name"": ""狗*"",
    ""language"": ""zh""
  }
}
");

            Assert.True(JToken.DeepEquals(expectedRequestBody, httpClient.PostedBody));

            Assert.True(response.IsSuccessful);

            List<Concept> concepts = response.Get();

            Assert.AreEqual("ai_8S2Vq3cR", concepts[0].ID);
            Assert.AreEqual("狗", concepts[0].Name);

            Assert.AreEqual("ai_JnCvnGd0", concepts[1].ID);
            Assert.AreEqual("狗仔队", concepts[1].Name);

            Assert.AreEqual("ai_5xTpcQTX", concepts[2].ID);
            Assert.AreEqual("狗窝", concepts[2].Name);
        }
    }
}
