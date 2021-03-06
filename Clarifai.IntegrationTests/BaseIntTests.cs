﻿using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Clarifai.API;
using Clarifai.API.Responses;
using Clarifai.DTOs.Inputs;
using Clarifai.Exceptions;
using NUnit.Framework;
using NUnit.Framework.Internal.Execution;

namespace Clarifai.IntegrationTests
{
    public class BaseIntTests
    {
        protected const string APPAREL1 =
            @"https://clarifai.com/developer/static/images/model-samples/apparel-001.jpg";

        protected const string CAT1 =
            @"https://clarifai.com/developer/static/images/model-samples/focus-001.jpg";
        protected const string CELEB1 =
            @"https://clarifai.com/developer/static/images/model-samples/celeb-001.jpg";
        protected const string FACE1 =
            @"https://clarifai.com/developer/static/images/model-samples/face-001.jpg";
        protected const string GIF1 =
            @"https://s3.amazonaws.com/samples.clarifai.com/D7qTae7IQLKSI.gif";

        protected const string BALLOONS_IMAGE_FILE = "balloons.jpg";
        protected const string BEER_VIDEO_FILE = "beer.mp4";

        protected IClarifaiClient Client;

        private readonly Random _random = new Random(Guid.NewGuid().GetHashCode());

        private readonly string _assetsDir = "Assets";

        [SetUp]
        public void SetUp()
        {
            String apiKey = Environment.GetEnvironmentVariable("CLARIFAI_API_KEY");
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                Assert.Inconclusive("The CLARIFAI_API_KEY environment variable must be set in order to run the integration tests.");
            }

            String baseUrl = Environment.GetEnvironmentVariable("CLARIFAI_API_BASE");
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                baseUrl = "https://api.clarifai.com";
            }

            Client = new ClarifaiClient(new ClarifaiHttpClient(apiKey, baseUrl));
        }

        protected string GenerateRandomID()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzZ0123456789";
            return new string(Enumerable.Repeat(chars, 20)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }

        protected byte[] ReadResource(string fileName)
        {
            var assembly = GetType().GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream(
                $"{GetType().Namespace}.{_assetsDir}.{fileName}");
            if (stream == null)
            {
                throw new Exception($"Resource {fileName} not found in {_assetsDir}.");
            }
            byte[] bytes;
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                bytes = ms.ToArray();
            }
            return bytes;
        }

        protected void AssertResponseSuccess(dynamic response, string message = null)
        {
            if (!response.IsSuccessful)
            {
                Console.WriteLine(response.Status.StatusCode);
                Console.WriteLine(response.Status.Description);
                if (response.Status.ErrorDetails != null)
                {
                    Console.WriteLine(response.Status.ErrorDetails);
                }

                if (message != null)
                {
                    Console.WriteLine(message);
                }
                Assert.Fail("Failed response");
            }
        }

        protected async Task WaitForSpecificInputsUpload(params String[] inputIDs)
        {
            foreach (string inputID in inputIDs)
            {
                long start = DateTimeOffset.Now.ToUnixTimeSeconds();
                while (true)
                {
                    if (DateTimeOffset.Now.ToUnixTimeSeconds() - start > 60)
                    {
                        throw new ClarifaiException(
                            $"Waited too long for input ID {inputID} upload.");
                    }

                    var response = await Client.GetInput(inputID).ExecuteAsync();
                    int statusCode = response.Get().Status.StatusCode;
                    // INPUT_IMAGE_DOWNLOAD_SUCCESS
                    if (statusCode == 30000)
                    {
                        break;
                    }
                    // not (INPUT_IMAGE_DOWNLOAD_PENDING or INPUT_IMAGE_DOWNLOAD_IN_PROGRESS)
                    else if (!(statusCode == 30001 || statusCode == 30003))
                    {
                        throw new ClarifaiException(
                            $"Waiting for input ID {inputID} failed because status code is " +
                            $"{statusCode}");
                    }

                    Thread.Sleep(200);
                }
            }
        }
    }
}
