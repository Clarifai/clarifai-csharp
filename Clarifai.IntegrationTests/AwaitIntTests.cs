using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Clarifai.API;
using Clarifai.API.Requests.Models;
using Clarifai.API.Responses;
using Clarifai.DTOs;
using Clarifai.DTOs.Feedbacks;
using Clarifai.DTOs.Inputs;
using Clarifai.DTOs.Models;
using Clarifai.DTOs.Models.Outputs;
using Clarifai.DTOs.Predictions;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Clarifai.IntegrationTests
{
    [TestFixture]
    public class AwaitIntTests : BaseIntTests
    {
        [Test]
        [Retry(3)]
        public async Task Aaaaaa()
        {
            var client = new ClarifaiClient();
            var modelsResponse = await client.GetModels().ExecuteAsync().ConfigureAwait(false);
//            var modelsResponse = await client.GetModels().ExecuteAsync();

            var models = modelsResponse.Get();

            foreach (Model model in models)
            {
                Console.WriteLine("THE MODEL: " + model.Name);
            }
        }
    }
}
