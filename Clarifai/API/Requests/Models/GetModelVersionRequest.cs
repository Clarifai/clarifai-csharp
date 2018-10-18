using Clarifai.DTOs.Models;
using Newtonsoft.Json.Linq;

namespace Clarifai.API.Requests.Models
{
    /// <summary>
    /// Request for retrieving a specific model-version.
    /// </summary>
    public class GetModelVersionRequest : ClarifaiRequest<ModelVersion>
    {
        protected override RequestMethod Method => RequestMethod.GET;
        protected override string Url => $"/v2/models/{_modelID}/versions/{_versionID}";

        private readonly string _modelID;
        private readonly string _versionID;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="modelID">the model ID</param>
        /// <param name="versionID">the version ID</param>
        public GetModelVersionRequest(IClarifaiHttpClient httpClient, string modelID,
            string versionID)
            : base(httpClient)
        {
            _modelID = modelID;
            _versionID = versionID;
        }

        /// <inheritdoc />
        protected override JObject HttpRequestBody()
        {
            return new JObject();
        }

        /// <inheritdoc />
        protected override ModelVersion Unmarshaller(dynamic jsonObject)
        {
            return ModelVersion.Deserialize(jsonObject.model_version);
        }
    }
}
