using Clarifai.DTOs.Models.Outputs;
using Newtonsoft.Json.Linq;

namespace Clarifai.API.Requests.Models
{
    /// <summary>
    /// A request for deleting a model.
    /// </summary>
    public class DeleteModelRequest : ClarifaiRequest<EmptyResponse>
    {
        protected override RequestMethod Method => RequestMethod.DELETE;
        protected override string Url => "/v2/models/" + _modelID;

        private readonly string _modelID;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="client">the Clarifai client</param>
        /// <param name="modelID">the model ID</param>
        public DeleteModelRequest(IClarifaiClient client, string modelID) : base(client)
        {
            _modelID = modelID;
        }

        /// <inheritdoc />
        protected override JObject HttpRequestBody()
        {
            return new JObject();
        }

        /// <inheritdoc />
        protected override EmptyResponse Unmarshaller(dynamic jsonObject)
        {
            return new EmptyResponse();
        }
    }
}
