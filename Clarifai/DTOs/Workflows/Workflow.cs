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

        /// <summary>
        /// Deserializes the object out of a JSON dynamic object.
        /// </summary>
        /// <param name="jsonObject">the JSON object</param>
        /// <returns>the deserialized object</returns>
        public static Workflow Deserialize(dynamic jsonObject)
        {
            return new Workflow((string) jsonObject.id, (string) jsonObject.app_id,
                (DateTime) jsonObject.created_at);
        }

        public static Workflow GrpcDeserialize(Internal.GRPC.Workflow workflow)
        {
            return new Workflow(workflow.Id, workflow.AppId, workflow.CreatedAt.ToDateTime());
        }
    }
}
