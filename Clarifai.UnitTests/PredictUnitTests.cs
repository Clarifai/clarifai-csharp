using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Clarifai.API;
using Clarifai.DTOs.Inputs;
using Clarifai.DTOs.Models.Outputs;
using Clarifai.DTOs.Predictions;
using Clarifai.UnitTests.Fakes;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Clarifai.UnitTests
{
    [TestFixture]
    public class PredictUnitTests
    {
        [Test]
        public async Task ConceptPredictRequestAndResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                postResponse: @"
{
    ""status"": {
        ""code"": 10000,
        ""description"": ""Ok""
    },
    ""outputs"": [{
        ""id"": ""@outputID"",
        ""status"": {
            ""code"": 10000,
            ""description"": ""Ok""
        },
        ""created_at"": ""2017-11-17T19:32:58.760477937Z"",
        ""model"": {
            ""id"": ""@modelID"",
            ""name"": ""@modelName"",
            ""created_at"": ""2016-03-09T17:11:39.608845Z"",
            ""app_id"": ""main"",
            ""output_info"": {
                ""message"": ""Show output_info with: GET /models/{model_id}/output_info"",
                ""type"": ""concept"",
                ""type_ext"": ""concept""
            },
            ""model_version"": {
                ""id"": ""@modelVersionID"",
                ""created_at"": ""2016-07-13T01:19:12.147644Z"",
                ""status"": {
                    ""code"": 21100,
                    ""description"": ""Model trained successfully""
                }
            },
            ""display_name"": ""@modelDisplayName""
        },
        ""input"": {
            ""id"": ""@inputID"",
            ""data"": {
                ""image"": {
                    ""url"": ""@imageUrl""
                }
            }
        },
        ""data"": {
            ""concepts"": [{
                ""id"": ""@conceptID1"",
                ""name"": ""@conceptName1"",
                ""value"": 0.99,
                ""app_id"": ""main""
            }, {
                ""id"": ""@conceptID2"",
                ""name"": ""@conceptName2"",
                ""value"": 0.98,
                ""app_id"": ""main""
            }]
        }
    }]
}
");

            var client = new ClarifaiClient(httpClient);
            var response = await client.Predict<Concept>(
                    "", new ClarifaiURLImage("@url"))
                .ExecuteAsync();
            ClarifaiOutput<Concept> output = response.Get();

            var expectedRequestBody = JObject.Parse(@"
{
  ""inputs"": [
    {
      ""id"": null,
      ""data"": {
        ""image"": {
          ""url"": ""@url""
        }
      }
    }
  ]
}
");
            Assert.True(JToken.DeepEquals(expectedRequestBody, httpClient.PostedBody));

            Assert.True(response.IsSuccessful);

            Assert.AreEqual("@inputID", output.Input.ID);

            Assert.AreEqual("@outputID", output.ID);
            Assert.AreEqual("@conceptID1", output.Data[0].ID);
        }

        [Test]
        public async Task ConceptBatchPredictRequestAndResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                postResponse: @"
{
    ""status"": {
        ""code"": 10000,
        ""description"": ""Ok""
    },
    ""outputs"": [{
        ""id"": ""@outputID1"",
        ""status"": {
            ""code"": 10000,
            ""description"": ""Ok""
        },
        ""created_at"": ""2017-11-17T19:32:58.760477937Z"",
        ""model"": {
            ""id"": ""@modelID1"",
            ""name"": ""@modelName1"",
            ""created_at"": ""2016-03-09T17:11:39.608845Z"",
            ""app_id"": ""main"",
            ""output_info"": {
                ""message"": ""Show output_info with: GET /models/{model_id}/output_info"",
                ""type"": ""concept"",
                ""type_ext"": ""concept""
            },
            ""model_version"": {
                ""id"": ""@modelVersionID1"",
                ""created_at"": ""2016-07-13T01:19:12.147644Z"",
                ""status"": {
                    ""code"": 21100,
                    ""description"": ""Model trained successfully""
                }
            },
            ""display_name"": ""@modelDisplayName1""
        },
        ""input"": {
            ""id"": ""@inputID1"",
            ""data"": {
                ""image"": {
                    ""url"": ""@imageUrl1""
                }
            }
        },
        ""data"": {
            ""concepts"": [{
                ""id"": ""@conceptID11"",
                ""name"": ""@conceptName11"",
                ""value"": 0.99,
                ""app_id"": ""main""
            }, {
                ""id"": ""@conceptID12"",
                ""name"": ""@conceptName12"",
                ""value"": 0.98,
                ""app_id"": ""main""
            }]
        }
    },
    {
        ""id"": ""@outputID2"",
        ""status"": {
            ""code"": 10000,
            ""description"": ""Ok""
        },
        ""created_at"": ""2017-11-17T19:32:58.760477937Z"",
        ""model"": {
            ""id"": ""@modelID2"",
            ""name"": ""@modelName2"",
            ""created_at"": ""2016-03-09T17:11:39.608845Z"",
            ""app_id"": ""main"",
            ""output_info"": {
                ""message"": ""Show output_info with: GET /models/{model_id}/output_info"",
                ""type"": ""concept"",
                ""type_ext"": ""concept""
            },
            ""model_version"": {
                ""id"": ""@modelVersionID2"",
                ""created_at"": ""2016-07-13T01:19:12.147644Z"",
                ""status"": {
                    ""code"": 21100,
                    ""description"": ""Model trained successfully""
                }
            },
            ""display_name"": ""@modelDisplayName2""
        },
        ""input"": {
            ""id"": ""@inputID2"",
            ""data"": {
                ""image"": {
                    ""url"": ""@imageUrl2""
                }
            }
        },
        ""data"": {
            ""concepts"": [{
                ""id"": ""@conceptID21"",
                ""name"": ""@conceptName21"",
                ""value"": 0.99,
                ""app_id"": ""main""
            }, {
                ""id"": ""@conceptID22"",
                ""name"": ""@conceptName22"",
                ""value"": 0.98,
                ""app_id"": ""main""
            }]
        }
    }
]
}
");

            var client = new ClarifaiClient(httpClient);
            var response = await client.BatchPredict<Concept>(
                    "",
                    new List<IClarifaiInput>
                    {
                        new ClarifaiURLImage("@url1"), new ClarifaiURLImage("@url2")
                    })
                .ExecuteAsync();
            List<ClarifaiOutput<Concept>> outputs = response.Get();

            var expectedRequestBody = JObject.Parse(@"
{
  ""inputs"": [
    {
      ""id"": null,
      ""data"": {
        ""image"": {
          ""url"": ""@url1""
        }
      }
    },
    {
      ""id"": null,
      ""data"": {
        ""image"": {
          ""url"": ""@url2""
        }
      }
    }
  ]
}
");
            Assert.True(JToken.DeepEquals(expectedRequestBody, httpClient.PostedBody));

            Assert.True(response.IsSuccessful);

            ClarifaiOutput<Concept> output1 = outputs[0];
            Assert.AreEqual("@inputID1", output1.Input.ID);
            Assert.AreEqual("@outputID1", output1.ID);
            Assert.AreEqual("@conceptID11", output1.Data[0].ID);
            Assert.AreEqual("@conceptID12", output1.Data[1].ID);

            ClarifaiOutput<Concept> output2 = outputs[1];
            Assert.AreEqual("@inputID2", output2.Input.ID);
            Assert.AreEqual("@outputID2", output2.ID);
            Assert.AreEqual("@conceptID21", output2.Data[0].ID);
            Assert.AreEqual("@conceptID22", output2.Data[1].ID);
        }
    }
}
