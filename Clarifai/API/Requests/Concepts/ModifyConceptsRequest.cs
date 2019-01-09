using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clarifai.Internal.GRPC;
using Google.Protobuf;
using Concept = Clarifai.DTOs.Predictions.Concept;

namespace Clarifai.API.Requests.Concepts
{
    /// <summary>
    /// A request for modifying concepts.
    /// </summary>
    public class ModifyConceptsRequest : ClarifaiRequest<List<Concept>>
    {
        private readonly IEnumerable<Concept> _concepts;

        protected override RequestMethod Method => RequestMethod.PATCH;
        protected override string Url => "/v2/concepts/";

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="concepts">the concepts</param>
        public ModifyConceptsRequest(IClarifaiHttpClient httpClient, params Concept[] concepts) :
            this(httpClient, concepts.ToList())
        { }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="concepts">the concepts</param>
        public ModifyConceptsRequest(IClarifaiHttpClient httpClient, IEnumerable<Concept> concepts)
            : base(httpClient)
        {
            _concepts = concepts;
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
            return await grpcClient.PatchConceptsAsync(new PatchConceptsRequest
            {
                Action = "overwrite",
                Concepts = {_concepts.Select(i => i.GrpcSerialize())}
            });
        }
    }
}
