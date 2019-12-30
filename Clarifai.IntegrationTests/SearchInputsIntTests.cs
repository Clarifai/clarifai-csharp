using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Clarifai.API.Responses;
using Clarifai.DTOs;
using Clarifai.DTOs.Inputs;
using Clarifai.DTOs.Models.Outputs;
using Clarifai.DTOs.Predictions;
using Clarifai.DTOs.Searches;
using Clarifai.Exceptions;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Clarifai.IntegrationTests
{
    [TestFixture]
    public class SearchInputsIntTests : BaseIntTests
    {
        [Test]
        [Retry(3)]
        public async Task SearchByConceptIDShouldBeSuccessful()
        {
            ClarifaiResponse<SearchInputsResult> response =
                await Client.SearchInputs(SearchBy.ConceptID("ai_mFqxrph2"))
                .ExecuteAsync();

            AssertResponseSuccess(response);
            Assert.NotNull(response.Get().SearchHits);
        }

        [Test]
        [Retry(3)]
        public async Task SearchByUserTaggedConceptIDShouldBeSuccessful()
        {
            ClarifaiResponse<SearchInputsResult> response =
                await Client.SearchInputs(SearchBy.UserTaggedConceptID("cat"))
                .ExecuteAsync();

            AssertResponseSuccess(response);
            Assert.NotNull(response.Get().SearchHits);
        }

        [Test]
        [Retry(3)]
        public async Task SearchByConceptNameShouldBeSuccessful()
        {
            ClarifaiResponse<SearchInputsResult> response =
                await Client.SearchInputs(SearchBy.ConceptName("cat"))
                .ExecuteAsync();

            // The search is either successful or nothing has been found.
            Assert.True(response.IsSuccessful || response.Status.StatusCode == 40002);
        }

        [Test]
        [Retry(3)]
        [Ignore("WIP")]
        public async Task SearchByConceptNameWithLanguageShouldBeSuccessful()
        {
            ClarifaiResponse<List<IClarifaiInput>> addInputResponse =
                await Client.AddInputs(
                        new ClarifaiURLImage(
                            CAT1,
                            positiveConcepts: new List<Concept>
                            {
                                new Concept("ネコ", "ネコ"),

                            },
                            allowDuplicateUrl: true))
                    .ExecuteAsync();

            AssertResponseSuccess(addInputResponse);

            string inputID = addInputResponse.Get()[0].ID;

            try
            {
                await WaitForSpecificInputsUpload(inputID);

                ClarifaiResponse<SearchInputsResult> response =
                    await Client.SearchInputs(new List<SearchBy>
                            {
                                SearchBy.ConceptID("ネコ") // Cat in japanese.
                            },
                            language: "jp")
                        .ExecuteAsync();

                AssertResponseSuccess(response);
                Assert.NotNull(response.Get().SearchHits);
            }
            finally
            {
                ClarifaiResponse<EmptyResponse> deleteInputResponse =
                    await Client.DeleteInputs(inputID).ExecuteAsync();
                AssertResponseSuccess(deleteInputResponse);
            }
        }

        [Test]
        [Retry(3)]
        public async Task SearchByImageUrlShouldBeSuccessful()
        {
            ClarifaiResponse<SearchInputsResult> response =
                await Client.SearchInputs(SearchBy.ImageURL(CELEB1))
                    .ExecuteAsync();

            AssertResponseSuccess(response);
            Assert.NotNull(response.Get().SearchHits);
        }

        [Test]
        [Retry(3)]
        public async Task SearchVisuallyByImageUrlShouldBeSuccessful()
        {
            ClarifaiResponse<SearchInputsResult> response =
                await Client.SearchInputs(SearchBy.ImageVisually(CELEB1))
                    .ExecuteAsync();

            AssertResponseSuccess(response);
            Assert.NotNull(response.Get().SearchHits);
        }

        [Test]
        [Retry(3)]
        public async Task SearchVisuallyByImageUrlUsingImageShouldBeSuccessful()
        {
            ClarifaiResponse<SearchInputsResult> response =
                await Client.SearchInputs(SearchBy.ImageVisually(new ClarifaiURLImage(CELEB1)))
                    .ExecuteAsync();

            AssertResponseSuccess(response);
            Assert.NotNull(response.Get().SearchHits);
        }

        [Test]
        [Retry(3)]
        public void SearchVisuallyByImageUrlWithCropShouldThrowWhenConstructed()
        {
            Assert.Throws<ClarifaiException>(() =>
            {
                SearchBy.ImageVisually(CELEB1, new Crop(0.1M, 0.2M, 0.3M, 0.4M));
            });
        }

        [Test]
        [Retry(3)]
        public async Task SearchVisuallyByImageFileShouldBeSuccessful()
        {
            ClarifaiResponse<SearchInputsResult> response =
                await Client.SearchInputs(SearchBy.ImageVisually(
                        new ClarifaiFileImage(ReadResource(BALLOONS_IMAGE_FILE))))
                    .ExecuteAsync();

            AssertResponseSuccess(response);
            Assert.NotNull(response.Get().SearchHits);
        }

        [Test]
        public void SearchVisuallyByImageFileWithCropShouldThrowWhenConstructed()
        {
            Assert.Throws<ClarifaiException>(() =>
            {
                SearchBy.ImageVisually(
                    new ClarifaiFileImage(ReadResource(BALLOONS_IMAGE_FILE)),
                    new Crop(0.1M, 0.2M, 0.3M, 0.4M));
            });
        }

        [Test]
        [Retry(3)]
        public async Task SearchByMetadataShouldBeSuccessful()
        {
            string randomValue = GenerateRandomID();

            ClarifaiResponse<List<IClarifaiInput>> addInputResponse =
                await Client.AddInputs(
                        new ClarifaiURLImage(
                            CAT1,
                            positiveConcepts: new List<Concept> { new Concept("cat") },
                            allowDuplicateUrl: true,
                            metadata: new JObject(
                                new JProperty("key1", "val1"),
                                new JProperty("key2", randomValue))))
                    .ExecuteAsync();

            AssertResponseSuccess(addInputResponse);

            string inputID = addInputResponse.Get()[0].ID;

            try
            {
                await WaitForSpecificInputsUpload(inputID);

                ClarifaiResponse<SearchInputsResult> response =
                    await Client.SearchInputs(new List<SearchBy>
                            {
                                SearchBy.Metadata(new JObject(new JProperty("key2", randomValue)))
                            })
                        .ExecuteAsync();
                Console.WriteLine(response.RawBody);

                AssertResponseSuccess(response);
                Assert.NotNull(response.Get().SearchHits);
                // Because the value we set is random, there should be exactly one hit.
                Assert.AreEqual(1, response.Get().SearchHits.Count);
            }
            finally
            {
                ClarifaiResponse<EmptyResponse> deleteInputResponse =
                    await Client.DeleteInputs(inputID).ExecuteAsync();
                AssertResponseSuccess(deleteInputResponse);
            }
        }

        [Test]
        [Retry(3)]
        public async Task SearchByGeoPointAndRadiusShouldBeSuccessful()
        {
            ClarifaiResponse<List<IClarifaiInput>> addInputResponse =
                await Client.AddInputs(
                        new ClarifaiURLImage(
                            CAT1,
                            positiveConcepts: new List<Concept> { new Concept("cat") },
                            allowDuplicateUrl: true,
                            geo: new GeoPoint(30, 40)))
                    .ExecuteAsync();

            AssertResponseSuccess(addInputResponse);

            string inputID = addInputResponse.Get()[0].ID;

            try
            {
                await WaitForSpecificInputsUpload(inputID);

                ClarifaiResponse<SearchInputsResult> response =
                    await Client.SearchInputs(new List<SearchBy>
                        {
                            SearchBy.Geo(new GeoPoint(31, 41),
                                new GeoRadius(1.5M, GeoRadius.RadiusUnit.WithinDegrees))
                        })
                        .ExecuteAsync();

                AssertResponseSuccess(response);
                Assert.NotNull(response.Get().SearchHits);
            }
            finally
            {
                ClarifaiResponse<EmptyResponse> deleteInputResponse =
                    await Client.DeleteInputs(inputID).ExecuteAsync();
                AssertResponseSuccess(deleteInputResponse);
            }
        }

        [Test]
        [Retry(3)]
        public async Task SearchByTwoGeoPointsShouldBeSuccessful()
        {
            ClarifaiResponse<List<IClarifaiInput>> addInputResponse =
                await Client.AddInputs(
                        new ClarifaiURLImage(
                            CAT1,
                            positiveConcepts: new List<Concept> { new Concept("cat") },
                            allowDuplicateUrl: true,
                            geo: new GeoPoint(30, 40)))
                    .ExecuteAsync();

            AssertResponseSuccess(addInputResponse);

            String inputID = addInputResponse.Get()[0].ID;
            try
            {
                await WaitForSpecificInputsUpload(inputID);

                ClarifaiResponse<SearchInputsResult> response =
                    await Client.SearchInputs(new List<SearchBy>
                        {
                            SearchBy.Geo(new GeoPoint(29, 39), new GeoPoint(31, 41))
                        })
                        .ExecuteAsync();

                AssertResponseSuccess(response);
                Assert.NotNull(response.Get().SearchHits);
            }
            finally
            {
                ClarifaiResponse<EmptyResponse> deleteInputResponse =
                    await Client.DeleteInputs(inputID).ExecuteAsync();
                AssertResponseSuccess(deleteInputResponse);
            }
        }
    }
}
