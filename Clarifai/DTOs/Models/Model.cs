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
        /// The HTTP client.
        /// </summary>
        public IClarifaiHttpClient HttpClient { get; }

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
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="modelID">the model ID</param>
        /// <param name="name">the model name</param>
        /// <param name="createdAt">date & time of model creation</param>
        /// <param name="appID">the application ID</param>
        /// <param name="modelVersion">the model version</param>
        /// <param name="outputInfo">the output info</param>
        protected Model(IClarifaiHttpClient httpClient, string modelID, string name,
            DateTime? createdAt, string appID, ModelVersion modelVersion, IOutputInfo outputInfo)
        {
            HttpClient = httpClient;
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
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="type">the type</param>
        /// <param name="model">the JSON dynamic object of a model</param>
        /// <returns>the deserialized object</returns>
        [Obsolete]
        public static Model Deserialize(IClarifaiHttpClient httpClient, Type type, dynamic model)
        {
            var typeToDeserialization = new Dictionary<Type, Func<Model>> {
                { typeof(Color), () => ColorModel.Deserialize(httpClient, model)},
                { typeof(Concept), () => ConceptModel.Deserialize(httpClient, model)},
                { typeof(Demographics), () => DemographicsModel.Deserialize(httpClient, model)},
                { typeof(Embedding), () => EmbeddingModel.Deserialize(httpClient, model)},
                { typeof(FaceConcepts), () => FaceConceptsModel.Deserialize(httpClient, model)},
                { typeof(FaceDetection), () => FaceDetectionModel.Deserialize(httpClient, model)},
                { typeof(FaceEmbedding), () => FaceEmbeddingModel.Deserialize(httpClient, model)},
                { typeof(Focus), () => FocusModel.Deserialize(httpClient, model)},
                { typeof(Logo), () => LogoModel.Deserialize(httpClient, model)},
                { typeof(Frame), () => VideoModel.Deserialize(httpClient, model)},
            };

            if (!typeToDeserialization.ContainsKey(type))
            {
                throw new ClarifaiException(string.Format("Unknown model type: {0}", type));
            }

            return typeToDeserialization[type]();
        }

        public static Model GrpcDeserialize(IClarifaiHttpClient httpClient, Type type,
            Internal.GRPC.Model model)
        {
            var typeToDeserialization = new Dictionary<Type, Func<Model>> {
                { typeof(Color), () => ColorModel.GrpcDeserialize(httpClient, model)},
                { typeof(Concept), () => ConceptModel.GrpcDeserialize(httpClient, model)},
                { typeof(Demographics), () => DemographicsModel.GrpcDeserialize(httpClient, model)},
                { typeof(Embedding), () => EmbeddingModel.GrpcDeserialize(httpClient, model)},
                { typeof(FaceConcepts), () => FaceConceptsModel.GrpcDeserialize(httpClient, model)},
                { typeof(FaceDetection), () => FaceDetectionModel.GrpcDeserialize(httpClient, model)},
                { typeof(FaceEmbedding), () => FaceEmbeddingModel.GrpcDeserialize(httpClient, model)},
                { typeof(Focus), () => FocusModel.GrpcDeserialize(httpClient, model)},
                { typeof(Logo), () => LogoModel.GrpcDeserialize(httpClient, model)},
                { typeof(Frame), () => VideoModel.GrpcDeserialize(httpClient, model)},
            };

            if (!typeToDeserialization.ContainsKey(type))
            {
                throw new ClarifaiException(string.Format("Unknown model type: {0}", type));
            }

            return typeToDeserialization[type]();
        }

        public override bool Equals(object obj)
        {
            return obj is Model model &&
                   EqualityComparer<IClarifaiHttpClient>.Default.Equals(HttpClient, model.HttpClient) &&
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
                       EqualityComparer<IClarifaiHttpClient>.Default.GetHashCode(HttpClient);
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
        protected Model(IClarifaiHttpClient httpClient, string modelID, string name,
            DateTime? createdAt, string appID, ModelVersion modelVersion, IOutputInfo outputInfo)
            : base(httpClient, modelID, name, createdAt, appID, modelVersion, outputInfo)
        { }

        /// <inheritdoc />
        public DeleteModelVersionRequest DeleteModelVersion(string modelVersionID)
        {
            return new DeleteModelVersionRequest(HttpClient, ModelID, modelVersionID);
        }

        /// <inheritdoc />
        public GetModelInputsRequest GetModelInputs()
        {
            return new GetModelInputsRequest(HttpClient, ModelID, ModelVersion?.ID);
        }

        /// <inheritdoc />
        public GetModelVersionRequest GetModelVersion(string modelVersionID)
        {
            return new GetModelVersionRequest(HttpClient, ModelID, modelVersionID);
        }

        /// <inheritdoc />
        public GetModelVersionsRequest GetModelVersions()
        {
            return new GetModelVersionsRequest(HttpClient, ModelID);
        }

        /// <inheritdoc />
        public PredictRequest<T> Predict(IClarifaiInput input, string language = null,
            decimal? minValue = null, int? maxConcepts = null,
            IEnumerable<Concept> selectConcepts = null, int? sampleMs = null)
        {
            return new PredictRequest<T>(HttpClient, ModelID, input, ModelVersion?.ID, language,
                minValue, maxConcepts, selectConcepts, sampleMs);
        }

        /// <inheritdoc />
        public BatchPredictRequest<T> BatchPredict(IEnumerable<IClarifaiInput> inputs,
            string language = null, decimal? minValue =  null, int? maxConcepts = null,
            IEnumerable<Concept> selectConcepts = null)
        {
            return new BatchPredictRequest<T>(HttpClient, ModelID, inputs, ModelVersion?.ID, language,
                minValue, maxConcepts, selectConcepts);
        }

        /// <inheritdoc />
        public TrainModelRequest<T> TrainModel()
        {
            return new TrainModelRequest<T>(HttpClient, ModelID);
        }

        /// <summary>
        /// Deserializes the object out of a JSON dynamic object.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="model">the JSON dynamic object of a model</param>
        /// <returns>the deserialized object</returns>
        [Obsolete]
        public static Model<T> Deserialize(IClarifaiHttpClient httpClient, dynamic model)
        {
            Type type = typeof(T);
            return Model.Deserialize(httpClient, type, model);
        }

        /// <summary>
        /// Deserializes the object out of a gRPC object.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="model">the JSON dynamic object of a model</param>
        /// <returns>the deserialized object</returns>
        public static Model<T> GrpcDeserialize(IClarifaiHttpClient httpClient,
            Internal.GRPC.Model model)
        {
            Type type = typeof(T);
            return (Model<T>) Model.GrpcDeserialize(httpClient, type, model);
        }
    }
}
