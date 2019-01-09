using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Clarifai.DTOs.Predictions
{
    /// <summary>
    /// Represents a string associated with an input (image or video). Also called
    /// label or tag.
    /// </summary>
    public class Concept : IPrediction
    {
        /// <summary>
        /// IPrediction type.
        /// </summary>
        public string TYPE => "concept";


        /// <summary>
        /// The concept ID.
        /// </summary>
        public string ID { get; }

        /// <summary>
        /// The concept name;
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Date & time of concept creation.
        /// </summary>
        public DateTime? CreatedAt { get; private set; }

        /// <summary>
        /// The application ID.
        /// </summary>
        public string AppID { get; private set; }

        /// <summary>
        /// The likelihood this concept is associated with a certain input.
        /// Only used together with an input, a concept value by itself has no meaning.
        /// </summary>
        public decimal? Value { get; private set; }

        /// <summary>
        /// The language of the Name field.
        /// </summary>
        public string Language { get; }


        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="id">the concept ID</param>
        /// <param name="name">the concept name</param>
        /// <param name="language">the language of the name</param>
        public Concept(string id, string name = null, string language = null)
        {
            ID = id;
            Name = name;
            Language = language;
        }

        /// <summary>
        /// Serializes this object into a new JSON object.
        /// </summary>
        /// <param name="value">true if concept is present in an input, false otherwise</param>
        /// <returns>a new JSON object</returns>
        public JObject Serialize(bool? value = null)
        {
            var body = new JObject(new JProperty("id", ID));
            if (Name != null)
            {
                body.Add(new JProperty("name", Name));
            }
            if (Language != null)
            {
                body.Add(new JProperty("language", Language));
            }
            if (value != null)
            {
                body.Add(new JProperty("value", value));
            }
            return body;
        }

        /// <summary>
        /// Deserializes this object from dynamic JSON object.
        /// </summary>
        /// <param name="jsonObject">the JSON object</param>
        /// <returns>a new instance of this class</returns>
        public static Concept Deserialize(dynamic jsonObject)
        {
            return new Concept(
                (string) jsonObject.id,
                jsonObject.name != null ? (string) jsonObject.name : null,
                jsonObject.language != null ? (string) jsonObject.language : null)
            {
                CreatedAt = (DateTime?) jsonObject.created_at,
                AppID = (string) jsonObject.app_id,
                Value = (decimal?) jsonObject.value,
            };
        }

        public Internal.GRPC.Concept GrpcSerialize(bool? value = null)
        {
            var concept = new Internal.GRPC.Concept
            {
                Id = ID
            };
            if (Name != null)
            {
                concept.Name = Name;
            }
            if (Language != null)
            {
                concept.Language = Language;
            }
            if (value != null)
            {
                concept.Value = value.Value ? 1 : 0;
            }
            else
            {
                concept.Value = 1;
            }
            return concept;
        }

        public static Concept GrpcDeserialize(Internal.GRPC.Concept grpcConcept)
        {
            return new Concept(
                grpcConcept.Id,
                grpcConcept.Name,
                grpcConcept.Language)
            {
                CreatedAt = grpcConcept.CreatedAt?.ToDateTime(),
                AppID = grpcConcept.AppId,
                Value = (decimal) grpcConcept.Value,
            };
        }

        public override string ToString()
        {
            return $"{TYPE}: ({ID}, {Name}, {Value}, {CreatedAt}, {AppID}, {Language})";
        }

        public override bool Equals(object obj)
        {
            return obj is Concept concept &&
                   ID == concept.ID &&
                   Name == concept.Name &&
                   EqualityComparer<DateTime?>.Default.Equals(CreatedAt, concept.CreatedAt) &&
                   AppID == concept.AppID &&
                   EqualityComparer<decimal?>.Default.Equals(Value, concept.Value) &&
                   Language == concept.Language;
        }

        public override int GetHashCode()
        {
            var hashCode = 1778342730;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ID);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 +
                       EqualityComparer<DateTime?>.Default.GetHashCode(CreatedAt);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(AppID);
            hashCode = hashCode * -1521134295 +
                       EqualityComparer<decimal?>.Default.GetHashCode(Value);
            hashCode = hashCode * -1521134295 +
                       EqualityComparer<string>.Default.GetHashCode(Language);
            return hashCode;
        }
    }
}
