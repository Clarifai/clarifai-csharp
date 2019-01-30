using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clarifai.API;
using Clarifai.API.Responses;
using Clarifai.DTOs;
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
    public class VariousModelsUnitTests
    {
        [Test]
        public async Task ColorGetModelRequestAndResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                getResponse: @"
{
  ""status"": {
    ""code"": 10000,
    ""description"": ""Ok""
  },
  ""model"": {
    ""id"": ""@modelID"",
    ""name"": ""color"",
    ""created_at"": ""2016-05-11T18:05:45.924367Z"",
    ""app_id"": ""main"",
    ""output_info"": {
      ""data"": {
        ""concepts"": [
          {
            ""id"": ""@conceptID1"",
            ""name"": ""AliceBlue"",
            ""value"": 1,
            ""created_at"": ""2017-06-15T20:40:52.248062Z"",
            ""language"": ""en"",
            ""app_id"": ""main""
          },
          {
            ""id"": ""@conceptID2"",
            ""name"": ""AntiqueWhite"",
            ""value"": 1,
            ""created_at"": ""2017-06-15T20:40:52.248062Z"",
            ""language"": ""en"",
            ""app_id"": ""main""
          }
        ]
      },
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
      ""active_concept_count"": 140,
      ""train_stats"": {}
    },
    ""display_name"": ""Color""
  }
}
");

            var client = new ClarifaiClient(httpClient);
            ClarifaiResponse<IModel<Color>> response =
                await client.GetModel<Color>("@modelID")
                    .ExecuteAsync();
            ColorModel model = (ColorModel) response.Get();

            Assert.AreEqual("/v2/models/@modelID/output_info", httpClient.RequestedUrl);

            Assert.True(response.IsSuccessful);

            Assert.AreEqual("@modelID", model.ModelID);
            Assert.AreEqual("@modelVersionID", model.ModelVersion.ID);

            List<Concept> concepts = model.OutputInfo.Concepts.ToList();

            Assert.AreEqual("@conceptID1", concepts[0].ID);
            Assert.AreEqual("AliceBlue", concepts[0].Name);

            Assert.AreEqual("@conceptID2", concepts[1].ID);
            Assert.AreEqual("AntiqueWhite", concepts[1].Name);
        }

        [Test]
        public async Task ColorPredictRequestAndResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                postResponse: @"
{
  ""status"": {
    ""code"": 10000,
    ""description"": ""Ok""
  },
  ""outputs"": [
    {
      ""id"": ""@outputID"",
      ""status"": {
        ""code"": 10000,
        ""description"": ""Ok""
      },
      ""created_at"": ""2019-01-30T10:17:40.806851955Z"",
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
            ""url"": ""@url""
          }
        }
      },
      ""data"": {
        ""colors"": [
          {
            ""raw_hex"": ""#f2f2f2"",
            ""w3c"": {
              ""hex"": ""#f5f5f5"",
              ""name"": ""WhiteSmoke""
            },
            ""value"": 0.929
          },
          {
            ""raw_hex"": ""#686078"",
            ""w3c"": {
              ""hex"": ""#708090"",
              ""name"": ""SlateGray""
            },
            ""value"": 0.02675
          }
        ]
      }
    }
  ]
}
");

            var client = new ClarifaiClient(httpClient);
            var response = await client.Predict<Color>(
                    "", new ClarifaiURLImage("@url"))
                .ExecuteAsync();
            ClarifaiOutput<Color> output = response.Get();

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

            Assert.AreEqual("@inputID", output.Input.ID);

            Assert.AreEqual("@outputID", output.ID);

            Assert.AreEqual("WhiteSmoke", output.Data[0].Name);
            Assert.AreEqual("#f2f2f2", output.Data[0].RawHex);
            Assert.AreEqual("#f5f5f5", output.Data[0].Hex);
            Assert.AreEqual(0.929m, output.Data[0].Value);

            Assert.AreEqual("SlateGray", output.Data[1].Name);
            Assert.AreEqual("#686078", output.Data[1].RawHex);
            Assert.AreEqual("#708090", output.Data[1].Hex);
            Assert.AreEqual(0.02675m, output.Data[1].Value);
        }

        [Test]
        public async Task DemographicsGetModelRequestAndResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                getResponse: @"
{
  ""status"": {
    ""code"": 10000,
    ""description"": ""Ok""
  },
  ""model"": {
    ""id"": ""@modelID"",
    ""name"": ""demographics"",
    ""created_at"": ""2016-12-23T06:08:44.271674Z"",
    ""app_id"": ""main"",
    ""output_info"": {
      ""data"": {
        ""concepts"": [
          {
            ""id"": ""@conceptID1"",
            ""name"": ""0"",
            ""value"": 1,
            ""created_at"": ""2017-03-28T22:11:16.610298Z"",
            ""language"": ""en"",
            ""app_id"": ""main""
          },
          {
            ""id"": ""@conceptID2"",
            ""name"": ""1"",
            ""value"": 1,
            ""created_at"": ""2017-03-28T22:11:16.610298Z"",
            ""language"": ""en"",
            ""app_id"": ""main""
          }
        ]
      },
      ""type"": ""facedetect"",
      ""type_ext"": ""facedetect-demographics""
    },
    ""model_version"": {
      ""id"": ""@modelVersionID"",
      ""created_at"": ""2016-12-23T06:08:44.271674Z"",
      ""status"": {
        ""code"": 21100,
        ""description"": ""Model trained successfully""
      },
      ""active_concept_count"": 104,
      ""train_stats"": {}
    },
    ""display_name"": ""Demographics""
  }
}
");

            var client = new ClarifaiClient(httpClient);
            ClarifaiResponse<IModel<Demographics>> response =
                await client.GetModel<Demographics>("@modelID")
                    .ExecuteAsync();
            DemographicsModel model = (DemographicsModel) response.Get();

            Assert.AreEqual("/v2/models/@modelID/output_info", httpClient.RequestedUrl);

            Assert.True(response.IsSuccessful);

            Assert.AreEqual("@modelID", model.ModelID);
            Assert.AreEqual("@modelVersionID", model.ModelVersion.ID);

            List<Concept> concepts = model.OutputInfo.Concepts.ToList();

            Assert.AreEqual("@conceptID1", concepts[0].ID);
            Assert.AreEqual("@conceptID2", concepts[1].ID);
        }

        [Test]
        public async Task DemographicsPredictRequestAndResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                postResponse: @"
{
  ""status"": {
    ""code"": 10000,
    ""description"": ""Ok""
  },
  ""outputs"": [
    {
      ""id"": ""@outputID"",
      ""status"": {
        ""code"": 10000,
        ""description"": ""Ok""
      },
      ""created_at"": ""2019-01-30T10:48:22.463688278Z"",
      ""model"": {
        ""id"": ""@modelID"",
        ""name"": ""demographics"",
        ""created_at"": ""2016-12-23T06:08:44.271674Z"",
        ""app_id"": ""main"",
        ""output_info"": {
          ""message"": ""Show output_info with: GET /models/{model_id}/output_info"",
          ""type"": ""facedetect"",
          ""type_ext"": ""facedetect-demographics""
        },
        ""model_version"": {
          ""id"": ""@modelVersionID"",
          ""created_at"": ""2016-12-23T06:08:44.271674Z"",
          ""status"": {
            ""code"": 21100,
            ""description"": ""Model trained successfully""
          },
          ""train_stats"": {}
        },
        ""display_name"": ""Demographics""
      },
      ""input"": {
        ""id"": ""@inputID"",
        ""data"": {
          ""image"": {
            ""url"": ""@url""
          }
        }
      },
      ""data"": {
        ""regions"": [
          {
            ""id"": ""@regionID"",
            ""region_info"": {
              ""bounding_box"": {
                ""top_row"": 0.1,
                ""left_col"": 0.2,
                ""bottom_row"": 0.3,
                ""right_col"": 0.4
              }
            },
            ""data"": {
              ""face"": {
                ""age_appearance"": {
                  ""concepts"": [
                    {
                      ""id"": ""@ageConcept1"",
                      ""name"": ""77"",
                      ""value"": 0.93078935,
                      ""app_id"": ""main""
                    },
                    {
                      ""id"": ""@ageConcept2"",
                      ""name"": ""78"",
                      ""value"": 0.92458177,
                      ""app_id"": ""main""
                    }
                  ]
                },
                ""gender_appearance"": {
                  ""concepts"": [
                    {
                      ""id"": ""@genderConcept1"",
                      ""name"": ""masculine"",
                      ""value"": 0.88848364,
                      ""app_id"": ""main""
                    },
                    {
                      ""id"": ""@genderConcept2"",
                      ""name"": ""feminine"",
                      ""value"": 0.111516364,
                      ""app_id"": ""main""
                    }
                  ]
                },
                ""multicultural_appearance"": {
                  ""concepts"": [
                    {
                      ""id"": ""@culturalConcept1"",
                      ""name"": ""black or african american"",
                      ""value"": 0.9993645,
                      ""app_id"": ""main""
                    },
                    {
                      ""id"": ""@culturalConcept2"",
                      ""name"": ""native hawaiian or pacific islander"",
                      ""value"": 0.011455884,
                      ""app_id"": ""main""
                    }
                  ]
                }
              }
            }
          }
        ]
      }
    }
  ]
}
");

            var client = new ClarifaiClient(httpClient);
            var response = await client.Predict<Demographics>(
                    "@modelID", new ClarifaiURLImage("@url"))
                .ExecuteAsync();
            ClarifaiOutput<Demographics> output = response.Get();

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
            Assert.AreEqual("/v2/models/@modelID/outputs", httpClient.RequestedUrl);

            Assert.True(response.IsSuccessful);

            Assert.AreEqual("@inputID", output.Input.ID);
            Assert.AreEqual("@outputID", output.ID);

            Demographics demo = output.Data[0];

            Assert.AreEqual(new Crop(0.1m, 0.2m, 0.3m, 0.4m), demo.Crop);

            Assert.AreEqual("@ageConcept1", demo.AgeAppearanceConcepts[0].ID);
            Assert.AreEqual("@ageConcept2", demo.AgeAppearanceConcepts[1].ID);

            Assert.AreEqual("@genderConcept1", demo.GenderAppearanceConcepts[0].ID);
            Assert.AreEqual("@genderConcept2", demo.GenderAppearanceConcepts[1].ID);

            Assert.AreEqual("@culturalConcept1", demo.MulticulturalAppearanceConcepts[0].ID);
            Assert.AreEqual("@culturalConcept2", demo.MulticulturalAppearanceConcepts[1].ID);
        }

        [Test]
        public async Task EmbeddingGetModelRequestAndResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                getResponse: @"
{
  ""status"": {
    ""code"": 10000,
    ""description"": ""Ok""
  },
  ""model"": {
    ""id"": ""@modelID"",
    ""name"": ""general"",
    ""created_at"": ""2016-06-17T22:01:04.144732Z"",
    ""app_id"": ""main"",
    ""output_info"": {
      ""type"": ""embed"",
      ""type_ext"": ""embed""
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
    ""display_name"": ""General Embedding""
  }
}
");

            var client = new ClarifaiClient(httpClient);
            ClarifaiResponse<IModel<Embedding>> response =
                await client.GetModel<Embedding>("@modelID")
                    .ExecuteAsync();
            EmbeddingModel model = (EmbeddingModel) response.Get();

            Assert.AreEqual("/v2/models/@modelID/output_info", httpClient.RequestedUrl);

            Assert.True(response.IsSuccessful);

            Assert.AreEqual("@modelID", model.ModelID);
            Assert.AreEqual("@modelVersionID", model.ModelVersion.ID);
        }

        [Test]
        public async Task EmbeddingPredictRequestAndResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                postResponse: @"
{
  ""status"": {
    ""code"": 10000,
    ""description"": ""Ok""
  },
  ""outputs"": [
    {
      ""id"": ""@outputID"",
      ""status"": {
        ""code"": 10000,
        ""description"": ""Ok""
      },
      ""created_at"": ""2019-01-30T15:53:21.949095428Z"",
      ""model"": {
        ""id"": ""@modelID"",
        ""name"": ""general"",
        ""created_at"": ""2016-06-17T22:01:04.144732Z"",
        ""app_id"": ""main"",
        ""output_info"": {
          ""message"": ""Show output_info with: GET /models/{model_id}/output_info"",
          ""type"": ""embed"",
          ""type_ext"": ""embed""
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
        ""display_name"": ""General Embedding""
      },
      ""input"": {
        ""id"": ""@inputID"",
        ""data"": {
          ""image"": {
            ""url"": ""@url""
          }
        }
      },
      ""data"": {
        ""embeddings"": [
          {
            ""vector"": [
              0.1,
              0.2,
              0.3
            ],
            ""num_dimensions"": 3
          }
        ]
      }
    }
  ]
}
");

            var client = new ClarifaiClient(httpClient);
            var response = await client.Predict<Embedding>(
                    "@modelID", new ClarifaiURLImage("@url"))
                .ExecuteAsync();
            ClarifaiOutput<Embedding> output = response.Get();

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
            Assert.AreEqual("/v2/models/@modelID/outputs", httpClient.RequestedUrl);

            Assert.True(response.IsSuccessful);

            Assert.AreEqual("@inputID", output.Input.ID);
            Assert.AreEqual("@outputID", output.ID);

            Embedding demo = output.Data[0];
            Assert.AreEqual(3, demo.NumDimensions);
            Assert.AreEqual(new[] {0.1m, 0.2m, 0.3m}, demo.Vector);
        }

        [Test]
        public async Task FaceConceptsGetModelRequestAndResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                getResponse: @"
{
  ""status"": {
    ""code"": 10000,
    ""description"": ""Ok""
  },
  ""model"": {
    ""id"": ""@modelID"",
    ""name"": ""@modelName"",
    ""created_at"": ""2017-05-16T19:20:38.733764Z"",
    ""app_id"": ""main"",
    ""output_info"": {
      ""data"": {
        ""concepts"": [
          {
            ""id"": ""@conceptID1"",
            ""name"": ""deborah kagan"",
            ""value"": 1,
            ""created_at"": ""2016-10-25T19:28:34.869737Z"",
            ""language"": ""en"",
            ""app_id"": ""main""
          },
          {
            ""id"": ""@conceptID2"",
            ""name"": ""deborah kara unger"",
            ""value"": 1,
            ""created_at"": ""2016-10-25T19:28:34.869737Z"",
            ""language"": ""en"",
            ""app_id"": ""main""
          },
          {
            ""id"": ""@conceptID3"",
            ""name"": ""deborah kerr"",
            ""value"": 1,
            ""created_at"": ""2016-10-25T19:28:34.869737Z"",
            ""language"": ""en"",
            ""app_id"": ""main""
          }
        ]
      },
      ""type"": ""concept"",
      ""type_ext"": ""facedetect-identity""
    },
    ""model_version"": {
      ""id"": ""@modelVersionID"",
      ""created_at"": ""2016-10-25T19:30:38.541073Z"",
      ""status"": {
        ""code"": 21100,
        ""description"": ""Model trained successfully""
      },
      ""active_concept_count"": 10553,
      ""train_stats"": {}
    },
    ""display_name"": ""Celebrity""
  }
}
");

            var client = new ClarifaiClient(httpClient);
            ClarifaiResponse<IModel<FaceConcepts>> response =
                await client.GetModel<FaceConcepts>("@modelID")
                    .ExecuteAsync();
            FaceConceptsModel model = (FaceConceptsModel) response.Get();

            Assert.AreEqual("/v2/models/@modelID/output_info", httpClient.RequestedUrl);

            Assert.True(response.IsSuccessful);

            Assert.AreEqual("@modelID", model.ModelID);
            Assert.AreEqual("@modelVersionID", model.ModelVersion.ID);

            var concepts = model.OutputInfo.Concepts.ToList();
            Assert.AreEqual("@conceptID1", concepts[0].ID);
            Assert.AreEqual("@conceptID2", concepts[1].ID);
            Assert.AreEqual("@conceptID3", concepts[2].ID);
        }

        [Test]
        public async Task FaceConceptsPredictRequestAndResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                postResponse: @"
{
  ""status"": {
    ""code"": 10000,
    ""description"": ""Ok""
  },
  ""outputs"": [
    {
      ""id"": ""@outputID"",
      ""status"": {
        ""code"": 10000,
        ""description"": ""Ok""
      },
      ""created_at"": ""2019-01-30T16:12:05.692036673Z"",
      ""model"": {
        ""id"": ""@modelID"",
        ""name"": ""celeb-v1.3"",
        ""created_at"": ""2016-10-25T19:30:38.541073Z"",
        ""app_id"": ""main"",
        ""output_info"": {
          ""message"": ""Show output_info with: GET /models/{model_id}/output_info"",
          ""type"": ""concept"",
          ""type_ext"": ""facedetect-identity""
        },
        ""model_version"": {
          ""id"": ""@modelVersionID"",
          ""created_at"": ""2016-10-25T19:30:38.541073Z"",
          ""status"": {
            ""code"": 21100,
            ""description"": ""Model trained successfully""
          },
          ""train_stats"": {}
        },
        ""display_name"": ""Celebrity""
      },
      ""input"": {
        ""id"": ""@inputID"",
        ""data"": {
          ""image"": {
            ""url"": ""@url""
          }
        }
      },
      ""data"": {
        ""regions"": [
          {
            ""id"": ""@regionID1"",
            ""region_info"": {
              ""bounding_box"": {
                ""top_row"": 0.1,
                ""left_col"": 0.2,
                ""bottom_row"": 0.3,
                ""right_col"": 0.4
              }
            },
            ""data"": {
              ""face"": {
                ""identity"": {
                  ""concepts"": [
                    {
                      ""id"": ""@conceptID11"",
                      ""name"": ""suri cruise"",
                      ""value"": 0.00035361873,
                      ""app_id"": ""main""
                    },
                    {
                      ""id"": ""@conceptID12"",
                      ""name"": ""daphne blunt"",
                      ""value"": 0.00023266333,
                      ""app_id"": ""main""
                    }
                  ]
                }
              }
            }
          },
          {
            ""id"": ""@regionID2"",
            ""region_info"": {
              ""bounding_box"": {
                ""top_row"": 0.5,
                ""left_col"": 0.6,
                ""bottom_row"": 0.7,
                ""right_col"": 0.8
              }
            },
            ""data"": {
              ""face"": {
                ""identity"": {
                  ""concepts"": [
                    {
                      ""id"": ""@conceptID21"",
                      ""name"": ""shiloh jolie-pitt"",
                      ""value"": 0.0010930675,
                      ""app_id"": ""main""
                    },
                    {
                      ""id"": ""@conceptID22"",
                      ""name"": ""suri cruise"",
                      ""value"": 0.000494421,
                      ""app_id"": ""main""
                    }
                  ]
                }
              }
            }
          }
        ]
      }
    }
  ]
}
");

            var client = new ClarifaiClient(httpClient);
            var response = await client.Predict<FaceConcepts>(
                    "@modelID", new ClarifaiURLImage("@url"))
                .ExecuteAsync();
            ClarifaiOutput<FaceConcepts> output = response.Get();

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
            Assert.AreEqual("/v2/models/@modelID/outputs", httpClient.RequestedUrl);

            Assert.True(response.IsSuccessful);

            Assert.AreEqual("@inputID", output.Input.ID);
            Assert.AreEqual("@outputID", output.ID);

            FaceConcepts faceConcepts1 = output.Data[0];
            Assert.AreEqual("@conceptID11", faceConcepts1.Concepts[0].ID);
            Assert.AreEqual("@conceptID12", faceConcepts1.Concepts[1].ID);

            FaceConcepts faceConcepts2 = output.Data[1];
            Assert.AreEqual("@conceptID21", faceConcepts2.Concepts[0].ID);
            Assert.AreEqual("@conceptID22", faceConcepts2.Concepts[1].ID);
        }

        [Test]
        public async Task FaceDetectionGetModelRequestAndResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                getResponse: @"
{
  ""status"": {
    ""code"": 10000,
    ""description"": ""Ok""
  },
  ""model"": {
    ""id"": ""@modelID"",
    ""name"": ""face"",
    ""created_at"": ""2016-10-25T19:30:38.541073Z"",
    ""app_id"": ""main"",
    ""output_info"": {
      ""type"": ""facedetect"",
      ""type_ext"": ""facedetect""
    },
    ""model_version"": {
      ""id"": ""@modelVersionID"",
      ""created_at"": ""2019-01-17T19:45:49.087547Z"",
      ""status"": {
        ""code"": 21100,
        ""description"": ""Model trained successfully""
      },
      ""train_stats"": {}
    },
    ""display_name"": ""Face Detection""
  }
}
");

            var client = new ClarifaiClient(httpClient);
            ClarifaiResponse<IModel<FaceDetection>> response =
                await client.GetModel<FaceDetection>("@modelID")
                    .ExecuteAsync();
            FaceDetectionModel model = (FaceDetectionModel) response.Get();

            Assert.AreEqual("/v2/models/@modelID/output_info", httpClient.RequestedUrl);

            Assert.True(response.IsSuccessful);

            Assert.AreEqual("@modelID", model.ModelID);
            Assert.AreEqual("@modelVersionID", model.ModelVersion.ID);
        }

        [Test]
        public async Task FaceDetectionPredictRequestAndResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                postResponse: @"
{
  ""status"": {
    ""code"": 10000,
    ""description"": ""Ok""
  },
  ""outputs"": [
    {
      ""id"": ""@outputID"",
      ""status"": {
        ""code"": 10000,
        ""description"": ""Ok""
      },
      ""created_at"": ""2019-01-30T16:23:07.989685196Z"",
      ""model"": {
        ""id"": ""@modelID"",
        ""name"": ""face"",
        ""created_at"": ""2016-10-25T19:30:38.541073Z"",
        ""app_id"": ""main"",
        ""output_info"": {
          ""message"": ""Show output_info with: GET /models/{model_id}/output_info"",
          ""type"": ""facedetect"",
          ""type_ext"": ""facedetect""
        },
        ""model_version"": {
          ""id"": ""@modelVersionID"",
          ""created_at"": ""2019-01-17T19:45:49.087547Z"",
          ""status"": {
            ""code"": 21100,
            ""description"": ""Model trained successfully""
          },
          ""train_stats"": {}
        },
        ""display_name"": ""Face Detection""
      },
      ""input"": {
        ""id"": ""@inputID"",
        ""data"": {
          ""image"": {
            ""url"": ""@url""
          }
        }
      },
      ""data"": {
        ""regions"": [
          {
            ""id"": ""@regionID1"",
            ""region_info"": {
              ""bounding_box"": {
                ""top_row"": 0.1,
                ""left_col"": 0.2,
                ""bottom_row"": 0.3,
                ""right_col"": 0.4
              }
            }
          },
          {
            ""id"": ""@regionID2"",
            ""region_info"": {
              ""bounding_box"": {
                ""top_row"": 0.5,
                ""left_col"": 0.6,
                ""bottom_row"": 0.7,
                ""right_col"": 0.8
              }
            }
          }
        ]
      }
    }
  ]
}
");

            var client = new ClarifaiClient(httpClient);
            var response = await client.Predict<FaceDetection>(
                    "@modelID", new ClarifaiURLImage("@url"))
                .ExecuteAsync();
            ClarifaiOutput<FaceDetection> output = response.Get();

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
            Assert.AreEqual("/v2/models/@modelID/outputs", httpClient.RequestedUrl);

            Assert.True(response.IsSuccessful);

            Assert.AreEqual("@inputID", output.Input.ID);
            Assert.AreEqual("@outputID", output.ID);

            Assert.AreEqual(new Crop(0.1m, 0.2m, 0.3m, 0.4m), output.Data[0].Crop);
            Assert.AreEqual(new Crop(0.5m, 0.6m, 0.7m, 0.8m), output.Data[1].Crop);
        }

        [Test]
        public async Task FaceEmbeddingGetModelRequestAndResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                getResponse: @"
{
  ""status"": {
    ""code"": 10000,
    ""description"": ""Ok""
  },
  ""model"": {
    ""id"": ""@modelID"",
    ""name"": ""face"",
    ""created_at"": ""2016-10-25T19:30:38.541073Z"",
    ""app_id"": ""main"",
    ""output_info"": {
      ""type"": ""embed"",
      ""type_ext"": ""detect-embed""
    },
    ""model_version"": {
      ""id"": ""@modelVersionID"",
      ""created_at"": ""2016-10-25T19:30:38.541073Z"",
      ""status"": {
        ""code"": 21100,
        ""description"": ""Model trained successfully""
      },
      ""train_stats"": {}
    },
    ""display_name"": ""Face Embedding""
  }
}
");

            var client = new ClarifaiClient(httpClient);
            ClarifaiResponse<IModel<FaceEmbedding>> response =
                await client.GetModel<FaceEmbedding>("@modelID")
                    .ExecuteAsync();
            FaceEmbeddingModel model = (FaceEmbeddingModel) response.Get();

            Assert.AreEqual("/v2/models/@modelID/output_info", httpClient.RequestedUrl);

            Assert.True(response.IsSuccessful);

            Assert.AreEqual("@modelID", model.ModelID);
            Assert.AreEqual("@modelVersionID", model.ModelVersion.ID);
        }

        [Test]
        public async Task FaceEmbeddingPredictRequestAndResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                postResponse: @"
{
  ""status"": {
    ""code"": 10000,
    ""description"": ""Ok""
  },
  ""outputs"": [
    {
      ""id"": ""@outputID"",
      ""status"": {
        ""code"": 10000,
        ""description"": ""Ok""
      },
      ""created_at"": ""2019-01-30T16:28:58.518441695Z"",
      ""model"": {
        ""id"": ""@modelID"",
        ""name"": ""face"",
        ""created_at"": ""2016-10-25T19:30:38.541073Z"",
        ""app_id"": ""main"",
        ""output_info"": {
          ""message"": ""Show output_info with: GET /models/{model_id}/output_info"",
          ""type"": ""embed"",
          ""type_ext"": ""detect-embed""
        },
        ""model_version"": {
          ""id"": ""@modelVersionID"",
          ""created_at"": ""2016-10-25T19:30:38.541073Z"",
          ""status"": {
            ""code"": 21100,
            ""description"": ""Model trained successfully""
          },
          ""train_stats"": {}
        },
        ""display_name"": ""Face Embedding""
      },
      ""input"": {
        ""id"": ""@inputID"",
        ""data"": {
          ""image"": {
            ""url"": ""@url""
          }
        }
      },
      ""data"": {
        ""regions"": [
          {
            ""id"": ""@regionID"",
            ""region_info"": {
              ""bounding_box"": {
                ""top_row"": 0.1,
                ""left_col"": 0.2,
                ""bottom_row"": 0.3,
                ""right_col"": 0.4
              }
            },
            ""data"": {
              ""embeddings"": [
                {
                  ""vector"": [
                    0.1,
                    0.2,
                    0.3
                  ],
                  ""num_dimensions"": 3
                }
              ]
            }
          }
        ]
      }
    }
  ]
}
");

            var client = new ClarifaiClient(httpClient);
            var response = await client.Predict<FaceEmbedding>(
                    "@modelID", new ClarifaiURLImage("@url"))
                .ExecuteAsync();
            ClarifaiOutput<FaceEmbedding> output = response.Get();

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
            Assert.AreEqual("/v2/models/@modelID/outputs", httpClient.RequestedUrl);

            Assert.True(response.IsSuccessful);

            Assert.AreEqual("@inputID", output.Input.ID);
            Assert.AreEqual("@outputID", output.ID);

            FaceEmbedding faceEmbedding = output.Data[0];
            Assert.AreEqual(new Crop(0.1m, 0.2m, 0.3m, 0.4m), faceEmbedding.Crop);
            Assert.AreEqual(new [] {0.1m, 0.2m, 0.3m}, faceEmbedding.Embeddings[0].Vector);
        }

        [Test]
        public async Task FocusGetModelRequestAndResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                getResponse: @"
{
  ""status"": {
    ""code"": 10000,
    ""description"": ""Ok""
  },
  ""model"": {
    ""id"": ""@modelID"",
    ""name"": ""focus"",
    ""created_at"": ""2017-03-06T22:57:00.660603Z"",
    ""app_id"": ""main"",
    ""output_info"": {
      ""type"": ""blur"",
      ""type_ext"": ""focus""
    },
    ""model_version"": {
      ""id"": ""@modelVersionID"",
      ""created_at"": ""2017-03-06T22:57:00.684652Z"",
      ""status"": {
        ""code"": 21100,
        ""description"": ""Model trained successfully""
      },
      ""train_stats"": {}
    },
    ""display_name"": ""Focus""
  }
}
");

            var client = new ClarifaiClient(httpClient);
            ClarifaiResponse<IModel<Focus>> response =
                await client.GetModel<Focus>("@modelID")
                    .ExecuteAsync();
            FocusModel model = (FocusModel) response.Get();

            Assert.AreEqual("/v2/models/@modelID/output_info", httpClient.RequestedUrl);

            Assert.True(response.IsSuccessful);

            Assert.AreEqual("@modelID", model.ModelID);
            Assert.AreEqual("@modelVersionID", model.ModelVersion.ID);
        }

        [Test]
        public async Task FocusPredictRequestAndResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                postResponse: @"
{
  ""status"": {
    ""code"": 10000,
    ""description"": ""Ok""
  },
  ""outputs"": [
    {
      ""id"": ""@outputID"",
      ""status"": {
        ""code"": 10000,
        ""description"": ""Ok""
      },
      ""created_at"": ""2019-01-30T16:36:40.235988209Z"",
      ""model"": {
        ""id"": ""@modelID"",
        ""name"": ""focus"",
        ""created_at"": ""2017-03-06T22:57:00.660603Z"",
        ""app_id"": ""main"",
        ""output_info"": {
          ""message"": ""Show output_info with: GET /models/{model_id}/output_info"",
          ""type"": ""blur"",
          ""type_ext"": ""focus""
        },
        ""model_version"": {
          ""id"": ""@modelVersionID"",
          ""created_at"": ""2017-03-06T22:57:00.684652Z"",
          ""status"": {
            ""code"": 21100,
            ""description"": ""Model trained successfully""
          },
          ""train_stats"": {}
        },
        ""display_name"": ""Focus""
      },
      ""input"": {
        ""id"": ""@inputID"",
        ""data"": {
          ""image"": {
            ""url"": ""@url""
          }
        }
      },
      ""data"": {
        ""focus"": {
          ""density"": 0.7,
          ""value"": 0.8
        },
        ""regions"": [
          {
            ""id"": ""@regionID"",
            ""region_info"": {
              ""bounding_box"": {
                ""top_row"": 0.1,
                ""left_col"": 0.2,
                ""bottom_row"": 0.3,
                ""right_col"": 0.4
              }
            },
            ""data"": {
              ""focus"": {
                ""density"": 0.5,
                ""value"": 0.6
              }
            }
          }
        ]
      }
    }
  ]
}
");

            var client = new ClarifaiClient(httpClient);
            var response = await client.Predict<Focus>(
                    "@modelID", new ClarifaiURLImage("@url"))
                .ExecuteAsync();
            ClarifaiOutput<Focus> output = response.Get();

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
            Assert.AreEqual("/v2/models/@modelID/outputs", httpClient.RequestedUrl);

            Assert.True(response.IsSuccessful);

            Assert.AreEqual("@inputID", output.Input.ID);
            Assert.AreEqual("@outputID", output.ID);

            Focus focus = output.Data[0];
            Assert.AreEqual(new Crop(0.1m, 0.2m, 0.3m, 0.4m), focus.Crop);
            Assert.AreEqual(0.5m, focus.Density);
            Assert.AreEqual(0.8m, focus.Value);
            // TODO(Rok) HIGH: Correctly expose Crop both density/value numbers.
        }

        [Test]
        public async Task LogoGetModelRequestAndResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                getResponse: @"
{
  ""status"": {
    ""code"": 10000,
    ""description"": ""Ok""
  },
  ""model"": {
    ""id"": ""@modelID"",
    ""name"": ""logo"",
    ""created_at"": ""2017-03-06T22:57:00.707216Z"",
    ""app_id"": ""main"",
    ""output_info"": {
      ""data"": {
        ""concepts"": [
          {
            ""id"": ""@conceptID1"",
            ""name"": ""3M"",
            ""value"": 1,
            ""created_at"": ""2017-05-22T18:13:37.682503Z"",
            ""language"": ""en"",
            ""app_id"": ""main""
          },
          {
            ""id"": ""@conceptID2"",
            ""name"": ""3Musketeers"",
            ""value"": 1,
            ""created_at"": ""2017-05-22T18:13:37.682503Z"",
            ""language"": ""en"",
            ""app_id"": ""main""
          }
        ]
      },
      ""type"": ""concept"",
      ""type_ext"": ""detection""
    },
    ""model_version"": {
      ""id"": ""@modelVersionID"",
      ""created_at"": ""2017-03-06T22:57:05.625525Z"",
      ""status"": {
        ""code"": 21100,
        ""description"": ""Model trained successfully""
      },
      ""active_concept_count"": 561,
      ""train_stats"": {}
    },
    ""display_name"": ""Logo""
  }
}
");

            var client = new ClarifaiClient(httpClient);
            ClarifaiResponse<IModel<Logo>> response =
                await client.GetModel<Logo>("@modelID")
                    .ExecuteAsync();
            LogoModel model = (LogoModel) response.Get();

            Assert.AreEqual("/v2/models/@modelID/output_info", httpClient.RequestedUrl);

            Assert.True(response.IsSuccessful);

            Assert.AreEqual("@modelID", model.ModelID);
            Assert.AreEqual("@modelVersionID", model.ModelVersion.ID);

            List<Concept> concepts = model.OutputInfo.Concepts.ToList();
            Assert.AreEqual("@conceptID1", concepts[0].ID);
            Assert.AreEqual("@conceptID2", concepts[1].ID);
        }

        [Test]
        public async Task LogoPredictRequestAndResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                postResponse: @"
{
  ""status"": {
    ""code"": 10000,
    ""description"": ""Ok""
  },
  ""outputs"": [
    {
      ""id"": ""@outputID"",
      ""status"": {
        ""code"": 10000,
        ""description"": ""Ok""
      },
      ""created_at"": ""2019-01-30T16:45:27.083256618Z"",
      ""model"": {
        ""id"": ""@modelID"",
        ""name"": ""logo"",
        ""created_at"": ""2017-03-06T22:57:00.707216Z"",
        ""app_id"": ""main"",
        ""output_info"": {
          ""message"": ""Show output_info with: GET /models/{model_id}/output_info"",
          ""type"": ""concept"",
          ""type_ext"": ""detection""
        },
        ""model_version"": {
          ""id"": ""@modelVersionID"",
          ""created_at"": ""2017-03-06T22:57:05.625525Z"",
          ""status"": {
            ""code"": 21100,
            ""description"": ""Model trained successfully""
          },
          ""train_stats"": {}
        },
        ""display_name"": ""Logo""
      },
      ""input"": {
        ""id"": ""@inputID"",
        ""data"": {
          ""image"": {
            ""url"": ""@url""
          }
        }
      },
      ""data"": {
        ""regions"": [
          {
            ""id"": ""@regionID"",
            ""region_info"": {
              ""bounding_box"": {
                ""top_row"": 0.1,
                ""left_col"": 0.2,
                ""bottom_row"": 0.3,
                ""right_col"": 0.4
              }
            },
            ""data"": {
              ""concepts"": [
                {
                  ""id"": ""@conceptID1"",
                  ""name"": ""I Can't Believe It's Not Butter"",
                  ""value"": 0.092014045
                },
                {
                  ""id"": ""@conceptID2"",
                  ""name"": ""Pepsi"",
                  ""value"": 0.06332539
                }
              ]
            }
          }
        ]
      }
    }
  ]
}
");

            var client = new ClarifaiClient(httpClient);
            var response = await client.Predict<Logo>(
                    "@modelID", new ClarifaiURLImage("@url"))
                .ExecuteAsync();
            ClarifaiOutput<Logo> output = response.Get();

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
            Assert.AreEqual("/v2/models/@modelID/outputs", httpClient.RequestedUrl);

            Assert.True(response.IsSuccessful);

            Assert.AreEqual("@inputID", output.Input.ID);
            Assert.AreEqual("@outputID", output.ID);

            Logo logo = output.Data[0];
            Assert.AreEqual(new Crop(0.1m, 0.2m, 0.3m, 0.4m), logo.Crop);

            Assert.AreEqual("@conceptID1", logo.Concepts[0].ID);
            Assert.AreEqual("@conceptID2", logo.Concepts[1].ID);
        }

        [Test]
        public async Task VideoGetModelRequestAndResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                getResponse: @"
{
  ""status"": {
    ""code"": 10000,
    ""description"": ""Ok""
  },
  ""model"": {
    ""id"": ""@modelID"",
    ""name"": ""nsfw-v1.0"",
    ""created_at"": ""2016-09-17T22:18:59.955626Z"",
    ""app_id"": ""main"",
    ""output_info"": {
      ""data"": {
        ""concepts"": [
          {
            ""id"": ""@conceptID1"",
            ""name"": ""nsfw"",
            ""value"": 1,
            ""created_at"": ""2016-09-17T22:18:50.338072Z"",
            ""language"": ""en"",
            ""app_id"": ""main""
          },
          {
            ""id"": ""@conceptID2"",
            ""name"": ""sfw"",
            ""value"": 1,
            ""created_at"": ""2016-09-17T22:18:50.338072Z"",
            ""language"": ""en"",
            ""app_id"": ""main""
          }
        ]
      },
      ""type"": ""concept"",
      ""type_ext"": ""concept""
    },
    ""model_version"": {
      ""id"": ""@modelVersionID"",
      ""created_at"": ""2018-01-23T19:25:09.618692Z"",
      ""status"": {
        ""code"": 21100,
        ""description"": ""Model trained successfully""
      },
      ""active_concept_count"": 2,
      ""train_stats"": {}
    },
    ""display_name"": ""NSFW""
  }
}
");

            var client = new ClarifaiClient(httpClient);
            ClarifaiResponse<IModel<Frame>> response =
                await client.GetModel<Frame>("@modelID")
                    .ExecuteAsync();
            VideoModel model = (VideoModel) response.Get();

            Assert.AreEqual("/v2/models/@modelID/output_info", httpClient.RequestedUrl);

            Assert.True(response.IsSuccessful);

            Assert.AreEqual("@modelID", model.ModelID);
            Assert.AreEqual("@modelVersionID", model.ModelVersion.ID);

            List<Concept> concepts = model.OutputInfo.Concepts.ToList();
            Assert.AreEqual("@conceptID1", concepts[0].ID);
            Assert.AreEqual("@conceptID2", concepts[1].ID);
        }

        [Test]
        public async Task VideoPredictRequestAndResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                postResponse: @"
{
  ""status"": {
    ""code"": 10000,
    ""description"": ""Ok""
  },
  ""outputs"": [
    {
      ""id"": ""@outputID"",
      ""status"": {
        ""code"": 10000,
        ""description"": ""Ok""
      },
      ""created_at"": ""2019-01-30T16:52:30.993694779Z"",
      ""model"": {
        ""id"": ""@modelID"",
        ""name"": ""nsfw-v1.0"",
        ""created_at"": ""2016-09-17T22:18:59.955626Z"",
        ""app_id"": ""main"",
        ""output_info"": {
          ""message"": ""Show output_info with: GET /models/{model_id}/output_info"",
          ""type"": ""concept"",
          ""type_ext"": ""concept""
        },
        ""model_version"": {
          ""id"": ""@modelVersionID"",
          ""created_at"": ""2018-01-23T19:25:09.618692Z"",
          ""status"": {
            ""code"": 21100,
            ""description"": ""Model trained successfully""
          },
          ""train_stats"": {}
        },
        ""display_name"": ""NSFW""
      },
      ""input"": {
        ""id"": ""@inputID"",
        ""data"": {
          ""video"": {
            ""url"": ""@url""
          }
        }
      },
      ""data"": {
        ""frames"": [
          {
            ""frame_info"": {
              ""index"": 0,
              ""time"": 0
            },
            ""data"": {
              ""concepts"": [
                {
                  ""id"": ""@conceptID1"",
                  ""name"": ""sfw"",
                  ""value"": 0.99452126,
                  ""app_id"": ""main""
                },
                {
                  ""id"": ""@conceptID2"",
                  ""name"": ""nsfw"",
                  ""value"": 0.005478708,
                  ""app_id"": ""main""
                }
              ]
            }
          }
        ]
      }
    }
  ]
}
");

            var client = new ClarifaiClient(httpClient);
            var response = await client.Predict<Frame>(
                    "@modelID", new ClarifaiURLImage("@url"))
                .ExecuteAsync();
            ClarifaiOutput<Frame> output = response.Get();

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
            Assert.AreEqual("/v2/models/@modelID/outputs", httpClient.RequestedUrl);

            Assert.True(response.IsSuccessful);

            Assert.AreEqual("@inputID", output.Input.ID);
            Assert.AreEqual("@outputID", output.ID);

            Frame frame = output.Data[0];
            Assert.AreEqual("@conceptID1", frame.Concepts[0].ID);
            Assert.AreEqual("@conceptID2", frame.Concepts[1].ID);
        }
    }
}
