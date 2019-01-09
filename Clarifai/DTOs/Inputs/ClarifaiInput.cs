using System;
using System.Collections.Generic;
using System.Linq;
using Clarifai.API.Requests;
using Clarifai.Exceptions;
using Clarifai.Internal.GRPC;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Newtonsoft.Json.Linq;
using Concept = Clarifai.DTOs.Predictions.Concept;
using Region = Clarifai.DTOs.Predictions.Region;

namespace Clarifai.DTOs.Inputs
{
    /// <inheritdoc />
    public abstract class ClarifaiInput : IClarifaiInput
    {

        /// <inheritdoc />
        public InputType Type { get; }

        /// <inheritdoc />
        public InputForm Form { get; }

        /// <inheritdoc />
        public string ID { get; }

        /// <inheritdoc />
        public IEnumerable<Concept> PositiveConcepts { get; }

        /// <inheritdoc />
        public IEnumerable<Concept> NegativeConcepts { get; }

        /// <inheritdoc />
        public JObject Metadata { get; }

        /// <inheritdoc />
        public DateTime? CreatedAt { get; }

        /// <inheritdoc />
        public GeoPoint Geo { get; }

        /// <inheritdoc />
        public List<Region> Regions { get; }

        protected ClarifaiInput(InputType type, InputForm form, string id,
            IEnumerable<Concept> positiveConcepts, IEnumerable<Concept> negativeConcepts,
            JObject metadata, DateTime? createdAt, GeoPoint geo, List<Region> regions)
        {
            Type = type;
            Form = form;
            ID = id;
            PositiveConcepts = positiveConcepts;
            NegativeConcepts = negativeConcepts;
            Metadata = metadata;
            CreatedAt = createdAt;
            Geo = geo;
            Regions = regions;
        }

        public abstract JObject Serialize();

        protected JObject Serialize(JProperty inputProperty)
        {
            var data = new JObject(inputProperty);

            var concepts = new List<JObject>();
            if (PositiveConcepts != null)
            {
                concepts.AddRange(PositiveConcepts.Select(c => c.Serialize(true)));
            }
            if (NegativeConcepts != null)
            {
                concepts.AddRange(NegativeConcepts.Select(c => c.Serialize(false)));
            }
            if (concepts.Any())
            {
                data["concepts"] = new JArray(concepts);
            }

            if (Metadata != null)
            {
                data["metadata"] = Metadata;
            }
            if (Geo != null)
            {
                data["geo"] = new JObject(new JProperty("geo_point", Geo.Serialize()));
            }
            var obj = new JObject(
                new JProperty("id", ID),
                new JProperty("data", data));
            if (CreatedAt != null)
            {
                obj["created_at"] = CreatedAt.Value.ToString("yyyy-MM-ddTHH:mm:ssZ");
            }
            return obj;
        }

        public abstract Input GrpcSerialize();

        protected Input GrpcSerialize(string inputType, IMessage imageOrVideo)
        {
            var concepts = new List<Internal.GRPC.Concept>();
            if (PositiveConcepts != null)
            {
                concepts.AddRange(PositiveConcepts.Select(c => c.GrpcSerialize(true)));
            }
            if (NegativeConcepts != null)
            {
                concepts.AddRange(NegativeConcepts.Select(c => c.GrpcSerialize(false)));
            }

            var geo = new Geo
            {
                GeoPoint = Geo != null ? Geo.GrpcSerialize() : new Internal.GRPC.GeoPoint(),
            };

            var data = new Data
            {
                Concepts = {concepts},
            };
            if (Geo != null)
            {
                data = new Data(data)
                {
                    Geo = geo
                };
            }
            if (Metadata != null)
            {
                data = new Data(data)
                {
                    Metadata = StructHelper.JObjectToStruct(Metadata)
                };
            }

            switch (inputType)
            {
                case "image":
                    data = new Data(data)
                    {
                        Image = (Image)imageOrVideo,
                    };
                    break;
                case "video":
                    data = new Data(data)
                    {
                        Video = (Video)imageOrVideo,
                    };
                    break;
                default:
                    throw new ClarifaiException($"Unknown inputType {inputType}");
            }

            var obj = new Input
            {
                Data = data,
            };

            if (ID != null)
            {
                obj = new Input(obj)
                {
                    Id = ID,
                };
            }

            if (CreatedAt.HasValue)
            {
                obj = new Input(obj)
                {
                    CreatedAt = Timestamp.FromDateTime(CreatedAt.Value),
                };
            }
            return obj;
        }

        /// <summary>
        /// Deserializes a JSON object into a concrete ClarifaiInput instance.
        /// It's either ClarifaiURLImage or ClarifaiURLVideo.
        /// </summary>
        /// <param name="jsonObject">the JSON object</param>
        /// <returns>a new ClarifaiInput instance</returns>
        public static ClarifaiInput Deserialize(dynamic jsonObject)
        {
            if (jsonObject.data.image != null)
            {
                if (jsonObject.data.image.url != null)
                {
                    return ClarifaiURLImage.Deserialize(jsonObject);
                }
                else
                {
                    return ClarifaiFileImage.Deserialize(jsonObject);
                }
            }
            else if (jsonObject.data.video != null)
            {
                if (jsonObject.data.video.url != null)
                {
                    return ClarifaiURLVideo.Deserialize(jsonObject);
                }
                else
                {
                    return ClarifaiFileVideo.Deserialize(jsonObject);
                }
            }
            else
            {
                throw new ClarifaiException(
                    string.Format("Unknown ClarifaiInput json response: {0}", jsonObject));
            }
        }

        public static ClarifaiInput GrpcDeserialize(Input input)
        {
            if (input.Data.Image != null)
            {
                if (input.Data.Image.Url != "")
                {
                    return ClarifaiURLImage.GrpcDeserialize(input);
                }
                else
                {
                    return ClarifaiFileImage.GrpcDeserialize(input);
                }
            }
            else if (input.Data.Video != null)
            {
                if (input.Data.Video.Url != "")
                {
                    return ClarifaiURLVideo.GrpcDeserialize(input);
                }
                else
                {
                    return ClarifaiFileVideo.GrpcDeserialize(input);
                }
            }
            else
            {
                throw new ClarifaiException(
                    string.Format("Unknown ClarifaiInput response: {0}", input));
            }
        }

        public override bool Equals(object obj)
        {
            return obj is ClarifaiInput input &&
                   ID == input.ID &&
                   EqualityComparer<IEnumerable<Concept>>.Default.Equals(PositiveConcepts, input.PositiveConcepts)
                   &&
                   EqualityComparer<JObject>.Default.Equals(Metadata, input.Metadata) &&
                   EqualityComparer<DateTime?>.Default.Equals(CreatedAt, input.CreatedAt) &&
                   EqualityComparer<GeoPoint>.Default.Equals(Geo, input.Geo);
        }

        public override int GetHashCode()
        {
            var hashCode = 833924800;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ID);
            hashCode = hashCode * -1521134295 +
                EqualityComparer<IEnumerable<Concept>>.Default.GetHashCode(PositiveConcepts);
            hashCode = hashCode * -1521134295 +
                EqualityComparer<JObject>.Default.GetHashCode(Metadata);
            hashCode = hashCode * -1521134295 +
                EqualityComparer<DateTime?>.Default.GetHashCode(CreatedAt);
            hashCode = hashCode * -1521134295 + EqualityComparer<GeoPoint>.Default.GetHashCode(Geo);
            return hashCode;
        }

        public override string ToString()
        {
            return string.Format($"[ClarifaiInput: (id: {ID})]");
        }
    }
}
