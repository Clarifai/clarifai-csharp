using System;
using System.Net;
using Clarifai.Internal.GRPC.Status;

namespace Clarifai.DTOs
{
    /// <summary>
    /// Status of a request to the Clarifai API.
    /// </summary>
    public class ClarifaiStatus
    {
        /// <summary>
        /// Status type.
        /// </summary>
        public enum StatusType { Successful, MixedSuccess, Failure, NetworkError}

        public StatusType Type { get; }
        public int StatusCode { get; }
        public string Description { get; }
        public string ErrorDetails { get; }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="statusType">the status type</param>
        /// <param name="statusCode">the status code</param>
        /// <param name="description">the description</param>
        /// <param name="errorDetails">error details</param>
        public ClarifaiStatus(StatusType statusType, int statusCode, string description,
            string errorDetails)
        {
            Type = statusType;
            StatusCode = statusCode;
            Description = description;
            ErrorDetails = errorDetails;
        }

        /// <summary>
        /// Deserializes a new instance out of the dynamic JSON object.
        /// </summary>
        /// <param name="status">the JSON object</param>
        /// <param name="httpStatusCode">the http status code</param>
        /// <returns>a new instance of ClarifaiStatus</returns>
        public static ClarifaiStatus Deserialize(dynamic status,
            HttpStatusCode httpStatusCode = HttpStatusCode.OK)
        {
            int statusCode = (int)status.code;
            bool success = 200 <= (int)httpStatusCode && (int)httpStatusCode < 300;
            StatusType statusType;
            Console.WriteLine((int)httpStatusCode);
            if (success)
            {
                if (statusCode == 10010)
                {
                    statusType = StatusType.MixedSuccess;
                }
                else
                {
                    statusType = StatusType.Successful;
                }
            }
            else
            {
                statusType = StatusType.Failure;
            }

            string errorDetails = null;
            if (status.details != null)
            {
                errorDetails = status.details;
            }
            return new ClarifaiStatus(statusType, statusCode, (string)status.description,
                errorDetails);
        }

        public static ClarifaiStatus GrpcDeserialize(Status status,
            HttpStatusCode httpStatusCode = HttpStatusCode.OK)
        {
            int statusCode = (int)status.Code;
            bool success = 200 <= (int)httpStatusCode && (int)httpStatusCode < 300;
            StatusType statusType;

            if (success)
            {
                if (statusCode == 10010)
                {
                    statusType = StatusType.MixedSuccess;
                }
                else
                {
                    statusType = StatusType.Successful;
                }
            }
            else
            {
                statusType = StatusType.Failure;
            }

            string errorDetails = null;
            if (status.Details != null)
            {
                errorDetails = status.Details;
            }
            return new ClarifaiStatus(statusType, statusCode, status.Description, errorDetails);
        }

        public override string ToString()
        {
            return $"[ClarifaiStatus: {Type}]";
        }
    }
}
