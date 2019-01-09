using System.Threading.Tasks;
using Clarifai.Internal.GRPC;
using Google.Protobuf;
using Concept = Clarifai.DTOs.Predictions.Concept;

namespace Clarifai.API.Requests.Concepts
{
    /// <summary>
    /// A request for getting concept.
    /// </summary>
    public class GetConceptRequest : ClarifaiRequest<Concept>
    {
        private readonly string _conceptID;

        protected override RequestMethod Method => RequestMethod.GET;
        protected override string Url => "/v2/concepts/" + _conceptID;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="conceptID">the concept ID</param>
        public GetConceptRequest(IClarifaiHttpClient httpClient, string conceptID)
            : base(httpClient)
        {
            _conceptID = conceptID;
        }

        /// <inheritdoc />
        protected override Concept Unmarshaller(dynamic responseD)
        {
            SingleConceptResponse response = responseD;

            return Concept.GrpcDeserialize(response.Concept);
        }

        /// <inheritdoc />
        protected override async Task<IMessage> GrpcRequestBody(V2.V2Client grpcClient)
        {
            return await grpcClient.GetConceptAsync(new Internal.GRPC.GetConceptRequest());
        }
    }
}
