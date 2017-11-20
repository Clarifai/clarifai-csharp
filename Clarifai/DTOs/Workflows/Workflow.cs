using System;

namespace Clarifai.DTOs.Workflows
{
    public class Workflow
    {
        public string ID { get; }
        public string AppID { get; }
        public DateTime CreatedAt { get; }

        private Workflow(string id, string appID, DateTime createdAt)
        {
            ID = id;
            AppID = appID;
            CreatedAt = createdAt;
        }

        public static Workflow Deserialize(dynamic jsonObject)
        {
            return new Workflow((string) jsonObject.id, (string) jsonObject.app_id,
                (DateTime) jsonObject.created_at);
        }
    }
}
