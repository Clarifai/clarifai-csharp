using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Clarifai.DTOs.Feedbacks
{
    /// <summary>
    /// Concept feedback.
    /// </summary>
    public class ConceptFeedback
    {
        private readonly string _conceptID;
        private readonly bool _value;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="conceptID">the concept ID</param>
        /// <param name="value">
        /// true if concept is present in the input, false if not present
        /// </param>
        public ConceptFeedback(string conceptID, bool value)
        {
            _conceptID = conceptID;
            _value = value;
        }

        /// <summary>
        /// Serializes the object to a JSON object.
        /// </summary>
        /// <returns>the JSON object</returns>
        public JObject Serialize()
        {
            return new JObject(
                new JProperty("id", _conceptID),
                new JProperty("value", _value));
        }

        public override bool Equals(object obj)
        {
            return obj is ConceptFeedback feedback &&
                   _conceptID == feedback._conceptID &&
                   _value == feedback._value;
        }

        public override int GetHashCode()
        {
            var hashCode = -503665294;
            hashCode = hashCode * -1521134295 +
                EqualityComparer<string>.Default.GetHashCode(_conceptID);
            hashCode = hashCode * -1521134295 + _value.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return $"[ConceptFeedback: (conceptID: {_conceptID}, value: {_value})]";
        }
    }
}
