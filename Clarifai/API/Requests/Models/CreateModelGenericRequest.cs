using System.Threading.Tasks;
using Clarifai.DTOs.Models;
using Clarifai.DTOs.Models.OutputsInfo;
using Clarifai.DTOs.Predictions;
using Clarifai.Internal.GRPC;
using Google.Protobuf;
using Model = Clarifai.Internal.GRPC.Model;

namespace Clarifai.API.Requests.Models
{
    /// <summary>
    /// A request for creating new models of all types.
    /// </summary>
    public class CreateModelGenericRequest<T> : ClarifaiRequest<IModel<T>> where T : IPrediction
    {
        protected override RequestMethod Method => RequestMethod.POST;
        protected override string Url => "/v2/models/";

        private readonly string _modelID;
        private readonly string _name;
        private readonly IOutputInfo _outputInfo;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="modelID">the model ID</param>
        /// <param name="name">the model name</param>
        /// <param name="outputInfo"></param>
        public CreateModelGenericRequest(IClarifaiHttpClient httpClient, string modelID,
            string name = null, IOutputInfo outputInfo = null) : base(httpClient)
        {
            _modelID = modelID;
            _name = name;
            _outputInfo = outputInfo;
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
            Model model = new Model
            {
                Id = _modelID
            };

            if (_name != null)
            {
                model = new Model(model)
                {
                    Name = _name
                };
            }

            if (_outputInfo != null)
            {
                model = new Model(model)
                {
                    OutputInfo = _outputInfo.GrpcSerialize()
                };
            }

            return await grpcClient.PostModelsAsync(new PostModelsRequest
            {
                Model = model
            });
        }
    }
}
