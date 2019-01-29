using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clarifai.API;
using Clarifai.API.Requests.Models;
using Clarifai.API.Responses;
using Clarifai.DTOs;
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
        public async Task AddInputsRequestAndResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                postResponse: @"
{
  ""status"": {
    ""code"": 10000,
    ""description"": ""Ok""
  },
  ""inputs"": [
    {
      ""id"": ""@inputID1"",
      ""data"": {
        ""image"": {
          ""url"": ""https://some.image.url1""
        },
        ""geo"": {
          ""geo_point"": {
            ""longitude"": 55,
            ""latitude"": 66
          }
        }
      },
      ""created_at"": ""2019-01-17T12:43:04.895006174Z"",
      ""modified_at"": ""2019-01-17T12:43:04.895006174Z"",
      ""status"": {
        ""code"": 30001,
        ""description"": ""Download pending""
      }
    },
    {
      ""id"": ""@inputID2"",
      ""data"": {
        ""image"": {
          ""url"": ""https://some.image.url2""
        }
      },
      ""created_at"": ""2019-01-17T12:43:04.895006174Z"",
      ""modified_at"": ""2019-01-17T12:43:04.895006174Z"",
      ""status"": {
        ""code"": 30001,
        ""description"": ""Download pending""
      }
    }
  ]
}
");
            var client = new ClarifaiClient(httpClient);
            ClarifaiResponse<List<IClarifaiInput>> response = await client.AddInputs(
                    new ClarifaiURLImage(
                        "https://some.image.url1",
                        id: "@inputID1",
                        allowDuplicateUrl: true,
                        geo: new GeoPoint(55, 66)),
                    new ClarifaiURLImage(
                        "https://some.image.url2",
                        id: "@inputID2",
                        allowDuplicateUrl: true))
                    .ExecuteAsync();


            var expectedRequestBody = JObject.Parse(@"
{
  ""inputs"": [
    {
      ""id"": ""@inputID1"",
      ""data"": {
        ""image"": {
          ""url"": ""https://some.image.url1"",
          ""allow_duplicate_url"": true
        },
        ""geo"": {
          ""geo_point"": {
            ""longitude"": 55,
            ""latitude"": 66
          }
        }
      }
    },
    {
      ""id"": ""@inputID2"",
      ""data"": {
        ""image"": {
          ""url"": ""https://some.image.url2"",
          ""allow_duplicate_url"": true
        }
      }
    }
  ]
}
");
            Assert.True(JToken.DeepEquals(expectedRequestBody, httpClient.PostedBody));

            Assert.True(response.IsSuccessful);
            Assert.AreEqual("Ok", response.Status.Description);

            ClarifaiURLImage input1 = (ClarifaiURLImage) response.Get()[0];
            ClarifaiURLImage input2 = (ClarifaiURLImage) response.Get()[1];

            Assert.AreEqual("@inputID1", input1.ID);
            Assert.AreEqual("https://some.image.url1", input1.URL);
            Assert.AreEqual(new GeoPoint(55, 66), input1.Geo);

            Assert.AreEqual("@inputID2", input2.ID);
            Assert.AreEqual("https://some.image.url2", input2.URL);
            Assert.IsNull(input2.Geo);
        }

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
              ""name"": ""@positiveConceptName1""
            },
            {
              ""id"": ""@positiveConcept2""
            },
            {
              ""id"": ""@negativeConcept1"",
              ""name"": ""@negativeConceptName1"",
              ""value"": 0
            },
            {
              ""id"": ""@negativeConcept2"",
              ""value"": 0
            },
          ]
        }
      }
    ],
    ""action"":""merge""
}
");
            Console.WriteLine(httpClient.PatchedBody);
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
        public async Task GetInputsRequestAndResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                getResponse: @"
{
  ""status"": {
    ""code"": 10000,
    ""description"": ""Ok""
  },
  ""inputs"": [
    {
      ""id"": ""@inputID1"",
      ""data"": {
        ""image"": {
          ""url"": ""https://some.image.url1""
        },
        ""geo"": {
          ""geo_point"": {
            ""longitude"": 55,
            ""latitude"": 66
          }
        }
      },
      ""created_at"": ""2019-01-17T14:02:21.216473Z"",
      ""modified_at"": ""2019-01-17T14:02:21.800792Z"",
      ""status"": {
        ""code"": 30000,
        ""description"": ""Download complete""
      }
    },
    {
      ""id"": ""@inputID2"",
      ""data"": {
        ""image"": {
          ""url"": ""https://some.image.url2""
        }
      },
      ""created_at"": ""2019-01-17T14:02:21.216473Z"",
      ""modified_at"": ""2019-01-17T14:02:21.800792Z"",
      ""status"": {
        ""code"": 30000,
        ""description"": ""Download complete""
      }
    }
  ]
}
");
            var client = new ClarifaiClient(httpClient);
            ClarifaiResponse<List<IClarifaiInput>> response = await client.GetInputs()
                .ExecuteAsync();

            Assert.AreEqual("/v2/inputs", httpClient.RequestedUrl);

            Assert.True(response.IsSuccessful);
            Assert.AreEqual("Ok", response.Status.Description);

            ClarifaiURLImage input1 = (ClarifaiURLImage) response.Get()[0];
            ClarifaiURLImage input2 = (ClarifaiURLImage) response.Get()[1];

            Assert.AreEqual("@inputID1", input1.ID);
            Assert.AreEqual("https://some.image.url1", input1.URL);
            Assert.AreEqual(new GeoPoint(55, 66), input1.Geo);

            Assert.AreEqual("@inputID2", input2.ID);
            Assert.AreEqual("https://some.image.url2", input2.URL);
            Assert.IsNull(input2.Geo);
        }

        [Test]
        public async Task GetInputRequestAndResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                getResponse: @"
{
  ""status"": {
    ""code"": 10000,
    ""description"": ""Ok""
  },
  ""input"": {
    ""id"": ""@inputID"",
    ""data"": {
      ""image"": {
        ""url"": ""https://some.image.url""
      },
      ""geo"": {
        ""geo_point"": {
          ""longitude"": 55,
          ""latitude"": 66
        }
      }
    },
    ""created_at"": ""2019-01-17T14:02:21.216473Z"",
    ""modified_at"": ""2019-01-17T14:02:21.800792Z"",
    ""status"": {
      ""code"": 30000,
      ""description"": ""Download complete""
    }
  }
}
");
            var client = new ClarifaiClient(httpClient);
            ClarifaiResponse<IClarifaiInput> response = await client.GetInput("@inputID")
                .ExecuteAsync();

            Assert.AreEqual("/v2/inputs/@inputID", httpClient.RequestedUrl);

            Assert.True(response.IsSuccessful);
            Assert.AreEqual("Ok", response.Status.Description);

            ClarifaiURLImage input = (ClarifaiURLImage) response.Get();

            Assert.AreEqual("@inputID", input.ID);
            Assert.AreEqual("https://some.image.url", input.URL);
            Assert.AreEqual(new GeoPoint(55, 66), input.Geo);
        }

        [Test]
        public async Task GetInputsStatusRequestAndResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                getResponse: @"
{
  ""status"": {
    ""code"": 10000,
    ""description"": ""Ok""
  },
  ""counts"": {
    ""processed"": 1,
    ""to_process"": 2,
    ""errors"": 3,
    ""processing"": 4,
    ""reindexed"": 5,
    ""to_reindex"": 6,
    ""reindex_errors"": 7,
    ""reindexing"": 8
  }
}
");
            var client = new ClarifaiClient(httpClient);
            ClarifaiResponse<ClarifaiInputsStatus> response = await client.GetInputsStatus()
                .ExecuteAsync();

            Assert.AreEqual("/v2/inputs/status", httpClient.RequestedUrl);

            Assert.True(response.IsSuccessful);
            Assert.AreEqual("Ok", response.Status.Description);

            ClarifaiInputsStatus inputsStatus = response.Get();

            Assert.AreEqual(1, inputsStatus.Processed);
            Assert.AreEqual(2, inputsStatus.ToProcess);
            Assert.AreEqual(3, inputsStatus.Errors);
            Assert.AreEqual(4, inputsStatus.Processing);
            // TODO(Rok) MEDIUM: Expose the other fields.
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

        [Test]
        public async Task DeleteInputsResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                deleteResponse: @"{""status"":{""code"":10000,""description"":""Ok""}}");

            var client = new ClarifaiClient(httpClient);
            ClarifaiResponse<EmptyResponse> response = await client.DeleteInputs(
                    "@inputID1", "@inputID2")
                .ExecuteAsync();

            var expectedRequestBody = JObject.Parse(@"
{
  ""ids"": [
    ""@inputID1"",
    ""@inputID2""
  ]
}
");
            Assert.True(JToken.DeepEquals(expectedRequestBody, httpClient.DeletedBody));


            Assert.True(response.IsSuccessful);
            Assert.AreEqual("Ok", response.Status.Description);
        }
    }
}
