using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Clarifai.API;
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
    public class InvalidResponseUnitTests
    {
        [Test]
        public async Task InvalidJsonShouldReturnNetworkErrorResponse()
        {
            var httpClient = new FkClarifaiHttpClient(
                getResponse: @"
{
    ""status"": {

        ""code"": 10000,
        ""description"": ""Ok""

    },
    ""model"": {
");

            var client = new ClarifaiClient(httpClient);
            var response = await client.GetModel<Concept>("").ExecuteAsync();

            Assert.False(response.IsSuccessful);
            Assert.AreEqual(ClarifaiStatus.StatusType.NetworkError, response.Status.Type);
            Assert.NotNull(response.Status.Description);
            Assert.NotNull(response.Status.ErrorDetails);
        }

        [Test]
        public async Task PredictRequestWithInvalidUrlShouldFail()
        {
            var httpClient = new FkClarifaiHttpClient(
                postResponse: @"
{
  ""status"": {
    ""code"": 10020,
    ""description"": ""Failure""
  },
  ""outputs"": [
  {
    ""id"": ""@outputID"",
    ""status"": {
      ""code"": 30002,
      ""description"": ""Download failed; check URL"",
      ""details"": ""404 Client Error: Not Found for url: @invalidURL""
    },
    ""created_at"": ""2019-01-20T19:39:15.460417224Z"",
    ""model"": {
      ""id"": ""@modelID"",
      ""name"": ""color"",
      ""created_at"": ""2016-05-11T18:05:45.924367Z"",
      ""app_id"": ""main"",
      ""output_info"": {
        ""message"": ""Show output_info with: GET /models/{model_id}/output_info"",
        ""type"": ""color"",
        ""type_ext"": ""color""
      },
      ""model_version"": {
        ""id"": ""@modelVersionID"",
        ""created_at"": ""2016-07-13T01:19:12.147644Z"",
        ""status"": {
          ""code"": 21100,
          ""description"": ""Model trained successfully""
        },
        ""train_stats"": {}
      },
      ""display_name"": ""Color""
    },
    ""input"": {
      ""id"": ""@inputID"",
      ""data"": {
        ""image"": {
          ""url"": ""@invalidURL""
        }
      }
    },
    ""data"": {}
  }
  ]
}
",
                lastResponseHttpStatusCode: HttpStatusCode.NotFound);

            var client = new ClarifaiClient(httpClient);
            var response = await client.Predict<Concept>(
                    "@modelID", new ClarifaiURLImage("@invalidURL"))
                .ExecuteAsync();
            ClarifaiOutput<Concept> output = response.Get();

            var expectedRequestBody = JObject.Parse(@"
{
  ""inputs"": [
    {
      ""data"": {
        ""image"": {
          ""url"": ""@invalidURL""
        }
      }
    }
  ]
}
");
            Assert.True(JToken.DeepEquals(expectedRequestBody, httpClient.PostedBody));

            Assert.AreEqual("/v2/models/@modelID/outputs", httpClient.RequestedUrl);

            Assert.False(response.IsSuccessful);
            Assert.AreEqual(ClarifaiStatus.StatusType.Failure, response.Status.Type);

            Assert.AreEqual(10020, response.Status.StatusCode);
            Assert.AreEqual("Failure", response.Status.Description);

            Assert.AreEqual(30002, output.Status.StatusCode);
            Assert.AreEqual("Download failed; check URL", output.Status.Description);
            Assert.AreEqual(
              "404 Client Error: Not Found for url: @invalidURL", output.Status.ErrorDetails);

            Assert.AreEqual("@inputID", output.Input.ID);
            Assert.AreEqual("@invalidURL", ((ClarifaiURLImage)output.Input).URL);
        }

        [Test]
        public async Task BatchPredictRequestWithOneInvalidUrlShouldReturnMixedSuccess()
        {
            var httpClient = new FkClarifaiHttpClient(
                postResponse: @"
{
  ""status"": {
    ""code"": 10010,
    ""description"": ""Mixed Success""
  },
  ""outputs"": [
    {
      ""id"": ""@outputID1"",
      ""status"": {
        ""code"": 10000,
        ""description"": ""Ok""
      },
      ""created_at"": ""2019-01-20T20:25:20.302505245Z"",
      ""model"": {
        ""id"": ""@modelID"",
        ""name"": ""general"",
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
          },
          ""train_stats"": {}
        },
        ""display_name"": ""General""
      },
      ""input"": {
        ""id"": ""@inputID1"",
        ""data"": {
          ""image"": {
            ""url"": ""@validURL""
          }
        }
      },
      ""data"": {
        ""concepts"": [
          {
            ""id"": ""@concept1"",
            ""name"": ""people"",
            ""value"": 0.9963381,
            ""app_id"": ""main""
          },
          {
            ""id"": ""@concept2"",
            ""name"": ""one"",
            ""value"": 0.9879057,
            ""app_id"": ""main""
          }
        ]
      }
    },
    {
      ""id"": ""@outputID2"",
      ""status"": {
        ""code"": 30002,
        ""description"": ""Download failed; check URL"",
        ""details"": ""404 Client Error: Not Found for url: @invalidURL""
      },
      ""created_at"": ""2019-01-20T20:25:20.302505245Z"",
      ""model"": {
        ""id"": ""@modelID"",
        ""name"": ""general"",
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
          },
          ""train_stats"": {}
        },
        ""display_name"": ""General""
      },
      ""input"": {
        ""id"": ""@inputID2"",
        ""data"": {
          ""image"": {
            ""url"": ""@invalidURL""
          }
        }
      },
      ""data"": {}
    }
  ]
}
");

            var client = new ClarifaiClient(httpClient);
            var response = await client.Predict<Concept>(
                    "@modelID",
                    new List<IClarifaiInput>
                    {
                      new ClarifaiURLImage("@validURL"),
                      new ClarifaiURLImage("@invalidURL"),
                    })
                .ExecuteAsync();
            List<ClarifaiOutput<Concept>> outputs = response.Get();

            var expectedRequestBody = JObject.Parse(@"
{
  ""inputs"": [
    {
      ""data"": {
        ""image"": {
          ""url"": ""@validURL""
        }
      }
    },
    {
      ""data"": {
        ""image"": {
          ""url"": ""@invalidURL""
        }
      }
    }
  ]
}
");
            Assert.True(JToken.DeepEquals(expectedRequestBody, httpClient.PostedBody));

            Assert.AreEqual("/v2/models/@modelID/outputs", httpClient.RequestedUrl);

            Assert.False(response.IsSuccessful);
            Assert.AreEqual(ClarifaiStatus.StatusType.MixedSuccess, response.Status.Type);

            Assert.AreEqual(10010, response.Status.StatusCode);
            Assert.AreEqual("Mixed Success", response.Status.Description);

            ClarifaiOutput<Concept> output1 = outputs[0];
            Assert.AreEqual(10000, output1.Status.StatusCode);
            Assert.AreEqual("Ok", output1.Status.Description);
            Assert.AreEqual("@inputID1", output1.Input.ID);
            Assert.AreEqual("@validURL", ((ClarifaiURLImage)output1.Input).URL);

            ClarifaiOutput<Concept> output2 = outputs[1];
            Assert.AreEqual(30002, output2.Status.StatusCode);
            Assert.AreEqual("Download failed; check URL", output2.Status.Description);
            Assert.AreEqual("404 Client Error: Not Found for url: @invalidURL",
              output2.Status.ErrorDetails);
            Assert.AreEqual("@inputID2", output2.Input.ID);
            Assert.AreEqual("@invalidURL", ((ClarifaiURLImage)output2.Input).URL);
        }
    }
}
