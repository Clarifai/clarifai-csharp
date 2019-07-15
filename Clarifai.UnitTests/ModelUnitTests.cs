using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clarifai.API;
using Clarifai.API.Requests.Models;
using Clarifai.API.Responses;
using Clarifai.DTOs.Inputs;
using Clarifai.DTOs.Models;
using Clarifai.DTOs.Models.Outputs;
using Clarifai.DTOs.Models.OutputsInfo;
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
    ""models"": [{
        ""id"": ""model-id1"",
        ""name"": ""model-name1"",
        ""created_at"": ""2017-05-16T19:20:38.733764Z"",
        ""app_id"": ""main"",
        ""output_info"": {
            ""data"": {
                ""concepts"": [{
                    ""id"": ""concept-id1"",
                    ""name"": ""concept-name1"",
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
    },
    {
        ""id"": ""model-id2"",
        ""name"": ""model-name2"",
        ""created_at"": ""2017-05-16T19:20:38.733764Z"",
        ""app_id"": ""main"",
        ""output_info"": {
            ""data"": {
                ""concepts"": [{
                    ""id"": ""concept-id2"",
                    ""name"": ""concept-name2"",
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
    }]
}
");            
            var client = new ClarifaiClient(httpClient);
            ClarifaiResponse<List<IModel>> response = await client.GetModels().ExecuteAsync();  
            var models = (List<IModel>)response.Get();

            Assert.True(response.IsSuccessful);
            Assert.AreEqual(models[0].ModelID, "model-id1");
            Assert.AreEqual(models[0].Name, "model-name1");
            Assert.AreEqual(models[0].ModelVersion.ID,"some-model-version-id");
            Assert.AreEqual(models[1].ModelID, "model-id2");
            Assert.AreEqual(models[1].AppID, "main");
            Assert.AreEqual(models[1].OutputInfo.Type,"concept");
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

            Assert.True(response.IsSuccessful);
            Assert.AreEqual("Ok", response.Status.Description);
        }

        [Test]
        public async Task DeleteModelResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                deleteResponse: @"{""status"":{""code"":10000,""description"":""Ok""}}");
            var client = new ClarifaiClient(httpClient);
            ClarifaiResponse<EmptyResponse> response = await client.DeleteModel("@modelID").ExecuteAsync();

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
            Assert.AreEqual(true, model.OutputInfo.AreConceptsMutuallyExclusive);
            Assert.AreEqual(true, model.OutputInfo.IsEnvironmentClosed);
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
            ClarifaiResponse<ConceptModel> response = await client.CreateModel(
                modelID: "@newmodelID",
                name: "@newModelName",
                concepts: new List<Concept> {new Concept("@conceptID1")}
                ).ExecuteAsync();

            var expectedRequestBody = JObject.Parse(@"
{
    ""model"":
      {
        ""id"": ""@newmodelID"",
        ""name"": ""@newModelName"",
        ""output_info"": {
          ""data"": {
            ""concepts"": [
              {
                ""id"": ""@conceptID1""
              }
            ]
          }
        }
      }
  }
");
            Assert.True(JToken.DeepEquals(expectedRequestBody, httpClient.PostedBody));

            Assert.True(response.IsSuccessful);
            Assert.AreEqual("Ok", response.Status.Description);
            Assert.AreEqual(response.Get().Name,"some-model-name");
            Assert.AreEqual(response.Get().ModelID,"some-model-id");
            Assert.AreEqual(response.Get().ModelVersion.ID,"some-model-version-id");
        }

        [Test]
        public async Task CreateGenericModelRequestAndResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                postResponse: @"
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
            ClarifaiResponse<IModel<Concept>> response = await client.CreateModelGeneric<Concept>(
                modelID: "@newmodelID",
                name: "@newModelName",
                outputInfo: new ConceptOutputInfo( new List<Concept> {new Concept("@conceptID1")})
            ).ExecuteAsync();

            var expectedRequestBody = JObject.Parse(@"
{
    ""model"":
      {
        ""id"": ""@newmodelID"",
        ""name"": ""@newModelName"",
        ""output_info"": {
            ""output_config"": {
                ""concepts_mutually_exclusive"": false,
                ""closed_environment"": false
            },
          ""data"": {
            ""concepts"": [
              {
                ""id"": ""@conceptID1""
              }
            ]
          }
        }
      }
  }
");
            Assert.True(JToken.DeepEquals(expectedRequestBody, httpClient.PostedBody));

            Assert.True(response.IsSuccessful);
            Assert.AreEqual("Ok", response.Status.Description);
            Assert.AreEqual(response.Get().Name,"some-model-name");
            Assert.AreEqual(response.Get().ModelID,"some-model-id");
            Assert.AreEqual(response.Get().ModelVersion.ID,"some-model-version-id");
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
            ClarifaiResponse<IModel<Concept>> response = await client.TrainModel<Concept>("some-model-id").ExecuteAsync();

            var expectedRequestBody = JObject.Parse(@"
{}
");
            Assert.True(JToken.DeepEquals(expectedRequestBody, httpClient.PostedBody));

            Assert.True(response.IsSuccessful);
            Assert.AreEqual("Ok", response.Status.Description);
            Assert.AreEqual(response.Get().Name,"some-model-name");
            Assert.AreEqual(response.Get().ModelID,"some-model-id");
            Assert.AreEqual(response.Get().ModelVersion.ID,"some-model-version-id");
        }
    }
}
