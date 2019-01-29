﻿using System;
using Clarifai.Internal.GRPC;
using Newtonsoft.Json.Linq;

namespace Clarifai.DTOs.Models.OutputsInfo
{
    /// <summary>
    /// Certain information regarding the Demographics model.
    /// </summary>
    public class DemographicsOutputInfo : IOutputInfo
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
        private DemographicsOutputInfo(string type, string typeExt, string message)
        {
            Type = type;
            TypeExt = typeExt;
            Message = message;
        }

        [Obsolete]
        public JObject Serialize()
        {
            return new JObject();
        }

        public OutputInfo GrpcSerialize()
        {
            return new OutputInfo();
        }

        [Obsolete]
        public static DemographicsOutputInfo Deserialize(dynamic jsonObject)
        {
            return new DemographicsOutputInfo(
                (string) jsonObject.type,
                (string) jsonObject.type_ext,
                (string) jsonObject.message
            );
        }

        public override string ToString()
        {
            return "[DemographicsOutputInfo]";
        }

        public static DemographicsOutputInfo GrpcDeserialize(OutputInfo outputInfo)
        {
            return new DemographicsOutputInfo(
                outputInfo.Type, outputInfo.TypeExt, outputInfo.Message);
        }
    }
}
