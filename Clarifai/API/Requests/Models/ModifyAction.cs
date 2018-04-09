using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Clarifai.API.Requests.Models
{
    /// <summary>
    /// A type of a modification.
    /// </summary>
    public class ModifyAction
    {
        /// <summary>
        /// The modification value.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="value">the modification value</param>
        private ModifyAction(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Merge will overwrite overwrite a key:value or append to an existing list of values.
        /// </summary>
        public static ModifyAction Merge => new ModifyAction("merge");

        /// <summary>
        /// Overwrite will overwrite a key:value or overwrite a list of values.
        /// </summary>
        public static ModifyAction Overwrite => new ModifyAction("overwrite");

        /// <summary>
        /// Remove will overwrite a key:value or delete anything in a list that matches the
        /// provided values' ids.
        /// </summary>
        public static ModifyAction Remove => new ModifyAction("remove");

        public JToken Serialize()
        {
            return Value;
        }

        public override bool Equals(object obj)
        {
            return obj is ModifyAction action &&
                   Value == action.Value;
        }

        public override int GetHashCode()
        {
            return -1937169414 + EqualityComparer<string>.Default.GetHashCode(Value);
        }
    }
}
