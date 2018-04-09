using System;
using System.Collections.Generic;
using Clarifai.DTOs.Predictions;
using Newtonsoft.Json.Linq;

namespace Clarifai.DTOs.Inputs
{
    /// <summary>
    /// The type of input. Either Image or Video.
    /// </summary>
    public enum InputType
    {
        Image,
        Video,
    }

    /// <summary>
    /// The input form. Either URL or File.
    /// </summary>
    public enum InputForm
    {
        URL,
        File,
    }

    /// <summary>
    /// Input to a model.
    /// </summary>
    public interface IClarifaiInput
    {
        /// <summary>
        /// Inputs type.
        /// </summary>
        InputType Type { get; }

        /// <summary>
        /// Inputs form.
        /// </summary>
        InputForm Form { get; }

        /// <summary>
        /// The input ID.
        /// </summary>
        string ID { get; }

        /// <summary>
        /// The concepts associated with the input.
        /// </summary>
        IEnumerable<Concept> PositiveConcepts { get; }

        /// <summary>
        /// The concepts not associated with the input.
        /// </summary>
        IEnumerable<Concept> NegativeConcepts { get; }

        /// <summary>
        /// The input metadata.
        /// </summary>
        JObject Metadata { get; }

        /// <summary>
        /// Time of creation.
        /// </summary>
        DateTime? CreatedAt { get; }

        /// <summary>
        /// Input's geographical point.
        /// </summary>
        GeoPoint Geo { get; }

        /// <summary>
        /// Input's regions.
        /// </summary>
        List<Region> Regions { get; }

        JObject Serialize();
    }
}
