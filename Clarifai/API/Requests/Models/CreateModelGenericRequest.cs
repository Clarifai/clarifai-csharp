using System.Collections.Generic;
using Clarifai.DTOs.Models;
using Clarifai.DTOs.Models.OutputsInfo;
using Clarifai.DTOs.Predictions;
using Newtonsoft.Json.Linq;

namespace Clarifai.API.Requests.Models
{
    /// <summary>
    /// A request for creating new models of all types.
    /// </summary>
    public class CreateModelGenericRequest<T> : ClarifaiRequest<IModel<T>> where T : IPrediction
    {
        protected override RequestMethod Method => RequestMethod.POST;
        protected override string Url => "/v2/models/";

        private readonly string _modelID;
        private readonly string _name;
        private readonly IOutputInfo _outputInfo;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="modelID">the model ID</param>
        /// <param name="name">the model name</param>
        /// <param name="outputInfo"></param>
        public CreateModelGenericRequest(IClarifaiHttpClient httpClient, string modelID,
            string name = null, IOutputInfo outputInfo = null) : base(httpClient)
        {
            _modelID = modelID;
            _name = name;
            _outputInfo = outputInfo;
        }

        /// <inheritdoc />
        protected override IModel<T> Unmarshaller(dynamic jsonObject)
        {
            return Model<T>.Deserialize(HttpClient, jsonObject.model);
        }

        /// <inheritdoc />
        protected override JObject HttpRequestBody()
        {
            var body = new JObject(new JProperty("id", _modelID));
            if (_name != null)
            {
                body.Add("name", _name);
            }

            if (_outputInfo != null)
            {
                body.Add("output_info", _outputInfo.Serialize());
            }
            return new JObject(
                new JProperty("model", body));
        }
    }
}
