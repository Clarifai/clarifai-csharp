using System;
using System.Collections.Generic;

namespace Clarifai.DTOs.Models
{
    /// <summary>
    /// Represents a specific model-version.
    /// </summary>
    public class ModelVersion
    {
        /// <summary>
        /// The ID of this model's version.
        /// </summary>
        public string ID { get; }

        /// <summary>
        /// Date & time of creation.
        /// </summary>
        public DateTime CreatedAt { get; }

        /// <summary>
        /// Model training status.
        /// </summary>
        public ModelTrainingStatus Status { get; }

        /// <summary>
        /// The number of active concepts.
        /// </summary>
        public int ActiveConceptCount { get; }

        /// <summary>
        /// The number of all inputs.
        /// </summary>
        public int TotalInputCount { get; }

        /// <summary>
        /// Model evaluation metrics status. Null if not available.
        /// </summary>
        public ModelMetricsStatus ModelMetricsStatus { get; }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="id">the model-version id</param>
        /// <param name="createdAt">the date & time of creation</param>
        /// <param name="status">the model training status</param>
        /// <param name="activeConceptCount">the active concept count</param>
        /// <param name="totalInputCount">the total input count</param>
        /// <param name="modelMetricsStatus">the model metrics status</param>
        private ModelVersion(string id, DateTime createdAt, ModelTrainingStatus status,
            int activeConceptCount = 0, int totalInputCount = 0,
            ModelMetricsStatus modelMetricsStatus = null)
        {
            ID = id;
            CreatedAt = createdAt;
            Status = status;
            ActiveConceptCount = activeConceptCount;
            TotalInputCount = totalInputCount;
            ModelMetricsStatus = modelMetricsStatus;
        }

        /// <summary>
        /// Deserializes the object out of a JSON dynamic object.
        /// </summary>
        /// <param name="jsonObject">the JSON dynamic object</param>
        /// <returns>the deserialized object</returns>
        [Obsolete]
        public static ModelVersion Deserialize(dynamic jsonObject)
        {
            int activeConceptCount = 0;
            if (jsonObject.active_concept_count != null)
            {
                activeConceptCount = (int) jsonObject.active_concept_count;
            }

            int totalInputCount = 0;
            if (jsonObject.total_input_count != null)
            {
                totalInputCount = (int) jsonObject.total_input_count;
            }

            ModelMetricsStatus modelMetricsStatus = null;
            if (jsonObject.metrics != null)
            {
                modelMetricsStatus = Models.ModelMetricsStatus.Deserialize(
                    jsonObject.metrics.status);
            }

            return new ModelVersion(
                (string) jsonObject.id,
                (DateTime) jsonObject.created_at,
                ModelTrainingStatus.Deserialize(jsonObject.status),
                activeConceptCount,
                totalInputCount,
                modelMetricsStatus);
        }

        /// <summary>
        /// Deserializes the object out of a gRPC object.
        /// </summary>
        /// <param name="modelVersion">the gRPC model version object</param>
        /// <returns>the deserialized object</returns>
        public static ModelVersion GrpcDeserialize(Internal.GRPC.ModelVersion modelVersion)
        {
            int activeConceptCount = Convert.ToInt32(modelVersion.ActiveConceptCount);

            int totalInputCount = Convert.ToInt32(modelVersion.TotalInputCount);

            ModelMetricsStatus modelMetricsStatus = null;
            if (modelVersion.Metrics != null)
            {
                modelMetricsStatus = ModelMetricsStatus.GrpcDeserialize(
                    modelVersion.Metrics.Status);
            }

            return new ModelVersion(
                modelVersion.Id,
                modelVersion.CreatedAt.ToDateTime(),
                ModelTrainingStatus.GrpcDeserialize(modelVersion.Status),
                activeConceptCount,
                totalInputCount,
                modelMetricsStatus);
        }

        public override bool Equals(object obj)
        {
            return obj is ModelVersion version &&
                   ID == version.ID &&
                   CreatedAt == version.CreatedAt &&
                   EqualityComparer<ModelTrainingStatus>.Default.Equals(Status, version.Status) &&
                   ActiveConceptCount == version.ActiveConceptCount &&
                   TotalInputCount == version.TotalInputCount &&
                   EqualityComparer<ModelMetricsStatus>.Default.Equals(ModelMetricsStatus,
                       version.ModelMetricsStatus);
        }

        public override int GetHashCode()
        {
            var hashCode = 469526008;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ID);
            hashCode = hashCode * -1521134295 + CreatedAt.GetHashCode();
            hashCode = hashCode * -1521134295 +
                       EqualityComparer<ModelTrainingStatus>.Default.GetHashCode(Status);
            hashCode = hashCode * -1521134295 + ActiveConceptCount.GetHashCode();
            hashCode = hashCode * -1521134295 + TotalInputCount.GetHashCode();
            hashCode = hashCode * -1521134295 +
                       EqualityComparer<ModelMetricsStatus>.Default.GetHashCode(ModelMetricsStatus);
            return hashCode;
        }

        public override string ToString()
        {
            return string.Format(
                "[ModelVersion: (ID: {0}, Status: {1}, CreatedAt: {2})]",
                ID, CreatedAt, Status);
        }
    }
}
