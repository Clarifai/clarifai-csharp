using System.Collections.Generic;
using Clarifai.DTOs.Predictions;
using Newtonsoft.Json.Linq;

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
        protected override List<Concept> Unmarshaller(dynamic jsonObject)
        {
            var concepts = new List<Concept>();
            foreach (var concept in jsonObject.concepts)
            {
                concepts.Add(Concept.Deserialize(concept));
            }
            return concepts;
        }

        /// <inheritdoc />
        protected override JObject PaginatedHttpRequestBody()
        {
            var body = base.PaginatedHttpRequestBody();

            body["concept_query"] = new JObject(
                    new JProperty("name", _query),
                    new JProperty("language", _language));
            return body;
        }
    }
}
