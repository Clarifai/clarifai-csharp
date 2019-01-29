using System;
using System.Collections.Generic;
using Clarifai.API.Requests;
using Clarifai.Internal.GRPC;
using Google.Protobuf;
using Newtonsoft.Json.Linq;
using Concept = Clarifai.DTOs.Predictions.Concept;
using Region = Clarifai.DTOs.Predictions.Region;

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
        public ClarifaiFileImage(byte[] bytes, string id = null,
            IEnumerable<Concept> positiveConcepts = null,
            IEnumerable<Concept> negativeConcepts = null, JObject metadata = null,
            DateTime? createdAt = null, GeoPoint geo = null, Crop crop = null,
            List<Region> regions = null)
            : base(InputType.Image, InputForm.File, id, positiveConcepts, negativeConcepts,
                  metadata, createdAt, geo, regions)
        {
            _bytes = bytes;
            Crop = crop;
        }

        /// <summary>
        /// Serializes this object into a new JSON object.
        /// </summary>
        /// <returns>a new JSON object</returns>
        [Obsolete]
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
        /// Serializes this object into a new GRPC object.
        /// </summary>
        /// <returns>a new JSON object</returns>
        public override Input GrpcSerialize()
        {
            var image = new Image
            {
                Base64 = ByteString.CopyFrom(_bytes),
            };
            if (Crop != null)
            {
                image = new Image(image)
                {
                    Crop = {Crop.GrpcSerializeAsArray()},
                };
            }
            return GrpcSerialize("image", image);
        }

        /// <summary>
        /// Deserializes the object out of a JSON dynamic object.
        /// </summary>
        /// <param name="jsonObject">the JSON dynamic object</param>
        /// <returns>the deserialized object</returns>
        [Obsolete]
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
            return new ClarifaiFileImage(
                bytes: Convert.FromBase64String((string) jsonObject.data.image.base64),
                id: (string) jsonObject.id,
                positiveConcepts: positiveConcepts,
                negativeConcepts: negativeConcepts,
                crop: crop,
                metadata: metadata,
                createdAt: createdAt,
                geo: geoPoint,
                regions: regions);
        }

        /// <summary>
        /// Deserializes the object out of a gRPC object.
        /// </summary>
        /// <param name="input">the input gRPC object</param>
        /// <returns>the deserialized object</returns>
        public new static ClarifaiFileImage GrpcDeserialize(Input input)
        {
            var positiveConcepts = new List<Concept>();
            var negativeConcepts = new List<Concept>();
            foreach (Internal.GRPC.Concept c in input.Data.Concepts)
            {
                Concept concept = Concept.GrpcDeserialize(c);
                if (concept.Value == 0.0M)
                {
                    negativeConcepts.Add(concept);
                }
                else
                {
                    positiveConcepts.Add(concept);
                }
            }
            Crop crop = null;
            if (input.Data.Image.Crop?.Count > 0)
            {
                crop = Crop.GrpcDeserialize(input.Data.Image.Crop);
            }
            JObject metadata = null;
            if (input.Data.Metadata != null)
            {
                metadata = StructHelper.StructToJObject(input.Data.Metadata);
            }
            GeoPoint geoPoint = null;
            if (input.Data.Geo != null)
            {
                geoPoint = GeoPoint.GrpcDeserialize(input.Data.Geo);
            }
            DateTime? createdAt = null;
            if (input.CreatedAt != null)
            {
                createdAt = input.CreatedAt.ToDateTime();
            }

            var regions = new List<Region>();
            if (input.Data?.Regions != null)
            {
                foreach (Internal.GRPC.Region region in input.Data.Regions)
                {
                    regions.Add(Region.GrpcDeserialize(region));
                }
            }
            return new ClarifaiFileImage(
                bytes: input.Data.Image.ToByteArray(),
                id: input.Id,
                positiveConcepts: positiveConcepts,
                negativeConcepts: negativeConcepts,
                crop: crop,
                metadata: metadata,
                createdAt: createdAt,
                geo: geoPoint,
                regions: regions);
        }

        private bool Equals(ClarifaiFileImage other)
        {
            return base.Equals(other) && Equals(_bytes, other._bytes) && Equals(Crop, other.Crop);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
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
