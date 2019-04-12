using System;
using System.Collections.Generic;
using System.Linq;
using Clarifai.DTOs.Predictions;
using Clarifai.Exceptions;
using Newtonsoft.Json.Linq;

namespace Clarifai.DTOs.Inputs
{
    /// <inheritdoc />
    public abstract class ClarifaiInput : IClarifaiInput
    {

        /// <inheritdoc />
        public InputType Type { get; }

        /// <inheritdoc />
        public InputForm Form { get; }

        /// <inheritdoc />
        public string ID { get; }

        /// <inheritdoc />
        public IEnumerable<Concept> PositiveConcepts { get; }

        /// <inheritdoc />
        public IEnumerable<Concept> NegativeConcepts { get; }

        /// <inheritdoc />
        public JObject Metadata { get; }

        /// <inheritdoc />
        public DateTime? CreatedAt { get; }

        /// <inheritdoc />
        public GeoPoint Geo { get; }

        /// <inheritdoc />
        public List<Region> Regions { get; }

        public abstract JObject Serialize();

        protected ClarifaiInput(InputType type, InputForm form, string id,
            IEnumerable<Concept> positiveConcepts, IEnumerable<Concept> negativeConcepts,
            JObject metadata, DateTime? createdAt, GeoPoint geo, List<Region> regions)
        {
            Type = type;
            Form = form;
            ID = id;
            PositiveConcepts = positiveConcepts;
            NegativeConcepts = negativeConcepts;
            Metadata = metadata;
            CreatedAt = createdAt;
            Geo = geo;
            Regions = regions;
        }

        protected JObject Serialize(JProperty inputProperty)
        {
            var data = new JObject(inputProperty);

            var concepts = new List<JObject>();
            if (PositiveConcepts != null)
            {
                concepts.AddRange(PositiveConcepts.Select(c => c.Serialize(true)));
            }
            if (NegativeConcepts != null)
            {
                concepts.AddRange(NegativeConcepts.Select(c => c.Serialize(false)));
            }
            if (concepts.Any())
            {
                data["concepts"] = new JArray(concepts);
            }

            if (Metadata != null)
            {
                data["metadata"] = Metadata;
            }
            if (Geo != null)
            {
                data["geo"] = new JObject(new JProperty("geo_point", Geo.Serialize()));
            }
            var obj = new JObject(
                new JProperty("id", ID),
                new JProperty("data", data));
            if (CreatedAt != null)
            {
                obj["created_at"] = CreatedAt.Value.ToString("yyyy-MM-ddTHH:mm:ssZ");
            }
            return obj;
        }

        /// <summary>
        /// Deserializes a JSON object into a concrete ClarifaiInput instance.
        /// It's either ClarifaiURLImage or ClarifaiURLVideo.
        /// </summary>
        /// <param name="jsonObject">the JSON object</param>
        /// <returns>a new ClarifaiInput instance</returns>
        public static ClarifaiInput Deserialize(dynamic jsonObject)
        {
            if (jsonObject.data.image != null)
            {
                if (jsonObject.data.image.base64 != null)
                {
                    return ClarifaiFileImage.Deserialize(jsonObject);
                }
                else
                {
                    return ClarifaiURLImage.Deserialize(jsonObject);
                }
            }
            else if (jsonObject.data.video != null)
            {
                if (jsonObject.data.video.base64 != null)
                {
                    return ClarifaiFileVideo.Deserialize(jsonObject);
                }
                else
                {
                    return ClarifaiURLVideo.Deserialize(jsonObject);
                }
            }
            else
            {
                throw new ClarifaiException(
                    string.Format("Unknown ClarifaiInput json response: {0}", jsonObject));
            }
        }

        public override bool Equals(object obj)
        {
            return obj is ClarifaiInput input &&
                   ID == input.ID &&
                   EqualityComparer<IEnumerable<Concept>>.Default.Equals(PositiveConcepts, input.PositiveConcepts)
                   &&
                   EqualityComparer<JObject>.Default.Equals(Metadata, input.Metadata) &&
                   EqualityComparer<DateTime?>.Default.Equals(CreatedAt, input.CreatedAt) &&
                   EqualityComparer<GeoPoint>.Default.Equals(Geo, input.Geo);
        }

        public override int GetHashCode()
        {
            var hashCode = 833924800;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ID);
            hashCode = hashCode * -1521134295 +
                EqualityComparer<IEnumerable<Concept>>.Default.GetHashCode(PositiveConcepts);
            hashCode = hashCode * -1521134295 +
                EqualityComparer<JObject>.Default.GetHashCode(Metadata);
            hashCode = hashCode * -1521134295 +
                EqualityComparer<DateTime?>.Default.GetHashCode(CreatedAt);
            hashCode = hashCode * -1521134295 + EqualityComparer<GeoPoint>.Default.GetHashCode(Geo);
            return hashCode;
        }

        public override string ToString()
        {
            return string.Format($"[ClarifaiInput: (id: {ID})]");
        }
    }
}
