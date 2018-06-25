using System.Collections.Generic;
using System.Linq;
using Clarifai.DTOs.Feedbacks;
using Clarifai.DTOs.Models.Outputs;
using Newtonsoft.Json.Linq;

namespace Clarifai.API.Requests.Feedbacks
{
    /// <summary>
    /// Used to send a feedback of prediction success to the model.
    /// </summary>
    public class ModelFeedbackRequest : ClarifaiRequest<EmptyResponse>
    {
        protected override RequestMethod Method => RequestMethod.POST;
        protected override string Url
        {
            get
            {
                if (_modelVersionID == null)
                {
                    return $"/v2/models/{_modelID}/feedback";
                }
                else
                {
                    return $"/v2/models/{_modelID}/versions/{_modelVersionID}/feedback";
                }
            }
        }

        private readonly string _modelID;
        private readonly string _imageUrl;
        private readonly string _inputID;
        private readonly string _outputID;
        private readonly IEnumerable<ConceptFeedback> _concepts;
        private readonly IEnumerable<RegionFeedback> _regions;
        private readonly string _modelVersionID;
        private readonly string _endUserID;
        private readonly string _sessionID;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="client">the Clarifai client</param>
        /// <param name="modelID">the model ID</param>
        /// <param name="imageUrl">the image URL</param>
        /// <param name="inputID">the input ID</param>
        /// <param name="outputID">the output ID</param>
        /// <param name="endUserID">the end user ID</param>
        /// <param name="sessionID">the session ID</param>
        /// <param name="concepts">the concepts</param>
        /// <param name="regions">the regions</param>
        /// <param name="modelVersionID">the model version ID</param>
        public ModelFeedbackRequest(IClarifaiClient client, string modelID, string imageUrl,
            string inputID, string outputID, string endUserID, string sessionID,
            IEnumerable<ConceptFeedback> concepts = null,
            IEnumerable<RegionFeedback> regions = null, string modelVersionID = null) : base(client)
        {
            _modelID = modelID;
            _imageUrl = imageUrl;
            _inputID = inputID;
            _outputID = outputID;
            _endUserID = endUserID;
            _sessionID = sessionID;
            _concepts = concepts;
            _regions = regions;
            _modelVersionID = modelVersionID;
        }

        /// <inheritdoc />
        protected override EmptyResponse Unmarshaller(dynamic jsonObject)
        {
            return new EmptyResponse();
        }

        /// <inheritdoc />
        protected override JObject HttpRequestBody()
        {
            var data = new JObject(
                new JProperty("image", new JObject(
                    new JProperty("url", _imageUrl))));
            if (_concepts != null)
            {
                data["concepts"] = new JArray(_concepts.Select(c => c.Serialize()));
            }
            if (_regions != null)
            {
                data["regions"] = new JArray(_regions.Select(r => r.Serialize()));
            }

            return new JObject(
                new JProperty("input", new JObject(
                    new JProperty("id", _inputID),
                    new JProperty("data", data),
                    new JProperty("feedback_info", new JObject(
                        new JProperty("event_type", "annotation"),
                        new JProperty("output_id", _outputID),
                        new JProperty("end_user_id", _endUserID),
                        new JProperty("session_id", _sessionID))))));
        }
    }
}
