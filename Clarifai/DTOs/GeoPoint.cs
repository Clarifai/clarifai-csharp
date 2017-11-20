using Newtonsoft.Json.Linq;

namespace Clarifai.DTOs
{
    /// <summary>
    /// The geographical location of an input.
    /// </summary>
    public class GeoPoint
    {
        /// <summary>
        /// The latitude - X axis.
        /// </summary>
        public decimal Latitude { get; }

        /// <summary>
        /// The longitude - Y axis.
        /// </summary>
        public decimal Longitude { get; }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="latitude">the latitude - latitude axis</param>
        /// <param name="longitude">the longitude - longitude axis</param>
        public GeoPoint(decimal latitude, decimal longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        /// <summary>
        /// Returns a new point moved by the new coordinates
        /// </summary>
        /// <param name="latitude">the latitude to translate by</param>
        /// <param name="longitude">the longitude to translate by</param>
        /// <returns>a translated geographical point</returns>
        public GeoPoint Translated(decimal latitude, decimal longitude)
        {
            return new GeoPoint(Latitude + latitude, Longitude + longitude);
        }

        public JObject Serialize()
        {
            return new JObject(
                new JProperty("latitude", Latitude),
                new JProperty("longitude", Longitude));
        }

        public static GeoPoint Deserialize(dynamic jsonObject)
        {
            dynamic point = jsonObject.geo_point;
            return new GeoPoint((decimal)point.latitude, (decimal)point.longitude);
        }

        public override bool Equals(object obj)
        {
            return obj is GeoPoint point &&
                   Latitude == point.Latitude &&
                   Longitude == point.Longitude;
        }

        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + Latitude.GetHashCode();
            hashCode = hashCode * -1521134295 + Longitude.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return string.Format("[GeoPoint: (latitude: {0}, longitude: {0})]",
                Latitude, Longitude);
        }
    }
}
