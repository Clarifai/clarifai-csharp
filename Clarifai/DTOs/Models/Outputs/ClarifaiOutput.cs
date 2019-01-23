using System;
using System.Collections.Generic;
using System.Linq;
using Clarifai.API;
using Clarifai.DTOs.Inputs;
using Clarifai.DTOs.Predictions;
using Clarifai.Exceptions;
using Clarifai.Internal.GRPC;
using Newtonsoft.Json.Linq;
using Color = Clarifai.DTOs.Predictions.Color;
using Concept = Clarifai.DTOs.Predictions.Concept;
using Embedding = Clarifai.DTOs.Predictions.Embedding;
using Focus = Clarifai.DTOs.Predictions.Focus;
using Frame = Clarifai.DTOs.Predictions.Frame;
using Region = Clarifai.Internal.GRPC.Region;

namespace Clarifai.DTOs.Models.Outputs
{
    /// <summary>
    /// Output returned after running a request. Encapsulates the response content and other
    /// data the response returns.
    /// </summary>
    public class ClarifaiOutput
    {
        /// <summary>
        /// The output ID.
        /// </summary>
        public string ID { get; }

        /// <summary>
        /// The output status.
        /// </summary>
        public ClarifaiStatus Status { get; }

        /// <summary>
        /// Date & time of output creation.
        /// </summary>
        public DateTime CreatedAt { get; }

        /// <summary>
        /// The input.
        /// </summary>
        public IClarifaiInput Input { get; }


        /// <summary>
        /// The data.
        /// </summary>
        public List<IPrediction> Data { get; }

        /// <summary>
        /// The model used to make this prediction.
        /// </summary>
        public IModel Model { get; }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="id">the output ID</param>
        /// <param name="status">the output status</param>
        /// <param name="createdAt">date & time of output creation</param>
        /// <param name="input">the input</param>
        /// <param name="data">the data</param>
        /// <param name="model">the model</param>
        protected ClarifaiOutput(string id, ClarifaiStatus status, DateTime createdAt,
            IClarifaiInput input, List<IPrediction> data, IModel model)
        {
            ID = id;
            Status = status;
            CreatedAt = createdAt;
            Input = input;
            Data = data;
            Model = model;
        }

        /// <summary>
        /// Deserializes the object out of a JSON dynamic object.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="modelType">the model type</param>
        /// <param name="jsonObject">the JSON object</param>
        /// <returns>the deserialized object</returns>
        public static ClarifaiOutput Deserialize(IClarifaiHttpClient httpClient,
            ModelType modelType, dynamic jsonObject)
        {
            dynamic data = DeserializePredictions(modelType, jsonObject);

            return new ClarifaiOutput(
                (string)jsonObject.id,
                ClarifaiStatus.Deserialize(jsonObject.status),
                (DateTime) jsonObject.created_at,
                jsonObject.input != null ? ClarifaiInput.Deserialize(jsonObject.input) : null,
                data,
                Models.Model.Deserialize(httpClient, modelType.Prediction, jsonObject.model));
        }

        public static ClarifaiOutput GrpcDeserialize(IClarifaiHttpClient httpClient,
            ModelType modelType, Output output)
        {
            dynamic data = GrpcDeserializePredictions(modelType, output);
            return new ClarifaiOutput(
                output.Id,
                ClarifaiStatus.GrpcDeserialize(output.Status),
                output.CreatedAt.ToDateTime(),
                output.Input != null ? ClarifaiInput.GrpcDeserialize(output.Input) : null,
                data,
                Models.Model.GrpcDeserialize(httpClient, modelType.Prediction, output.Model));
        }

