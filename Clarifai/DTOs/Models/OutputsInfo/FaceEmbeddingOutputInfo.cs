namespace Clarifai.DTOs.Models.OutputsInfo
{
    /// <summary>
    /// Certain information regarding the Embedding model.
    /// </summary>
    public class FaceEmbeddingOutputInfo : IOutputInfo
    {
        /// <inheritdoc />
        public string Type { get; }

        /// <inheritdoc />
        public string TypeExt { get; }

        /// <inheritdoc />
        public string Message { get; }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="type">the type</param>
        /// <param name="typeExt">the type ext</param>
        /// <param name="message">the message</param>
        private FaceEmbeddingOutputInfo(string type, string typeExt, string message)
        {
            Type = type;
            TypeExt = typeExt;
            Message = message;
        }

        public static FaceEmbeddingOutputInfo Deserialize(dynamic jsonObject)
        {
            return new FaceEmbeddingOutputInfo(
                (string) jsonObject.type,
                (string) jsonObject.type_ext,
                (string) jsonObject.message
            );
        }

        public override string ToString()
        {
            return "[FaceEmbeddingOutputInfo]";
        }
    }
}
