using Clarifai.DTOs.Models;
using Newtonsoft.Json.Linq;

namespace Clarifai.API.Requests.Models
{
    /// <summary>
    /// Runs a model evaluation to test model's version performance using cross validation.
    /// </summary>
    public class ModelEvaluationRequest : ClarifaiRequest<ModelVersion>
    {
        protected override RequestMethod Method => RequestMethod.POST;
        protected override string Url => $"/v2/models/{_modelId}/versions/{_versionId}/metrics";

        private readonly string _modelId;
        private readonly string _versionId;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="modelID">the model ID</param>
        /// <param name="versionID">the version ID of the model</param>
        public ModelEvaluationRequest(IClarifaiHttpClient httpClient, string modelID, string versionID)
            : base(httpClient)
        {
            _modelId = modelID;
            _versionId = versionID;
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
