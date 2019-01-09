using System.Collections.Generic;
using System.Linq;
using Clarifai.Internal.GRPC;

namespace Clarifai.DTOs.Searches
{
    public class SearchInputsResult
    {
        public string ID { get; }
        public List<SearchHit> SearchHits { get; }

        private SearchInputsResult(string id, List<SearchHit> searchHits)
        {
            ID = id;
            SearchHits = searchHits;
        }

        public static SearchInputsResult Deserialize(dynamic jsonObject)
        {
            var searchHits = new List<SearchHit>();
            foreach (dynamic hit in jsonObject.hits)
            {
                searchHits.Add(SearchHit.Deserialize(hit));
            }
            return new SearchInputsResult((string)jsonObject.id, searchHits);
        }

        public static SearchInputsResult GrpcDeserialize(MultiSearchResponse response)
        {
            return new SearchInputsResult(
                response.Id, response.Hits.Select(SearchHit.GrpcDeserialize).ToList());
        }
    }
}
