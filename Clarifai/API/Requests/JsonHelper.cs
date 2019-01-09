using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Clarifai.API.Requests
{
    public static class JsonHelper
    {
        private static readonly JsonSerializer _serializer = new JsonSerializer();

        public static JToken DeserializeWithLowerCasePropertyNames(string json)
        {
            using (var textReader = new StringReader(json))
            using (var jsonReader = new LowerCasePropertyNameJsonReader(textReader))
            {
                return _serializer.Deserialize<JToken>(jsonReader);
            }
        }

        private class LowerCasePropertyNameJsonReader : JsonTextReader
        {
            public LowerCasePropertyNameJsonReader(TextReader textReader)
                : base(textReader)
            {
            }

            public override object Value
            {
                get
                {
                    if (TokenType != JsonToken.PropertyName)
                    {
                        return base.Value;
                    }

                    string key = ((string) base.Value);
                    var sb = new StringBuilder();
                    for (int i = 0; i < key.Length; i++)
                    {
                        char c = key[i];

                        if (i > 0 && char.IsUpper(c))
                        {
                            sb.Append("_");
                            sb.Append(char.ToLower(c));
                        }
                        else
                        {
                            sb.Append(c);
                        }
                    }
                    return sb.ToString();
                }
            }
        }
    }
}
