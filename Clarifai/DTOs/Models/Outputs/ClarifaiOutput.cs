﻿using System;
using System.Collections.Generic;
using System.Linq;
using Clarifai.API;
using Clarifai.DTOs.Inputs;
using Clarifai.DTOs.Predictions;
using Clarifai.Exceptions;
using Newtonsoft.Json.Linq;

namespace Clarifai.DTOs.Models.Outputs
{
    /// <summary>
    /// Output returned after running a request. Encapsulates the response content and other
    /// data the response returnes.
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
                    case "Embedding":
                    {
                        foreach (dynamic embedding in jsonObject.data.embeddings)
                        {
                            data.Add(Embedding.Deserialize(embedding));
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
                    case "Frame":
                    {
                        foreach (dynamic frame in jsonObject.data.frames)
                        {
                            data.Add(Frame.Deserialize(frame));
                        }
                        break;
                    }
                    case "DetectConcept":
                    {
                        foreach (dynamic detectConcept in jsonObject.data.regions)
                        {
                            data.Add(Detection.Deserialize(detectConcept));
                        }
                        break;
                    }
                    case "Detection":
                    {
                        foreach (dynamic detection in jsonObject.data.regions)
                        {
                            data.Add(Detection.Deserialize(detection));
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
    }
}
