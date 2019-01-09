using System.Collections.Generic;
using System.Threading.Tasks;
using Clarifai.DTOs.Models;
using Clarifai.Internal.GRPC;
using Google.Protobuf;
using Concept = Clarifai.DTOs.Predictions.Concept;

namespace Clarifai.API.Requests.Models
{
    /// <summary>
    /// Request for modifying a model.
    /// </summary>
    public class ModifyModelRequest : ClarifaiRequest<ConceptModel>
    {
        protected override RequestMethod Method => RequestMethod.PATCH;
        protected override string Url => "/v2/models";

        private readonly string _modelID;
        private readonly ModifyAction _action;
        private readonly string _name;
        private readonly IEnumerable<Concept> _concepts;
        private readonly bool? _areConceptsMutuallyExclusive;
        private readonly bool? _isEnvironmentClosed;
        private readonly string _language;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="modelID">the model ID</param>
        /// <param name="action">the modification action</param>
        /// <param name="name">the new model name</param>
        /// <param name="concepts">the concepts update the model with</param>
        /// <param name="areConceptsMutuallyExclusive">are concepts mutually exclusive</param>
        /// <param name="isEnvironmentClosed">is environment closed</param>
        /// <param name="language">the language</param>
        public ModifyModelRequest(IClarifaiHttpClient httpClient, string modelID,
            ModifyAction action = null, string name = null, IEnumerable<Concept> concepts = null,
            bool? areConceptsMutuallyExclusive = null, bool? isEnvironmentClosed = null,
            string language = null) : base(httpClient)
        {
            _modelID = modelID;
            _action = action;
            _name = name;
            _concepts = concepts;
            _areConceptsMutuallyExclusive = areConceptsMutuallyExclusive;
            _isEnvironmentClosed = isEnvironmentClosed;
            _language = language;
        }

        /// <inheritdoc />
        protected override ConceptModel Unmarshaller(dynamic responseD)
        {
            MultiModelResponse response = responseD;

            return ConceptModel.GrpcDeserialize(HttpClient, response.Models[0]);
        }

        /// <inheritdoc />
        protected override async Task<IMessage> GrpcRequestBody(V2.V2Client grpcClient)
        {
            var model = new ConceptModel(HttpClient, _modelID, name: _name);

            return await grpcClient.PatchModelsAsync(new PatchModelsRequest
            {
                Models = {
                    model.GrpcSerialize(_concepts, _areConceptsMutuallyExclusive,
                        _isEnvironmentClosed, _language)
                },
                Action = (_action ?? ModifyAction.Merge).GrpcSerialize()
            });
        }
    }
}
