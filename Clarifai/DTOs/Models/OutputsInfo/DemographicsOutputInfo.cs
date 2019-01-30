using System;
using System.Collections.Generic;
using Clarifai.Internal.GRPC;
using Newtonsoft.Json.Linq;
using Concept = Clarifai.DTOs.Predictions.Concept;

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
        /// The concepts.
        /// </summary>
        public IEnumerable<Concept> Concepts { get; }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="type">the type</param>
        /// <param name="typeExt">the type ext</param>
        /// <param name="message">the message</param>
        /// <param name="concepts"></param>
        private DemographicsOutputInfo(string type, string typeExt, string message,
            IEnumerable<Concept> concepts)
        {
            Type = type;
            TypeExt = typeExt;
            Message = message;
            Concepts = concepts;
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
                (string) jsonObject.message,
                new List<Concept>()
            );
        }

        public static DemographicsOutputInfo GrpcDeserialize(OutputInfo outputInfo)
        {
            List<Concept> concepts = null;
            if (outputInfo.Data?.Concepts != null)
            {
                concepts = new List<Concept>();
                foreach (var concept in outputInfo.Data.Concepts)
                {
                    concepts.Add(Concept.GrpcDeserialize(concept));
                }
            }
            return new DemographicsOutputInfo(
                outputInfo.Type,
                outputInfo.TypeExt,
                outputInfo.Message,
                concepts);
        }

        public override string ToString()
        {
            return "[DemographicsOutputInfo]";
        }
    }
}
