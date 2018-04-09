using Clarifai.Exceptions;
using System.Collections.Generic;

namespace Clarifai.DTOs.Models
{
    /// <summary>
    /// Represents the current status of a model.
    /// </summary>
    public class ModelTrainingStatus
    {
        public int StatusCode { get; }
        public string Description { get; }

        /// <summary>
        /// This model has been trained.
        /// </summary>
        public static ModelTrainingStatus Trained => new ModelTrainingStatus(21100);

        /// <summary>
        /// This model is currently being trained by the server.
        /// </summary>
        public static ModelTrainingStatus TrainingInProgress => new ModelTrainingStatus(21101);

        /// <summary>
        /// This model hasn't been trained. Use {@link ClarifaiClient#trainModel(String)} or
        /// {@link Model#train()} to train it.
        /// </summary>
        public static ModelTrainingStatus NotYetTrained => new ModelTrainingStatus(21102);

        /// <summary>
        /// This model is in the queue to be trained by the server.
        /// </summary>
        public static ModelTrainingStatus TrainingQueued => new ModelTrainingStatus(21103);

        /// <summary>
        /// Model training had no data.
        /// </summary>
        public static ModelTrainingStatus ModelTrainingNoData => new ModelTrainingStatus(21110);

        /// <summary>
        /// There are no positive examples for this model, so it cannot be trained. Please add at
        /// least one positive example for each of the model's concepts before trying to train it.
        /// </summary>
        public static ModelTrainingStatus NoPositiveExamples => new ModelTrainingStatus(21111);

        /// <summary>
        /// Custom model training was ONE_VS_N but with a single class.
        /// </summary>
        public static ModelTrainingStatus ModelTrainingOneVsNSingleClass =>
            new ModelTrainingStatus(21112);

        /// <summary>
        /// Training took longer than the server allows.
        /// </summary>
        public static ModelTrainingStatus ModelTrainingTimedOut => new ModelTrainingStatus(21113);

        /// <summary>
        /// Training got error waiting on asset pipeline to finish.
        /// </summary>
        public static ModelTrainingStatus ModelTrainingWaitingError =>
            new ModelTrainingStatus(21114);

        /// <summary>
        /// Training threw an unknown error.
        /// </summary>
        public static ModelTrainingStatus ModelTrainingUnknownError =>
            new ModelTrainingStatus(21115);

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="statusCode">the status code</param>
        /// <param name="description">the description</param>
        private ModelTrainingStatus(int statusCode, string description = "")
        {
            StatusCode = statusCode;
            Description = description;
        }

        /// <summary>
        /// Returns true if status is an error.
        /// </summary>
        /// <returns>is an error</returns>
        public bool IsError()
        {
            return 21110 <= StatusCode && StatusCode <= 21119;
        }

        /// <summary>
        /// Returns true if the training has stopped.
        /// </summary>
        /// <returns>has training stopped</returns>
        public bool IsTerminalEvent()
        {
            return IsError() || StatusCode == Trained.StatusCode;
        }

        /// <summary>
        /// Deserializes the object out of a JSON dynamic object.
        /// </summary>
        /// <param name="jsonObject">the JSON dynamic object</param>
        /// <returns>the deserialized object</returns>
        public static ModelTrainingStatus Deserialize(dynamic jsonObject)
        {
            int statusCode = (int)jsonObject.code;
            if (21100 <= statusCode && statusCode <= 21103 ||
                21110 <= statusCode && statusCode <= 21115)
            {
                return new ModelTrainingStatus(statusCode, (string)jsonObject.description);
            }
            else
            {
                throw new ClarifaiException(
                    "This version of the API client does not recognize the model training status: "
                    + statusCode);
            }
        }

        public override bool Equals(object obj)
        {
            return obj is ModelTrainingStatus status &&
                   StatusCode == status.StatusCode;
        }

        public override int GetHashCode()
        {
            return -763886418 + StatusCode.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("[ModelTrainingStatus: (StatusCode: {0}, Description: {1})]",
                StatusCode, Description);
        }
    }
}
