using System;
using Clarifai.API;
using Clarifai.DTOs.Models.OutputsInfo;
using Clarifai.DTOs.Predictions;

namespace Clarifai.DTOs.Models
{
    /// <summary>
    /// The face detection model finds regions where faces are detected.
    /// </summary>
    public class FaceDetectionModel : Model<FaceDetection>
    {
        /// <summary>
        /// The output info.
        /// </summary>
        public new FaceDetectionOutputInfo OutputInfo => (FaceDetectionOutputInfo) base.OutputInfo;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="modelID">the model ID</param>
        /// <param name="name">the model name</param>
        /// <param name="createdAt">date & time of model creation</param>
        /// <param name="appID">the application ID</param>
        /// <param name="modelVersion">the model version</param>
        /// <param name="outputInfo">the output info</param>
        public FaceDetectionModel(IClarifaiHttpClient httpClient, string modelID,
            string name = null, DateTime? createdAt = null, string appID = null,
            ModelVersion modelVersion = null, FaceDetectionOutputInfo outputInfo = null)
            : base(httpClient, modelID, name, createdAt, appID, modelVersion, outputInfo)
        { }

        /// <summary>
        /// Deserializes the JSON object to a new instance of this class.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="model">the JSON object</param>
        /// <returns>a new instance</returns>
        [Obsolete]
        public new static FaceDetectionModel Deserialize(IClarifaiHttpClient httpClient,
            dynamic model)
        {
            return new FaceDetectionModel(
                httpClient,
                (string)model.id,
                name: (string)model.name,
                createdAt: (DateTime)model.created_at,
                appID: (string)model.app_id,
                outputInfo: FaceDetectionOutputInfo.Deserialize(model.output_info),
                modelVersion: Models.ModelVersion.Deserialize(model.model_version));
        }

        /// <summary>
        /// Deserializes the gRPC object to a new instance of this class.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="model">the gRPC model object</param>
        /// <returns>a new instance</returns>
        public new static FaceDetectionModel GrpcDeserialize(IClarifaiHttpClient httpClient,
            Internal.GRPC.Model model)
        {
            return new FaceDetectionModel(
                httpClient,
                model.Id,
                name: model.Name,
                createdAt: model.CreatedAt?.ToDateTime(),
                appID: model.AppId,
                outputInfo: FaceDetectionOutputInfo.GrpcDeserialize(model.OutputInfo),
                modelVersion: ModelVersion.GrpcDeserialize(model.ModelVersion));
        }

        public override string ToString()
        {
            return $"[FaceDetectionModel: (modelID: {ModelID}]";
        }
    }
}
