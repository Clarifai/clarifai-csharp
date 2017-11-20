using Clarifai.DTOs.Models.Outputs;
using Newtonsoft.Json.Linq;

namespace Clarifai.API.Requests.Models
{
    /// <summary>
    /// Deletes all custom models.
    /// </summary>
    public class DeleteAllModelsRequest : ClarifaiRequest<EmptyResponse>
    {
        protected override RequestMethod Method => RequestMethod.DELETE;
        protected override string Url => "/v2/models/";

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="client">the Clarifai client</param>
        public DeleteAllModelsRequest(IClarifaiClient client) : base(client)
        { }

        /// <inheritdoc />
        protected override JObject HttpRequestBody()
        {
            return new JObject(
                new JProperty("delete_all", true));
        }

        /// <inheritdoc />
        protected override EmptyResponse Unmarshaller(dynamic jsonObject)
        {
            return new EmptyResponse();
        }
    }
}
