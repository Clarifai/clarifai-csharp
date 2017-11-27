using System.Collections.Generic;
using System.Threading.Tasks;
using Clarifai.API;
using Clarifai.DTOs;
using Clarifai.DTOs.Inputs;
using Clarifai.DTOs.Searches;
using Clarifai.UnitTests.Fakes;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Clarifai.UnitTests
{
    [TestFixture]
    public class SearchInputsUnitTests
    {
        [Test]
        public async Task SearchInputsByIDRequestAndResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                postResponse: @"
{
  ""status"": {
    ""code"": 10000,
    ""description"": ""Ok""
  },
  ""hits"": [
    {
      ""score"": 0.99,
      ""input"": {
        ""id"": ""@inputID"",
        ""created_at"": ""2016-11-22T17:06:02Z"",
        ""data"": {
          ""image"": {
            ""url"": ""@inputURL""
          }
        },
        ""status"": {
          ""code"": 30000,
          ""description"": ""Download complete""
        }
      }
    }
  ]
}
");

            var client = new ClarifaiClient(httpClient);
            var response = await client.SearchInputs(SearchBy.ConceptID("@conceptID"))
                .ExecuteAsync();

            var expectedRequestBody = JObject.Parse(@"
{
  ""query"": {
    ""ands"": [
      {
        ""output"": {
          ""data"": {
            ""concepts"": [
              {
                ""id"": ""@conceptID""
              }
            ]
          }
        }
      }
    ]
  }
}
");

            Assert.True(JToken.DeepEquals(expectedRequestBody, httpClient.PostedBody));
            Assert.True(response.IsSuccessful);

            List<SearchHit> searchHits = response.Get().SearchHits;
            Assert.AreEqual(1, searchHits.Count);
            Assert.AreEqual("@inputID", searchHits[0].Input.ID);

            IClarifaiInput input = searchHits[0].Input;
            Assert.AreEqual(InputType.Image, input.Type);
            Assert.AreEqual(InputForm.URL, input.Form);

            ClarifaiURLImage image = (ClarifaiURLImage) input;
            Assert.AreEqual("@inputURL", image.URL);
        }

        [Test]
        public async Task SearchInputsByNameRequestAndResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                postResponse: @"
{
  ""status"": {
    ""code"": 10000,
    ""description"": ""Ok""
  },
  ""hits"": [
    {
      ""score"": 0.99,
      ""input"": {
        ""id"": ""@inputID"",
        ""created_at"": ""2016-11-22T17:06:02Z"",
        ""data"": {
          ""image"": {
            ""url"": ""@inputURL""
          }
        },
        ""status"": {
          ""code"": 30000,
          ""description"": ""Download complete""
        }
      }
    }
  ]
}
");

            var client = new ClarifaiClient(httpClient);
            var response = await client.SearchInputs(SearchBy.ConceptName("@conceptName"))
                .ExecuteAsync();

            var expectedRequestBody = JObject.Parse(@"
{
  ""query"": {
    ""ands"": [
      {
        ""output"": {
          ""data"": {
            ""concepts"": [
              {
                ""name"": ""@conceptName""
              }
            ]
          }
        }
      }
    ]
  }
}
");

            Assert.True(JToken.DeepEquals(expectedRequestBody, httpClient.PostedBody));
            Assert.True(response.IsSuccessful);

            List<SearchHit> searchHits = response.Get().SearchHits;
            Assert.AreEqual(1, searchHits.Count);
            Assert.AreEqual("@inputID", searchHits[0].Input.ID);

            IClarifaiInput input = searchHits[0].Input;
            Assert.AreEqual(InputType.Image, input.Type);
            Assert.AreEqual(InputForm.URL, input.Form);

            ClarifaiURLImage image = (ClarifaiURLImage) input;
            Assert.AreEqual("@inputURL", image.URL);
        }

        [Test]
        public async Task SearchInputsByGeoLocationRequestAndResponseShouldBeCorrect()
        {
            var httpClient = new FkClarifaiHttpClient(
                postResponse: @"
{
  ""status"": {
    ""code"": 10000,
    ""description"": ""Ok""
  },
  ""hits"": [
    {
      ""score"": 0.99,
      ""input"": {
        ""id"": ""@inputID"",
        ""created_at"": ""2016-11-22T17:06:02Z"",
        ""data"": {
          ""image"": {
            ""url"": ""@inputURL""
          }
        },
        ""status"": {
          ""code"": 30000,
          ""description"": ""Download complete""
        }
      }
    }
  ]
}
");

            var client = new ClarifaiClient(httpClient);
            var response = await client.SearchInputs(SearchBy.Geo(new GeoPoint(1.5M, -1),
                    new GeoRadius(1, GeoRadius.RadiusUnit.WithinKilometers)))
                .ExecuteAsync();

            var expectedRequestBody = JObject.Parse(@"
{
  ""query"": {
    ""ands"": [
      {
        ""input"": {
          ""data"": {
            ""geo"": {
              ""geo_point"": {
                ""longitude"": 1.5,
                ""latitude"": -1.0
              },
              ""geo_limit"": {
                ""type"": ""withinKilometers"",
                ""value"": 1.0
              }
            }
          }
        }
      }
    ]
  }
}");

            Assert.True(JToken.DeepEquals(expectedRequestBody, httpClient.PostedBody));
            Assert.True(response.IsSuccessful);

            List<SearchHit> searchHits = response.Get().SearchHits;
            Assert.AreEqual(1, searchHits.Count);
            Assert.AreEqual("@inputID", searchHits[0].Input.ID);

            IClarifaiInput input = searchHits[0].Input;
            Assert.AreEqual(InputType.Image, input.Type);
            Assert.AreEqual(InputForm.URL, input.Form);

            ClarifaiURLImage image = (ClarifaiURLImage) input;
            Assert.AreEqual("@inputURL", image.URL);
        }
    }
}
