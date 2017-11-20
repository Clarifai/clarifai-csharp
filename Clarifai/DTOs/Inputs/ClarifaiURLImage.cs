using System;
using System.Collections.Generic;
using Clarifai.DTOs.Predictions;
using Newtonsoft.Json.Linq;

namespace Clarifai.DTOs.Inputs
{
    /// <summary>
    /// An image at a certain URL.
    /// </summary>
    public class ClarifaiURLImage : ClarifaiInput
    {
        public string URL { get; }
        public bool? AllowDuplicateUrl { get; }

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
        public ClarifaiURLImage(string url, string id = null, bool? allowDuplicateUrl = null,
            IEnumerable<Concept> positiveConcepts = null,
            IEnumerable<Concept> negativeConcepts = null, JObject metadata = null,
            DateTime? createdAt = null, GeoPoint geo = null)
            : base(InputType.Image, InputForm.URL, id, positiveConcepts, negativeConcepts, metadata,
                  createdAt, geo)
        {
            URL = url;
            AllowDuplicateUrl = allowDuplicateUrl;
        }

        /// <summary>
        /// Serializes this object into a new JSON object.
        /// </summary>
        /// <returns>a new JSON object</returns>
        public override JObject Serialize()
        {
            return Serialize(
                new JProperty("image", new JObject(
                    new JProperty("url", URL),
                    new JProperty("allow_duplicate_url", AllowDuplicateUrl))));
        }

        /// <summary>
        /// Deserializes the object out of a JSON dynamic object.
        /// </summary>
        /// <param name="jsonObject">the JSON dynamic object</param>
        /// <returns>the deserialized object</returns>
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
            return new ClarifaiURLImage(
                id: (string) jsonObject.id,
                url: (string) jsonObject.data.image.url,
                positiveConcepts: positiveConcepts,
                negativeConcepts: negativeConcepts,
                metadata: metadata,
                createdAt: createdAt,
                geo: geoPoint);
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
