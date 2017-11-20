using System;
using Clarifai.API;
using Clarifai.DTOs.Models.OutputsInfo;
using Clarifai.DTOs.Predictions;

namespace Clarifai.DTOs.Models
{
    /// <summary>
    /// The video model runs predictions on a video.
    /// </summary>
    public class VideoModel : Model<Frame>
    {
        /// <summary>
        /// The output info.
        /// </summary>
        public new VideoOutputInfo OutputInfo => (VideoOutputInfo) base.OutputInfo;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="client">the Clarifai client</param>
        /// <param name="modelID">the model ID</param>
        /// <param name="name">the model name</param>
        /// <param name="createdAt">date & time of model creation</param>
        /// <param name="appID">the application ID</param>
        /// <param name="modelVersion">the model version</param>
        /// <param name="outputInfo">the output info</param>
        public VideoModel(IClarifaiClient client, string modelID, string name = null,
            DateTime? createdAt = null, string appID = null, ModelVersion modelVersion = null,
            VideoOutputInfo outputInfo = null)
            : base(client, modelID, name, createdAt, appID, modelVersion, outputInfo)
        { }

        /// <summary>
        /// Deserializes the JSON object to a new instance of this class.
        /// </summary>
        /// <param name="client">the Clarifai client</param>
        /// <param name="model">the JSON object</param>
        /// <returns>a new instance</returns>
        public new static VideoModel Deserialize(IClarifaiClient client, dynamic model)
        {
            return new VideoModel(
                client,
                (string)model.id,
                name: (string)model.name,
                createdAt: (DateTime)model.created_at,
                appID: (string)model.app_id,
                outputInfo: VideoOutputInfo.Deserialize(model.output_info),
                modelVersion: Models.ModelVersion.Deserialize(model.model_version));
        }

        public override string ToString()
        {
            return $"[VideoModel: (modelID: {ModelID}]";
        }
    }
}
