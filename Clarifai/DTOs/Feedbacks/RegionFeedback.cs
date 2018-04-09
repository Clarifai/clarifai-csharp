using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Newtonsoft.Json.Linq;

namespace Clarifai.DTOs.Feedbacks
{
    /// <summary>
    /// Region feedback.
    /// </summary>
    public class RegionFeedback
    {
        private readonly Crop _crop;
        private readonly Feedback _feedback;
        private readonly IEnumerable<ConceptFeedback> _concepts;
        private readonly string _regionID;
        private readonly FaceFeedback _faceFeedback;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// true if region is present in the input, false if not present
        public RegionFeedback(Crop crop = null, Feedback feedback = null,
            IEnumerable<ConceptFeedback> concepts = null, string regionID = null,
            FaceFeedback faceFeedback = null)
        {
            _crop = crop;
            _feedback = feedback;
            _concepts = concepts;
            _regionID = regionID;
            _faceFeedback = faceFeedback;
        }

        /// <summary>
        /// Serializes the object to a JSON object.
        /// </summary>
        /// <returns>the JSON object</returns>
        public JObject Serialize()
        {
            var data = new JObject();
            if (_concepts != null && _concepts.Any())
            {
                data["concepts"] = new JArray(_concepts.Select(c => c.Serialize()));
            }

            if (_faceFeedback != null)
            {
                data["face"] = _faceFeedback.Serialize();
            }

            var body = new JObject(
                new JProperty("region_info", new JObject(
                    new JProperty("bounding_box", _crop.SerializeAsObject()),
                    new JProperty("feedback", _feedback.Value))),
                new JProperty("data", data));
            if (_regionID != null)
            {
                body["id"] = _regionID;
            }
            return body;
        }

        public override bool Equals(object obj)
        {
            return obj is RegionFeedback feedback &&
                   EqualityComparer<Crop>.Default.Equals(_crop, feedback._crop) &&
                   EqualityComparer<Feedback>.Default.Equals(_feedback, feedback._feedback) &&
                   EqualityComparer<IEnumerable<ConceptFeedback>>.Default.Equals(_concepts,
                       feedback._concepts);
        }

        public override int GetHashCode()
        {
            var hashCode = 647450140;
            hashCode = hashCode * -1521134295 + EqualityComparer<Crop>.Default.GetHashCode(_crop);
            hashCode = hashCode * -1521134295 +
                       EqualityComparer<Feedback>.Default.GetHashCode(_feedback);
            hashCode = hashCode * -1521134295 +
                       EqualityComparer<IEnumerable<ConceptFeedback>>.Default
                           .GetHashCode(_concepts);
            return hashCode;
        }

        public override string ToString()
        {
            return $"[RegionFeedback: (crop: {_crop}, feedback: {_feedback}, concepts: " +
                $"{_concepts})]";
        }
    }
}
