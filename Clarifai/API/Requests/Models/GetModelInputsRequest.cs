using System.Collections.Generic;
using Clarifai.DTOs.Inputs;
using Newtonsoft.Json.Linq;

namespace Clarifai.API.Requests.Models
{
    /// <summary>
    /// Returns the model's inputs.
    /// </summary>
    public class GetModelInputsRequest : ClarifaiPaginatedRequest<List<IClarifaiInput>>
    {
        protected override RequestMethod Method => RequestMethod.GET;
        protected override string Url
        {
            get
            {
                if (_versionID == null)
                {
                    return $"/v2/models/{_modelID}/inputs";
                }
                else
                {
                    return $"/v2/models/{_modelID}/versions/{_versionID}/inputs";
                }
            }
        }

        private readonly string _modelID;
        private readonly string _versionID;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="client">the Clarifai client</param>
        /// <param name="modelID">the model ID</param>
        /// <param name="versionID">the model's version ID</param>
        public GetModelInputsRequest(IClarifaiClient client, string modelID,
            string versionID = null) : base(client)
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
        protected override List<IClarifaiInput> Unmarshaller(dynamic jsonObject)
        {
            var inputs = new List<IClarifaiInput>();
            foreach (dynamic input in jsonObject.inputs)
            {
                inputs.Add(ClarifaiInput.Deserialize(input));
            }
            return inputs;
        }
    }
}
