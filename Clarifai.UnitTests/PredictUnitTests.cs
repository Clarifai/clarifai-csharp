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

            Assert.AreEqual("@modelID", output.Model.ModelID);
            Assert.AreEqual("@modelName", output.Model.Name);
            Assert.AreEqual("@modelVersionID", output.Model.ModelVersion.ID);
            Assert.AreEqual("concept", output.Model.OutputInfo.TypeExt);
        }

        [Test]
        public async Task ConceptPredictRequestWithArgumentsAndResponseShouldBeCorrect()
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
      ""created_at"": ""2019-01-29T17:15:32.450063489Z"",
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
          },
          ""train_stats"": {}
        },
        ""display_name"": ""General""
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
        ""concepts"": [
          {
            ""id"": ""@conceptID1"",
            ""name"": ""menschen"",
            ""value"": 0.9963381,
            ""app_id"": ""main""
          },
          {
            ""id"": ""@conceptID2"",
            ""name"": ""ein"",
            ""value"": 0.9879057,
            ""app_id"": ""main""
          },
          {
            ""id"": ""@conceptID3"",
            ""name"": ""Porträt"",
            ""value"": 0.98490834,
            ""app_id"": ""main""
          }
        ]
      }
    }
  ]
}
");

            var client = new ClarifaiClient(httpClient);
            var response = await client.Predict<Concept>(
                    "",
                    new ClarifaiURLImage("@url"),
                    minValue: 0.98m,
                    maxConcepts: 3,
                    language: "de")
                .ExecuteAsync();
            ClarifaiOutput<Concept> output = response.Get();

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
  ],
  ""model"": {
    ""output_info"": {
      ""output_config"": {
        ""language"": ""de"",
        ""max_concepts"": 3,
        ""min_value"": 0.98
      }
    }
  }
}
");
            Assert.True(JToken.DeepEquals(expectedRequestBody, httpClient.PostedBody));

            Assert.True(response.IsSuccessful);

            Assert.AreEqual("@inputID", output.Input.ID);

            Assert.AreEqual("@outputID", output.ID);
            Assert.AreEqual("@conceptID1", output.Data[0].ID);

            Assert.AreEqual("@modelID", output.Model.ModelID);
            Assert.AreEqual("@modelName", output.Model.Name);
            Assert.AreEqual("@modelVersionID", output.Model.ModelVersion.ID);
            Assert.AreEqual("concept", output.Model.OutputInfo.TypeExt);
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
            var response = await client.Predict<Concept>(
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
      ""data"": {
        ""image"": {
          ""url"": ""@url1""
        }
      }
    },
    {
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

        [Test]
        public async Task ConceptBatchPredictRequestWithArgumentsAndResponseShouldBeCorrect()
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
      ""id"": ""@outputID1"",
      ""status"": {
        ""code"": 10000,
        ""description"": ""Ok""
      },
      ""created_at"": ""2019-01-29T16:45:43.793810775Z"",
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
      ""input"": {
        ""id"": ""@inputID1"",
        ""data"": {
          ""image"": {
            ""url"": ""https://clarifai.com/developer/static/images/model-samples/celeb-001.jpg""
          }
        }
      },
      ""data"": {
        ""concepts"": [
          {
            ""id"": ""@conceptID11"",
            ""name"": ""menschen"",
            ""value"": 0.9963381,
            ""app_id"": ""main""
          },
          {
            ""id"": ""@conceptID12"",
            ""name"": ""ein"",
            ""value"": 0.9879057,
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
      ""created_at"": ""2019-01-29T16:45:43.793810775Z"",
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
      ""input"": {
        ""id"": ""@inputID2"",
        ""data"": {
          ""image"": {
            ""url"": ""https://clarifai.com/developer/static/images/model-samples/apparel-001.jpg""
          }
        }
      },
      ""data"": {
        ""concepts"": [
          {
            ""id"": ""@conceptID21"",
            ""name"": ""brillen und kontaktlinsen"",
            ""value"": 0.99984586,
            ""app_id"": ""main""
          },
          {
            ""id"": ""@conceptID22"",
            ""name"": ""linse"",
            ""value"": 0.999823,
            ""app_id"": ""main""
          }
        ]
      }
    }
  ]
}
");

            var client = new ClarifaiClient(httpClient);
            var response = await client.Predict<Concept>(
                    "",
                    new List<IClarifaiInput>
                    {
                        new ClarifaiURLImage("@url1"), new ClarifaiURLImage("@url2")
                    },
                    language: "de",
                    maxConcepts: 2,
                    minValue: 0.98m)
                .ExecuteAsync();
            List<ClarifaiOutput<Concept>> outputs = response.Get();

            var expectedRequestBody = JObject.Parse(@"
{
  ""inputs"": [
    {
      ""data"": {
        ""image"": {
          ""url"": ""@url1""
        }
      }
    },
    {
      ""data"": {
        ""image"": {
          ""url"": ""@url2""
        }
      }
    }
  ],
  ""model"": {
    ""output_info"": {
      ""output_config"": {
        ""language"": ""de"",
        ""max_concepts"": 2,
        ""min_value"": 0.98
      }
    }
  }
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


        // To be future-proof against expansion, response objects with unknown fields should be
        // parsed correctly and unknown fields ignored.
        [Test]
        public async Task ConceptPredictWithUnknownResponseFieldsShouldSucceed()
        {
            var httpClient = new FkClarifaiHttpClient(
                postResponse: @"
{
    ""status"": {
        ""code"": 10000,
        ""description"": ""Ok"",
        ""unknown_field"": ""val""
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
                ""type_ext"": ""concept"",
                ""unknown_field"": ""val""
            },
            ""model_version"": {
                ""id"": ""@modelVersionID"",
                ""created_at"": ""2016-07-13T01:19:12.147644Z"",
                ""status"": {
                    ""code"": 21100,
                    ""description"": ""Model trained successfully""
                },
                ""unknown_field"": ""val""
            },
            ""display_name"": ""@modelDisplayName"",
            ""unknown_field"": ""val""
        },
        ""input"": {
            ""id"": ""@inputID"",
            ""data"": {
                ""image"": {
                    ""url"": ""@imageUrl"",
                    ""unknown_field"": ""val""
                },
                ""unknown_field"": ""val""
            },
            ""unknown_field"": ""val""
        },
        ""data"": {
            ""concepts"": [{
                ""id"": ""@conceptID1"",
                ""name"": ""@conceptName1"",
                ""value"": 0.99,
                ""app_id"": ""main"",
                ""unknown_field"": ""val""
            }, {
                ""id"": ""@conceptID2"",
                ""name"": ""@conceptName2"",
                ""value"": 0.98,
                ""app_id"": ""main"",
                ""unknown_field"": ""val""
            }],
            ""unknown_field"": ""val""
        },
        ""unknown_field"": ""val""
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

            Assert.AreEqual("@modelID", output.Model.ModelID);
            Assert.AreEqual("@modelName", output.Model.Name);
            Assert.AreEqual("@modelVersionID", output.Model.ModelVersion.ID);
            Assert.AreEqual("concept", output.Model.OutputInfo.TypeExt);
        }
    }
}
