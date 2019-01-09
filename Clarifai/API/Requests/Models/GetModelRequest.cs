using System.Threading.Tasks;
using Clarifai.DTOs.Models;
using Clarifai.DTOs.Predictions;
using Clarifai.Internal.GRPC;
using Google.Protobuf;

namespace Clarifai.API.Requests.Models
{
    /// <summary>
    /// A request for getting a model.
    /// </summary>
    /// <typeparam name="T">the model type</typeparam>
    public class GetModelRequest<T> : ClarifaiRequest<IModel<T>> where T : IPrediction
    {
        protected override RequestMethod Method => RequestMethod.GET;
        protected override string Url => "/v2/models/" + _modelID + "/output_info";

        private readonly string _modelID;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="modelID">the model ID</param>
        public GetModelRequest(IClarifaiHttpClient httpClient, string modelID) : base(httpClient)
        {
            _modelID = modelID;
        }

        /// <inheritdoc />
        protected override IModel<T> Unmarshaller(dynamic responseD)
        {
            SingleModelResponse response = responseD;
            return Model<T>.GrpcDeserialize(HttpClient, response.Model);
        }

        /// <inheritdoc />
        protected override async Task<IMessage> GrpcRequestBody(V2.V2Client grpcClient)
        {
            return await grpcClient.GetModelOutputInfoAsync(new GetModelRequest());
        }
    }
}
