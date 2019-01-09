using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Clarifai.Internal.GRPC;

namespace Clarifai.DTOs
{
    public class GeoRadius
    {
        public decimal Value { get; }
        public RadiusUnit Unit { get; }

        public GeoRadius(decimal value, RadiusUnit unit)
        {
            Value = value;
            Unit = unit;
        }

        public JObject Serialize()
        {
            return new JObject(
                new JProperty("type", Unit.Value),
                new JProperty("value", Value));
        }

        public GeoLimit GrpcSerialize()
        {
            return new GeoLimit
            {
                Type = Unit.Value,
                Value = (float) Value,
            };
        }

        public override bool Equals(object obj)
        {
            return obj is GeoRadius radius &&
                   Value == radius.Value &&
                   EqualityComparer<RadiusUnit>.Default.Equals(Unit, radius.Unit);
        }

        public override int GetHashCode()
        {
            var hashCode = -177567199;
            hashCode = hashCode * -1521134295 + Value.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<RadiusUnit>.Default.GetHashCode(Unit);
            return hashCode;
        }

        public override string ToString()
        {
            return $"[GeoRadius: (value: {Value}, unit: {Unit})]";
        }

        /// <summary>
        /// Units used in geographical radius.
        ///
        /// Note: since latitude/longitude are angles, it's possible to express distance as
        /// an angle in degrees or radians.
        /// </summary>
        public class RadiusUnit
        {
            public static readonly RadiusUnit WithinMiles = new RadiusUnit("withinMiles");
            public static readonly RadiusUnit WithinKilometers = new RadiusUnit("withinKilometers");
            public static readonly RadiusUnit WithinDegrees = new RadiusUnit("withinDegrees");
            public static readonly RadiusUnit WithinRadians = new RadiusUnit("withinRadians");

            public string Value { get; }

            private RadiusUnit(string value)
            {
                Value = value;
            }

            public override bool Equals(object obj)
            {
                return obj is RadiusUnit unit &&
                       Value == unit.Value;
            }

            public override int GetHashCode()
            {
                return -1937169414 + EqualityComparer<string>.Default.GetHashCode(Value);
            }

            public override string ToString()
            {
                return $"[RadiusUnit: (value: {Value})]";
            }
        }
    }
}
