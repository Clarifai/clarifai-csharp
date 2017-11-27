using System.Collections.Generic;
using System.Threading.Tasks;
using Clarifai.API.Responses;
using Clarifai.DTOs;
using Clarifai.DTOs.Inputs;
using Clarifai.DTOs.Models.Outputs;
using Clarifai.DTOs.Predictions;
using Clarifai.DTOs.Searches;
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

            Assert.True(response.IsSuccessful);
            Assert.NotNull(response.Get().SearchHits);
        }

        [Test]
        [Retry(3)]
        public async Task SearchByUserTaggedConceptIDShouldBeSuccessful()
        {
            ClarifaiResponse<SearchInputsResult> response =
                await Client.SearchInputs(SearchBy.UserTaggedConceptID("cat"))
                .ExecuteAsync();

            Assert.True(response.IsSuccessful);
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
        [Ignore("Not yet finished.")]
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

            Assert.True(addInputResponse.IsSuccessful);

            try
            {
                ClarifaiResponse<SearchInputsResult> response =
                    await Client.SearchInputs(new List<SearchBy>
                            {
                                SearchBy.ConceptID("ネコ") // Cat in japanese.
                            },
                            language: "jp")
                        .ExecuteAsync();

                Assert.True(response.IsSuccessful);
                Assert.NotNull(response.Get().SearchHits);
            }
            finally
            {
                string id = addInputResponse.Get()[0].ID;
                ClarifaiResponse<EmptyResponse> deleteInputResponse =
                    await Client.DeleteInputs(id).ExecuteAsync();
                Assert.True(deleteInputResponse.IsSuccessful);
            }
        }

        [Test]
        [Retry(3)]
        public async Task SearchByImageUrlShouldBeSuccessful()
        {
            ClarifaiResponse<SearchInputsResult> response =
                await Client.SearchInputs(SearchBy.ImageURL(CELEB1))
                    .ExecuteAsync();

            Assert.True(response.IsSuccessful);
            Assert.NotNull(response.Get().SearchHits);
        }

        [Test]
        [Retry(3)]
        public async Task SearchByImageURLShouldBeSuccessful()
        {
            ClarifaiResponse<SearchInputsResult> response =
                await Client.SearchInputs(SearchBy.ImageURL(new ClarifaiURLImage(CELEB1)))
                    .ExecuteAsync();

            Assert.True(response.IsSuccessful);
            Assert.NotNull(response.Get().SearchHits);
        }

        [Test]
        [Retry(3)]
        public async Task SearchByImageURLWithCropShouldBeSuccessful()
        {
            ClarifaiResponse<SearchInputsResult> response =
                await Client.SearchInputs(SearchBy.ImageURL(
                        new ClarifaiURLImage(CELEB1),
                        new Crop(0.1M, 0.2M, 0.3M, 0.4M)))
                    .ExecuteAsync();

            Assert.True(response.IsSuccessful);
            Assert.NotNull(response.Get().SearchHits);
        }

        [Test]
        [Retry(3)]
        public async Task SearchByImageBytesShouldBeSuccessful()
        {
            ClarifaiResponse<SearchInputsResult> response =
                await Client.SearchInputs(SearchBy.ImageBytes(new ClarifaiFileImage(
                        ReadResource(BALLOONS_IMAGE_FILE))))
                    .ExecuteAsync();

            Assert.True(response.IsSuccessful);
            Assert.NotNull(response.Get().SearchHits);
        }

        [Test]
        [Retry(3)]
        public async Task SearchByImageBytesWithCropShouldBeSuccessful()
        {
            ClarifaiResponse<SearchInputsResult> response =
                await Client.SearchInputs(SearchBy.ImageBytes(new ClarifaiFileImage(
                        ReadResource(BALLOONS_IMAGE_FILE)),
                        new Crop(0.1M, 0.2M, 0.3M, 0.4M)))
                    .ExecuteAsync();

            Assert.True(response.IsSuccessful);
            Assert.NotNull(response.Get().SearchHits);
        }

        [Test]
        [Retry(3)]
        public async Task SearchByMetadataShouldBeSuccessful()
        {
            var metadata = new JObject(
                new JProperty("key1", "val1"),
                new JProperty("key2", "val2"));

            ClarifaiResponse<List<IClarifaiInput>> addInputResponse =
                await Client.AddInputs(
                        new ClarifaiURLImage(
                            CAT1,
                            positiveConcepts: new List<Concept> { new Concept("cat") },
                            allowDuplicateUrl: true,
                            metadata: metadata))
                    .ExecuteAsync();

            Assert.True(addInputResponse.IsSuccessful);

            try
            {
                ClarifaiResponse<SearchInputsResult> response =
                    await Client.SearchInputs(new List<SearchBy>
                            {
                                SearchBy.Metadata(metadata)
                            })
                        .ExecuteAsync();

                Assert.True(response.IsSuccessful);
                Assert.NotNull(response.Get().SearchHits);
            }
            finally
            {
                string id = addInputResponse.Get()[0].ID;
                ClarifaiResponse<EmptyResponse> deleteInputResponse =
                    await Client.DeleteInputs(id).ExecuteAsync();
                Assert.True(deleteInputResponse.IsSuccessful);
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

            Assert.True(addInputResponse.IsSuccessful);

            try
            {
                ClarifaiResponse<SearchInputsResult> response =
                    await Client.SearchInputs(new List<SearchBy>
                        {
                            SearchBy.Geo(new GeoPoint(31, 41),
                                new GeoRadius(1.5M, GeoRadius.RadiusUnit.WithinDegrees))
                        })
                        .ExecuteAsync();

                Assert.True(response.IsSuccessful);
                Assert.NotNull(response.Get().SearchHits);
            }
            finally
            {
                string id = addInputResponse.Get()[0].ID;
                ClarifaiResponse<EmptyResponse> deleteInputResponse =
                    await Client.DeleteInputs(id).ExecuteAsync();
                Assert.True(deleteInputResponse.IsSuccessful);
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

            Assert.True(addInputResponse.IsSuccessful);

            try
            {
                ClarifaiResponse<SearchInputsResult> response =
                    await Client.SearchInputs(new List<SearchBy>
                        {
                            SearchBy.Geo(new GeoPoint(29, 39), new GeoPoint(31, 41))
                        })
                        .ExecuteAsync();

                Assert.True(response.IsSuccessful);
                Assert.NotNull(response.Get().SearchHits);
            }
            finally
            {
                string id = addInputResponse.Get()[0].ID;
                ClarifaiResponse<EmptyResponse> deleteInputResponse =
                    await Client.DeleteInputs(id).ExecuteAsync();
                Assert.True(deleteInputResponse.IsSuccessful);
            }
        }
    }
}
