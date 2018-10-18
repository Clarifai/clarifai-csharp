using System;
using System.Collections.Generic;

namespace Clarifai.Solutions.Moderation.DTOs
{
    public class ModerationStatus
    {
        public int StatusCode { get; }
        public string Description { get; }
        public string InputID { get; }
        public List<ModerationDetail> ModerationDetails { get; }

        public ModerationStatus(int statusCode, string description, string inputId,
            List<ModerationDetail> moderationDetails)
        {
            StatusCode = statusCode;
            Description = description;
            InputID = inputId;
            ModerationDetails = moderationDetails;
        }

        public static ModerationStatus Deserialize(dynamic moderationStatus)
        {
            var details = new List<ModerationDetail>();
            if (moderationStatus.details != null)
            {
                foreach (dynamic detail in moderationStatus.details)
                {
                    details.Add(ModerationDetail.Deserialize(detail));
                }
            }

            return new ModerationStatus(
                (int)moderationStatus.code,
                (string)moderationStatus.description,
                (moderationStatus.input_id != null) ? (string)moderationStatus.input_id : null,
                details);
        }
    }
}
