using System.Collections.Generic;
using System.Linq;
using Clarifai.Exceptions;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Newtonsoft.Json.Linq;

namespace Clarifai.API.Requests
{
    /// <summary>
    /// Converts a Protobuf Struct object to JObject, and back.
    /// </summary>
    public static class StructHelper
    {
        public static Struct JObjectToStruct(JObject jsonObject)
        {
            var fields = new MapField<string, Value>();
            foreach (JProperty property in jsonObject.Properties())
            {
                fields.Add(property.Name, JTokenToValue(property.Value));
            }
            return new Struct()
            {
                Fields = {fields}
            };
        }

        private static Value JTokenToValue(JToken propertyValue)
        {
            switch (propertyValue.Type)
            {
                case JTokenType.Object:
                {
                    return new Value
                    {
                        StructValue = JObjectToStruct((JObject) propertyValue)
                    };
                }
                case JTokenType.Array:
                {
                    var values = new List<Value>();
                    foreach (JToken token in (JArray) propertyValue)
                    {
                        values.Add(JTokenToValue(token));
                    }

                    return Value.ForList(values.ToArray());
                }
                case JTokenType.Null:
                {
                    return Value.ForNull();
                }
                case JTokenType.Boolean:
                {
                    return Value.ForBool(propertyValue.Value<bool>());
                }
                case JTokenType.String:
                {
                    return Value.ForString(propertyValue.Value<string>());
                }
                case JTokenType.Integer:
                {
                    return Value.ForNumber(propertyValue.Value<int>());
                }
                case JTokenType.Float:
                {
                    return Value.ForNumber(propertyValue.Value<float>());
                }
                default:
                {
                    throw new ClarifaiException("Unknown metadata JsonObject field type.");
                }
            }
        }

        public static JObject StructToJObject(Struct s)
        {
            var jsonObject = new JObject();
            foreach (KeyValuePair<string, Value> pair in s.Fields)
            {
                jsonObject[pair.Key] = ValueToJToken(pair.Value);
            }
            return jsonObject;
        }

        private static JToken ValueToJToken(Value value)
        {
            switch (value.KindCase)
            {
                case Value.KindOneofCase.StructValue:
                {
                    return StructToJObject(value.StructValue);
                }
                case Value.KindOneofCase.ListValue:
                {
                    return new JArray(value.ListValue.Values.Select(ValueToJToken));
                }
                case Value.KindOneofCase.NullValue:
                {
                    return JValue.CreateNull();
                }
                case Value.KindOneofCase.BoolValue:
                {
                    return new JValue(value.BoolValue);
                }
                case Value.KindOneofCase.StringValue:
                {
                    return new JValue(value.StringValue);
                }
                case Value.KindOneofCase.NumberValue:
                {
                    return new JValue(value.NumberValue);
                }
                default:
                {
                    throw new ClarifaiException("Unknown Value field type.");
                }
            }
        }
    }
}
