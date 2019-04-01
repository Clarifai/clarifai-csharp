using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clarifai.API;
using Clarifai.API.Requests.Models;
using Clarifai.API.Responses;
using Clarifai.DTOs.Inputs;
using Clarifai.DTOs.Models;
using Clarifai.DTOs.Models.Outputs;
using Clarifai.DTOs.Predictions;
using Clarifai.UnitTests.Fakes;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Clarifai.UnitTests
{
    [TestFixture]
    public class ModelUnitTests
    {
        [Test]
        public async Task CreateModelGenericRequestAndResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                postResponse: @"
{
  ""status"": {
    ""code"": 10000,
    ""description"": ""Ok""
  },
  ""model"": {
    ""id"": ""@modelID"",
    ""name"": ""@modelName"",
    ""created_at"": ""2019-01-18T13:51:40.798977081Z"",
    ""app_id"": ""@appID"",
    ""output_info"": {
      ""output_config"": {
        ""concepts_mutually_exclusive"": false,
        ""closed_environment"": false,
        ""max_concepts"": 0,
        ""min_value"": 0
      },
      ""message"": ""Show output_info with: GET /models/{model_id}/output_info"",
      ""type"": ""concept"",
      ""type_ext"": ""concept""
    },
    ""model_version"": {
      ""id"": ""@modelVersionID"",
      ""created_at"": ""2019-01-18T13:51:40.818805581Z"",
      ""status"": {
        ""code"": 21102,
        ""description"": ""Model not yet trained""
      },
      ""train_stats"": {}
    }
  }
}
");
            var client = new ClarifaiClient(httpClient);
            ClarifaiResponse<IModel<Concept>> response = await client.CreateModelGeneric<Concept>(
                "@modelID", name: "@modelName"
                ).ExecuteAsync();

            var expectedRequestBody = JObject.Parse(@"
{
  ""model"": {
    ""id"": ""@modelID"",
    ""name"": ""@modelName""
  }
}
");

            Assert.True(JToken.DeepEquals(expectedRequestBody, httpClient.PostedBody));

            Assert.True(response.IsSuccessful);
            Assert.AreEqual("Ok", response.Status.Description);

            IModel<Concept> model = response.Get();
            Assert.AreEqual("@modelID", model.ModelID);
            Assert.AreEqual("@modelName", model.Name);
            Assert.AreEqual("@modelVersionID", model.ModelVersion.ID);
        }

        [Test]
        public async Task CreateModelRequestAndResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                postResponse: @"
{
  ""status"": {
    ""code"": 10000,
    ""description"": ""Ok""
  },
  ""model"": {
    ""id"": ""@modelID"",
    ""name"": ""@modelName"",
    ""created_at"": ""2019-01-22T11:54:12.375436048Z"",
    ""app_id"": ""@appID"",
    ""output_info"": {
      ""output_config"": {
        ""concepts_mutually_exclusive"": false,
        ""closed_environment"": false,
        ""max_concepts"": 0,
        ""min_value"": 0
      },
      ""message"": ""Show output_info with: GET /models/{model_id}/output_info"",
      ""type"": ""concept"",
      ""type_ext"": ""concept""
    },
    ""model_version"": {
      ""id"": ""@modelVersionID"",
      ""created_at"": ""2019-01-22T11:54:12.406406642Z"",
      ""status"": {
        ""code"": 21102,
        ""description"": ""Model not yet trained""
      },
      ""active_concept_count"": 2,
      ""train_stats"": {}
    }
  }
}
");
            var client = new ClarifaiClient(httpClient);
            ClarifaiResponse<ConceptModel> response =
                await client.CreateModel(
                    "@modelID",
                    name: "@modelName",
                    concepts: new List<Concept>
                    {
                        new Concept("dog"),
                        new Concept("cat"),
                    },
                    areConceptsMutuallyExclusive: false,
                    isEnvironmentClosed: false)
                .ExecuteAsync();

            var expectedRequestBody = JObject.Parse(@"
{
  ""model"": {
    ""id"": ""@modelID"",
    ""name"": ""@modelName"",
    ""output_info"": {
      ""data"": {
        ""concepts"": [
          {
            ""id"": ""dog""
          },
          {
            ""id"": ""cat""
          }
        ]
      },
      ""output_config"": {}
    }
  }
}
");

            Assert.True(JToken.DeepEquals(expectedRequestBody, httpClient.PostedBody));

            Assert.True(response.IsSuccessful);
            Assert.AreEqual("Ok", response.Status.Description);

            IModel<Concept> model = response.Get();
            Assert.AreEqual("@modelID", model.ModelID);
            Assert.AreEqual("@modelName", model.Name);
            Assert.AreEqual("@modelVersionID", model.ModelVersion.ID);
        }

        [Test]
        public async Task GetModelResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                getResponse: @"
{
  ""status"": {
    ""code"": 10000,
    ""description"": ""Ok""
  },
  ""model"": {
    ""id"": ""some-model-id"",
    ""name"": ""some-model-name"",
    ""created_at"": ""2017-05-16T19:20:38.733764Z"",
    ""app_id"": ""main"",
    ""output_info"": {
      ""data"": {
        ""concepts"": [{
          ""id"": ""some-concept-id"",
          ""name"": ""safe"",
          ""created_at"": ""2017-05-16T19:20:38.450157Z"",
          ""language"": ""en"",
          ""app_id"": ""main""
        }]
      },
      ""type"": ""concept"",
      ""type_ext"": ""concept""
    },
    ""model_version"": {
      ""id"": ""some-model-version-id"",
      ""created_at"": ""2017-05-16T19:20:38.733764Z"",
      ""status"": {
        ""code"": 21100,
        ""description"": ""Model trained successfully""
      },
      ""active_concept_count"": 5
    },
    ""display_name"": ""Moderation""
  }
}
");

            var client = new ClarifaiClient(httpClient);
            var response = await client.GetModel<Concept>("").ExecuteAsync();
            var model = (ConceptModel)response.Get();

            Assert.True(response.IsSuccessful);
            Assert.AreEqual("some-model-id", model.ModelID);
            Assert.AreEqual("some-model-name", model.Name);
            Assert.AreEqual("some-concept-id", model.OutputInfo.Concepts.First().ID);
            Assert.AreEqual("some-model-version-id", model.ModelVersion.ID);
        }

        [Test]
        public async Task GetModelsResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                getResponse: @"
{
  ""status"": {
    ""code"": 10000,
      ""description"": ""Ok""
    },
  ""models"": [
    {
      ""id"": ""@modelID1"",
      ""name"": ""@modelName1"",
      ""created_at"": ""2019-01-16T23:33:46.605294Z"",
      ""app_id"": ""main"",
      ""output_info"": {
        ""message"": ""Show output_info with: GET /models/{model_id}/output_info"",
        ""type"": ""facedetect"",
        ""type_ext"": ""facedetect""
      },
      ""model_version"": {
        ""id"": ""28b2ff6148684aa2b18a34cd004b4fac"",
        ""created_at"": ""2019-01-16T23:33:46.605294Z"",
        ""status"": {
          ""code"": 21100,
          ""description"": ""Model trained successfully""
        },
        ""train_stats"": {}
      },
      ""display_name"": ""Face Detection""
    },
    {
      ""id"": ""@modelID2"",
      ""name"": ""@modelName2"",
      ""created_at"": ""2019-01-16T23:33:46.605294Z"",
      ""app_id"": ""main"",
      ""output_info"": {
        ""message"": ""Show output_info with: GET /models/{model_id}/output_info"",
        ""type"": ""embed"",
        ""type_ext"": ""detect-embed""
      },
      ""model_version"": {
        ""id"": ""fc6999e5eb274dfdba826f6b1c7ffdab"",
        ""created_at"": ""2019-01-16T23:33:46.605294Z"",
        ""status"": {
          ""code"": 21100,
          ""description"": ""Model trained successfully""
        },
        ""train_stats"": {}
      },
      ""display_name"": ""Face Embedding""
    }
  ]
}
");

            var client = new ClarifaiClient(httpClient);
            var response = await client.GetModels().ExecuteAsync();
            List<IModel> models = response.Get();

            Assert.AreEqual("/v2/models/", httpClient.RequestedUrl);

            Assert.True(response.IsSuccessful);

            FaceDetectionModel model1 = (FaceDetectionModel) models[0];
            Assert.AreEqual("@modelID1", model1.ModelID);
            Assert.AreEqual("@modelName1", model1.Name);
            Assert.AreEqual("facedetect", model1.OutputInfo.TypeExt);

            FaceEmbeddingModel model2 = (FaceEmbeddingModel) models[1];
            Assert.AreEqual("@modelID2", model2.ModelID);
            Assert.AreEqual("@modelName2", model2.Name);
            Assert.AreEqual("detect-embed", model2.OutputInfo.TypeExt);
        }

        [Test]
        public async Task GetModelInputsResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                getResponse: @"
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
        ""id"": ""@conceptID"",
        ""name"": ""@conceptName"",
        ""value"": 1,
        ""app_id"": ""@conceptAppID""
      }]
    },
    ""created_at"": ""2017-10-15T16:30:52.964888Z"",
    ""status"": {
      ""code"": 30000,
      ""description"": ""Download complete""
    }
  }]
}
");
            var client = new ClarifaiClient(httpClient);
            ClarifaiResponse<List<IClarifaiInput>> response = await client.GetModelInputs(
                "", "").ExecuteAsync();


            Assert.True(response.IsSuccessful);
            Assert.AreEqual("Ok", response.Status.Description);

            IClarifaiInput input = response.Get()[0];
            Assert.AreEqual("@inputID", input.ID);
            Assert.AreEqual("@conceptID", input.PositiveConcepts.First().ID);
        }

        [Test]
        public async Task DeleteAllModelsResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                deleteResponse: @"{""status"":{""code"":10000,""description"":""Ok""}}");
            var client = new ClarifaiClient(httpClient);
            ClarifaiResponse<EmptyResponse> response = await client.DeleteAllModels()
                .ExecuteAsync();

            Assert.AreEqual("/v2/models/", httpClient.RequestedUrl);

            Assert.True(response.IsSuccessful);
            Assert.AreEqual("Ok", response.Status.Description);
        }

        [Test]
        public async Task ModifyModelRequestAndResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                patchResponse: @"
{
  ""status"": {
    ""code"": 10000,
    ""description"": ""Ok""
  },
  ""models"": [{
    ""id"": ""@modelID"",
    ""name"": ""@newModelName"",
    ""created_at"": ""2017-11-27T08:35:13.911899Z"",
    ""app_id"": ""@appID"",
    ""output_info"": {
      ""output_config"": {
        ""concepts_mutually_exclusive"": true,
        ""closed_environment"": true
      },
      ""message"": ""Show output_info with: GET /models/{model_id}/output_info"",
      ""type"": ""concept"",
      ""type_ext"": ""concept""
    },
    ""model_version"": {
      ""id"": ""@modelVersionID"",
      ""created_at"": ""2017-11-27T08:35:14.298376733Z"",
      ""status"": {
        ""code"": 21102,
        ""description"": ""Model not yet trained""
      }
    }
  }]
}
");
            var client = new ClarifaiClient(httpClient);
            var response = await client.ModifyModel(
                    "@modelID",
                    ModifyAction.Merge,
                    name: "@newModelName",
                    concepts: new List<Concept> {new Concept("@conceptID1")},
                    areConceptsMutuallyExclusive: true,
                    isEnvironmentClosed: true)
                .ExecuteAsync();

            var expectedRequestBody = JObject.Parse(@"
{
  ""models"": [
    {
      ""id"": ""@modelID"",
      ""name"": ""@newModelName"",
      ""output_info"": {
        ""data"": {
          ""concepts"": [
            {
              ""id"": ""@conceptID1""
            }
          ]
        },
        ""output_config"": {
          ""concepts_mutually_exclusive"": true,
          ""closed_environment"": true
        }
      }
    }
  ],
  ""action"": ""merge""
}
");

            Assert.True(JToken.DeepEquals(expectedRequestBody, httpClient.PatchedBody));

            Assert.True(response.IsSuccessful);
            Assert.AreEqual("Ok", response.Status.Description);

            ConceptModel model = response.Get();
            Assert.AreEqual("@modelID", model.ModelID);
            Assert.AreEqual("@newModelName", model.Name);
            Assert.AreEqual("@modelVersionID", model.ModelVersion.ID);
            Assert.IsTrue(model.OutputInfo.AreConceptsMutuallyExclusive);
            Assert.IsTrue(model.OutputInfo.IsEnvironmentClosed);
        }

        [Test]
        public async Task DeleteModelRequestAndResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                deleteResponse: @"
{
    ""status"": {
        ""code"": 10000,
        ""description"": ""Ok""
    }
}
");
            var client = new ClarifaiClient(httpClient);
            var response = await client.DeleteModel("@modelID")
                .ExecuteAsync();

            Assert.AreEqual("/v2/models/@modelID", httpClient.RequestedUrl);

            Assert.True(response.IsSuccessful);
            Assert.AreEqual("Ok", response.Status.Description);
        }

        [Test]
        public async Task TrainModelRequestAndResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                postResponse: @"
{
  ""status"": {
    ""code"": 10000,
    ""description"": ""Ok""
  },
  ""model"": {
    ""id"": ""@modelID"",
    ""name"": ""@modelName"",
    ""created_at"": ""2019-01-20T15:51:21.641006Z"",
    ""app_id"": ""@appID"",
    ""output_info"": {
      ""output_config"": {
        ""concepts_mutually_exclusive"": false,
        ""closed_environment"": false,
        ""max_concepts"": 0,
        ""min_value"": 0
      },
      ""message"": ""Show output_info with: GET /models/{model_id}/output_info"",
      ""type"": ""concept"",
      ""type_ext"": ""concept""
    },
    ""model_version"": {
      ""id"": ""@modelVersionID"",
      ""created_at"": ""2019-01-20T15:51:25.093744401Z"",
      ""status"": {
        ""code"": 21103,
        ""description"": ""Custom model is currently in queue for training, waiting on inputs to process.""
      },
      ""active_concept_count"": 2,
      ""train_stats"": {}
    }
  }
}
");
            var client = new ClarifaiClient(httpClient);
            var response = await client.TrainModel<Concept>("@modelID")
                .ExecuteAsync();

            Assert.AreEqual("/v2/models/@modelID/versions", httpClient.RequestedUrl);

            Assert.True(response.IsSuccessful);
            Assert.AreEqual("Ok", response.Status.Description);

            IModel<Concept> model = response.Get();
            Assert.AreEqual("@modelID", model.ModelID);
            Assert.AreEqual("@modelName", model.Name);
            Assert.AreEqual("@modelVersionID", model.ModelVersion.ID);
            Assert.AreEqual(2, model.ModelVersion.ActiveConceptCount);
        }
    }
}
