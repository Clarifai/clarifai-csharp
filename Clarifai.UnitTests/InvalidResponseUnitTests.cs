using System.Threading.Tasks;
using Clarifai.API;
using Clarifai.DTOs;
using Clarifai.DTOs.Predictions;
using Clarifai.UnitTests.Fakes;
using NUnit.Framework;

namespace Clarifai.UnitTests
{
    [TestFixture]
    public class InvalidResponseUnitTests
    {
        [Test]
        public async Task InvalidJsonShouldReturnNetworkErrorResponse()
        {
            var httpClient = new FkClarifaiHttpClient(
                getResponse: @"
{
    ""status"": {

        ""code"": 10000,
        ""description"": ""Ok""

    },
    ""model"": {
");

            var client = new ClarifaiClient(httpClient);
            var response = await client.GetModel<Concept>("").ExecuteAsync();

            Assert.False(response.IsSuccessful);
            Assert.AreEqual(ClarifaiStatus.StatusType.NetworkError, response.Status.Type);
            Assert.NotNull(response.Status.Description);
            Assert.NotNull(response.Status.ErrorDetails);
        }
    }
}
