using System;
using System.Collections.Generic;
using Clarifai.API;
using Clarifai.API.Requests.Models;
using Clarifai.DTOs.Inputs;
using Clarifai.DTOs.Models.OutputsInfo;
using Clarifai.DTOs.Predictions;
using Clarifai.Exceptions;

namespace Clarifai.DTOs.Models
{
    /// <inheritdoc />
    public abstract class Model : IModel
    {
        /// <summary>
        /// The Clarifai client.
        /// </summary>
        public IClarifaiClient Client { get; }

        /// <summary>
        /// The model ID.
        /// </summary>
        public string ModelID { get; }

        /// <summary>
        /// The model name.
        /// </summary>
        public string Name { get; }

        public IOutputInfo OutputInfo { get; }

        /// <summary>
        /// Date & time of model creation.
        /// </summary>
        public DateTime? CreatedAt { get; }

        /// <summary>
        /// The application ID.
        /// </summary>
        public string AppID { get; }

        /// <summary>
        /// The model version.
        /// </summary>
        public ModelVersion ModelVersion { get; }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="client">the Clarifai client</param>
        /// <param name="modelID">the model ID</param>
        /// <param name="name">the model name</param>
        /// <param name="createdAt">date & time of model creation</param>
        /// <param name="appID">the application ID</param>
        /// <param name="modelVersion">the model version</param>
        /// <param name="outputInfo">the output info</param>
        protected Model(IClarifaiClient client, string modelID, string name, DateTime? createdAt,
            string appID, ModelVersion modelVersion, IOutputInfo outputInfo)
        {
            Client = client;
            ModelID = modelID;
            Name = name;
            CreatedAt = createdAt;
            AppID = appID;
            ModelVersion = modelVersion;
            OutputInfo = outputInfo;
        }

        /// <summary>
        /// Deserializes the object out of a JSON dynamic object for a certain type.
        /// </summary>
        /// <param name="client">the Clarifai client</param>
        /// <param name="type">the type</param>
        /// <param name="model">the JSON dynamic object of a model</param>
        /// <returns>the deserialized object</returns>
        public static Model Deserialize(IClarifaiClient client, Type type, dynamic model)
        {
            var typeToDeserialization = new Dictionary<Type, Func<Model>> {
                { typeof(Color), () => ColorModel.Deserialize(client, model)},
                { typeof(Concept), () => ConceptModel.Deserialize(client, model)},
                { typeof(Demographics), () => DemographicsModel.Deserialize(client, model)},
                { typeof(Embedding), () => EmbeddingModel.Deserialize(client, model)},
                { typeof(FaceConcepts), () => FaceConceptsModel.Deserialize(client, model)},
                { typeof(FaceDetection), () => FaceDetectionModel.Deserialize(client, model)},
                { typeof(FaceEmbedding), () => FaceEmbeddingModel.Deserialize(client, model)},
                { typeof(Focus), () => FocusModel.Deserialize(client, model)},
                { typeof(Logo), () => LogoModel.Deserialize(client, model)},
                { typeof(Frame), () => VideoModel.Deserialize(client, model)},
            };

            if (!typeToDeserialization.ContainsKey(type))
            {
                throw new ClarifaiException(string.Format("Unknown model type: {}", type));
            }

            return typeToDeserialization[type]();
        }

        public override bool Equals(object obj)
        {
            return obj is Model model &&
                   EqualityComparer<IClarifaiClient>.Default.Equals(Client, model.Client) &&
                   ModelID == model.ModelID &&
                   Name == model.Name &&
                   EqualityComparer<DateTime?>.Default.Equals(CreatedAt, model.CreatedAt) &&
                   AppID == model.AppID &&
                   EqualityComparer<ModelVersion>.Default.Equals(ModelVersion, model.ModelVersion);
        }

        public override int GetHashCode()
        {
            var hashCode = 208641880;
            hashCode = hashCode * -1521134295 +
                       EqualityComparer<IClarifaiClient>.Default.GetHashCode(Client);
            hashCode = hashCode * -1521134295 +
                       EqualityComparer<string>.Default.GetHashCode(ModelID);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 +
                       EqualityComparer<DateTime?>.Default.GetHashCode(CreatedAt);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(AppID);
            hashCode = hashCode * -1521134295 +
                       EqualityComparer<ModelVersion>.Default.GetHashCode(ModelVersion);
            return hashCode;
        }

        public override string ToString()
        {
            return $"[Model: (modelID: {ModelID}]";
        }
    }

    /// <inheritdoc cref="IModel{T}" />
    public class Model<T> : Model, IModel<T> where T : IPrediction
    {
        /// <inheritdoc />
        protected Model(IClarifaiClient client, string modelID, string name, DateTime? createdAt,
            string appID, ModelVersion modelVersion, IOutputInfo outputInfo)
            : base(client, modelID, name, createdAt, appID, modelVersion, outputInfo)
        { }

        /// <inheritdoc />
        public DeleteModelVersionRequest DeleteModelVersion(string modelVersionID)
        {
            return new DeleteModelVersionRequest(Client, ModelID, modelVersionID);
        }

        /// <inheritdoc />
        public GetModelInputsRequest GetModelInputs()
        {
            return new GetModelInputsRequest(Client, ModelID, ModelVersion?.ID);
        }

        /// <inheritdoc />
        public GetModelVersionRequest GetModelVersion(string modelVersionID)
        {
            return new GetModelVersionRequest(Client, ModelID, modelVersionID);
        }

        /// <inheritdoc />
        public GetModelVersionsRequest GetModelVersions()
        {
            return new GetModelVersionsRequest(Client, ModelID);
        }

        /// <inheritdoc />
        public PredictRequest<T> Predict(IClarifaiInput input, string language = null,
            decimal? minValue = null, int? maxConcepts = null,
            IEnumerable<Concept> selectConcepts = null)
        {
            return Predict(new List<IClarifaiInput> {input}, language, minValue, maxConcepts,
                selectConcepts);
        }

        /// <inheritdoc />
        public PredictRequest<T> Predict(IEnumerable<IClarifaiInput> inputs, string language = null,
            decimal? minValue =  null, int? maxConcepts = null,
            IEnumerable<Concept> selectConcepts = null)
        {
            return new PredictRequest<T>(Client, ModelID, inputs, ModelVersion?.ID, language,
                minValue, maxConcepts, selectConcepts);
        }

        /// <inheritdoc />
        public TrainModelRequest<T> TrainModel()
        {
            return new TrainModelRequest<T>(Client, ModelID);
        }

        /// <summary>
        /// Deserializes the object out of a JSON dynamic object.
        /// </summary>
        /// <param name="client">the Clarifai client</param>
        /// <param name="model">the JSON dynamic object of a model</param>
        /// <returns>the deserialized object</returns>
        public static Model<T> Deserialize(IClarifaiClient client, dynamic model)
        {
            Type type = typeof(T);
            return Model.Deserialize(client, type, model);
        }
    }
}
