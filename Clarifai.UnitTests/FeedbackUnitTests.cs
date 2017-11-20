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
                new List<ConceptFeedback>
                {
                    new ConceptFeedback("dog", true),
                    new ConceptFeedback("cat", false)
                }).ExecuteAsync();


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
                ""id"": ""dog"",
                ""value"": true
            },
            {
                ""id"": ""cat"",
                ""value"": false
            }
            ]
        },
        ""feedback_info"": {
            ""event_type"": ""annotation"",
            ""output_id"": ""@outputID"",
            ""end_user_id"": ""@endUserID"",
            ""session_id"": ""@sessionID""
        }
    }
}
");
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
