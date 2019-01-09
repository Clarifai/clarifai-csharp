using System;
using System.Collections.Generic;
using Clarifai.API;
using Clarifai.API.Requests;
using Clarifai.DTOs.Inputs;
using Clarifai.Exceptions;
using Clarifai.Solutions.Moderation.DTOs;
using Newtonsoft.Json.Linq;

namespace Clarifai.Solutions.Moderation.API.Requests.Inputs
{
    public class GetModerationStatusRequest: ClarifaiJsonRequest<ModerationStatus>
    {
        protected override RequestMethod Method => RequestMethod.GET;
        protected override string Url => $"/v2/inputs/{_inputID}/outputs";

        private readonly string _inputID;

        public GetModerationStatusRequest(IClarifaiHttpClient httpClient, string inputID)
            : base(httpClient)
        {
            _inputID = inputID;
        }

        /// <inheritdoc />
        protected override JObject HttpRequestBody()
        {
            return new JObject();
        }

        /// <inheritdoc />
        protected override ModerationStatus Unmarshaller(dynamic jsonObject)
        {
            return ModerationStatus.Deserialize(jsonObject.moderation.status);
        }
    }
}
