using Clarifai.API.Requests.Models;
using Clarifai.DTOs.Inputs;
using Clarifai.DTOs.Predictions;
using System;
using System.Collections.Generic;
using Clarifai.API;
using Clarifai.DTOs.Models.OutputsInfo;

namespace Clarifai.DTOs.Models
{
    /// <summary>
    /// Represents a model. Models are used to run predictions on inputs.
    /// </summary>
    public interface IModel
    {
        string AppID { get; }
        IClarifaiClient Client { get; }
        DateTime? CreatedAt { get; }
        string ModelID { get; }
        ModelVersion ModelVersion { get; }
        string Name { get; }
        IOutputInfo OutputInfo { get; }
    }

    /// <inheritdoc />
    /// <typeparam name="T">the model type</typeparam>
    public interface IModel<T> : IModel where T : IPrediction
    {
        /// <summary>
        /// A shorthand method for executing <see cref="DeleteModelVersionRequest"/> for this model.
        /// </summary>
        /// <returns>a new <see cref="DeleteModelVersionRequest"/></returns>
        DeleteModelVersionRequest DeleteModelVersion(string modelVersionID);

        /// <summary>
        /// A shorthand method for executing <see cref="GetModelInputsRequest"/> for this model.
        /// </summary>
        /// <returns>a new <see cref="GetModelInputsRequest"/></returns>
        GetModelInputsRequest GetModelInputs();

        /// <summary>
        /// A shorthand method for executing <see cref="GetModelVersionRequest"/> for this model.
        /// </summary>
        /// <returns>a new <see cref="GetModelVersionRequest"/></returns>
        GetModelVersionRequest GetModelVersion(string modelVersionID);

        /// <summary>
        /// A shorthand method for executing <see cref="GetModelVersionsRequest"/> for this model.
        /// </summary>
        /// <returns>a new <see cref="GetModelVersionsRequest"/></returns>
        GetModelVersionsRequest GetModelVersions();

        /// <summary>
        /// A shorthand method for executing <see cref="PredictRequest{T}"/> for this model.
        /// </summary>
        /// <param name="input">the input to run a prediction on</param>
        /// <param name="language">the language</param>
        /// <param name="minValue">
        /// only preditions with a value greater than or equal to to minValue will be returned
        /// </param>
        /// <param name="maxConcepts">
        /// the maximum maxConcepts number of predictions that will be returned
        /// </param>
        /// <param name="selectConcepts">only selectConcepts will be returned</param>
        /// <returns>a new PredictRequest instance</returns>
        PredictRequest<T> Predict(IClarifaiInput input, string language = null,
            decimal? minValue =  null, int? maxConcepts = null,
            IEnumerable<Concept> selectConcepts = null);

        /// <summary>
        /// A shorthand method for executing <see cref="PredictRequest{T}"/> for this model.
        /// </summary>
        /// <param name="inputs">the inputs to run a prediction on</param>
        /// <param name="language">the language</param>
        /// <param name="minValue">
        /// only preditions with a value greater than or equal to to minValue will be returned
        /// </param>
        /// <param name="maxConcepts">
        /// the maximum maxConcepts number of predictions that will be returned
        /// </param>
        /// <param name="selectConcepts">only selectConcepts will be returned</param>
        /// <returns>a new PredictRequest instance</returns>
        PredictRequest<T> Predict(IEnumerable<IClarifaiInput> inputs, string language = null,
            decimal? minValue =  null, int? maxConcepts = null,
            IEnumerable<Concept> selectConcepts = null);

        /// <summary>
        /// A shorthand method for executing <see cref="TrainModelRequest{T}"/> for this model.
        /// </summary>
        /// <returns>a new <see cref="TrainModelRequest{T}"/></returns>
        TrainModelRequest<T> TrainModel();
    }
}