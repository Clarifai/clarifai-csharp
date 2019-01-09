using System;
using System.Collections.Generic;
using Clarifai.Internal.GRPC;

namespace Clarifai.DTOs.Feedbacks
{
    /// <summary>
    /// Feedback for the originally predicted region.
    /// </summary>
    public class Feedback
    {
        /// <summary>
        /// Predicted region was accurate.
        /// </summary>
        public static Feedback Accurate => new Feedback("accurate");

        /// <summary>
        /// Predicted region's coordinates should be modified.
        /// </summary>
        public static Feedback Misplaced => new Feedback("misplaced");

        /// <summary>
        /// The region was not originally detected by the model.
        /// </summary>
        public static Feedback NotDetected => new Feedback("not_detected");

        /// <summary>
        /// The region should not have been detected by the model.
        /// </summary>
        public static Feedback FalsePositive => new Feedback("false_positive");

        /// <summary>
        /// The modification value.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="value">the feedback value</param>
        private Feedback(string value)
        {
            Value = value;
        }

        public RegionInfoFeedback GrpcSerialize()
        {
            switch (Value)
            {
                case "accurate":
                    return RegionInfoFeedback.Accurate;
                case "misplaced":
                    return RegionInfoFeedback.Misplaced;
                case "not_detected":
                    return RegionInfoFeedback.NotDetected;
                case "false_positive":
                    return RegionInfoFeedback.FalsePositive;
                default:
                    throw new NotImplementedException("Not implemented");
            }
        }

        public override bool Equals(object obj)
        {
            return obj is Feedback feedback &&
                   Value == feedback.Value;
        }

        public override int GetHashCode()
        {
            return -1937169414 + EqualityComparer<string>.Default.GetHashCode(Value);
        }

        public override string ToString()
        {
            return $"[Feedback: (value: {Value})]";
        }
    }
}