        protected static List<IPrediction> DeserializePredictions(ModelType modelType,
            dynamic jsonObject)
        {
            var propertyValues = (JObject) jsonObject.data;

            var data = new List<IPrediction>();
            if (propertyValues.Count > 0)
            {
                string typeName = modelType.Prediction.Name;
                switch (typeName)
                {
                    case "Color":
                    {
                        foreach (dynamic color in jsonObject.data.colors)
                        {
                            data.Add(Color.Deserialize(color));
                        }
                        break;
                    }
                    case "Concept":
                    {
                        foreach (dynamic concept in jsonObject.data.concepts)
                        {
                            data.Add(Concept.Deserialize(concept));
                        }
                        break;
                    }
                    case "Demographics":
                    {
                        foreach (dynamic demographics in jsonObject.data.regions)
                        {
                            data.Add(Demographics.Deserialize(demographics));
                        }
                        break;
                    }
                    case "Embedding":
                    {
                        foreach (dynamic embedding in jsonObject.data.embeddings)
                        {
                            data.Add(Embedding.Deserialize(embedding));
                        }
                        break;
                    }
                    case "FaceConcepts":
                    {
                        foreach (dynamic faceConcepts in
                            jsonObject.data.regions)
                        {
                            data.Add(FaceConcepts.Deserialize(faceConcepts));
                        }
                        break;
                    }
                    case "FaceDetection":
                    {
                        foreach (dynamic faceDetection in jsonObject.data.regions)
                        {
                            data.Add(FaceDetection.Deserialize(faceDetection));
                        }
                        break;
                    }
                    case "FaceEmbedding":
                    {
                        foreach (dynamic faceEmbedding in jsonObject.data.regions)
                        {
                            data.Add(FaceEmbedding.Deserialize(faceEmbedding));
                        }
                        break;
                    }
                    case "Focus":
                    {
                        foreach (dynamic focus in jsonObject.data.regions)
                        {
                            data.Add(Focus.Deserialize(focus,
                                (decimal) jsonObject.data.focus.value));
                        }
                        break;
                    }
                    case "Frame":
                    {
                        foreach (dynamic frame in jsonObject.data.frames)
                        {
                            data.Add(Frame.Deserialize(frame));
                        }
                        break;
                    }
                    case "Logo":
                    {
                        foreach (dynamic logo in jsonObject.data.regions)
                        {
                            data.Add(Logo.Deserialize(logo));
                        }
                        break;
                    }
                    default:
                    {
                        throw new ClarifaiException(
                            string.Format("Unknown output type `{0}`", typeName));
                    }
                }
            }
            return data;
        }

        protected static List<IPrediction> GrpcDeserializePredictions(ModelType modelType,
            Output output)
        {
            var data = new List<IPrediction>();
            string typeName = modelType.Prediction.Name;
            switch (typeName)
            {
                case "Color":
                {
                    foreach (Internal.GRPC.Color color in output.Data.Colors)
                    {
                        data.Add(Color.GrpcDeserialize(color));
                    }
                    break;
                }
                case "Concept":
                {
                    foreach (Internal.GRPC.Concept concept in output.Data.Concepts)
                    {
                        data.Add(Concept.GrpcDeserialize(concept));
                    }
                    break;
                }
                case "Demographics":
                {
                    foreach (Region demographics in output.Data.Regions)
                    {
                        data.Add(Demographics.GrpcDeserialize(demographics));
                    }
                    break;
                }
                case "Embedding":
                {
                    foreach (Internal.GRPC.Embedding embedding in output.Data.Embeddings)
                    {
                        data.Add(Embedding.GrpcDeserialize(embedding));
                    }
                    break;
                }
                case "FaceConcepts":
                {
                    foreach (Region faceConcepts in output.Data.Regions)
                    {
                        data.Add(FaceConcepts.GrpcDeserialize(faceConcepts));
                    }
                    break;
                }
                case "FaceDetection":
                {
                    foreach (Region faceDetection in output.Data.Regions)
                    {
                        data.Add(FaceDetection.GrpcDeserialize(faceDetection));
                    }
                    break;
                }
                case "FaceEmbedding":
                {
                    foreach (Region faceEmbedding in output.Data.Regions)
                    {
                        data.Add(FaceEmbedding.GrpcDeserialize(faceEmbedding));
                    }
                    break;
                }
                case "Focus":
                {
                    foreach (Region focus in output.Data.Regions)
                    {
                        data.Add(Focus.GrpcDeserialize(focus, (decimal) output.Data.Focus.Value));
                    }
                    break;
                }
                case "Frame":
                {
                    foreach (Internal.GRPC.Frame frame in output.Data.Frames)
                    {
                        data.Add(Frame.GrpcDeserialize(frame));
                    }
                    break;
                }
                case "Logo":
                {
                    foreach (Region logo in output.Data.Regions)
                    {
                        data.Add(Logo.GrpcDeserialize(logo));
                    }
                    break;
                }
                default:
                {
                    throw new ClarifaiException(
                        string.Format("Unknown output type `{0}`", typeName));
                }
            }
            return data;
        }

