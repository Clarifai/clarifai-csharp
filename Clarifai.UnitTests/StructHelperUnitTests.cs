using Clarifai.API.Requests;
using Google.Protobuf.WellKnownTypes;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Clarifai.UnitTests
{
    [TestFixture]
    public class MetadataConverterUnitTests
    {

        [Test]
        public void ConvertsPrimitiveFieldsToStruct()
        {
            Assert.AreEqual(
                new Struct
                {
                    Fields =
                    {
                        {"field1", Value.ForString("value1")},
                        {"field2", Value.ForNumber(3)},
                        {"field3", Value.ForNumber(1.0)},
                        {"field4", Value.ForBool(true)},
                        {"field5", Value.ForNull()},
                    }
                },
                StructHelper.JObjectToStruct(JObject.Parse(@"
{
  ""field1"": ""value1"",
  ""field2"": 3,
  ""field3"": 1.0,
  ""field4"": true,
  ""field5"": null
}
")));
        }

        [Test]
        public void ConvertsSingleArrayFieldToStruct()
        {
            Assert.AreEqual(
                new Struct
                {
                    Fields =
                    {
                        {
                            "field1",
                            Value.ForList(
                                Value.ForString("str"),
                                Value.ForNumber(3),
                                Value.ForNumber(1.0),
                                Value.ForBool(true),
                                Value.ForNull())
                        }
                    }
                },
                StructHelper.JObjectToStruct(JObject.Parse(
                    @"{""field1"": [""str"", 3, 1.0, true, null]}")));
        }

        [Test]
        public void ConvertsSingleObjectFieldToStruct()
        {
            Assert.AreEqual(
                new Struct
                {
                    Fields =
                    {
                        {
                            "field1",
                            Value.ForStruct(new Struct
                            {
                                Fields =
                                {
                                    {"key1", Value.ForString("str")},
                                    {"key2", Value.ForNumber(3)},
                                    {"key3", Value.ForNumber(1.0)},
                                    {"key4", Value.ForBool(true)},
                                    {"key5", Value.ForNull()},
                                }
                            })
                        },
                        {"field2", Value.ForString("key2")}
                    }
                },
                StructHelper.JObjectToStruct(JObject.Parse(@"
{
  ""field1"": {
    ""key1"": ""str"",
    ""key2"": 3,
    ""key3"": 1.0,
    ""key4"": true,
    ""key5"": null
  },
  ""field2"": ""key2""
}
")));
        }

        [Test]
        public void ConvertsPrimitiveFieldsToJObject()
        {
            Assert.AreEqual(
                JObject.Parse(@"
{
  ""field1"": ""value1"",
  ""field2"": 3.0,
  ""field3"": 1.0,
  ""field4"": true,
  ""field5"": null
}
"),
                StructHelper.StructToJObject(
                    new Struct
                    {
                        Fields =
                        {
                            {"field1", Value.ForString("value1")},
                            {"field2", Value.ForNumber(3)},
                            {"field3", Value.ForNumber(1.0)},
                            {"field4", Value.ForBool(true)},
                            {"field5", Value.ForNull()},
                        }
                    }
                    ));
        }

        [Test]
        public void ConvertsSingleArrayFieldToJObject()
        {
            Assert.AreEqual(
                JObject.Parse(@"{""field1"": [""str"", 3.0, 1.0, true, null]}"),
                StructHelper.StructToJObject(
                    new Struct
                    {
                        Fields =
                        {
                            {
                                "field1",
                                Value.ForList(
                                    Value.ForString("str"),
                                    Value.ForNumber(3),
                                    Value.ForNumber(1.0),
                                    Value.ForBool(true),
                                    Value.ForNull())
                            }
                        }
                    }
                    ));
        }

        [Test]
        public void ConvertsSingleObjectFieldToJObject()
        {
            Assert.AreEqual(
                JObject.Parse(@"
{
  ""field1"": {
    ""key1"": ""str"",
    ""key2"": 3.0,
    ""key3"": 1.0,
    ""key4"": true,
    ""key5"": null
  },
  ""field2"": ""key2""
}
"),
                StructHelper.StructToJObject(
                    new Struct
                    {
                        Fields =
                        {
                            {
                                "field1",
                                Value.ForStruct(new Struct
                                {
                                    Fields =
                                    {
                                        {"key1", Value.ForString("str")},
                                        {"key2", Value.ForNumber(3)},
                                        {"key3", Value.ForNumber(1.0)},
                                        {"key4", Value.ForBool(true)},
                                        {"key5", Value.ForNull()},
                                    }
                                })
                            },
                            {"field2", Value.ForString("key2")}
                        }
                    }
                    ));
        }
    }
}