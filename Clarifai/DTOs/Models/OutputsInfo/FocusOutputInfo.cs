namespace Clarifai.DTOs.Models.OutputsInfo
{
    /// <summary>
    /// Certain information regarding the Focus model.
    /// </summary>
    public class FocusOutputInfo : IOutputInfo
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
        private FocusOutputInfo(string type, string typeExt, string message)
        {
            Type = type;
            TypeExt = typeExt;
            Message = message;
        }

        public static FocusOutputInfo Deserialize(dynamic jsonObject)
        {
            return new FocusOutputInfo(
                (string) jsonObject.type,
                (string) jsonObject.type_ext,
                (string) jsonObject.message
            );
        }

        public override string ToString()
        {
            return "[FocusOutputInfo]";
        }
    }
}
