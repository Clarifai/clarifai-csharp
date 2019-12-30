using System;
using Clarifai.DTOs.Inputs;
using Clarifai.Exceptions;
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

        /// <summary>
        /// A search clause that will match inputs that had images with the given URL.
        ///
        /// Note: This is NOT a visual-similarity search. This is a simple string search for the
        /// given image's URL. For visual similarity please use
        /// <see cref="ImageVisually(string,Crop)"/>
        /// </summary>
        /// <param name="url">the URL of the image to search by</param>
        /// <returns>a new SearchBy instance</returns>
        public static SearchBy ImageURL(string url)
        {
            return new SearchByImageURL(url);
        }

        public static SearchBy ImageVisually(ClarifaiURLImage image, Crop crop = null)
        {
            return ImageVisually(image.URL, crop);
        }

        public static SearchBy ImageVisually(string url, Crop crop = null)
        {
            return new SearchByImageVisuallyWithUrl(url, crop);
        }

        public static SearchBy ImageVisually(ClarifaiFileImage fileImage, Crop crop = null)
        {
            return ImageVisually(fileImage.Bytes, crop);
        }

        public static SearchBy ImageVisually(byte[] bytes, Crop crop = null)
        {
            return new SearchByImageVisuallyWithBytes(bytes, crop);
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

            public SearchByImageURL(string imageUrl, Crop crop = null)
            {
                _imageUrl = imageUrl;

                if (crop != null)
                {
                    throw new ClarifaiException(
                        "The `crop` argument is not used/supported by any more by SearchByImageURL."
                    );
                }
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

        private class SearchByImageVisuallyWithUrl : SearchBy
        {
            private readonly string _imageUrl;

            public SearchByImageVisuallyWithUrl (string imageUrl, Crop crop = null)
            {
                _imageUrl = imageUrl;

                if (crop != null)
                {
                    throw new ClarifaiException(
                        "The `crop` argument is not used/supported by any more by " +
                        "SearchByImageVisuallyWithUrl.");
                }
            }

            public override JObject Serialize()
            {
                return new JObject(
                    new JProperty("output", new JObject(
                        new JProperty("input", new JObject(
                            new JProperty("data", new JObject(
                                new JProperty("image", new JObject(
                                    new JProperty("url", _imageUrl))))))))));
            }
        }

        private class SearchByImageVisuallyWithBytes : SearchBy
        {
            private readonly byte[] _bytes;

            public SearchByImageVisuallyWithBytes(byte[] bytes, Crop crop = null)
            {
                _bytes = bytes;

                if (crop != null)
                {
                    throw new ClarifaiException(
                        "The `crop` argument is not used/supported by any more by " +
                        "SearchByImageVisuallyWithUrl.");
                }
            }

            public override JObject Serialize()
            {
                return new JObject(
                    new JProperty("output", new JObject(
                        new JProperty("input", new JObject(
                            new JProperty("data", new JObject(
                                new JProperty("image", new JObject(
                                    new JProperty("base64", Convert.ToBase64String(_bytes))))))))))
                );
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
                    new JProperty("input", new JObject(
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
