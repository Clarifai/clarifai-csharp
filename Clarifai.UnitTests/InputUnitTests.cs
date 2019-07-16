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
            ""description"": ""Input GET success""
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

        [Test]
        public async Task DeleteInputResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                deleteResponse: @"{""status"":{""code"":10000,""description"":""Ok""}}");
            var client = new ClarifaiClient(httpClient);
            ClarifaiResponse<EmptyResponse> response = await client.DeleteInputs(
                    "@inputID1", "@inputID2")
                .ExecuteAsync();

            Assert.True(response.IsSuccessful);
            Assert.AreEqual("Ok", response.Status.Description);
        }

        [Test]
        public async Task GetInputResponseShouldBeCorrect()
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
                ""url"": ""@imageURL""
            },
            ""concepts"": [
                {
                  ""id"": ""@positiveConcept"",
                  ""value"": 1
                },
                {
                  ""id"": ""@negativeConcept1"",
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
    }
}
");
            var client = new ClarifaiClient(httpClient);
            ClarifaiResponse<IClarifaiInput> response = await client.GetInput("@inputID").ExecuteAsync();
            var input = (ClarifaiURLImage)response.Get();

            Assert.True(response.IsSuccessful);
            Assert.AreEqual(input.ID,"@inputID");  
            Assert.AreEqual(input.URL,"@imageURL"); 
            Assert.AreEqual(input.PositiveConcepts.ElementAt(0).ID, "@positiveConcept"); 
            Assert.AreEqual(input.NegativeConcepts.ElementAt(0).ID, "@negativeConcept1"); 
            Assert.AreEqual(input.NegativeConcepts.ElementAt(1).ID, "@negativeConcept2"); 
        }


        [Test]
        public async Task GetInputsResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                getResponse: @"
{
    ""status"": {
        ""code"": 10000,
        ""description"": ""Ok""
    },
    ""inputs"": [{
        ""id"": ""@inputID1"",
        ""data"": {
            ""image"": {
                ""url"": ""@imageURL1""
            },
            ""concepts"": [
                {
                  ""id"": ""@positiveConcept"",
                  ""value"": 1
                },
                {
                  ""id"": ""@negativeConcept"",
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
    },
    {
        ""id"": ""@inputID2"",
        ""data"": {
            ""image"": {
                ""url"": ""@imageURL2""
            },
            ""geo"": {
                ""geo_point"": {
                    ""longitude"": 55,
                    ""latitude"": 66
                }
            }
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
            ClarifaiResponse<List<IClarifaiInput>> response = await client.GetInputs().ExecuteAsync();  
            var inputs = (List<IClarifaiInput>)response.Get();

            Assert.True(response.IsSuccessful);
            Assert.AreEqual(inputs[0].ID,"@inputID1"); 
            Assert.AreEqual(inputs[0].PositiveConcepts.ElementAt(0).ID, "@positiveConcept"); 
            Assert.AreEqual(inputs[0].NegativeConcepts.ElementAt(0).ID, "@negativeConcept"); 
            Assert.AreEqual(inputs[1].ID,"@inputID2"); 
            Assert.AreEqual(inputs[1].Geo.Longitude,55);
            Assert.AreEqual(inputs[1].Geo.Latitude,66); 
        }  


        [Test]
        public async Task GetInputsStatusResponseShouldBeCorrect()
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
            ClarifaiResponse<ClarifaiInputsStatus> response = await client.GetInputsStatus().ExecuteAsync(); 
            var status = (ClarifaiInputsStatus)response.Get();

            Assert.True(response.IsSuccessful);
            Assert.AreEqual(status.Processed,1);
            Assert.AreEqual(status.ToProcess,2);
            Assert.AreEqual(status.Errors,3);
            Assert.AreEqual(status.Processing,4);
        }


        [Test]
        public async Task AddInputsResponseAndResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                postResponse: @"
{
    ""status"": {
        ""code"": 10000,
        ""description"": ""Ok""
    },
    ""inputs"": [{
        ""id"": ""@inputID1"",
        ""data"": {
            ""image"": {
                ""url"": ""@imageURL1""
            },
            ""concepts"": [
                {
                  ""id"": ""@positiveConcept"",
                  ""value"": 1
                },
                {
                  ""id"": ""@negativeConcept"",
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
    },
    {
        ""id"": ""@inputID2"",
        ""data"": {
            ""image"": {
                ""url"": ""@imageURL2""
            },
            ""geo"": {
                ""geo_point"": {
                    ""longitude"": 55,
                    ""latitude"": 66
                }
            }
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
            ClarifaiResponse<List<IClarifaiInput>> response = await client.AddInputs(
                new  IClarifaiInput[]
                {
                    new ClarifaiURLImage("@imageURL1","@inputID1",
                        positiveConcepts: new List<Concept> {new Concept("@positiveConcept")},
                        negativeConcepts: new List<Concept> {new Concept("@negativeConcept")}),
                    new ClarifaiURLImage("@imageURL2","@inputID2",
                        geo: new DTOs.GeoPoint(55,66)) 
                }
            ).ExecuteAsync(); 


            var expectedRequestBody = JObject.Parse(@"
{
    ""inputs"": [
      {
        ""id"": ""@inputID1"",
        ""data"": {
            ""image"": {
                ""url"": ""@imageURL1""
            },
            ""concepts"": [
                {
                  ""id"": ""@positiveConcept"",
                  ""value"": true
                },
                {
                  ""id"": ""@negativeConcept"",
                  ""value"": false
                }
            ]
        }
      },
      {
        ""id"": ""@inputID2"",
        ""data"": {
            ""image"": {
                ""url"": ""@imageURL2""
            },
            ""geo"": {
                ""geo_point"": {
                    ""longitude"": 55.0,
                    ""latitude"": 66.0
                }
            }
        }
      }  
   ]
}
");

            Assert.True(JToken.DeepEquals(expectedRequestBody, httpClient.PostedBody));

            Assert.True(response.IsSuccessful);
            Assert.AreEqual("Ok", response.Status.Description);

            var inputs = (List<IClarifaiInput>)response.Get();
            Assert.True(response.IsSuccessful);
            Assert.AreEqual(inputs[0].ID,"@inputID1"); 
            Assert.AreEqual(inputs[0].PositiveConcepts.ElementAt(0).ID, "@positiveConcept"); 
            Assert.AreEqual(inputs[0].NegativeConcepts.ElementAt(0).ID, "@negativeConcept"); 
            Assert.AreEqual(inputs[1].ID,"@inputID2"); 
            Assert.AreEqual(inputs[1].Geo.Longitude,55);
            Assert.AreEqual(inputs[1].Geo.Latitude,66); 
        }
    }
}
