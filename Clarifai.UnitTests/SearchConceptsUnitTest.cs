using System.Collections.Generic;
using System.Threading.Tasks;
using Clarifai.API;
using Clarifai.API.Responses;
using Clarifai.DTOs.Predictions;
using Clarifai.UnitTests.Fakes;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.Diagnostics;
using System;

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
    ""concepts"": [{
        ""id"": ""@positiveConcept1"",
        ""name"": ""@positiveConceptName1"",
        ""created_at"": ""2017-10-15T16:28:28.901994Z"",
        ""language"": ""en"",
        ""app_id"": ""@appID""
    }, {
        ""id"": ""@positiveConcept2"",
        ""created_at"": ""2017-10-15T16:26:46.667104Z"",
        ""language"": ""en"",
        ""app_id"": ""@appID""
    }]
}
");

            var client = new ClarifaiClient(httpClient);
            var response = await client.SearchConcepts("positives","en").ExecuteAsync();

            var expectedRequestBody = JObject.Parse(@"
{
    ""concept_query"": {
      ""name"": ""positives"",
      ""language"": ""en""
    }
}
");
            Assert.True(JToken.DeepEquals(expectedRequestBody, httpClient.PostedBody));

            Assert.True(response.IsSuccessful);

            Assert.AreEqual(response.Get()[0].ID,"@positiveConcept1");
            Assert.AreEqual(response.Get()[0].Name,"@positiveConceptName1");
            Assert.AreEqual(response.Get()[1].ID,"@positiveConcept2");
        }
    }
}