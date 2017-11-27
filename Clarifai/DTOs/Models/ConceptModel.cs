using System;
using System.Collections.Generic;
using System.Linq;
using Clarifai.API;
using Clarifai.API.Requests.Models;
using Clarifai.DTOs.Models.OutputsInfo;
using Clarifai.DTOs.Predictions;
using Newtonsoft.Json.Linq;

namespace Clarifai.DTOs.Models
{
    /// <summary>
    /// The concept model associates inputs with concepts. Users can train their own
    /// concept models and use them for prediction.
    /// </summary>
    public class ConceptModel : Model<Concept>
    {
        /// <summary>
        /// The output info.
        /// </summary>
        public new ConceptOutputInfo OutputInfo => (ConceptOutputInfo) base.OutputInfo;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="client">the Clarifai client</param>
        /// <param name="modelID">the model ID</param>
        /// <param name="name">the model name</param>
        /// <param name="createdAt">date & time of model creation</param>
        /// <param name="appID">the application ID</param>
        /// <param name="modelVersion">the model version</param>
        /// <param name="outputInfo">the output info</param>
        public ConceptModel(IClarifaiClient client, string modelID, string name = null,
            DateTime? createdAt = null, string appID = null, ModelVersion modelVersion = null,
            ConceptOutputInfo outputInfo = null)
            : base(client, modelID, name, createdAt, appID, modelVersion, outputInfo)
        { }

        /// <summary>
        /// A shorthand method for executing <see cref="ModifyModelRequest"/> for this model..
        /// </summary>
        /// <param name="action">the modification action</param>
        /// <param name="name">the new model name</param>
        /// <param name="concepts">the concepts update the model with</param>
        /// <param name="areConceptsMutuallyExclusive">are concepts mutually exclusive</param>
        /// <param name="isEnvironmentClosed">is environment closed</param>
        /// <param name="language">the language</param>
        public ModifyModelRequest Modify(ModifyAction action = null, string name = null,
            IEnumerable<Concept> concepts = null, bool? areConceptsMutuallyExclusive = null,
            bool? isEnvironmentClosed = null, string language = null)
        {
            return new ModifyModelRequest(Client, ModelID, action, name, concepts,
                areConceptsMutuallyExclusive, isEnvironmentClosed, language);
        }

        /// <summary>
        /// Serializes the instance to a new JSON object.
        /// </summary>
        /// <returns>a JSON object</returns>
        public JObject Serialize(IEnumerable<Concept> concepts,
            bool? areConceptsMutuallyExclusive = null, bool? isEnvironmentClosed = null,
            string language = null)
        {
            var body = new JObject(new JProperty("id", ModelID));
            if (Name != null)
            {
                body.Add("name", Name);
            }
            if (AppID != null)
            {
                body.Add("app_id", AppID);
            }
            if (CreatedAt != null)
            {
                body.Add("created_at", CreatedAt);
            }

            var outputInfo = new JObject();
            if (concepts != null)
            {
                outputInfo.Add("data", new JObject(
                    new JProperty("concepts",
                        new JArray(concepts.Select(c => c.Serialize())))));
            }

            var outputConfig = new JObject();
            if (areConceptsMutuallyExclusive != null)
            {
                outputConfig.Add("concepts_mutually_exclusive", areConceptsMutuallyExclusive);
            }
            if (isEnvironmentClosed != null)
            {
                outputConfig.Add("closed_environment", isEnvironmentClosed);
            }
            if (language != null)
            {
                outputConfig.Add("language", language);
            }

            if (outputConfig.Count > 0)
            {
                outputInfo.Add("output_config", outputConfig);
            }

            if (outputInfo.Count > 0)
            {
                body.Add("output_info", outputInfo);
            }
            return body;
        }

        /// <summary>
        /// Deserializes the JSON object to a new instance of this class.
        /// </summary>
        /// <param name="client">the Clarifai client</param>
        /// <param name="model">the JSON object</param>
        /// <returns>a new instance</returns>
        public new static ConceptModel Deserialize(IClarifaiClient client, dynamic model)
        {
            return new ConceptModel(
                client,
                (string)model.id,
                name: (string)model.name,
                createdAt: (DateTime)model.created_at,
                appID: (string)model.app_id,
                outputInfo: ConceptOutputInfo.Deserialize(model.output_info),
                modelVersion: Models.ModelVersion.Deserialize(model.model_version));
        }

        public override string ToString()
        {
            return $"[ConceptModel: (modelID: {ModelID}]";
        }
    }
}
