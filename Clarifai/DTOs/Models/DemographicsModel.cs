using System;
using Clarifai.API;
using Clarifai.DTOs.Models.OutputsInfo;
using Clarifai.DTOs.Predictions;
using System.Collections.Generic;

namespace Clarifai.DTOs.Models
{
    /// <summary>
    /// The demographics model finds faces and their demographics appearances.
    /// </summary>
    public class DemographicsModel : Model<Demographics>
    {
        /// <summary>
        /// The output info.
        /// </summary>
        public new DemographicsOutputInfo OutputInfo => (DemographicsOutputInfo) base.OutputInfo;

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
        public DemographicsModel(IClarifaiHttpClient httpClient, string modelID, string name = null,
            DateTime? createdAt = null, string appID = null, ModelVersion modelVersion = null,
            DemographicsOutputInfo outputInfo = null)
            : base(httpClient, modelID, name, createdAt, appID, modelVersion, outputInfo)
        { }

        /// <summary>
        /// Deserializes the JSON object to a new instance of this class.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="model">the JSON object</param>
        /// <returns>a new instance</returns>
        public new static DemographicsModel Deserialize(IClarifaiHttpClient httpClient,
            dynamic model)
        {
            return new DemographicsModel(
                httpClient,
                (string)model.id,
                name: (string)model.name,
                createdAt: (DateTime)model.created_at,
                appID: (string)model.app_id,
                outputInfo: DemographicsOutputInfo.Deserialize(model.output_info),
                modelVersion: Models.ModelVersion.Deserialize(model.model_version));
        }

        public override string ToString()
        {
            return $"[DemographicsModel: (modelID: {ModelID}]";
        }
    }
}
