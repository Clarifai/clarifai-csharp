using System.Collections.Generic;
using System.Threading.Tasks;
using Clarifai.API;
using Clarifai.DTOs.Predictions;
using Clarifai.UnitTests.Fakes;
using NUnit.Framework;

namespace Clarifai.UnitTests
{
    [TestFixture]
    public class ConceptsUnitTests
    {
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
            var response = await client.GetConcept("").ExecuteAsync();

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
    }
}
