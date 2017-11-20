using System;
using System.IO;
using System.Xml.Linq;
using Clarifai.API;
using NUnit.Framework;

namespace Clarifai.IntegrationTests
{
    [TestFixture]
    public class VersionCheck
    {
        /// <summary>
        /// Checks whether the csproj version and the version which is sent in the request
        /// header match.
        /// </summary>
        [Test]
        public void NugetAndRequestVersionsShouldMatch()
        {
            string requestVersion = new ClarifaiHttpClient("").CurrentVersion;

            string csprojFilePath = "../../../../Clarifai/Clarifai.csproj";
            XDocument x = XDocument.Load(csprojFilePath);
            string csprojVersion = x.Root.Element("PropertyGroup").Element("PackageVersion").Value;

            Assert.AreEqual(requestVersion, csprojVersion);
        }
    }
}
