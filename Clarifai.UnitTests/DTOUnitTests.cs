using System.Linq;
using Clarifai.DTOs.Inputs;
using Clarifai.DTOs.Models;
using Clarifai.DTOs.Models.OutputsInfo;
using Clarifai.DTOs.Predictions;
using Clarifai.DTOs.Workflows;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Clarifai.UnitTests
{
    [TestFixture]
    public class DTOUnitTests
    {
        [Test]
        public void ConceptOutputInfoShouldBeCorrectlyDeserialized()
        {
            var responseContent = JsonConvert.DeserializeObject<dynamic>(@"
{
    ""output_config"": {
            ""concepts_mutually_exclusive"": false,
            ""closed_environment"": true
        },
    ""message"": ""Show output_info with: GET /models/{model_id}/output_info"",
    ""type"": ""concept"",
    ""type_ext"": ""concept""
}");
            ConceptOutputInfo outputInfo = ConceptOutputInfo.Deserialize(responseContent);
            Assert.IsTrue(outputInfo.IsEnvironmentClosed);
            Assert.IsFalse(outputInfo.AreConceptsMutuallyExclusive);
            Assert.IsNull(outputInfo.Language);
        }

        [Test]
        public void ConceptShouldBeCorrectlyDeserialized()
        {
            var json = JsonConvert.DeserializeObject<dynamic>(@"
{
    ""id"": ""ai_HLmqFqBf"",
    ""name"": ""train"",
    ""value"": 0.9989112,
    ""app_id"": ""main""
}");
            Concept concept = Concept.Deserialize(json);
            Assert.AreEqual("ai_HLmqFqBf", concept.ID);
            Assert.AreEqual("train", concept.Name);
            Assert.AreEqual(0.9989112, concept.Value);
            Assert.AreEqual("main", concept.AppID);
        }

        [Test]
        public void ConceptModelShouldBeCorrectlyDeserialized()
        {
            var json = JsonConvert.DeserializeObject<dynamic>(@"
{
    ""id"": ""some-model-id"",
    ""name"": ""general-v1.3"",
    ""created_at"": ""2016-03-09T17:11:39.608845Z"",
    ""app_id"": ""main"",
    ""output_info"": {
        ""message"": ""Show output_info with: GET /models/{model_id}/output_info"",
        ""type"": ""concept"",
        ""type_ext"": ""concept""
    },
    ""model_version"": {
        ""id"": ""some-model-version-id"",
        ""created_at"": ""2016-07-13T01:19:12.147644Z"",
        ""status"": {
            ""code"": 21100,
            ""description"": ""Model trained successfully""
        }
    }
}");
            ConceptModel model = ConceptModel.Deserialize(null, json);
            Assert.AreEqual("some-model-id", model.ModelID);
            Assert.AreEqual("general-v1.3", model.Name);
            Assert.AreEqual("main", model.AppID);
            Assert.AreEqual("some-model-version-id", model.ModelVersion.ID);
            Assert.AreEqual(21100, model.ModelVersion.Status.StatusCode);
        }

        [Test]
        public void InputShouldBeCorrectlyDeserialized()
        {
            var json = JsonConvert.DeserializeObject<dynamic>(@"
{
        ""id"": ""@inputID"",
        ""data"": {
            ""image"": {
                ""url"": ""@imageURL""
            },
            ""concepts"": [
                {
                  ""id"": ""@positiveConcept"",
                  ""value"": 1
                },
                {
                  ""id"": ""@negativeConcept2"",
                  ""value"": 0
                }
            ]
        },
        ""created_at"": ""2017-10-13T20:53:00.253139Z"",
        ""modified_at"": ""2017-10-13T20:53:00.868659782Z"",
        ""status"": {
            ""code"": 30200,
            ""description"": ""Input image modification success""
        }
}");
            ClarifaiInput input = ClarifaiInput.Deserialize(json);
            Assert.AreEqual(input.ID,"@inputID");
            Assert.AreEqual(input.PositiveConcepts.ElementAt(0).ID,"@positiveConcept");
            Assert.AreEqual(input.NegativeConcepts.ElementAt(0).ID,"@negativeConcept2");
        }

        [Test]
        public void WorkflowShouldBeCorrectlyDeserialized()
        {
            var json = JsonConvert.DeserializeObject<dynamic>(@"
{
    ""id"": ""@workflowID"",
    ""app_id"": ""@appID"",
    ""created_at"": ""2017-07-10T01:45:05.672880Z""
}");
            Workflow workflow = Workflow.Deserialize(json);
            Assert.AreEqual(workflow.ID,"@workflowID");
            Assert.AreEqual(workflow.AppID,"@appID");
            Assert.AreEqual(workflow.CreatedAt.Year,2017);
        }
    }
}
