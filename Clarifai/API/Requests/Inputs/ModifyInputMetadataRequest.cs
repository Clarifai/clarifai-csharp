﻿using Clarifai.DTOs.Inputs;
using Newtonsoft.Json.Linq;

namespace Clarifai.API.Requests.Inputs
{
    /// <summary>
    /// A request for adding the given metadata to this input's metadata.
    /// </summary>
    public class ModifyInputMetadataRequest : ClarifaiRequest<IClarifaiInput>
    {
        protected override RequestMethod Method => RequestMethod.PATCH;
        protected override string Url => "/v2/inputs";

        private readonly string _inputID;
        private readonly JObject _metadata;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="inputID">the input ID</param>
        /// <param name="metadata">the input's new metadata</param>
        public ModifyInputMetadataRequest(IClarifaiHttpClient httpClient, string inputID,
            JObject metadata)
            : base(httpClient)
        {
            _inputID = inputID;
            _metadata = metadata;
        }

        /// <inheritdoc />
        protected override IClarifaiInput Unmarshaller(dynamic jsonObject)
        {
            return ClarifaiInput.Deserialize(jsonObject.inputs[0]);
        }

        /// <inheritdoc />
        protected override JObject HttpRequestBody()
        {
            return new JObject(
                new JProperty("action", "overwrite"),
                new JProperty("inputs", new JArray(new JObject(
                    new JProperty("id", _inputID),
                    new JProperty("data", new JObject(
                        new JProperty("metadata", _metadata)))))));
        }
    }
}
