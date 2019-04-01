using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Clarifai.Exceptions;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using Grpc.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Clarifai.API.Requests
{
    public class JsonCallInvoker : CallInvoker
    {
        private readonly string _url;
        private readonly RequestMethod _httpMethod;
        private readonly IClarifaiHttpClient _httpClient;
        private readonly JsonFormatter _jsonFormatter;
        private readonly JsonParser _jsonParser;

        public JsonCallInvoker(
            string url,
            RequestMethod httpMethod,
            IClarifaiHttpClient httpClient,
            JsonFormatter jsonFormatter,
            JsonParser jsonParser)
        {
            _url = url;
            _httpMethod = httpMethod;
            _httpClient = httpClient;
            _jsonFormatter = jsonFormatter;
            _jsonParser = jsonParser;
        }

        public override TResponse BlockingUnaryCall<TRequest, TResponse>(
            Method<TRequest, TResponse> method,
            string host,
            CallOptions options,
            TRequest request)
        {
            return CallBackend(method, request).Result;
        }

        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
            Method<TRequest, TResponse> method, string host, CallOptions options,
            TRequest request)
        {
            Task<TResponse> requestAsync = CallBackend(method, request);

            return new AsyncUnaryCall<TResponse>(
                requestAsync,
                Task.Run(() => new Metadata()),
                () => new Status(),
                () => new Metadata(),
                () => { }
            );
        }

        private async Task<TResponse> CallBackend<TRequest, TResponse>(
            Method<TRequest, TResponse> method, TRequest request)
        {
            Console.WriteLine("REQUEST:");
            Console.WriteLine(HttpRequestBody(request));
            string responseBody = await HttpRequest(request);
            Console.WriteLine("RESPONSE:");
            Console.WriteLine(responseBody);

            Type methodReturnType = method.ResponseMarshaller.Deserializer.GetMethodInfo()
                .ReturnType;
            PropertyInfo descriptor = methodReturnType.GetRuntimeProperty("Descriptor");
            MessageDescriptor messageDescriptor = (MessageDescriptor)descriptor.GetValue(
                methodReturnType);

            IMessage message = _jsonParser.Parse(responseBody, messageDescriptor);

            return (TResponse) message;
        }

        private async Task<string> HttpRequest<TResponse>(TResponse request)
        {
            switch (_httpMethod)
            {
                case RequestMethod.GET:
                {
                    return await _httpClient.GetAsync(_url);
                }
                case RequestMethod.POST:
                {
                    return await _httpClient.PostAsync(_url, HttpRequestBody(request));
                }
                case RequestMethod.PATCH:
                {
                    return await _httpClient.PatchAsync(_url, HttpRequestBody(request));
                }
                case RequestMethod.DELETE:
                {
                    return await _httpClient.DeleteAsync(_url, HttpRequestBody(request));
                }
                default:
                {
                    throw new ClarifaiException("Unknown RequestMethod: " + _httpMethod);
                }
            }
        }

        private JObject HttpRequestBody<TResponse>(TResponse request)
        {
            var stringWriter = new StringWriter();
            _jsonFormatter.WriteValue(stringWriter, request);
            string requestBody = stringWriter.ToString();
            requestBody = ConvertKeysToSnakeCase(requestBody);
            return JsonConvert.DeserializeObject<JObject>(requestBody);
        }

        /// <summary>
        /// Protobuf's JsonFormatter converts all snake_case keys to camelCase.
        /// We have to convert them back to snake_case manually.
        /// </summary>
        /// <remarks>
        /// This won't be needed anymore if, at some point, JsonFormatter takes a parameter
        /// disabling this conversion. See:
        /// https://github.com/protocolbuffers/protobuf/issues/5583
        /// </remarks>
        /// <param name="requestBody">the JSON request bdy</param>
        /// <returns>adjusted JSON request body</returns>
        /// <exception cref="NotImplementedException"></exception>
        private string ConvertKeysToSnakeCase(string requestBody)
        {
            JToken token = JsonHelper.DeserializeWithLowerCasePropertyNames(requestBody);

            IEnumerable<JToken> conceptsPropertyIter = AllTokens(token).Where(
                t => t.Type == JTokenType.Property && ((JProperty) t).Name == "concepts");
            foreach (JToken conceptsProperty in conceptsPropertyIter )
            {
                foreach (JToken conceptList in conceptsProperty.Children())
                {
                    foreach (JToken conceptToken in conceptList.Children())
                    {
                        JObject concept = (JObject) conceptToken;
                        JToken selectToken = concept.SelectToken("value");
                        if (selectToken == null)
                        {
                            concept.Add("value", 0);
                        }
                        else if ((int)selectToken == 1)
                        {
                            concept.Remove("value");
                        }
                    }
                }
            }

            return JsonConvert.SerializeObject(token);
        }

        private IEnumerable<JToken> AllTokens(JToken obj) {
            var toSearch = new Stack<JToken>(obj.Children());
            while (toSearch.Count > 0) {
                JToken inspected = toSearch.Pop();
                yield return inspected;
                foreach (var child in inspected) {
                    toSearch.Push(child);
                }
            }
        }

        public override AsyncServerStreamingCall<TResponse>
            AsyncServerStreamingCall<TRequest, TResponse>(Method<TRequest, TResponse> method,
                string host, CallOptions options, TRequest request)
        {
            throw new NotImplementedException();
        }

        public override AsyncClientStreamingCall<TRequest, TResponse>
            AsyncClientStreamingCall<TRequest, TResponse>(Method<TRequest, TResponse> method,
                string host, CallOptions options)
        {
            throw new NotImplementedException();
        }

        public override AsyncDuplexStreamingCall<TRequest, TResponse>
            AsyncDuplexStreamingCall<TRequest, TResponse>(Method<TRequest, TResponse> method,
                string host, CallOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
