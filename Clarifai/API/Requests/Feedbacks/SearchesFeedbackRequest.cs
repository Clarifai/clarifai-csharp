using System.Threading.Tasks;
using Clarifai.DTOs.Models.Outputs;
using Clarifai.Internal.GRPC;
using Google.Protobuf;

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
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="inputID">the input ID of a correct image (hit)</param>
        /// <param name="searchID">ID of the search from SearchInputsRequest.</param>
        /// <param name="endUserID">the ID associated with your end user</param>
        /// <param name="sessionID">the ID associated with your user's interface</param>
        public SearchesFeedbackRequest(IClarifaiHttpClient httpClient, string inputID, string searchID,
            string endUserID, string sessionID) : base(httpClient)
        {
            _inputId = inputID;
            _searchId = searchID;
            _endUserId = endUserID;
            _sessionId = sessionID;
        }

        /// <inheritdoc />
        protected override EmptyResponse Unmarshaller(dynamic responseD)
        {
            return new EmptyResponse();
        }

        /// <inheritdoc />
        protected override async Task<IMessage> GrpcRequestBody(V2.V2Client grpcClient)
        {
            return await grpcClient.PostSearchFeedbackAsync(new PostSearchFeedbackRequest
            {
                Input = new Input
                {
                    Id = _inputId,
                    FeedbackInfo = new FeedbackInfo
                    {
                        EventType = EventType.SearchClick,
                        SearchId = _searchId,
                        EndUserId = _endUserId,
                        SessionId = _sessionId
                    }
                }
            });
        }
    }
}
