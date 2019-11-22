using System;
using System.Collections.Generic;
using Clarifai.DTOs.Predictions;
using Newtonsoft.Json.Linq;

namespace Clarifai.DTOs.Inputs
{
    /// <summary>
    /// A file image.
    /// </summary>
    public class ClarifaiFileImage : ClarifaiInput
    {
        private readonly byte[] _bytes;

        /// <summary>
        /// A copy of the original image bytes.
        /// </summary>
        public byte[] Bytes => (byte[]) _bytes.Clone();

        /// <summary>
        /// The bounding box.
        /// </summary>
        public Crop Crop { get; }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="bytes">the image bytes</param>
        /// <param name="id">the ID</param>
        /// <param name="positiveConcepts">the concepts associated with the image</param>
        /// <param name="negativeConcepts">the concepts not associated with the image</param>
        /// <param name="metadata">the video's optional metadata by which you can search</param>
        /// <param name="createdAt">the date & time of video's creation</param>
        /// <param name="geo">input's geographical point</param>
        /// <param name="crop">the crop</param>
        /// <param name="regions">the regions</param>
        /// <param name="status">the status</param>
        public ClarifaiFileImage(byte[] bytes, string id = null,
            IEnumerable<Concept> positiveConcepts = null,
            IEnumerable<Concept> negativeConcepts = null, JObject metadata = null,
            DateTime? createdAt = null, GeoPoint geo = null, Crop crop = null,
            List<Region> regions = null, ClarifaiStatus status = null)
            : base(InputType.Image, InputForm.File, id, positiveConcepts, negativeConcepts,
                  metadata, createdAt, geo, regions, status)
        {
            _bytes = bytes;
            Crop = crop;
        }

        /// <summary>
        /// Serializes this object into a new JSON object.
        /// </summary>
        /// <returns>a new JSON object</returns>
        public override JObject Serialize()
        {
            var image = new JObject(
                new JProperty("base64", Convert.ToBase64String(_bytes)));
            if (Crop != null)
            {
                image.Add("crop", Crop.SerializeAsArray());
            }
            return Serialize(
                new JProperty("image", image));
        }

        /// <summary>
        /// Deserializes the object out of a JSON dynamic object.
        /// </summary>
        /// <param name="jsonObject">the JSON dynamic object</param>
        /// <returns>the deserialized object</returns>
        public new static ClarifaiFileImage Deserialize(dynamic jsonObject)
        {
            var positiveConcepts = new List<Concept>();
            var negativeConcepts = new List<Concept>();
            if (jsonObject.data.concepts != null)
            {
                foreach (dynamic c in jsonObject.data.concepts)
                {
                    var concept = Concept.Deserialize(c);
                    if (concept.Value == 0.0M)
                    {
                        negativeConcepts.Add(concept);
                    }
                    else
                    {
                        positiveConcepts.Add(concept);
                    }
                }
            }
            Crop crop = null;
            if (jsonObject.data.image.crop != null)
            {
                crop = DTOs.Crop.Deserialize(jsonObject.data.image.crop);
            }
            JObject metadata = null;
            if (jsonObject.data.metadata != null)
            {
                metadata = (JObject) jsonObject.data.metadata;
            }
            GeoPoint geoPoint = null;
            if (jsonObject.data.geo != null)
            {
                geoPoint = GeoPoint.Deserialize(jsonObject.data.geo);
            }
            DateTime? createdAt = null;
            if (jsonObject.created_at != null)
            {
                createdAt = (DateTime)jsonObject.created_at;
            }

            var regions = new List<Region>();
            if (jsonObject.data != null && jsonObject.data.regions != null)
            {
                foreach (dynamic region in jsonObject.data.regions)
                {
                    regions.Add(Region.Deserialize(region));
                }
            }

            ClarifaiStatus status = null;
            if (jsonObject.status != null)
            {
                status = ClarifaiStatus.Deserialize(jsonObject.status);
            }

            return new ClarifaiFileImage(
                bytes: Convert.FromBase64String((string) jsonObject.data.image.base64),
                id: (string) jsonObject.id,
                positiveConcepts: positiveConcepts,
                negativeConcepts: negativeConcepts,
                crop: crop,
                metadata: metadata,
                createdAt: createdAt,
                geo: geoPoint,
                regions: regions,
                status: status);
        }

        private bool Equals(ClarifaiFileImage other)
        {
            return base.Equals(other) && Equals(_bytes, other._bytes) && Equals(Crop, other.Crop);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ClarifaiFileImage) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ (_bytes != null ? _bytes.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Crop != null ? Crop.GetHashCode() : 0);
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"[ClarifaiFileImage: (id: {ID})]";
        }
    }
}
