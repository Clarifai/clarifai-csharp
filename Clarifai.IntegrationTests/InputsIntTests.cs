using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clarifai.API.Requests.Models;
using Clarifai.API.Responses;
using Clarifai.DTOs;
using Clarifai.DTOs.Inputs;
using Clarifai.DTOs.Models.Outputs;
using Clarifai.DTOs.Predictions;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Clarifai.IntegrationTests
{
    [TestFixture]
    public class InputsIntTests : BaseIntTests
    {
        [Test]
        [Retry(3)]
        public async Task AddingGettingAndDeletingInputsShouldBeSuccessful()
        {
            string inputID1 = GenerateRandomID();
            string inputID2 = GenerateRandomID();
            try
            {
                var geoPoint = new GeoPoint(55, 66);

                /*
                 * Add new inputs.
                 */
                ClarifaiResponse<List<IClarifaiInput>> addResponse = await Client.AddInputs(
                        new ClarifaiURLImage(
                            CELEB1,
                            id: inputID1,
                            allowDuplicateUrl: true,
                            geo: geoPoint),
                        new ClarifaiURLImage(APPAREL1, id: inputID2, allowDuplicateUrl: true))
                    .ExecuteAsync();
                AssertResponseSuccess(addResponse);

                /*
                 * Get inputs' status.
                 */
                ClarifaiResponse<ClarifaiInputsStatus> getInputsStatusResponse =
                    await Client.GetInputsStatus().ExecuteAsync();
                Assert.AreEqual(ClarifaiStatus.StatusType.Successful,
                    getInputsStatusResponse.Status.Type);

                /*
                 * Get the inputs.
                 */
                ClarifaiResponse<List<IClarifaiInput>> getResponse = await Client.GetInputs()
                    .ExecuteAsync();
                AssertResponseSuccess(getResponse);

                /*
                 * Get input 1.
                 */
                ClarifaiResponse<IClarifaiInput> getInput1Response = await Client.GetInput(inputID1)
                    .ExecuteAsync();
                AssertResponseSuccess(getInput1Response);
                Assert.AreEqual(geoPoint, getInput1Response.Get().Geo);
                Assert.NotNull(getInput1Response.Get().CreatedAt);

                /*
                 * Get input 2.
                 */
                ClarifaiResponse<IClarifaiInput> getInput2Response = await Client.GetInput(inputID1)
                    .ExecuteAsync();
                AssertResponseSuccess(getInput2Response);
            }
            finally
            {
                /*
                 * Delete the inputs.
                 */
                var deleteResponse = await Client.DeleteInputs(inputID1, inputID2)
                    .ExecuteAsync();
                AssertResponseSuccess(deleteResponse);
            }
        }

        [Test]
        [Retry(3)]
        public async Task AddingInputWithConceptsShouldBeSuccessful()
        {
            string inputID = GenerateRandomID();
            try
            {
                /*
                 * Add input with concepts.
                 */
                ClarifaiResponse<List<IClarifaiInput>> addResponse = await Client.AddInputs(
                        new ClarifaiURLImage(
                            CELEB1,
                            id: inputID,
                            positiveConcepts: new List<Concept>
                            {
                                new Concept("concept1"), new Concept("concept2"),
                            },
                            allowDuplicateUrl: true))
                    .ExecuteAsync();
                AssertResponseSuccess(addResponse);
            }
            finally
            {
                /*
                 * Delete the input.
                 */
                await DeleteInput(inputID);
            }
        }

        [Test]
        [Retry(3)]
        public async Task OverwrittingConceptsForInputShouldBeSuccessful()
        {
            string inputID = GenerateRandomID();
            ClarifaiResponse<List<IClarifaiInput>> addResponse =
                await AddInputWithConcept1AndConcept2(inputID);
            AssertResponseSuccess(addResponse);


            try
            {
                ClarifaiResponse<IClarifaiInput> modifyResponse = await Client.ModifyInput(
                        inputID,
                        ModifyAction.Overwrite,
                        positiveConcepts: new List<Concept> {new Concept("concept3")})
                    .ExecuteAsync();
                AssertResponseSuccess(modifyResponse);

                List<Concept> concepts = await GetConcepts(inputID);
                Assert.AreEqual(1, concepts.Count);
                Assert.Contains("concept3", concepts.Select(c => c.ID).ToList());
            }
            finally
            {
                /*
                 * Delete the input.
                 */
                await DeleteInput(inputID);
            }
        }

        [Test]
        [Retry(3)]
        public async Task MergingConceptsForInputShouldBeSuccessful()
        {
            string inputID = GenerateRandomID();
            ClarifaiResponse<List<IClarifaiInput>> addResponse =
                await AddInputWithConcept1AndConcept2(inputID);
            AssertResponseSuccess(addResponse);

            try
            {
                ClarifaiResponse<IClarifaiInput> modifyResponse = await Client.ModifyInput(
                        inputID,
                        ModifyAction.Merge,
                        new List<Concept> {new Concept("concept2"), new Concept("concept3")})
                    .ExecuteAsync();
                AssertResponseSuccess(modifyResponse);

                List<Concept> concepts = await GetConcepts(inputID);
                Assert.AreEqual(3, concepts.Count);
                List<string> conceptIDs = concepts.Select(c => c.ID).ToList();
                Assert.Contains("concept1", conceptIDs);
                Assert.Contains("concept2", conceptIDs);
                Assert.Contains("concept3", conceptIDs);
            }
            finally
            {
                /*
                 * Delete the input.
                 */
                await DeleteInput(inputID);
            }
        }

        [Test]
        [Retry(3)]
        public async Task RemovingConceptsForInputShouldBeSuccessful()
        {
            string inputID = GenerateRandomID();
            ClarifaiResponse<List<IClarifaiInput>> addResponse =
                await AddInputWithConcept1AndConcept2(inputID);
            AssertResponseSuccess(addResponse);

            try
            {
                ClarifaiResponse<IClarifaiInput> modifyResponse = await Client.ModifyInput(
                        inputID,
                        ModifyAction.Remove,
                        new List<Concept> {new Concept("concept2")})
                    .ExecuteAsync();
                AssertResponseSuccess(modifyResponse);

                List<Concept> concepts = await GetConcepts(inputID);
                Assert.AreEqual(1, concepts.Count);
                List<string> conceptIDs = concepts.Select(c => c.ID).ToList();
                Assert.Contains("concept1", conceptIDs);
            }
            finally
            {
                /*
                 * Delete the input.
                 */
                await DeleteInput(inputID);
            }
        }

        [Test]
        [Retry(3)]
        public async Task ModifyingImageMetadataShouldBeSuccessful()
        {
            string inputID = GenerateRandomID();
            ClarifaiResponse<List<IClarifaiInput>> addResponse = await Client.AddInputs(
                    new ClarifaiURLImage(
                        CELEB1,
                        id: inputID,
                        positiveConcepts: new List<Concept> {new Concept("concept1")},
                        allowDuplicateUrl: true,
                        metadata: new JObject(new JProperty("key1", "val1"),
                            new JProperty("key2", "val2"))))
                .ExecuteAsync();
            AssertResponseSuccess(addResponse);

            try
            {
                ClarifaiResponse<IClarifaiInput> modifyResponse = await Client.ModifyInputMetadata(
                        inputID,
                        new JObject(new JProperty("key3", "val3")))
                    .ExecuteAsync();
                AssertResponseSuccess(modifyResponse);

                ClarifaiResponse<IClarifaiInput> getResponse = await Client.GetInput(inputID)
                    .ExecuteAsync();
                AssertResponseSuccess(getResponse);

                Assert.AreEqual("val3", getResponse.Get().Metadata["key3"].Value<string>());
            }
            finally
            {
                /*
                 * Delete the input.
                 */
                await DeleteInput(inputID);
            }
        }

        [Test]
        [Retry(3)]
        public async Task DeleteAllInputsRequestShouldBeSuccessful()
        {
            ClarifaiResponse<EmptyResponse> response = await Client.DeleteAllInputs()
                .ExecuteAsync();
            AssertResponseSuccess(response);
        }

        [Test]
        public async Task PositiveAndNegativeConceptsShouldBeHandledCorrectly()
        {
            string inputID = GenerateRandomID();

            var addResponse = await Client.AddInputs(
                    new ClarifaiURLImage(
                        CELEB1,
                        inputID,
                        positiveConcepts: new List<Concept>
                        {
                            new Concept("person"),
                            new Concept("human")
                        },
                        negativeConcepts: new List<Concept> {new Concept("apple")},
                        allowDuplicateUrl: true
                    ))
                .ExecuteAsync();

            AssertResponseSuccess(addResponse);

            try
            {
                var modifyResponse = await Client.ModifyInput(
                        inputID,
                        ModifyAction.Merge,
                        positiveConcepts: new List<Concept> {new Concept("male")},
                        negativeConcepts: new List<Concept> {new Concept("pear")})
                    .ExecuteAsync();

                IClarifaiInput img = modifyResponse.Get();

                List<string> positiveConcepts = img.PositiveConcepts.Select(c => c.ID).ToList();
                List<string> negativeConcepts = img.NegativeConcepts.Select(c => c.ID).ToList();

                CollectionAssert.AreEquivalent(
                    new List<string> {"person", "human", "male"},
                    positiveConcepts);

                CollectionAssert.AreEquivalent(
                    new List<string> {"apple", "pear"},
                    negativeConcepts);
            }
            finally
            {
                await Client.DeleteInputs(inputID).ExecuteAsync();
            }
        }

        [Test]
        [Retry(3)]
        public async Task AddingImageUrlWithCropShouldBeSuccessful()
        {
            string inputID = GenerateRandomID();
            try
            {
                /*
                 * Add input with concepts.
                 */
                ClarifaiResponse<List<IClarifaiInput>> addResponse = await Client.AddInputs(
                        new ClarifaiURLImage(
                            CELEB1,
                            id: inputID,
                            crop: new Crop(0.1M, 0.2M, 0.3M, 0.4M),
                            allowDuplicateUrl: true))
                    .ExecuteAsync();
                AssertResponseSuccess(addResponse);
                var input = (ClarifaiURLImage) addResponse.Get()[0];
                Assert.AreEqual(new Crop(0.1M, 0.2M, 0.3M, 0.4M), input.Crop);
            }
            finally
            {
                /*
                 * Delete the input.
                 */
                await DeleteInput(inputID);
            }
        }

        [Test]
        [Retry(3)]
        public async Task AddingImageFileWithCropShouldBeSuccessful()
        {
            string inputID = GenerateRandomID();
            try
            {
                /*
                 * Add input with concepts.
                 */
                ClarifaiResponse<List<IClarifaiInput>> addResponse = await Client.AddInputs(
                        new ClarifaiFileImage(
                            ReadResource(BALLOONS_IMAGE_FILE),
                            id: inputID,
                            crop: new Crop(0.1M, 0.2M, 0.3M, 0.4M)))
                    .ExecuteAsync();
                AssertResponseSuccess(addResponse);

                ClarifaiURLImage input = (ClarifaiURLImage) addResponse.Get()[0];
                Assert.AreEqual(new Crop(0.1M, 0.2M, 0.3M, 0.4M), input.Crop);
            }
            finally
            {
                /*
                 * Delete the input.
                 */
                await DeleteInput(inputID);
            }
        }

        private async Task DeleteInput(string inputID)
        {
            var response = await Client.DeleteInputs(inputID)
                .ExecuteAsync();
            AssertResponseSuccess(response);
        }

        private async Task<ClarifaiResponse<List<IClarifaiInput>>> AddInputWithConcept1AndConcept2(
            string inputID)
        {
            ClarifaiResponse<List<IClarifaiInput>> addResponse = await Client.AddInputs(
                    new ClarifaiURLImage(
                        CELEB1,
                        id: inputID,
                        positiveConcepts: new List<Concept>
                        {
                            new Concept("concept1"),
                            new Concept("concept2"),
                        },
                        allowDuplicateUrl: true))
                .ExecuteAsync();
            AssertResponseSuccess(addResponse);
            return addResponse;
        }

        private async Task<List<Concept>> GetConcepts(string inputID)
        {
            ClarifaiResponse<IClarifaiInput> getResponse = await Client.GetInput(inputID)
                .ExecuteAsync();
            AssertResponseSuccess(getResponse);
            List<Concept> concepts = getResponse.Get().PositiveConcepts.ToList();
            return concepts;
        }
    }
}
