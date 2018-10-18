using Clarifai.DTOs.Predictions;

namespace Clarifai.Solutions.Moderation.DTOs
{
    public class ModerationDetail
    {
        public Concept Concept { get; }
        public int Code { get; }
        public string Description { get; }
        public decimal ThresholdMin { get; }
        public decimal ThresholdMax { get; }

        public ModerationDetail(Concept concept, int code, string description, decimal thresholdMin,
            decimal thresholdMax)
        {
            Concept = concept;
            Code = code;
            Description = description;
            ThresholdMin = thresholdMin;
            ThresholdMax = thresholdMax;
        }

        public static ModerationDetail Deserialize(dynamic moderationDetail)
        {
            return new ModerationDetail(
                Clarifai.DTOs.Predictions.Concept.Deserialize(moderationDetail.concept),
                (int)moderationDetail.code,
                (string)moderationDetail.description,
                (decimal)moderationDetail.threshold_min,
                (decimal)moderationDetail.threshold_max
            );
        }
    }
}
