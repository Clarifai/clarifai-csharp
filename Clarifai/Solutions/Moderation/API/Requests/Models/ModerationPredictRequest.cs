using System.Collections.Generic;
using Clarifai.API;
using Clarifai.API.Requests;
using Clarifai.DTOs.Inputs;
using Clarifai.Exceptions;
using Clarifai.Solutions.Moderation.DTOs;
using Newtonsoft.Json.Linq;

namespace Clarifai.Solutions.Moderation.API.Requests.Models
{
    public class ModerationPredictRequest: ClarifaiRequest<ModerationOutput>
    {
        protected override RequestMethod Method => RequestMethod.POST;
        protected override string Url => $"/v2/models/{_modelID}/outputs";

        private readonly string _modelID;
        private readonly IClarifaiInput _input;

        public ModerationPredictRequest(IClarifaiHttpClient httpClient, string modelID,
            IClarifaiInput input): base(httpClient)
        {
            _modelID = modelID;
            _input = input;
        }

        /// <inheritdoc />
        protected override JObject HttpRequestBody()
        {
            return new JObject(
                new JProperty("inputs", new JArray(_input.Serialize())));
        }

        /// <inheritdoc />
        protected override ModerationOutput Unmarshaller(dynamic jsonObject)
        {
            if (jsonObject.outputs != null && jsonObject.outputs.Count == 1)
            {
                var jsonOutput = jsonObject.outputs[0];
                return ModerationOutput.Deserialize(jsonOutput);
            }
            throw new ClarifaiException("The response does not contain exactly one output.");
        }
    }
}
