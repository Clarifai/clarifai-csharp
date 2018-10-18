using System.Collections.Generic;
using System.Linq;
using Clarifai.DTOs.Predictions;
using Newtonsoft.Json.Linq;

namespace Clarifai.API.Requests.Concepts
{
    /// <summary>
    /// A request for adding concepts.
    /// </summary>
    public class AddConceptsRequest : ClarifaiRequest<List<Concept>>
    {
        private readonly IEnumerable<Concept> _concepts;

        protected override RequestMethod Method => RequestMethod.POST;
        protected override string Url => "/v2/concepts/";

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="concepts">the concepts</param>
        public AddConceptsRequest(IClarifaiHttpClient httpClient, params Concept[] concepts)
            : this(httpClient, concepts.ToList())
        { }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="concepts">the concepts</param>
        public AddConceptsRequest(IClarifaiHttpClient httpClient, IEnumerable<Concept> concepts)
            : base(httpClient)
        {
            _concepts = concepts;
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
        protected override JObject HttpRequestBody()
        {
            return new JObject(
                new JProperty("concepts",
                    new JArray(_concepts.Select(i => i.Serialize()))));
        }
    }
}
