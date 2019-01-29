using System;
using Clarifai.Internal.GRPC;
using Newtonsoft.Json.Linq;

namespace Clarifai.DTOs
{
    /// <summary>
    /// The geographical location of an input.
    /// </summary>
    public class GeoPoint
    {
        /// <summary>
        /// The longitude - X axis.
        /// </summary>
        public decimal Longitude { get; }

        /// <summary>
        /// The latitude - Y axis.
        /// </summary>
        public decimal Latitude { get; }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="longitude">the longitude - longitude axis</param>
        /// <param name="latitude">the latitude - latitude axis</param>
        public GeoPoint(decimal longitude, decimal latitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        /// <summary>
        /// Returns a new point moved by the new coordinates.
        /// </summary>
        /// <param name="longitude">the longitude to translate by</param>
        /// <param name="latitude">the latitude to translate by</param>
        /// <returns>a translated geographical point</returns>
        public GeoPoint Translated(decimal longitude, decimal latitude)
        {
            return new GeoPoint(Longitude + longitude, Latitude + latitude);
        }

        [Obsolete]
        public JObject Serialize()
        {
            return new JObject(
                new JProperty("longitude", Longitude),
                new JProperty("latitude", Latitude));
        }

        public Internal.GRPC.GeoPoint GrpcSerialize()
        {
            return new Internal.GRPC.GeoPoint
            {
                Longitude = (float) Longitude,
                Latitude = (float) Latitude,
            };
        }

        [Obsolete]
        public static GeoPoint Deserialize(dynamic jsonObject)
        {
            dynamic point = jsonObject.geo_point;
            return new GeoPoint((decimal)point.longitude, (decimal)point.latitude);
        }

        public static GeoPoint GrpcDeserialize(Geo dataGeo)
        {
            Internal.GRPC.GeoPoint geoPoint = dataGeo.GeoPoint;
            return new GeoPoint((decimal) geoPoint.Longitude, (decimal) geoPoint.Latitude);
        }

        public override bool Equals(object obj)
        {
            return obj is GeoPoint point &&
                   Longitude == point.Longitude &&
                   Latitude == point.Latitude;
        }

        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + Longitude.GetHashCode();
            hashCode = hashCode * -1521134295 + Latitude.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return string.Format("[GeoPoint: (longitude: {0}, latitude: {1})]",
                Longitude, Latitude);
        }
    }
}
