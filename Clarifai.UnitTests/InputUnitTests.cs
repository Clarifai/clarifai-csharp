using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clarifai.API;
using Clarifai.API.Requests.Models;
using Clarifai.API.Responses;
using Clarifai.DTOs.Inputs;
using Clarifai.DTOs.Models.Outputs;
using Clarifai.DTOs.Predictions;
using Clarifai.UnitTests.Fakes;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Clarifai.UnitTests
{
    [TestFixture]
    public class InputUnitTests
    {
        [Test]
        public async Task ModifyInputRequestAndResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                patchResponse: @"
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
            ""concepts"": [
                {
                  ""id"": ""@positiveConcept1"",
                  ""name"": ""@positiveConceptName1"",
                  ""value"": 1
                },
                {
                  ""id"": ""@positiveConcept2"",
                  ""value"": 1
                },
                {
                  ""id"": ""@negativeConcept1"",
                  ""name"": ""@negativeConceptName1"",
                  ""value"": 0
                },
                {
                  ""id"": ""@negativeConcept2"",
                  ""value"": 0
                }
            ]
        },
        ""created_at"": ""2017-10-13T20:53:00.253139Z"",
        ""modified_at"": ""2017-10-13T20:53:00.868659782Z"",
        ""status"": {
            ""code"": 30200,
            ""description"": ""Input image modification success""
        }
    }]
}
");
            var client = new ClarifaiClient(httpClient);
            ClarifaiResponse<IClarifaiInput> response = await client.ModifyInput(
                "@inputID", ModifyAction.Merge,
                positiveConcepts: new List<Concept>
                {
                    new Concept("@positiveConcept1", "@positiveConceptName1"),
                    new Concept("@positiveConcept2")
                },
                negativeConcepts: new List<Concept>
                {
                    new Concept("@negativeConcept1", "@negativeConceptName1"),
                    new Concept("@negativeConcept2")
                }
            ).ExecuteAsync();


            var expectedRequestBody = JObject.Parse(@"
{
    ""inputs"": [
      {
        ""id"": ""@inputID"",
        ""data"": {
          ""concepts"": [
            {
              ""id"": ""@positiveConcept1"",
              ""name"": ""@positiveConceptName1"",
              ""value"": true
            },
            {
              ""id"": ""@positiveConcept2"",
              ""value"": true
            },
            {
              ""id"": ""@negativeConcept1"",
              ""name"": ""@negativeConceptName1"",
              ""value"": false
            },
            {
              ""id"": ""@negativeConcept2"",
              ""value"": false
            },
          ]
        }
      }
    ],
    ""action"":""merge""
}
");
            Assert.True(JToken.DeepEquals(expectedRequestBody, httpClient.PatchedBody));

            Assert.True(response.IsSuccessful);
            Assert.AreEqual("Ok", response.Status.Description);

            List<string> posConcepts = response.Get().PositiveConcepts.Select(c => c.ID).ToList();
            List<string> negConcepts = response.Get().NegativeConcepts.Select(c => c.ID).ToList();
            CollectionAssert.AreEqual(
                new List<string>{"@positiveConcept1", "@positiveConcept2"},
                posConcepts);
            CollectionAssert.AreEqual(
                new List<string>{"@negativeConcept1", "@negativeConcept2"},
                negConcepts);
        }

        [Test]
        public async Task ModifyInputMetadataRequestAndResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                patchResponse: @"
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
                ""id"": ""concept1"",
                ""name"": ""concept1"",
                ""value"": 1,
                ""app_id"": ""@appID""
            }],
            ""metadata"": {
                ""@key1"": ""@value1"",
                ""@key2"": ""@value2""
            }
        },
        ""created_at"": ""2017-11-02T15:08:22.005157Z"",
        ""modified_at"": ""2017-11-02T15:08:23.071624222Z"",
        ""status"": {
            ""code"": 30200,
            ""description"": ""Input image modification success""
        }
    }]
}
");
            var client = new ClarifaiClient(httpClient);
            ClarifaiResponse<IClarifaiInput> response = await client.ModifyInputMetadata(
                "@inputID",
                new JObject(
                    new JProperty("@key1", "@value1"),
                    new JProperty("@key2", "@value2"))
                ).ExecuteAsync();


            var expectedRequestBody = JObject.Parse(@"
{
    ""inputs"": [
      {
        ""id"": ""@inputID"",
        ""data"": {
          ""metadata"": {
            ""@key1"": ""@value1"",
            ""@key2"": ""@value2""
          }
        }
      }
    ],
    ""action"":""overwrite""
}
");
            Assert.True(JToken.DeepEquals(expectedRequestBody, httpClient.PatchedBody));

            Assert.True(response.IsSuccessful);
            Assert.AreEqual("Ok", response.Status.Description);

            JObject metadata = response.Get().Metadata;
            Assert.AreEqual(2, metadata.Count);
            Assert.AreEqual("@value1", metadata["@key1"].Value<string>());
            Assert.AreEqual("@value2", metadata["@key2"].Value<string>());
        }

        [Test]
        public async Task DeleteAllInputsResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                deleteResponse: @"{""status"":{""code"":10000,""description"":""Ok""}}");
            var client = new ClarifaiClient(httpClient);
            ClarifaiResponse<EmptyResponse> response = await client.DeleteAllInputs()
                .ExecuteAsync();

            Assert.True(response.IsSuccessful);
            Assert.AreEqual("Ok", response.Status.Description);
        }
    }
}
