using System.Collections.Generic;
using System.Linq;
using Clarifai.DTOs.Searches;
using Newtonsoft.Json.Linq;

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
        /// <param name="client">the Clarifai client</param>
        /// <param name="searchBys">the search clauses</param>
        public SearchInputsRequest(IClarifaiClient client, params SearchBy[] searchBys)
            : this(client, searchBys.ToList())
        { }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="client">the Clarifai client</param>
        /// <param name="searchBys">the search clauses</param>
        /// <param name="language">the language</param>
        public SearchInputsRequest(IClarifaiClient client, IEnumerable<SearchBy> searchBys,
            string language = null) : base(client)
        {
            _searchBys = searchBys;
            _language = language;
        }

        /// <inheritdoc />
        protected override SearchInputsResult Unmarshaller(dynamic jsonObject)
        {
            return SearchInputsResult.Deserialize(jsonObject);
        }

        /// <inheritdoc />
        protected override JObject HttpRequestBody()
        {
            var query = new JObject(
                new JProperty("ands", _searchBys.Select(c => c.Serialize())));
            if (_language != null)
            {
                query["language"] = _language;
            }
            return new JObject(new JProperty("query", query));
        }
    }
}
