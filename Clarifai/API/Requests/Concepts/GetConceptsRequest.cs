using System.Collections.Generic;
using Clarifai.DTOs.Predictions;
using Newtonsoft.Json.Linq;

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
        protected override JObject HttpRequestBody()
        {
            return new JObject();
        }
    }
}
