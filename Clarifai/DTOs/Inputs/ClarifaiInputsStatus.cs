namespace Clarifai.DTOs.Inputs
{
    /// <summary>
    /// Returns the status of inputs processing.
    /// </summary>
    public class ClarifaiInputsStatus
    {
        /// <summary>
        /// Number of inputs that have been procesed.
        /// </summary>
        public int Processed { get; }

        /// <summary>
        /// Number of inputs that are yet to be processed.
        /// </summary>
        public int ToProcess { get; }

        /// <summary>
        /// Number of inputs that weren't able to be processed.
        /// </summary>
        public int Errors { get; }

        /// <summary>
        /// Number of inputs that are being processed.
        /// </summary>
        public int Processing { get; }

        /// <summary>
        /// Ctor.
        /// </summary>
        private ClarifaiInputsStatus(int processed, int toProcess, int errors, int processing)
        {
            Processed = processed;
            ToProcess = toProcess;
            Errors = errors;
            Processing = processing;
        }

        /// <summary>
        /// Deserializes the object out of a JSON dynamic object.
        /// </summary>
        /// <param name="jsonObject">the JSON dynamic object</param>
        /// <returns>the deserialized object</returns>
        public static ClarifaiInputsStatus Deserialize(dynamic jsonObject)
        {
            return new ClarifaiInputsStatus((int)jsonObject.processed, (int)jsonObject.to_process,
                (int)jsonObject.errors, (int)jsonObject.processing);
        }

        public override bool Equals(object obj)
        {
            return obj is ClarifaiInputsStatus status &&
                   Processed == status.Processed &&
                   ToProcess == status.ToProcess &&
                   Errors == status.Errors &&
                   Processing == status.Processing;
        }

        public override int GetHashCode()
        {
            var hashCode = -1374379752;
            hashCode = hashCode * -1521134295 + Processed.GetHashCode();
            hashCode = hashCode * -1521134295 + ToProcess.GetHashCode();
            hashCode = hashCode * -1521134295 + Errors.GetHashCode();
            hashCode = hashCode * -1521134295 + Processing.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return string.Format(
                "[ClarifaiInputsStatus: (Processed: {0}), (To Process: {1}), (Errors: {2}), " +
                "(Processing: {3})]",
                Processed, ToProcess, Errors, Processing);
        }
    }
}
