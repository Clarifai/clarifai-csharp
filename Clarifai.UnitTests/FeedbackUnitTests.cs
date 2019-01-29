using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Clarifai.API;
using Clarifai.API.Responses;
using Clarifai.DTOs.Feedbacks;
using Clarifai.DTOs.Models.Outputs;
using Clarifai.UnitTests.Fakes;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Clarifai.UnitTests
{
    [TestFixture]
    public class FeedbackUnitTests
    {
        [Test]
        public async Task ModelFeedbackRequestAndResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                postResponse: @"{""status"":{""code"":10000,""description"":""Ok""}}");
            var client = new ClarifaiClient(httpClient);
            ClarifaiResponse<EmptyResponse> response = await client.ModelFeedback(
                "@modelID", "@imageURL", "@inputID", "@outputID", "@endUserID", "@sessionID",
                concepts: new List<ConceptFeedback>
                {
                    new ConceptFeedback("dog", true),
                    new ConceptFeedback("cat", false)
                },
                regions: new List<RegionFeedback>
                {
                    new RegionFeedback(
                        new DTOs.Crop(0.1m, 0.1m, 0.2m, 0.2m),
                        Feedback.NotDetected,
                        concepts: new List<ConceptFeedback>
                        {
                            new ConceptFeedback("freeman", true),
                            new ConceptFeedback("eminem", false),
                        }
                        ),
                }
                ).ExecuteAsync();


            var expectedRequestBody = JObject.Parse(@"
{
  ""input"": {
    ""id"": ""@inputID"",
    ""data"": {
      ""image"": {
        ""url"": ""@imageURL""
      },
      ""concepts"": [
        {
          ""id"": ""dog""
        },
        {
          ""id"": ""cat"",
          ""value"": 0
        }
      ],
      ""regions"": [
        {
          ""region_info"": {
            ""bounding_box"": {
              ""top_row"": 0.1,
              ""left_col"": 0.1,
              ""bottom_row"": 0.2,
              ""right_col"": 0.2
            },
            ""feedback"": ""not_detected""
          },
          ""data"": {
            ""concepts"": [
              {
                ""id"": ""freeman""
              },
              {
                ""id"": ""eminem"",
                ""value"": 0
              }
            ]
          }
        }
      ]
    },
    ""feedback_info"": {
      ""end_user_id"": ""@endUserID"",
      ""session_id"": ""@sessionID"",
      ""event_type"": ""annotation"",
      ""output_id"": ""@outputID""
    }
  }
}
");
            Console.WriteLine(expectedRequestBody);
            Console.WriteLine(httpClient.PostedBody);
            Assert.True(JToken.DeepEquals(expectedRequestBody, httpClient.PostedBody));

            Assert.True(response.IsSuccessful);
            Assert.AreEqual("Ok", response.Status.Description);
        }

        [Test]
        public async Task SearchesFeedbackRequestAndResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                postResponse: @"{""status"":{""code"":10000,""description"":""Ok""}}");
            var client = new ClarifaiClient(httpClient);
            ClarifaiResponse<EmptyResponse> response = await client.SearchesFeedback(
                "@inputID", "@searchID", "@endUserID", "@sessionID"
            ).ExecuteAsync();

            var expectedRequestBody = JObject.Parse(@"
{
    ""input"": {
        ""id"": ""@inputID"",
        ""feedback_info"": {
            ""event_type"":   ""search_click"",
            ""search_id"":    ""@searchID"",
            ""end_user_id"":  ""@endUserID"",
            ""session_id"":   ""@sessionID""
        }
    }
}
");
            Assert.True(JToken.DeepEquals(expectedRequestBody, httpClient.PostedBody));

            Assert.True(response.IsSuccessful);
            Assert.AreEqual("Ok", response.Status.Description);
        }
    }
}
