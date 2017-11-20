using System;
using System.Collections.Generic;
using System.Linq;
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
        /// Ctor.
        /// </summary>
        /// <param name="id">the output ID</param>
        /// <param name="status">the output status</param>
        /// <param name="createdAt">date & time of output creation</param>
        /// <param name="input">the input</param>
        /// <param name="data">the data</param>
        protected ClarifaiOutput(string id, ClarifaiStatus status, DateTime createdAt,
            IClarifaiInput input, List<IPrediction> data)
        {
            ID = id;
            Status = status;
            CreatedAt = createdAt;
            Input = input;
            Data = data;
        }

        /// <summary>
        /// Deserializes the object out of a JSON dynamic object.
        /// </summary>
        /// <param name="typeName">name of the type</param>
        /// <param name="jsonObject">the JSON object</param>
        /// <returns>the deserialized object</returns>
        public static ClarifaiOutput Deserialize(string typeName, dynamic jsonObject)
        {
            dynamic data = DeserializePredictions(typeName, jsonObject);

            return new ClarifaiOutput(
                (string)jsonObject.id,
                ClarifaiStatus.Deserialize(jsonObject.status),
                (DateTime) jsonObject.created_at,
                jsonObject.input != null ? ClarifaiInput.Deserialize(jsonObject.input) : null,
                data);
        }

        protected static List<IPrediction> DeserializePredictions(string typeName,
            dynamic jsonObject)
        {
            var propertyValues = (JObject) jsonObject.data;

            var data = new List<IPrediction>();
            if (propertyValues.Count > 0)
            {
                string name = propertyValues.Properties().First().Name;
                switch (typeName)
                {
                    case "Color":
                    {
                        foreach (var color in jsonObject.data[name])
                        {
                            data.Add(Color.Deserialize(color));
                        }
                        break;
                    }
                    case "Concept":
                    {
                        foreach (var concept in jsonObject.data[name])
                        {
                            data.Add(Concept.Deserialize(concept));
                        }
                        break;
                    }
                    case "Demographics":
                    {
                        foreach (var demographics in jsonObject.data[name])
                        {
                            data.Add(Demographics.Deserialize(demographics));
                        }
                        break;
                    }
                    case "Embedding":
                    {
                        foreach (var embedding in jsonObject.data[name])
                        {
                            data.Add(Embedding.Deserialize(embedding));
                        }
                        break;
                    }
                    case "FaceConcepts":
                    {
                        foreach (var faceConcepts in jsonObject.data[name])
                        {
                            data.Add(FaceConcepts.Deserialize(faceConcepts));
                        }
                        break;
                    }
                    case "FaceDetection":
                    {
                        foreach (var faceDetection in jsonObject.data[name])
                        {
                            data.Add(FaceDetection.Deserialize(faceDetection));
                        }
                        break;
                    }
                    case "FaceEmbedding":
                    {
                        foreach (var faceEmbedding in jsonObject.data[name])
                        {
                            data.Add(FaceEmbedding.Deserialize(faceEmbedding));
                        }
                        break;
                    }
                    case "Focus":
                    {
                        foreach (var focus in jsonObject.data.regions)
                        {
                            data.Add(Focus.Deserialize(focus,
                                (decimal) jsonObject.data.focus.value));
                        }
                        break;
                    }
                    case "Frame":
                    {
                        foreach (var frame in jsonObject.data[name])
                        {
                            data.Add(Frame.Deserialize(frame));
                        }
                        break;
                    }
                    case "Logo":
                    {
                        foreach (var logo in jsonObject.data[name])
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
            IClarifaiInput input, List<IPrediction> rawData)
            : base(id, status, createdAt, input, rawData)
        { }

        /// <summary>
        /// Deserializes the object out of a JSON dynamic object.
        /// </summary>
        /// <param name="jsonObject">the JSON dynamic object of an output</param>
        /// <returns>the deserialized object</returns>
        public static ClarifaiOutput<T> Deserialize(dynamic jsonObject)
        {
            Type type = typeof(T);
            dynamic data = DeserializePredictions(type.Name, jsonObject);
            return new ClarifaiOutput<T>(
                (string)jsonObject.id,
                ClarifaiStatus.Deserialize(jsonObject.status),
                (DateTime) jsonObject.created_at,
                jsonObject.input != null ? ClarifaiInput.Deserialize(jsonObject.input) : null,
                data);
        }
    }
}
