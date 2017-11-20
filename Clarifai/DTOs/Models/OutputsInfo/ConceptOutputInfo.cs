using System.Collections.Generic;
using Clarifai.DTOs.Predictions;

namespace Clarifai.DTOs.Models.OutputsInfo
{
    /// <summary>
    /// Output info for concept model.
    /// </summary>
    public class ConceptOutputInfo : IOutputInfo
    {
        /// <inheritdoc />
        public string Type { get; }

        /// <inheritdoc />
        public string TypeExt { get; }

        /// <inheritdoc />
        public string Message { get; }

        /// <summary>
        /// The concepts.
        /// </summary>
        public IEnumerable<Concept> Concepts { get; }

        /// <summary>
        /// Are concepts exclusive.
        /// </summary>
        public bool AreConceptsMutuallyExclusive { get; }

        /// <summary>
        /// Is environment closed.
        /// </summary>
        public bool IsEnvironmentClosed { get; }

        /// <summary>
        /// The language.
        /// </summary>
        public string Language { get; }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="type">the type</param>
        /// <param name="typeExt">the type ext</param>
        /// <param name="message">the message</param>
        /// <param name="concepts">the concepts</param>
        /// <param name="areConceptsMutuallyExclusive">are concepts exclusive</param>
        /// <param name="isEnvironmentClosed">is environment closed</param>
        /// <param name="language">the lanugage</param>
        private ConceptOutputInfo(string type, string typeExt, string message,
            IEnumerable<Concept> concepts, bool areConceptsMutuallyExclusive = false,
            bool isEnvironmentClosed = false,
            string language = null)
        {
            Type = type;
            TypeExt = typeExt;
            Message = message;
            Concepts = concepts;
            AreConceptsMutuallyExclusive = areConceptsMutuallyExclusive;
            IsEnvironmentClosed = isEnvironmentClosed;
            Language = language;
        }

        /// <summary>
        /// Deserializes the object out of a JSON dynamic object.
        /// </summary>
        /// <param name="jsonObject">the JSON dynamic object</param>
        /// <returns>the deserialized object</returns>
        public static ConceptOutputInfo Deserialize(dynamic jsonObject)
        {
            List<Concept> concepts = null;
            if (jsonObject.data != null)
            {
                concepts = new List<Concept>();
                foreach (var concept in jsonObject.data.concepts) {
                    concepts.Add(Concept.Deserialize(concept));
                }
            }
            bool areConceptsMutuallyExclusive = false;
            bool isEnvironmentClosed = false;
            if (jsonObject.output_config != null)
            {
                areConceptsMutuallyExclusive = jsonObject.output_config.concepts_mutually_exclusive;
                isEnvironmentClosed = jsonObject.output_config.closed_environment;
            }
            return new ConceptOutputInfo(
                (string) jsonObject.type,
                (string) jsonObject.type_ext,
                (string) jsonObject.message,
                concepts,
                areConceptsMutuallyExclusive,
                isEnvironmentClosed,
                (string) jsonObject.language);
        }

        public override bool Equals(object obj)
        {
            return obj is ConceptOutputInfo info &&
                   EqualityComparer<IEnumerable<Concept>>.Default.Equals(Concepts, info.Concepts) &&
                   AreConceptsMutuallyExclusive == info.AreConceptsMutuallyExclusive &&
                   IsEnvironmentClosed == info.IsEnvironmentClosed &&
                   Language == info.Language;
        }

        public override int GetHashCode()
        {
            var hashCode = 1420864546;
            hashCode = hashCode * -1521134295 +
                       EqualityComparer<IEnumerable<Concept>>.Default.GetHashCode(Concepts);
            hashCode = hashCode * -1521134295 + AreConceptsMutuallyExclusive.GetHashCode();
            hashCode = hashCode * -1521134295 + IsEnvironmentClosed.GetHashCode();
            hashCode = hashCode * -1521134295 +
                       EqualityComparer<string>.Default.GetHashCode(Language);
            return hashCode;
        }

        public override string ToString()
        {
            return $"[ConceptOutputInfo: (Concepts: {Concepts})]";
        }
    }
}