        public override bool Equals(object obj)
        {
            return obj is ClarifaiOutput output &&
                   ID == output.ID &&
                   EqualityComparer<ClarifaiStatus>.Default.Equals(Status, output.Status) &&
                   CreatedAt == output.CreatedAt &&
                   EqualityComparer<IClarifaiInput>.Default.Equals(Input, output.Input) &&
                   EqualityComparer<List<IPrediction>>.Default.Equals(Data, output.Data);
        }

        public override int GetHashCode()
        {
            var hashCode = 218169885;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ID);
            hashCode = hashCode * -1521134295 +
                       EqualityComparer<ClarifaiStatus>.Default.GetHashCode(Status);
            hashCode = hashCode * -1521134295 + CreatedAt.GetHashCode();
            hashCode = hashCode * -1521134295 +
                       EqualityComparer<IClarifaiInput>.Default.GetHashCode(Input);
            hashCode = hashCode * -1521134295 +
                       EqualityComparer<List<IPrediction>>.Default.GetHashCode(Data);
            return hashCode;
        }

        public override string ToString()
        {
            return $"[ClarifaiOutput: (ID: {ID})]";
        }
    }


    /// <inheritdoc />
    /// <typeparam name="T">the data type</typeparam>
    public class ClarifaiOutput<T> : ClarifaiOutput where T : IPrediction
    {
        public new List<T> Data => base.Data.Cast<T>().ToList();

        /// <inheritdoc />
        private ClarifaiOutput(string id, ClarifaiStatus status, DateTime createdAt,
            IClarifaiInput input, List<IPrediction> rawData, IModel model)
            : base(id, status, createdAt, input, rawData, model)
        { }

        /// <summary>
        /// Deserializes the object out of a JSON dynamic object.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="jsonObject">the JSON dynamic object of an output</param>
        /// <returns>the deserialized object</returns>
        public static ClarifaiOutput<T> Deserialize(IClarifaiHttpClient httpClient,
            dynamic jsonObject)
        {
            Type type = typeof(T);
            ModelType modelType = ModelType.ConstructFromName(type.Name);

            dynamic data = DeserializePredictions(modelType, jsonObject);
            return new ClarifaiOutput<T>(
                (string)jsonObject.id,
                ClarifaiStatus.Deserialize(jsonObject.status),
                (DateTime) jsonObject.created_at,
                jsonObject.input != null ? ClarifaiInput.Deserialize(jsonObject.input) : null,
                data,
                Models.Model.Deserialize(httpClient, modelType.Prediction, jsonObject.model));
        }

        public static ClarifaiOutput<T> GrpcDeserialize(IClarifaiHttpClient httpClient,
            Output output)
        {
            Type type = typeof(T);
            ModelType modelType = ModelType.ConstructFromName(type.Name);

            List<IPrediction> data = GrpcDeserializePredictions(modelType, output);
            return new ClarifaiOutput<T>(
                output.Id,
                ClarifaiStatus.GrpcDeserialize(output.Status),
                output.CreatedAt.ToDateTime(),
                output.Input != null ? ClarifaiInput.GrpcDeserialize(output.Input) : null,
                data,
                Models.Model.GrpcDeserialize(httpClient, modelType.Prediction, output.Model));
        }
    }
}
