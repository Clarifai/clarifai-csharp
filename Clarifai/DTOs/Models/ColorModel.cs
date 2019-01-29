using System;
using Clarifai.API;
using Clarifai.DTOs.Models.OutputsInfo;
using Clarifai.DTOs.Predictions;
using System.Collections.Generic;

namespace Clarifai.DTOs.Models
{
    /// <summary>
    /// The color model associates inputs with the dominant colors.
    /// </summary>
    public class ColorModel : Model<Color>
    {
        /// <summary>
        /// The output info.
        /// </summary>
        public new ColorOutputInfo OutputInfo => (ColorOutputInfo) base.OutputInfo;

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
        public ColorModel(IClarifaiHttpClient httpClient, string modelID, string name = null,
            DateTime? createdAt = null, string appID = null, ModelVersion modelVersion = null,
            ColorOutputInfo outputInfo = null)
            : base(httpClient, modelID, name, createdAt, appID, modelVersion, outputInfo)
        {
        }

        /// <summary>
        /// Deserializes the JSON object to a new instance of this class.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="model">the JSON object</param>
        /// <returns>a new instance</returns>
        [Obsolete]
        public new static ColorModel Deserialize(IClarifaiHttpClient httpClient, dynamic model)
        {
            return new ColorModel(
                httpClient,
                (string)model.id,
                name: (string)model.name,
                createdAt: (DateTime)model.created_at,
                appID: (string)model.app_id,
                outputInfo: ColorOutputInfo.Deserialize(model.output_info),
                modelVersion: Models.ModelVersion.Deserialize(model.model_version));
        }


        /// <summary>
        /// Deserializes the gRPC object to a new instance of this class.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="model">the gRPC model object</param>
        /// <returns>a new instance</returns>
        public new static ColorModel GrpcDeserialize(IClarifaiHttpClient httpClient,
            Internal.GRPC.Model model)
        {
            return new ColorModel(
                httpClient,
                model.Id,
                name: model.Name,
                createdAt: model.CreatedAt?.ToDateTime(),
                appID: model.AppId,
                outputInfo: ColorOutputInfo.GrpcDeserialize(model.OutputInfo),
                modelVersion: ModelVersion.GrpcDeserialize(model.ModelVersion));
        }

        public override bool Equals(object obj)
        {
            return obj is ColorModel model &&
                   base.Equals(obj) &&
                   EqualityComparer<ColorOutputInfo>.Default.Equals(OutputInfo, model.OutputInfo);
        }

        public override int GetHashCode()
        {
            var hashCode = -678022824;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<ColorOutputInfo>.Default.GetHashCode(OutputInfo);
            return hashCode;
        }

        public override string ToString()
        {
            return $"[ColorModel: (modelID: {ModelID}]";
        }
    }
}
