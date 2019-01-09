using System.Collections.Generic;

namespace Clarifai.DTOs.Predictions
{
    /// <summary>
    /// Represents a color associated with a certain input.
    /// </summary>
    public class Color : IPrediction
    {
        /// <summary>
        /// IPrediction type.
        /// </summary>
        public string TYPE => "color";

        /// <summary>
        /// Raw hex.
        /// </summary>
        public string RawHex { get; }

        /// <summary>
        /// Web safe hex.
        /// </summary>
        public string Hex { get; }

        /// <summary>
        /// Web safe color name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Value of the color. Only used in association with an input.
        /// </summary>
        public decimal Value { get; }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="rawHex">the raw hex</param>
        /// <param name="hex">the hex</param>
        /// <param name="name">the name</param>
        /// <param name="value">the value</param>
        private Color(string rawHex, string hex, string name, decimal value)
        {
            RawHex = rawHex;
            Hex = hex;
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Deserializes this object from dynamic JSON object.
        /// </summary>
        /// <param name="colorJsonObject">the JSON object</param>
        /// <returns>a new instance of this class</returns>
        public static Color Deserialize(dynamic colorJsonObject)
        {
            return new Color(
                (string)colorJsonObject.raw_hex,
                (string)colorJsonObject.w3c.hex,
                (string)colorJsonObject.w3c.name,
                (decimal)colorJsonObject.value);
        }

        /// <summary>
        /// Deserializes this object from a gRPC object.
        /// </summary>
        /// <param name="color">the gRPC color object</param>
        /// <returns>a new instance of this class</returns>
        public static Color GrpcDeserialize(Internal.GRPC.Color color)
        {
            return new Color(
                color.RawHex,
                color.W3C.Hex,
                color.W3C.Name,
                (decimal) color.Value);
        }

        public override bool Equals(object obj)
        {
            return obj is Color color &&
                   RawHex == color.RawHex &&
                   Hex == color.Hex &&
                   Name == color.Name &&
                   Value == color.Value;
        }

        public override int GetHashCode()
        {
            var hashCode = -1924459456;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(RawHex);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Hex);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + Value.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return $"[Color: (name: {Name})]";
        }
    }
}
