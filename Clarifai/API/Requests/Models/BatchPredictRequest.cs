using System.Collections.Generic;
using System.Linq;
using Clarifai.DTOs.Inputs;
using Clarifai.DTOs.Models.Outputs;
using Clarifai.DTOs.Predictions;
using Newtonsoft.Json.Linq;

namespace Clarifai.API.Requests.Models
{
    /// <summary>
    /// Request for running predictions on a model for multiple inputs at the same time.
    /// </summary>
    /// <typeparam name="T">the model type</typeparam>
    public class BatchPredictRequest<T> : ClarifaiRequest<List<ClarifaiOutput<T>>>
        where T : IPrediction
    {
        protected override RequestMethod Method => RequestMethod.POST;
        protected override string Url
        {
            get
            {
                if (_modelVersionID == null)
                {
                    return $"/v2/models/{_modelID}/outputs";
                }
                else
                {
                    return $"/v2/models/{_modelID}/versions/{_modelVersionID}/outputs";
                }
            }
        }

        private readonly string _modelID;
        private readonly IEnumerable<IClarifaiInput> _inputs;
        private readonly string _modelVersionID;
        private readonly string _language;
        private readonly decimal? _minValue;
        private readonly int? _maxConcepts;
        private readonly IEnumerable<Concept> _selectConcepts;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="modelID">the model ID</param>
        /// <param name="inputs">Clarifai inputs</param>
        /// <param name="modelVersionID">the model version ID - leave null for latest</param>
        /// <param name="language">the language</param>
        /// <param name="minValue">
        /// only predictions with a value greater than or equal to to minValue will be returned
        /// </param>
        /// <param name="maxConcepts">
        /// the maximum maxConcepts number of predictions that will be returned
        /// </param>
        /// <param name="selectConcepts">only selectConcepts will be returned</param>
        public BatchPredictRequest(IClarifaiHttpClient httpClient, string modelID,
            IEnumerable<IClarifaiInput> inputs, string modelVersionID = null,
            string language = null, decimal? minValue =  null, int? maxConcepts = null,
            IEnumerable<Concept> selectConcepts = null)
            : base(httpClient)
        {
            _modelID = modelID;
            _inputs = inputs;
            _modelVersionID = modelVersionID;
            _language = language;
            _minValue = minValue;
            _maxConcepts = maxConcepts;
            _selectConcepts = selectConcepts;
        }

        /// <inheritdoc />
        protected override JObject HttpRequestBody()
        {
            JObject body = new JObject(
                new JProperty("inputs", new JArray(_inputs.Select(i => i.Serialize()))));

            if (_language != null || _minValue != null || _maxConcepts != null ||
                _selectConcepts != null)
            {
                var outputConfig = new JObject();
                if (_language != null)
                {
                    outputConfig.Add("language", _language);
                }
                if (_minValue != null)
                {
                    outputConfig.Add("min_value", _minValue);
                }
                if (_maxConcepts != null)
                {
                    outputConfig.Add("max_concepts", _maxConcepts);
                }

                if (_selectConcepts != null)
                {
                    outputConfig.Add("select_concepts",
                        new JArray(_selectConcepts.Select(c => c.Serialize())));
                }

                body.Add(new JProperty("model", new JObject(
                    new JProperty("output_info", new JObject(
                        new JProperty("output_config", outputConfig))))));
            }
            return body;
        }

        /// <inheritdoc />
        protected override List<ClarifaiOutput<T>> Unmarshaller(dynamic jsonObject)
        {
            var outputs = new List<ClarifaiOutput<T>>();
            foreach (var jsonOutput in jsonObject.outputs)
            {
                outputs.Add(ClarifaiOutput<T>.Deserialize(HttpClient, jsonOutput));
            }
            return outputs;
        }
    }
}
