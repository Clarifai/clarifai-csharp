namespace Clarifai.Exceptions
{
    /// <summary>
    /// Exception that is thrown whenever Clairfai-type error is encountered.
    /// </summary>
    public class ClarifaiException : System.Exception
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="msg">the message</param>
        public ClarifaiException(string msg) : base(msg)
        { }
    }
}
