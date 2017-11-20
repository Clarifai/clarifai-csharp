using System;
using Clarifai.Exceptions;
using System.Collections.Generic;

namespace Clarifai.DTOs.Models
{
    /// <summary>
    /// Represents the current status of a model.
    /// </summary>
    public class ModelMetricsStatus
    {
        public int StatusCode { get; }
        public string Description { get; }

        /// <summary>
        /// Model was successfully evaluated.
        /// </summary>
        public static ModelMetricsStatus ModelEvaluated => new ModelMetricsStatus(21300);

        /// <summary>
        /// Model is evaluating.
        /// </summary>
        public static ModelMetricsStatus ModelEvaluating => new ModelMetricsStatus(21301);

        /// <summary>
        /// Model evaluation has not yet been run.
        /// </summary>
        public static ModelMetricsStatus ModelNotEvaluated => new ModelMetricsStatus(21302);

        /// <summary>
        /// Model is queued for evaluation.
        /// </summary>
        public static ModelMetricsStatus ModelQueuedForEvaluation => new ModelMetricsStatus(21303);

        /// <summary>
        /// Model evaluation timed out.
        /// </summary>
        public static ModelMetricsStatus ModelEvaluationTimedOut => new ModelMetricsStatus(21310);

        /// <summary>
        /// Model evaluation timed out waiting on inputs to process.
        /// </summary>
        public static ModelMetricsStatus ModelEvaluationWaitingError =>
            new ModelMetricsStatus(21311);

        /// <summary>
        /// Model evaluation unknown internal error.
        /// </summary>
        public static ModelMetricsStatus ModelEvaluationUnknownError =>
            new ModelMetricsStatus(21312);

        /// <summary>
        /// Model evaluation failed because there are not enough annotated inputs. Please
        /// have at least 2 concepts in your model with 5 labelled inputs each before evaluating.
        /// </summary>
        public static ModelMetricsStatus ModelEvaluationNeedLabels => new ModelMetricsStatus(21315);

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="statusCode">the status code</param>
        /// <param name="description">the description</param>
        private ModelMetricsStatus(int statusCode, string description = "")
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
            return 21310 <= StatusCode && StatusCode <= 21319;
        }

        /// <summary>
        /// Returns true if the model evaluation has stopped.
        /// </summary>
        /// <returns>has model evaluation stopped</returns>
        public bool IsTerminalEvent()
        {
            return IsError() || StatusCode == ModelEvaluated.StatusCode;
        }

        /// <summary>
        /// Deserializes the object out of a JSON dynamic object.
        /// </summary>
        /// <param name="jsonObject">the JSON dynamic object</param>
        /// <returns>the deserialized object</returns>
        public static ModelMetricsStatus Deserialize(dynamic jsonObject)
        {
            int statusCode = (int) jsonObject.code;
            if (21300 <= statusCode && statusCode <= 21303 ||
                21310 <= statusCode && statusCode <= 21319)
            {
                return new ModelMetricsStatus(statusCode, (string) jsonObject.description);
            }
            else
            {
                throw new ClarifaiException(
                    "This version of the API client does not recognize the model metrics status: "
                    + statusCode);
            }
        }

        public override bool Equals(object obj)
        {
            var status = obj as ModelMetricsStatus;
            return status != null &&
                   StatusCode == status.StatusCode;
        }

        public override int GetHashCode()
        {
            return -763886418 + StatusCode.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("[ModelMetricsStatus: (StatusCode: {0}, Description: {1})]",
                StatusCode, Description);
        }
    }
}
