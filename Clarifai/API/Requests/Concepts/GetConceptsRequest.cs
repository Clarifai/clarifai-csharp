using System.Collections.Generic;
using System.Threading.Tasks;
using Clarifai.Internal.GRPC;
using Google.Protobuf;
using Concept = Clarifai.DTOs.Predictions.Concept;

namespace Clarifai.API.Requests.Concepts
{
    /// <summary>
    /// A request for getting concepts.
    /// </summary>
    public class GetConceptsRequest : ClarifaiPaginatedRequest<List<Concept>>
    {
        protected override RequestMethod Method => RequestMethod.GET;
        protected override string Url => "/v2/concepts/";

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        public GetConceptsRequest(IClarifaiHttpClient httpClient) : base(httpClient)
        { }

        /// <inheritdoc />
        protected override List<Concept> Unmarshaller(dynamic responseD)
        {
            MultiConceptResponse response = responseD;

            var concepts = new List<Concept>();
            foreach (Internal.GRPC.Concept concept in response.Concepts)
            {
                concepts.Add(Concept.GrpcDeserialize(concept));
            }
            return concepts;
        }

        /// <inheritdoc />
        protected override async Task<IMessage> GrpcRequestBody(V2.V2Client grpcClient)
        {
            return await grpcClient.ListConceptsAsync(new ListConceptsRequest());
        }
    }
}
