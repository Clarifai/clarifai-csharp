using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clarifai.DTOs.Searches;
using Clarifai.Internal.GRPC;
using Google.Protobuf;

namespace Clarifai.API.Requests.Inputs
{
    /// <summary>
    /// A request for searching inputs.
    /// </summary>
    public class SearchInputsRequest : ClarifaiPaginatedRequest<SearchInputsResult>
    {
        protected override RequestMethod Method => RequestMethod.POST;
        protected override string Url => "/v2/searches";

        private readonly IEnumerable<SearchBy> _searchBys;
        private readonly string _language;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="searchBys">the search clauses</param>
        public SearchInputsRequest(IClarifaiHttpClient httpClient, params SearchBy[] searchBys)
            : this(httpClient, searchBys.ToList())
        { }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="searchBys">the search clauses</param>
        /// <param name="language">the language</param>
        public SearchInputsRequest(IClarifaiHttpClient httpClient, IEnumerable<SearchBy> searchBys,
            string language = null) : base(httpClient)
        {
            _searchBys = searchBys;
            _language = language;
        }

        /// <inheritdoc />
        protected override SearchInputsResult Unmarshaller(dynamic responseD)
        {
            MultiSearchResponse response = responseD;
            return SearchInputsResult.GrpcDeserialize(response);
        }

        /// <inheritdoc />
        protected override async Task<IMessage> GrpcRequestBody(V2.V2Client grpcClient)
        {
            var query = new Query
            {
                Ands = { _searchBys.Select(sb => sb.GrpcSerialize()) }
            };
            if (_language != null)
            {
                query = new Query(query)
                {
                    Language = _language
                };
            }
            return await grpcClient.PostSearchesAsync(new PostSearchesRequest
            {
                Query = query
            });
        }
    }
}
