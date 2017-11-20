using System;
using System.Collections.Generic;
using Clarifai.DTOs.Predictions;
using Newtonsoft.Json.Linq;

namespace Clarifai.DTOs.Inputs
{
    /// <summary>
    /// URL video that's going to be input in a model prediction.
    /// </summary>
    public class ClarifaiURLVideo : ClarifaiInput
    {
        public string URL { get; }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="url">the video URL</param>
        /// <param name="id">the ID</param>
        /// <param name="positiveConcepts">the concepts associated with the video</param>
        /// <param name="negativeConcepts">the concepts not associated with the video</param>
        /// <param name="metadata">the video's optional metadata by which you can search</param>
        /// <param name="createdAt">the date & time of video's creation</param>
        /// <param name="geo">input's geographical point</param>
        public ClarifaiURLVideo(string url, string id = null,
            IEnumerable<Concept> positiveConcepts = null,
            IEnumerable<Concept> negativeConcepts = null, JObject metadata = null,
            DateTime? createdAt = null, GeoPoint geo = null)
            : base(InputType.Video, InputForm.URL, id, positiveConcepts, negativeConcepts, metadata,
                  createdAt, geo)

        {
            URL = url;
        }

        /// <summary>
        /// Serializes this object into a new JSON object.
        /// </summary>
        /// <returns>a new JSON object</returns>
        public override JObject Serialize()
        {
            return Serialize(
                new JProperty("video", new JObject(
                    new JProperty("url", URL))));
        }

        /// <summary>
        /// Deserializes the object out of a JSON dynamic object.
        /// </summary>
        /// <param name="jsonObject">the JSON dynamic object</param>
        /// <returns>the deserialized object</returns>
        public new static ClarifaiURLVideo Deserialize(dynamic jsonObject)
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
            return new ClarifaiURLVideo(
                id: (string) jsonObject.id,
                url: (string) jsonObject.data.video.url,
                positiveConcepts: positiveConcepts,
                negativeConcepts: negativeConcepts,
                metadata: metadata,
                createdAt: createdAt,
                geo: geoPoint);
        }

        public override bool Equals(object obj)
        {
            return obj is ClarifaiURLVideo video &&
                   base.Equals(obj) &&
                   URL == video.URL;
        }

        public override int GetHashCode()
        {
            var hashCode = 1969840958;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(URL);
            return hashCode;
        }

        public override string ToString()
        {
            return string.Format($"[{ID}, {URL}]");
        }
    }
}
