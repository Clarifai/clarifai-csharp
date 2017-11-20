using Clarifai.DTOs.Models.Outputs;
using Newtonsoft.Json.Linq;

namespace Clarifai.API.Requests.Feedbacks
{
    /// <summary>
    /// This request is meant to collect the correctly searched inputs, which is usually done by
    /// capturing your end user's clicks on the given search results. Your feedback will help us
    /// improve our search algorithm.
    /// </summary>
    public class SearchesFeedbackRequest : ClarifaiRequest<EmptyResponse>
    {
        protected override RequestMethod Method => RequestMethod.POST;
        protected override string Url => "/v2/searches/feedback";

        private readonly string _inputId;
        private readonly string _searchId;
        private readonly string _endUserId;
        private readonly string _sessionId;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="client">the Clarifai client</param>
        /// <param name="inputID">the input ID of a correct image (hit)</param>
        /// <param name="searchID">ID of the search from SearchInputsRequest.</param>
        /// <param name="endUserID">the ID associated with your end user</param>
        /// <param name="sessionID">the ID associated with your user's interface</param>
        public SearchesFeedbackRequest(IClarifaiClient client, string inputID, string searchID,
            string endUserID, string sessionID) : base(client)
        {
            _inputId = inputID;
            _searchId = searchID;
            _endUserId = endUserID;
            _sessionId = sessionID;
        }

        /// <inheritdoc />
        protected override EmptyResponse Unmarshaller(dynamic jsonObject)
        {
            return new EmptyResponse();
        }

        /// <inheritdoc />
        protected override JObject HttpRequestBody()
        {
            return new JObject(
                new JProperty("input", new JObject(
                    new JProperty("id", _inputId),
                    new JProperty("feedback_info", new JObject(
                        new JProperty("event_type", "search_click"),
                        new JProperty("search_id", _searchId),
                        new JProperty("end_user_id", _endUserId),
                        new JProperty("session_id", _sessionId))))));
        }
    }
}
