using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Clarifai.API;
using Clarifai.DTOs.Inputs;
using Clarifai.DTOs.Models.Outputs;
using Clarifai.DTOs.Predictions;
using Clarifai.DTOs.Workflows;
using Clarifai.UnitTests.Fakes;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Clarifai.UnitTests
{
    [TestFixture]
    public class WorkflowUnitTests
    {
      private const string TINY_PNG_IMAGE_BASE64 =
        "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z/C/HgAGgwJ/lK3Q6wAAAABJRU5ErkJggg==";

        [Test]
        public async Task WorkflowPredictWithURLShouldBeSuccessful()
        {
            var httpClient = new FkClarifaiHttpClient(
                postResponse: @"
{
  ""status"": {
    ""code"": 10000,
    ""description"": ""Ok""
  },
  ""workflow"": {
    ""id"": ""@workflowID"",
    ""app_id"": ""@appID"",
    ""created_at"": ""2017-07-10T01:45:05.672880Z""
  },
  ""results"": [
    {
      ""status"": {
        ""code"": 10000,
        ""description"": ""Ok""
      },
      ""input"": {
        ""id"": ""@inputID"",
        ""data"": {
          ""image"": {
            ""url"": ""@inputURL""
          }
        }
      },
      ""outputs"": [
        {
          ""id"": ""@outputID1"",
          ""status"": {
            ""code"": 10000,
            ""description"": ""Ok""
          },
          ""created_at"": ""2017-07-10T12:01:44.929928529Z"",
          ""model"": {
            ""id"": ""d16f390eb32cad478c7ae150069bd2c6"",
            ""name"": ""moderation"",
            ""created_at"": ""2017-05-12T21:28:00.471607Z"",
            ""app_id"": ""main"",
            ""output_info"": {
              ""message"": ""Show output_info with: GET /models/{model_id}/output_info"",
              ""type"": ""concept"",
              ""type_ext"": ""concept""
            },
            ""model_version"": {
              ""id"": ""b42ac907ac93483484483a0040a386be"",
              ""created_at"": ""2017-05-12T21:28:00.471607Z"",
              ""status"": {
                ""code"": 21100,
                ""description"": ""Model trained successfully""
              }
            }
          },
          ""data"": {
            ""concepts"": [
              {
                ""id"": ""@conceptID11"",
                ""name"": ""safe"",
                ""value"": 0.99999714,
                ""app_id"": ""main""
              }
            ]
          }
        },
        {
          ""id"": ""@outputID2"",
          ""status"": {
            ""code"": 10000,
            ""description"": ""Ok""
          },
          ""created_at"": ""2017-07-10T12:01:44.929941126Z"",
          ""model"": {
            ""id"": ""aaa03c23b3724a16a56b629203edc62c"",
            ""name"": ""general-v1.3"",
            ""created_at"": ""2016-02-26T23:38:40.086101Z"",
            ""app_id"": ""main"",
            ""output_info"": {
              ""message"": ""Show output_info with: GET /models/{model_id}/output_info"",
              ""type"": ""concept"",
              ""type_ext"": ""concept""
            },
            ""model_version"": {
              ""id"": ""aa9ca48295b37401f8af92ad1af0d91d"",
              ""created_at"": ""2016-07-13T00:58:55.915745Z"",
              ""status"": {
                ""code"": 21100,
                ""description"": ""Model trained successfully""
              }
            }
          },
          ""data"": {
            ""concepts"": [
              {
                ""id"": ""@conceptID21"",
                ""name"": ""train"",
                ""value"": 0.9989112,
                ""app_id"": ""main""
              },
              {
                ""id"": ""@conceptID22"",
                ""name"": ""railway"",
                ""value"": 0.9975532,
                ""app_id"": ""main""
              }
            ]
          }
        }
      ]
    }
  ]
}
");

            var client = new ClarifaiClient(httpClient);
            var response = await client.WorkflowPredict(
                    "", new ClarifaiURLImage("@url"))
                .ExecuteAsync();
            var workflow = response.Get();

            var expectedRequestBody = JObject.Parse(@"
{
  ""inputs"": [
    {
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
            Assert.AreEqual("@workflowID", workflow.Workflow.ID);
            Assert.AreEqual("@appID", workflow.Workflow.AppID);

            WorkflowResult results = workflow.WorkflowResult;
            Assert.AreEqual("@inputID", results.Input.ID);

            ClarifaiOutput output1 = results.Predictions[0];
            Assert.AreEqual("@outputID1", output1.ID);
            Assert.AreEqual("@conceptID11", ((Concept)output1.Data[0]).ID);

            ClarifaiOutput output2 = results.Predictions[1];
            Assert.AreEqual("@outputID2", output2.ID);
            Assert.AreEqual("@conceptID21", ((Concept)output2.Data[0]).ID);
            Assert.AreEqual("@conceptID22", ((Concept)output2.Data[1]).ID);
        }

        [Test]
        public async Task WorkflowBatchPredictRequestAndResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                postResponse: @"
{
  ""status"": {
    ""code"": 10000,
    ""description"": ""Ok""
  },
  ""workflow"": {
    ""id"": ""@workflowID"",
    ""app_id"": ""@appID"",
    ""created_at"": ""2017-07-10T01:45:05.672880Z""
  },
  ""results"": [
    {
      ""status"": {
        ""code"": 10000,
        ""description"": ""Ok""
      },
      ""input"": {
        ""id"": ""@inputID"",
        ""data"": {
          ""image"": {
            ""url"": ""@inputURL""
          }
        }
      },
      ""outputs"": [
        {
          ""id"": ""@outputID1"",
          ""status"": {
            ""code"": 10000,
            ""description"": ""Ok""
          },
          ""created_at"": ""2017-07-10T12:01:44.929928529Z"",
          ""model"": {
            ""id"": ""d16f390eb32cad478c7ae150069bd2c6"",
            ""name"": ""moderation"",
            ""created_at"": ""2017-05-12T21:28:00.471607Z"",
            ""app_id"": ""main"",
            ""output_info"": {
              ""message"": ""Show output_info with: GET /models/{model_id}/output_info"",
              ""type"": ""concept"",
              ""type_ext"": ""concept""
            },
            ""model_version"": {
              ""id"": ""b42ac907ac93483484483a0040a386be"",
              ""created_at"": ""2017-05-12T21:28:00.471607Z"",
              ""status"": {
                ""code"": 21100,
                ""description"": ""Model trained successfully""
              }
            }
          },
          ""data"": {
            ""concepts"": [
              {
                ""id"": ""@conceptID11"",
                ""name"": ""safe"",
                ""value"": 0.99999714,
                ""app_id"": ""main""
              }
            ]
          }
        },
        {
          ""id"": ""@outputID2"",
          ""status"": {
            ""code"": 10000,
            ""description"": ""Ok""
          },
          ""created_at"": ""2017-07-10T12:01:44.929941126Z"",
          ""model"": {
            ""id"": ""aaa03c23b3724a16a56b629203edc62c"",
            ""name"": ""general-v1.3"",
            ""created_at"": ""2016-02-26T23:38:40.086101Z"",
            ""app_id"": ""main"",
            ""output_info"": {
              ""message"": ""Show output_info with: GET /models/{model_id}/output_info"",
              ""type"": ""concept"",
              ""type_ext"": ""concept""
            },
            ""model_version"": {
              ""id"": ""aa9ca48295b37401f8af92ad1af0d91d"",
              ""created_at"": ""2016-07-13T00:58:55.915745Z"",
              ""status"": {
                ""code"": 21100,
                ""description"": ""Model trained successfully""
              }
            }
          },
          ""data"": {
            ""concepts"": [
              {
                ""id"": ""@conceptID21"",
                ""name"": ""train"",
                ""value"": 0.9989112,
                ""app_id"": ""main""
              },
              {
                ""id"": ""@conceptID22"",
                ""name"": ""railway"",
                ""value"": 0.9975532,
                ""app_id"": ""main""
              }
            ]
          }
        }
      ]
    }
  ]
}
");

            var client = new ClarifaiClient(httpClient);
            var response = await client.WorkflowPredict(
                    "", new ClarifaiURLImage("@url"))
                .ExecuteAsync();
            var workflow = response.Get();

            var expectedRequestBody = JObject.Parse(@"
{
  ""inputs"": [
    {
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
            Assert.AreEqual("@workflowID", workflow.Workflow.ID);
            Assert.AreEqual("@appID", workflow.Workflow.AppID);

            WorkflowResult results = workflow.WorkflowResult;
            Assert.AreEqual("@inputID", results.Input.ID);

            ClarifaiOutput output1 = results.Predictions[0];
            Assert.AreEqual("@outputID1", output1.ID);
            Assert.AreEqual("@conceptID11", ((Concept)output1.Data[0]).ID);

            ClarifaiOutput output2 = results.Predictions[1];
            Assert.AreEqual("@outputID2", output2.ID);
            Assert.AreEqual("@conceptID21", ((Concept)output2.Data[0]).ID);
            Assert.AreEqual("@conceptID22", ((Concept)output2.Data[1]).ID);
        }

        [Test]
        public async Task WorkflowPredictWithBase64ShouldBeSuccessful()
        {
            var httpClient = new FkClarifaiHttpClient(
                postResponse: @"
{
  ""status"": {
    ""code"": 10000,
    ""description"": ""Ok""
  },
  ""workflow"": {
    ""id"": ""@workflowID"",
    ""app_id"": ""@appID"",
    ""created_at"": ""2017-06-15T15:17:30.462323Z""
  },
  ""results"": [
    {
      ""status"": {
        ""code"": 10000,
        ""description"": ""Ok""
      },
      ""input"": {
        ""id"": ""@inputID"",
        ""data"": {
          ""image"": {
            ""base64"": ""iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z/C/HgAGgwJ/lK3Q6wAAAABJRU5ErkJggg==""
          }
        }
      },
      ""outputs"": [
        {
          ""id"": ""@outputID1"",
          ""status"": {
            ""code"": 10000,
            ""description"": ""Ok""
          },
          ""created_at"": ""2019-01-20T18:22:36.057985725Z"",
          ""model"": {
            ""id"": ""bd367be194cf45149e75f01d59f77ba7"",
            ""name"": ""food-items-v1.0"",
            ""created_at"": ""2016-09-17T22:18:59.955626Z"",
            ""app_id"": ""main"",
            ""output_info"": {
              ""message"": ""Show output_info with: GET /models/{model_id}/output_info"",
              ""type"": ""concept"",
              ""type_ext"": ""concept""
            },
            ""model_version"": {
              ""id"": ""dfebc169854e429086aceb8368662641"",
              ""created_at"": ""2016-09-17T22:18:59.955626Z"",
              ""status"": {
                ""code"": 21100,
                ""description"": ""Model trained successfully""
              },
              ""train_stats"": {}
            },
            ""display_name"": ""Food""
          },
          ""data"": {
            ""concepts"": [
              {
                ""id"": ""@conceptID11"",
                ""name"": ""raspberry"",
                ""value"": 0.8684727,
                ""app_id"": ""main""
              },
              {
                ""id"": ""@conceptID12"",
                ""name"": ""strawberry"",
                ""value"": 0.7979152,
                ""app_id"": ""main""
              }
            ]
          }
        },
        {
          ""id"": ""@outputID2"",
          ""status"": {
            ""code"": 10000,
            ""description"": ""Ok""
          },
          ""created_at"": ""2019-01-20T18:22:36.058002759Z"",
          ""model"": {
            ""id"": ""aaa03c23b3724a16a56b629203edc62c"",
            ""name"": ""general"",
            ""created_at"": ""2016-03-09T17:11:39.608845Z"",
            ""app_id"": ""main"",
            ""output_info"": {
              ""message"": ""Show output_info with: GET /models/{model_id}/output_info"",
              ""type"": ""concept"",
              ""type_ext"": ""concept""
            },
            ""model_version"": {
              ""id"": ""aa9ca48295b37401f8af92ad1af0d91d"",
              ""created_at"": ""2016-07-13T01:19:12.147644Z"",
              ""status"": {
                ""code"": 21100,
                ""description"": ""Model trained successfully""
              },
              ""train_stats"": {}
            },
            ""display_name"": ""General""
          },
          ""data"": {
            ""concepts"": [
              {
                ""id"": ""@conceptID21"",
                ""name"": ""design"",
                ""value"": 0.9859183,
                ""app_id"": ""main""
              },
              {
                ""id"": ""@conceptID22"",
                ""name"": ""art"",
                ""value"": 0.98318106,
                ""app_id"": ""main""
              }
            ]
          }
        }
      ]
    }
  ]
}
");

            var client = new ClarifaiClient(httpClient);
            var response = await client.WorkflowPredict(
                    "", new ClarifaiFileImage(Convert.FromBase64String(TINY_PNG_IMAGE_BASE64)))
                .ExecuteAsync();
            var workflow = response.Get();

            var expectedRequestBody = JObject.Parse(@"
{
  ""inputs"": [
    {
      ""data"": {
        ""image"": {
          ""base64"": ""iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z/C/HgAGgwJ/lK3Q6wAAAABJRU5ErkJggg==""
        }
      }
    }
  ]
}
");
            Assert.True(JToken.DeepEquals(expectedRequestBody, httpClient.PostedBody));

            Assert.True(response.IsSuccessful);
            Assert.AreEqual("@workflowID", workflow.Workflow.ID);
            Assert.AreEqual("@appID", workflow.Workflow.AppID);

            WorkflowResult results = workflow.WorkflowResult;
            Assert.AreEqual("@inputID", results.Input.ID);

            ClarifaiOutput output1 = results.Predictions[0];
            Assert.AreEqual("@outputID1", output1.ID);
            Assert.AreEqual("@conceptID11", ((Concept)output1.Data[0]).ID);

            ClarifaiOutput output2 = results.Predictions[1];
            Assert.AreEqual("@outputID2", output2.ID);
            Assert.AreEqual("@conceptID21", ((Concept)output2.Data[0]).ID);
            Assert.AreEqual("@conceptID22", ((Concept)output2.Data[1]).ID);
        }
    }
}
