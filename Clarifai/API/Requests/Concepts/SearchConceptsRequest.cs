using System.Collections.Generic;
using System.Threading.Tasks;
using Clarifai.Internal.GRPC;
using Google.Protobuf;
using Concept = Clarifai.DTOs.Predictions.Concept;

namespace Clarifai.API.Requests.Concepts
{
    /// <summary>
    /// A request for searching concepts.
    /// </summary>
    public class SearchConceptsRequest : ClarifaiPaginatedRequest<List<Concept>>
    {
        protected override RequestMethod Method => RequestMethod.POST;
        protected override string Url =>  "/v2/concepts/searches/";

        private readonly string _query;
        private readonly string _language;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="query">the query to search concepts by</param>
        /// <param name="language">the language</param>
        public SearchConceptsRequest(IClarifaiHttpClient httpClient, string query,
            string language = null)
            : base(httpClient)
        {
            _query = query;
            _language = language;
        }

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
            var conceptQuery = new ConceptQuery
            {
                Name = _query,
            };
            if (_language != null)
            {
                conceptQuery.Language = _language;
            }
            return await grpcClient.PostConceptsSearchesAsync(new PostConceptsSearchesRequest
            {
                ConceptQuery = conceptQuery
            });
        }
    }
}
