using Clarifai.DTOs.Models;
using Clarifai.DTOs.Predictions;
using Newtonsoft.Json.Linq;

namespace Clarifai.API.Requests.Models
{
    /// <summary>
    /// A request for training a model.
    /// </summary>
    /// <typeparam name="T">the model type</typeparam>
    public class TrainModelRequest<T> : ClarifaiRequest<IModel<T>> where T : IPrediction
    {
        protected override RequestMethod Method => RequestMethod.POST;
        protected override string Url => $"/v2/models/{_modelID}/versions";

        private readonly string _modelID;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="client">the Clarifai client</param>
        /// <param name="modelID">the model ID</param>
        public TrainModelRequest(IClarifaiClient client, string modelID) : base(client)
        {
            _modelID = modelID;
        }

        /// <inheritdoc />
        protected override JObject HttpRequestBody()
        {
            return new JObject();
        }

        /// <inheritdoc />
        protected override IModel<T> Unmarshaller(dynamic jsonObject)
        {
            return Model<T>.Deserialize(Client, jsonObject.model);
        }
    }
}
