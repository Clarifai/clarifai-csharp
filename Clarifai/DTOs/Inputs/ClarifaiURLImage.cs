using System;
using System.Collections.Generic;
using Clarifai.API.Requests;
using Clarifai.Internal.GRPC;
using Newtonsoft.Json.Linq;
using Concept = Clarifai.DTOs.Predictions.Concept;
using Region = Clarifai.DTOs.Predictions.Region;

namespace Clarifai.DTOs.Inputs
{
    /// <summary>
    /// An image at a certain URL.
    /// </summary>
    public class ClarifaiURLImage : ClarifaiInput
    {
        /// <summary>
        /// The URL where image is located.
        /// </summary>
        public string URL { get; }

        public bool? AllowDuplicateUrl { get; }

        /// <summary>
        /// The bounding box.
        /// </summary>
        public Crop Crop { get; }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="url">the image URL</param>
        /// <param name="id">the ID</param>
        /// <param name="allowDuplicateUrl">should allow duplicate URLs</param>
        /// <param name="positiveConcepts">the concepts associated with the image</param>
        /// <param name="negativeConcepts">the concepts not associated with the image</param>
        /// <param name="metadata">the image's optional metadata by which you can search</param>
        /// <param name="createdAt">the date & time of image's creation</param>
        /// <param name="geo">input's geographical point</param>
        /// <param name="crop">the crop</param>
        /// <param name="regions">the regions</param>
        public ClarifaiURLImage(string url, string id = null, bool? allowDuplicateUrl = null,
            IEnumerable<Concept> positiveConcepts = null,
            IEnumerable<Concept> negativeConcepts = null, JObject metadata = null,
            DateTime? createdAt = null, GeoPoint geo = null, Crop crop = null,
            List<Region> regions = null)
            : base(InputType.Image, InputForm.URL, id, positiveConcepts, negativeConcepts,
                metadata, createdAt, geo, regions)
        {
            URL = url;
            AllowDuplicateUrl = allowDuplicateUrl;
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
                new JProperty("url", URL));
            if (Crop != null)
            {
                image.Add("crop", Crop.SerializeAsArray());
            }
            if (AllowDuplicateUrl != null)
            {
                image.Add("allow_duplicate_url", AllowDuplicateUrl);
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
                Url = URL,
            };
            if (Crop != null)
            {
                image = new Image(image)
                {
                    Crop = {Crop.GrpcSerializeAsArray()}
                };
            }

            if (AllowDuplicateUrl != null)
            {
                image = new Image(image)
                {
                    AllowDuplicateUrl = AllowDuplicateUrl.Value
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
        public new static ClarifaiURLImage Deserialize(dynamic jsonObject)
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
            return new ClarifaiURLImage(
                id: (string) jsonObject.id,
                url: (string) jsonObject.data.image.url,
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
        public new static ClarifaiURLImage GrpcDeserialize(Input input)
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
            return new ClarifaiURLImage(
                id: input.Id,
                url: input.Data.Image.Url,
                positiveConcepts: positiveConcepts,
                negativeConcepts: negativeConcepts,
                crop: crop,
                metadata: metadata,
                createdAt: createdAt,
                geo: geoPoint,
                regions: regions);
        }

        public override bool Equals(object obj)
        {
            return obj is ClarifaiURLImage image &&
                   base.Equals(obj) &&
                   URL == image.URL &&
                   EqualityComparer<bool?>.Default.Equals(AllowDuplicateUrl,
                       image.AllowDuplicateUrl);
        }

        public override int GetHashCode()
        {
            var hashCode = -1679662782;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(URL);
            hashCode = hashCode * -1521134295 +
                       EqualityComparer<bool?>.Default.GetHashCode(AllowDuplicateUrl);
            return hashCode;
        }

        public override string ToString()
        {
            return string.Format($"[ClarifaiURLImage: (id: {ID}, url: {URL})]");
        }
    }
}
