using System;
using Clarifai.Internal.GRPC;
using Newtonsoft.Json.Linq;

namespace Clarifai.DTOs.Models.OutputsInfo
{
    public interface IOutputInfo
    {
        /// <summary>
        /// The type.
        /// </summary>
        string Type { get; }

        /// <summary>
        /// The type extension - it uniquely defines a model type.
        /// </summary>
        string TypeExt { get; }

        /// <summary>
        /// The message.
        /// </summary>
        string Message { get; }

        [Obsolete]
        JObject Serialize();

        OutputInfo GrpcSerialize();
    }
}
