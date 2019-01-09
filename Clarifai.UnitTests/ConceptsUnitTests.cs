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
    public class ConceptsUnitTests
    {
        [Test]
        public async Task AddConceptsRequestAndResponseShouldBeCorrect()
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
      ""id"": ""@conceptID1"",
      ""name"": ""@conceptID1"",
      ""value"": 1,
      ""created_at"": ""2019-01-14T16:42:42.210598955Z"",
      ""language"": ""en"",
      ""app_id"": ""c102e505581f49d2956e3caa2e1a0dc9""
    },
    {
      ""id"": ""@conceptID2"",
      ""name"": ""@conceptID2"",
      ""value"": 1,
      ""created_at"": ""2019-01-14T16:42:42.210605836Z"",
      ""language"": ""en"",
      ""app_id"": ""c102e505581f49d2956e3caa2e1a0dc9""
    }
  ]
}
");

            var client = new ClarifaiClient(httpClient);
            var response = await client.AddConcepts(
                    new Concept("@conceptID1"), new Concept("@conceptID2"))
                .ExecuteAsync();

            Assert.AreEqual("/v2/concepts/", httpClient.RequestedUrl);
            var expectedRequestBody = JObject.Parse(@"
{
  ""concepts"": [
    {
      ""id"": ""@conceptID1""
    },
    {
      ""id"": ""@conceptID2""
    }
  ]
}
");

            Assert.True(JToken.DeepEquals(expectedRequestBody, httpClient.PostedBody));

            Assert.True(response.IsSuccessful);

            List<Concept> concepts = response.Get();

            Assert.AreEqual("@conceptID1", concepts[0].ID);
            Assert.AreEqual("@conceptID2", concepts[1].ID);
        }

        [Test]
        public async Task GetConceptResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                getResponse: @"
{
    ""status"": {
        ""code"": 10000,
        ""description"": ""Ok""
    },
    ""concept"": {
        ""id"": ""@conceptID"",
        ""name"": ""@conceptName"",
        ""created_at"": ""2017-10-02T11:34:20.419915Z"",
        ""language"": ""en"",
        ""app_id"": ""@appID""
    }
}
");

            var client = new ClarifaiClient(httpClient);
            var response = await client.GetConcept("@conceptID").ExecuteAsync();

            Assert.AreEqual("/v2/concepts/@conceptID", httpClient.RequestedUrl);

            Assert.True(response.IsSuccessful);

            Concept concept = response.Get();

            Assert.AreEqual("@conceptID", concept.ID);
            Assert.AreEqual("@conceptName", concept.Name);
            Assert.AreEqual("@appID", concept.AppID);
        }

        [Test]
        public async Task GetConceptsResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                getResponse: @"
{
    ""status"": {
        ""code"": 10000,
        ""description"": ""Ok""
    },
    ""concepts"": [{
        ""id"": ""@conceptID1"",
        ""name"": ""@conceptName1"",
        ""created_at"": ""2017-10-15T16:28:28.901994Z"",
        ""language"": ""en"",
        ""app_id"": ""@appID""
    }, {
        ""id"": ""@conceptID2"",
        ""name"": ""@conceptName2"",
        ""created_at"": ""2017-10-15T16:26:46.667104Z"",
        ""language"": ""en"",
        ""app_id"": ""@appID""
    }]
}
");

            var client = new ClarifaiClient(httpClient);
            var response = await client.GetConcepts().ExecuteAsync();

            Assert.True(response.IsSuccessful);

            List<Concept> concepts = response.Get();
            Assert.AreEqual(2, concepts.Count);

            Concept concept1 = concepts[0];
            Concept concept2 = concepts[1];

            Assert.AreEqual("@conceptID1", concept1.ID);
            Assert.AreEqual("@conceptName1", concept1.Name);
            Assert.AreEqual("@appID", concept1.AppID);

            Assert.AreEqual("@conceptID2", concept2.ID);
            Assert.AreEqual("@conceptName2", concept2.Name);
            Assert.AreEqual("@appID", concept2.AppID);
        }

        [Test]
        public async Task ModifyConceptsRequestAndResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                patchResponse: @"
{
  ""status"": {
    ""code"": 10000,
    ""description"": ""Ok""
  },
  ""concepts"": [
    {
      ""id"": ""@conceptID"",
      ""name"": ""@newConceptName"",
      ""value"": 1,
      ""created_at"": ""2019-01-15T14:11:43.864812079Z"",
      ""language"": ""en"",
      ""app_id"": ""@appID""
    }
  ]
}
");

            var client = new ClarifaiClient(httpClient);
            var response = await client.ModifyConcepts(
                    new Concept("@conceptID", name: "@newConceptName"))
                .ExecuteAsync();

            Assert.AreEqual("/v2/concepts/", httpClient.RequestedUrl);
            var expectedRequestBody = JObject.Parse(@"
{
  ""concepts"": [
    {
      ""id"": ""@conceptID"",
      ""name"": ""@newConceptName""
    }
  ],
  ""action"": ""overwrite""
}
");

            Assert.True(JToken.DeepEquals(expectedRequestBody, httpClient.PatchedBody));

            Assert.True(response.IsSuccessful);

            List<Concept> concepts = response.Get();

            Assert.AreEqual("@conceptID", concepts[0].ID);
            Assert.AreEqual("@newConceptName", concepts[0].Name);
        }

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
    }
}
