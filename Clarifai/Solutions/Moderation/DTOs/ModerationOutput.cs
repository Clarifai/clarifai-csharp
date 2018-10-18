using System;
using System.Collections.Generic;
using System.Linq;
using Clarifai.DTOs;
using Clarifai.DTOs.Inputs;
using Clarifai.DTOs.Models;
using Clarifai.DTOs.Predictions;
using Clarifai.Exceptions;
using Newtonsoft.Json.Linq;

namespace Clarifai.Solutions.Moderation.DTOs
{
    public class ModerationOutput
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
        public List<Concept> Data { get; }

        /// <summary>
        /// The moderation status.
        /// </summary>
        public ModerationStatus ModerationStatus { get;  }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="id">the output ID</param>
        /// <param name="status">the output status</param>
        /// <param name="createdAt">date & time of output creation</param>
        /// <param name="input">the input</param>
        /// <param name="data">the data</param>
        protected ModerationOutput(string id, ClarifaiStatus status, DateTime createdAt,
            IClarifaiInput input, List<Concept> data, ModerationStatus moderationStatus)
        {
            ID = id;
            Status = status;
            CreatedAt = createdAt;
            Input = input;
            Data = data;
            ModerationStatus = moderationStatus;
        }

        /// <summary>
        /// Deserializes the object out of a JSON dynamic object.
        /// </summary>
        /// <param name="modelType">the model type</param>
        /// <param name="jsonObject">the JSON object</param>
        /// <returns>the deserialized object</returns>
        public static ModerationOutput Deserialize(dynamic jsonObject)
        {
            List<Concept> data = DeserializePredictions(jsonObject);

            return new ModerationOutput(
                (string)jsonObject.id,
                ClarifaiStatus.Deserialize(jsonObject.status),
                (DateTime) jsonObject.created_at,
                jsonObject.input != null ? ClarifaiInput.Deserialize(jsonObject.input) : null,
                data,
                Clarifai.Solutions.Moderation.DTOs.ModerationStatus.Deserialize(
                    jsonObject.moderation.status
                    ));
        }

        protected static List<Concept> DeserializePredictions(dynamic jsonObject)
        {
            var propertyValues = (JObject) jsonObject.data;

            var data = new List<Concept>();
            if (propertyValues.Count > 0)
            {
                foreach (dynamic concept in jsonObject.data.concepts)
                {
                    data.Add(Concept.Deserialize(concept));
                }
            }
            return data;
        }

        public override string ToString()
        {
            return $"[ModerationOutput: (ID: {ID})]";
        }
    }
}
