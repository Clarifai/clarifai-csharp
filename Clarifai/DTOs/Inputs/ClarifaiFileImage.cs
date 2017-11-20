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
        /// Ctor.
        /// </summary>
        /// <param name="bytes">the image bytes</param>
        /// <param name="id">the ID</param>
        /// <param name="positiveConcepts">the concepts associated with the image</param>
        /// <param name="negativeConcepts">the concepts not associated with the image</param>
        /// <param name="metadata">the video's optional metadata by which you can search</param>
        /// <param name="createdAt">the date & time of video's creation</param>
        /// <param name="geo">input's geographical point</param>
        public ClarifaiFileImage(byte[] bytes, string id = null,
            IEnumerable<Concept> positiveConcepts = null,
            IEnumerable<Concept> negativeConcepts = null, JObject metadata = null,
            DateTime? createdAt = null, GeoPoint geo = null)
            : base(InputType.Image, InputForm.File, id, positiveConcepts, negativeConcepts,
                  metadata, createdAt, geo)
        {
            _bytes = bytes;
        }

        /// <summary>
        /// Serializes this object into a new JSON object.
        /// </summary>
        /// <returns>a new JSON object</returns>
        public override JObject Serialize()
        {
            return Serialize(
                new JProperty("image", new JObject(
                    new JProperty("base64", Convert.ToBase64String(_bytes)))));
        }

        public override bool Equals(object obj)
        {
            return obj is ClarifaiFileImage image &&
                   base.Equals(obj) &&
                   EqualityComparer<byte[]>.Default.Equals(_bytes, image._bytes);
        }

        public override int GetHashCode()
        {
            var hashCode = 162908149;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 +
                EqualityComparer<byte[]>.Default.GetHashCode(_bytes);
            return hashCode;
        }

        public override string ToString()
        {
            return $"[ClarifaiFileImage: (id: {ID})]";
        }
    }
}
