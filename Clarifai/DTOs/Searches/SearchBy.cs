using System;
using Clarifai.DTOs.Inputs;
using Newtonsoft.Json.Linq;

namespace Clarifai.DTOs.Searches
{
    /// <summary>
    /// A collection of various methods by which to search for inputs.
    /// </summary>
    public abstract class SearchBy
    {
        public abstract JObject Serialize();

        public static SearchBy ConceptID(string id)
        {
            return new SearchByConceptID("output", id);
        }

        public static SearchBy UserTaggedConceptID(string id)
        {
            return new SearchByConceptID("input", id);
        }

        public static SearchBy ConceptName(string name)
        {
            return new SearchByConceptName(name);
        }

        public static SearchBy ImageURL(ClarifaiURLImage image)
        {
            return ImageURL(image.URL);
        }

        public static SearchBy ImageURL(string URL)
        {
            return new SearchByImageURL(URL);
        }

        public static SearchBy ImageBytes(ClarifaiFileImage fileImage)
        {
            return ImageBytes(fileImage.Bytes);
        }

        public static SearchBy ImageBytes(byte[] bytes)
        {
            return new SearchByImageBytes(bytes);
        }

        public static SearchBy Metadata(JObject metadata)
        {
            return new SearchByMetadata(metadata);
        }

        public static SearchBy Geo(GeoPoint geoPoint, GeoRadius geoRadius)
        {
            return new SearchByGeoCircle(geoPoint, geoRadius);
        }

        public static SearchBy Geo(GeoPoint geoPoint1, GeoPoint geoPoint2)
        {
            return new SearchByGeoRect(geoPoint1, geoPoint2);
        }

        private class SearchByConceptID : SearchBy
        {
            private readonly string _owningObjectID;
            private readonly string _id;

            public SearchByConceptID(string owningObjectID, string id)
            {
                _owningObjectID = owningObjectID;
                _id = id;
            }

            public override JObject Serialize()
            {
                return new JObject(
                    new JProperty(_owningObjectID, new JObject(
                        new JProperty("data", new JObject(
                            new JProperty("concepts", new JArray(new JObject(
                                new JProperty("id", _id)))))))));
            }
        }

        private class SearchByConceptName : SearchBy
        {
            private readonly string _name;

            public SearchByConceptName(string name)
            {
                _name = name;
            }

            public override JObject Serialize()
            {
                return new JObject(
                    new JProperty("output", new JObject(
                        new JProperty("data", new JObject(
                            new JProperty("concepts", new JArray(new JObject(
                                new JProperty("name", _name)))))))));
            }
        }

        private class SearchByImageURL : SearchBy
        {
            private readonly string _imageUrl;

            public SearchByImageURL(string imageUrl)
            {
                _imageUrl = imageUrl;
            }

            public override JObject Serialize()
            {
                return new JObject(
                    new JProperty("input", new JObject(
                        new JProperty("data", new JObject(
                            new JProperty("image", new JObject(
                                new JProperty("url", _imageUrl))))))));
            }
        }

        private class SearchByImageBytes : SearchBy
        {
            private readonly byte[] _bytes;

            public SearchByImageBytes(byte[] bytes)
            {
                _bytes = bytes;
            }

            public override JObject Serialize()
            {
                return new JObject(
                    new JProperty("output", new JObject(
                        new JProperty("data", new JObject(
                            new JProperty("image", new JObject(
                                new JProperty("base64", Convert.ToBase64String(_bytes)))))))));
            }
        }

        private class SearchByMetadata : SearchBy
        {
            private readonly JObject _metadata;

            public SearchByMetadata(JObject metadata)
            {
                _metadata = metadata;
            }

            public override JObject Serialize()
            {
                return new JObject(
                    new JProperty("output", new JObject(
                        new JProperty("data", new JObject(
                            new JProperty("metadata", _metadata))))));
            }
        }

        private class SearchByGeoCircle : SearchBy
        {
            private readonly GeoPoint _geoPoint;
            private readonly GeoRadius _geoRadius;

            public SearchByGeoCircle(GeoPoint geoPoint, GeoRadius geoRadius)
            {
                _geoPoint = geoPoint;
                _geoRadius = geoRadius;
            }

            public override JObject Serialize()
            {
                return new JObject(
                    new JProperty("input", new JObject(
                        new JProperty("data", new JObject(
                            new JProperty("geo", new JObject(
                                new JProperty("geo_point", _geoPoint.Serialize()),
                                new JProperty("geo_limit", _geoRadius.Serialize()))))))));
            }
        }

        private class SearchByGeoRect : SearchBy
        {
            private readonly GeoPoint _geoPoint1;
            private readonly GeoPoint _geoPoint2;

            public SearchByGeoRect(GeoPoint geoPoint1, GeoPoint geoPoint2)
            {
                _geoPoint1 = geoPoint1;
                _geoPoint2 = geoPoint2;
            }

            public override JObject Serialize()
            {
                return new JObject(
                    new JProperty("input", new JObject(
                        new JProperty("data", new JObject(
                            new JProperty("geo", new JObject(
                                new JProperty("geo_box", new JArray(
                                    new JObject(new JProperty("geo_point", _geoPoint1.Serialize())),
                                    new JObject(new JProperty("geo_point", _geoPoint2.Serialize())))
                                ))))))));
            }
        }
    }
}
