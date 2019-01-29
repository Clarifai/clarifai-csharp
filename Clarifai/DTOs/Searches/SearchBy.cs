using System;
using System.Collections.Generic;
using Clarifai.API.Requests;
using Clarifai.DTOs.Inputs;
using Clarifai.Exceptions;
using Clarifai.Internal.GRPC;
using Google.Protobuf;
using Newtonsoft.Json.Linq;

namespace Clarifai.DTOs.Searches
{
    /// <summary>
    /// A collection of various methods by which to search for inputs.
    /// </summary>
    public abstract class SearchBy
    {
        [Obsolete]
        public abstract JObject Serialize();

        public abstract And GrpcSerialize();

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

            [Obsolete]
            public override JObject Serialize()
            {
                return new JObject(
                    new JProperty(_ownerObjectKey, new JObject(
                        new JProperty("data", new JObject(
                            new JProperty("concepts", new JArray(new JObject(
                                new JProperty("id", _id)))))))));
            }

            public override And GrpcSerialize()
            {
                var data = new Data
                {
                    // Since the JSON serializer removes the default value 0 at serialization,
                    // we set here Value = 1 to later remove it, and in case of 0, explicitly set
                    // it.
                    Concepts = { new Concept { Id = _id, Value = 1}}
                };
                if (_ownerObjectKey == "input")
                {
                    return new And
                    {
                        Input = new Input
                        {
                            Data = data
                        }
                    };
                }
                else if (_ownerObjectKey == "output")
                {
                    return new And
                    {
                        Output = new Output
                        {
                            Data = data
                        }
                    };
                }
                else
                {
                    throw new ClarifaiException($"Unknown ownerObjectKey {_ownerObjectKey}");
                }
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

            [Obsolete]
            public override JObject Serialize()
            {
                return new JObject(
                    new JProperty(_ownerObjectKey, new JObject(
                        new JProperty("data", new JObject(
                            new JProperty("concepts", new JArray(new JObject(
                                new JProperty("name", _name)))))))));
            }

            public override And GrpcSerialize()
            {
                var data = new Data
                {
                    // Since the JSON serializer removes the default value 0 at serialization,
                    // we set here Value = 1 to later remove it, and in case of 0, explicitly set
                    // it.
                    Concepts = { new Concept { Name = _name, Value = 1}}
                };
                if (_ownerObjectKey == "input")
                {
                    return new And
                    {
                        Input = new Input
                        {
                            Data = data
                        }
                    };
                }
                else if (_ownerObjectKey == "output")
                {
                    return new And
                    {
                        Output = new Output
                        {
                            Data = data
                        }
                    };
                }
                else
                {
                    throw new ClarifaiException($"Unknown ownerObjectKey {_ownerObjectKey}");
                }
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

            [Obsolete]
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

            public override And GrpcSerialize()
            {
                var image = new Image
                {
                    Url = _imageUrl
                };
                if (_crop != null)
                {
                    image = new Image(image)
                    {
                        Crop = {_crop.GrpcSerializeAsArray()}
                    };
                }
                return new And
                {
                    Input = new Input
                    {
                        Data = new Data
                        {
                            Image = image
                        }
                    }
                };
            }
        }

        private class SearchByImageVisuallyWithUrl : SearchBy
        {
            private readonly string _imageUrl;
            private readonly Crop _crop;

            public SearchByImageVisuallyWithUrl (string imageUrl, Crop crop = null)
            {
                _imageUrl = imageUrl;
                _crop = crop;
            }

            [Obsolete]
            public override JObject Serialize()
            {
                var image = new JObject(
                    new JProperty("url", _imageUrl));
                if (_crop != null)
                {
                    image.Add("crop", _crop.SerializeAsArray());
                }
                return new JObject(
                    new JProperty("output", new JObject(
                        new JProperty("input", new JObject(
                            new JProperty("data", new JObject(
                                new JProperty("image", image))))))));
            }

            public override And GrpcSerialize()
            {
                var image = new Image
                {
                    Url = _imageUrl
                };
                if (_crop != null)
                {
                    image = new Image(image)
                    {
                        Crop = {_crop.GrpcSerializeAsArray()}
                    };
                }
                return new And
                {
                    Output = new Output
                    {
                        Input = new Input
                        {
                            Data = new Data
                            {
                                Image = image
                            }
                        }
                    }
                };
            }
        }

        private class SearchByImageVisuallyWithBytes : SearchBy
        {
            private readonly byte[] _bytes;
            private readonly Crop _crop;

            public SearchByImageVisuallyWithBytes(byte[] bytes, Crop crop = null)
            {
                _bytes = bytes;
                _crop = crop;
            }

            [Obsolete]
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
                        new JProperty("input", new JObject(
                            new JProperty("data", new JObject(
                                new JProperty("image", image))))))));
            }

            public override And GrpcSerialize()
            {
                var image = new Image
                {
                    Base64 = ByteString.CopyFrom(_bytes)
                };
                if (_crop != null)
                {
                    image = new Image(image)
                    {
                        Crop = {_crop.GrpcSerializeAsArray()}
                    };
                }
                return new And
                {
                    Output = new Output
                    {
                        Input = new Input
                        {
                            Data = new Data
                            {
                                Image = image
                            }
                        }
                    }
                };
            }
        }

        private class SearchByMetadata : SearchBy
        {
            private readonly JObject _metadata;

            public SearchByMetadata(JObject metadata)
            {
                _metadata = metadata;
            }

            [Obsolete]
            public override JObject Serialize()
            {
                return new JObject(
                    new JProperty("input", new JObject(
                        new JProperty("data", new JObject(
                            new JProperty("metadata", _metadata))))));
            }

            public override And GrpcSerialize()
            {
                return new And
                {
                    Input = new Input
                    {
                        Data = new Data
                        {
                            Metadata = StructHelper.JObjectToStruct(_metadata)
                        }
                    }
                };
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

            [Obsolete]
            public override JObject Serialize()
            {
                return new JObject(
                    new JProperty("input", new JObject(
                        new JProperty("data", new JObject(
                            new JProperty("geo", new JObject(
                                new JProperty("geo_point", _geoPoint.Serialize()),
                                new JProperty("geo_limit", _geoRadius.Serialize()))))))));
            }

            public override And GrpcSerialize()
            {
                return new And
                {
                    Input = new Input
                    {
                        Data = new Data
                        {
                            Geo = new Geo
                            {
                                GeoPoint = _geoPoint.GrpcSerialize(),
                                GeoLimit = _geoRadius.GrpcSerialize()
                            }
                        }
                    }
                };
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

            [Obsolete]
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

            public override And GrpcSerialize()
            {
                return new And
                {
                    Input = new Input
                    {
                        Data = new Data
                        {
                            Geo = new Geo
                            {
                                GeoBox = {
                                    new List<GeoBoxedPoint>
                                    {
                                        new GeoBoxedPoint { GeoPoint = _geoPoint1.GrpcSerialize() },
                                        new GeoBoxedPoint { GeoPoint = _geoPoint2.GrpcSerialize() },
                                    }
                                }
                            }
                        }
                    }
                };
            }
        }
    }
}
