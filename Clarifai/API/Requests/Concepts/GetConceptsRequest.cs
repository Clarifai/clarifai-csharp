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
        /// <param name="client">the Clarifai client</param>
        public GetConceptsRequest(IClarifaiClient client) : base(client)
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
