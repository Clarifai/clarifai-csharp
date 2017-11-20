namespace Clarifai.DTOs.Models.OutputsInfo
{
    /// <summary>
    /// Certain information regarding the FaceConcepts model.
    /// </summary>
    public class FaceConceptsOutputInfo : IOutputInfo
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
        private FaceConceptsOutputInfo(string type, string typeExt, string message)
        {
            Type = type;
            TypeExt = typeExt;
            Message = message;
        }

        public static FaceConceptsOutputInfo Deserialize(dynamic jsonObject)
        {
            return new FaceConceptsOutputInfo(
                (string) jsonObject.type,
                (string) jsonObject.type_ext,
                (string) jsonObject.message
            );
        }

        public override string ToString()
        {
            return "[FaceConceptsOutputInfo]";
        }
    }
}
