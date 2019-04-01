﻿using System;
using Clarifai.API;
using Clarifai.DTOs.Models.OutputsInfo;
using Clarifai.DTOs.Predictions;

namespace Clarifai.DTOs.Models
{
    /// <summary>
    /// The face concepts model finds regions where faces are detected, and associates concepts
    /// to those regions.
    /// </summary>
    public class FaceConceptsModel : Model<FaceConcepts>
    {
        /// <summary>
        /// The output info.
        /// </summary>
        public new FaceConceptsOutputInfo OutputInfo => (FaceConceptsOutputInfo) base.OutputInfo;

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
        public FaceConceptsModel(IClarifaiHttpClient httpClient, string modelID, string name = null,
            DateTime? createdAt = null, string appID = null, ModelVersion modelVersion = null,
            FaceConceptsOutputInfo outputInfo = null)
            : base(httpClient, modelID, name, createdAt, appID, modelVersion, outputInfo)
        { }

        /// <summary>
        /// Deserializes the JSON object to a new instance of this class.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="model">the JSON object</param>
        /// <returns>a new instance</returns>
        [Obsolete]
        public new static FaceConceptsModel Deserialize(IClarifaiHttpClient httpClient,
            dynamic model)
        {
            return new FaceConceptsModel(
                httpClient,
                (string)model.id,
                name: (string)model.name,
                createdAt: (DateTime)model.created_at,
                appID: (string)model.app_id,
                outputInfo: FaceConceptsOutputInfo.Deserialize(model.output_info),
                modelVersion: Models.ModelVersion.Deserialize(model.model_version));
        }

        /// <summary>
        /// Deserializes the gRPC object to a new instance of this class.
        /// </summary>
        /// <param name="httpClient">the HTTP client</param>
        /// <param name="model">the gRPC model object</param>
        /// <returns>a new instance</returns>
        public new static FaceConceptsModel GrpcDeserialize(IClarifaiHttpClient httpClient,
            Internal.GRPC.Model model)
        {
            return new FaceConceptsModel(
                httpClient,
                model.Id,
                name: model.Name,
                createdAt: model.CreatedAt?.ToDateTime(),
                appID: model.AppId,
                outputInfo: FaceConceptsOutputInfo.GrpcDeserialize(model.OutputInfo),
                modelVersion: ModelVersion.GrpcDeserialize(model.ModelVersion));
        }

        public override string ToString()
        {
            return $"[FaceConceptsModel: (modelID: {ModelID}]";
        }
    }
}