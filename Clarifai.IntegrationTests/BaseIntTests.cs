using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Clarifai.API;
using NUnit.Framework;

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

        protected readonly IClarifaiClient Client = new ClarifaiClient(
            Environment.GetEnvironmentVariable("CLARIFAI_API_KEY"));

        private readonly Random _random = new Random(Guid.NewGuid().GetHashCode());

        private readonly string _assetsDir = "Assets";

        [SetUp]
        public void SetUp()
        {
            if (string.IsNullOrWhiteSpace(
                Environment.GetEnvironmentVariable("CLARIFAI_API_KEY")))
            {
                Assert.Inconclusive("The CLARIFAI_API_KEY environment variable must be set in order to rum the integration tests.");
            }
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
    }
}