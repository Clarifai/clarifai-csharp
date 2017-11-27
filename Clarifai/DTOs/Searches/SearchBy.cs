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
            return new SearchByConceptName("output", name);
        }

        public static SearchBy UserTaggedConceptName(string name)
        {
            return new SearchByConceptName("input", name);
        }

        public static SearchBy ImageURL(ClarifaiURLImage image, Crop crop = null)
        {
            return ImageURL(image.URL, crop);
        }

        public static SearchBy ImageURL(string URL, Crop crop = null)
        {
            return new SearchByImageURL(URL, crop);
        }

        public static SearchBy ImageBytes(ClarifaiFileImage fileImage, Crop crop = null)
        {
            return ImageBytes(fileImage.Bytes, crop);
        }

        public static SearchBy ImageBytes(byte[] bytes, Crop crop = null)
        {
            return new SearchByImageBytes(bytes, crop);
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
            private readonly string _ownerObjectKey;
            private readonly string _id;

            public SearchByConceptID(string ownerObjectKey, string id)
            {
                _ownerObjectKey = ownerObjectKey;
                _id = id;
            }

            public override JObject Serialize()
            {
                return new JObject(
                    new JProperty(_ownerObjectKey, new JObject(
                        new JProperty("data", new JObject(
                            new JProperty("concepts", new JArray(new JObject(
                                new JProperty("id", _id)))))))));
            }
        }

        private class SearchByConceptName : SearchBy
        {
            private readonly string _ownerObjectKey;
            private readonly string _name;

            public SearchByConceptName(string ownerObjectKey, string name)
            {
                _ownerObjectKey = ownerObjectKey;
                _name = name;
            }

            public override JObject Serialize()
            {
                return new JObject(
                    new JProperty(_ownerObjectKey, new JObject(
                        new JProperty("data", new JObject(
                            new JProperty("concepts", new JArray(new JObject(
                                new JProperty("name", _name)))))))));
            }
        }

        private class SearchByImageURL : SearchBy
        {
            private readonly string _imageUrl;
            private readonly Crop _crop;

            public SearchByImageURL(string imageUrl, Crop crop = null)
            {
                _imageUrl = imageUrl;
                _crop = crop;
            }

            public override JObject Serialize()
            {
                var image = new JObject(
                    new JProperty("url", _imageUrl));
                if (_crop != null)
                {
                    image.Add("crop", _crop.SerializeAsArray());
                }
                return new JObject(
                    new JProperty("input", new JObject(
                        new JProperty("data", new JObject(
                            new JProperty("image", image))))));
            }
        }

        private class SearchByImageBytes : SearchBy
        {
            private readonly byte[] _bytes;
            private readonly Crop _crop;

            public SearchByImageBytes(byte[] bytes, Crop crop = null)
            {
                _bytes = bytes;
                _crop = crop;
            }

            public override JObject Serialize()
            {
                var image = new JObject(
                    new JProperty("base64", Convert.ToBase64String(_bytes)));
                if (_crop != null)
                {
                    image.Add("crop", _crop.SerializeAsArray());
                }
                return new JObject(
                    new JProperty("output", new JObject(
                        new JProperty("data", new JObject(
                            new JProperty("image", image))))));
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
