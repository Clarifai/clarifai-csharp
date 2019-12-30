using System.Collections.Generic;
using Clarifai.API.Requests.Concepts;
using Clarifai.API.Requests.Inputs;
using Clarifai.API.Requests.Models;
using Clarifai.DTOs.Inputs;
using Clarifai.DTOs.Models;
using Clarifai.DTOs.Models.OutputsInfo;
using Clarifai.DTOs.Predictions;
using Clarifai.DTOs.Searches;
using Newtonsoft.Json.Linq;

namespace Clarifai.API
{
    /// <summary>
    /// An interface containing all of the methods that are used to make requests to
    /// the Clarifai API.
    /// </summary>
    public interface IClarifaiClient
    {
        /// <summary>
        /// The default models available to be used for running predictions on inputs.
        /// </summary>
        PublicModels PublicModels { get; }

        Solutions.Solutions Solutions { get; }

        IClarifaiHttpClient HttpClient { get; }

        /// <summary>
        /// Gets concepts.
        /// </summary>
        /// <returns>a new GetConceptsRequest instance</returns>
        GetConceptsRequest GetConcepts();

        /// <summary>
        /// Gets concept.
        /// </summary>
        /// <returns>a new GetConceptRequest instance</returns>
        GetConceptRequest GetConcept(string conceptID);

        /// <summary>
        /// Adds concepts.
        /// </summary>
        /// <param name="concepts">the concepts</param>
        /// <returns>a new AddConceptsRequest instance</returns>
        AddConceptsRequest AddConcepts(params Concept[] concepts);

        /// <summary>
        /// Adds concepts.
        /// </summary>
        /// <param name="concepts">the concepts</param>
        /// <returns>a new AddConceptsRequest instance</returns>
        AddConceptsRequest AddConcepts(IEnumerable<Concept> concepts);

        /// <summary>
        /// Modifies concepts.
        /// </summary>
        /// <param name="concepts">the concepts</param>
        /// <returns>a new ModifyConceptsRequest instance</returns>
        ModifyConceptsRequest ModifyConcepts(params Concept[] concepts);

        /// <summary>
        /// Modifies concepts.
        /// </summary>
        /// <param name="concepts">the concepts</param>
        /// <returns>a new ModifyConceptsRequest instance</returns>
        ModifyConceptsRequest ModifyConcepts(IEnumerable<Concept> concepts);

        /// <summary>
        /// Searches for concepts.
        /// </summary>
        /// <param name="query">the query used in search</param>
        /// <param name="language">the language</param>
        /// <returns>a new SearchConceptsRequest instance</returns>
        SearchConceptsRequest SearchConcepts(string query, string language = null);

        /// <summary>
        /// Gets inputs.
        /// </summary>
        /// <returns>a new GetInputsRequest instance</returns>
        GetInputsRequest GetInputs();

        /// <summary>
        /// Gets input.
        /// </summary>
        /// <returns>a new GetInputRequest instance</returns>
        GetInputRequest GetInput(string inputID);

        /// <summary>
        /// Adds inputs.
        /// </summary>
        /// <param name="inputs">the inputs</param>
        /// <returns>a new AddInputsRequest instance</returns>
        AddInputsRequest AddInputs(params IClarifaiInput[] inputs);

        /// <summary>
        /// Adds inputs.
        /// </summary>
        /// <param name="inputs">the inputs</param>
        /// <returns>a new AddInputsRequest instance</returns>
        AddInputsRequest AddInputs(IEnumerable<IClarifaiInput> inputs);

        /// <summary>
        /// Modifies an input.
        /// </summary>
        /// <param name="inputID">the input ID</param>
        /// <param name="action">the action</param>
        /// <param name="positiveConcepts">the concepts associated with the input</param>
        /// <param name="negativeConcepts">the concepts not associated with the input</param>
        /// <returns>a new ModifyInputRequest instance</returns>
        ModifyInputRequest ModifyInput(string inputID, ModifyAction action,
            IEnumerable<Concept> positiveConcepts = null,
            IEnumerable<Concept> negativeConcepts = null);

        /// <summary>
        /// Sets the given metadata to this input's metadata.
        ///
        /// The keys in the new metadata are parsed depth-first, and the existing metadata is
        /// checked for a conflicting key at that location.
        ///
        /// If the existing metadata does not have a key that conflicts with an entry in the new
        /// metadata, that new entry is added to the existing metadata.
        ///
        /// If the existing metadata DOES have a key that conflicts with an entry in the new
        /// metadata:
        /// - If the existing and new values are of different types(primitive vs list vs
        ///   dictionary), the new value will overwrite the existing value;
        /// - Otherwise, if the existing and new values are both primitives or both lists, the new
        ///   value will overwrite the existing value;
        /// - Otherwise, both the existing and new value must be dictionaries, and the new
        ///   dictionary will be merged into the existing dictionary, with conflicts being resolved
        ///   in the same manner described above.
        /// </summary>
        /// <param name="inputID">the input ID</param>
        /// <param name="metadata">the new input's metadata</param>
        /// <returns>a new ModifyInputRequest instance</returns>
        ModifyInputMetadataRequest ModifyInputMetadata(string inputID, JObject metadata);

        /// <summary>
        /// Deletes inputs.
        /// </summary>
        /// <param name="inputIDs">the input IDs</param>
        /// <returns>a new DeleteInputsRequest instance</returns>
        DeleteInputsRequest DeleteInputs(params string[] inputIDs);

        /// <summary>
        /// Deletes inputs.
        /// </summary>
        /// <param name="inputIDs">the input IDs</param>
        /// <returns>a new DeleteInputsRequest instance</returns>
        DeleteInputsRequest DeleteInputs(IEnumerable<string> inputIDs);


        /// <summary>
        /// Deletes all inputs.
        /// </summary>
        /// <returns>a new DeleteAllInputsRequest instance</returns>
        DeleteAllInputsRequest DeleteAllInputs();

        /// <summary>
        /// If you add inputs in bulk, they will process in the background. With this method you
        /// retrieve all inputs' status.
        /// </summary>
        /// <returns>a new GetInputsStatusRequest instance</returns>
        GetInputsStatusRequest GetInputsStatus();

        /// <summary>
        /// Runs a prediction on an input using a certain <see cref="Model{T}"/>.
        /// </summary>
        /// <typeparam name="T">the model type</typeparam>
        /// <param name="modelID">the model ID</param>
        /// <param name="input">the input to run predictions on</param>
        /// <param name="modelVersionID">the model version ID - leave null for latest</param>
        /// <param name="language">the language</param>
        /// <param name="minValue">
        /// only predictions with a value greater than or equal to to minValue will be returned
        /// </param>
        /// <param name="maxConcepts">
        /// the maximum maxConcepts number of predictions that will be returned
        /// </param>
        /// <param name="selectConcepts">only selectConcepts will be returned</param>
        /// <param name="sampleMs">video frame prediction every [sampleMs] milliseconds</param>
        /// <returns>a new PredictionRequest instance</returns>
        PredictRequest<T> Predict<T>(string modelID, IClarifaiInput input,
            string modelVersionID = null, string language = null, decimal? minValue = null,
            int? maxConcepts = null, IEnumerable<Concept> selectConcepts = null,
            int? sampleMs = null)
            where T : IPrediction;

        /// <summary>
        /// Runs a prediction on multiple inputs using a certain <see cref="Model{T}"/>.
        /// </summary>
        /// <typeparam name="T">the model type</typeparam>
        /// <param name="modelID">the model ID</param>
        /// <param name="inputs">the inputs to run predictions on</param>
        /// <param name="modelVersionID">the model version ID - leave null for latest</param>
        /// <param name="language">the language</param>
        /// <param name="minValue">
        /// only predictions with a value greater than or equal to to minValue will be returned
        /// </param>
        /// <param name="maxConcepts">
        /// the maximum maxConcepts number of predictions that will be returned
        /// </param>
        /// <param name="selectConcepts">only selectConcepts will be returned</param>
        /// <returns>a new PredictionRequest instance</returns>
        BatchPredictRequest<T> Predict<T>(string modelID, IEnumerable<IClarifaiInput> inputs,
            string modelVersionID = null, string language = null, decimal? minValue = null,
            int? maxConcepts = null, IEnumerable<Concept> selectConcepts = null)
            where T : IPrediction;

        /// <summary>
        /// Using workflows, you can predict using multiple models with one request.
        /// The latency that would otherwise be required for each model predict request is with
        /// workflow predict reduced to a latency of one request.
        /// </summary>
        /// <param name="workflowID">the workflow ID</param>
        /// <param name="input">the input to run a prediction on</param>
        /// <param name="minValue">return only results that have at least this value</param>
        /// <param name="maxConcepts">the maximum number of concepts to return</param>
        /// <returns>a new WorkflowPredictRequest instance</returns>
        WorkflowPredictRequest WorkflowPredict(string workflowID, IClarifaiInput input,
            decimal? minValue = null, int? maxConcepts = null);

        /// <summary>
        /// Using workflows, you can predict using multiple models with one request.
        /// The latency that would otherwise be required for each model predict request is with
        /// workflow predict reduced to a latency of one request.
        /// </summary>
        /// <param name="workflowID">the workflow ID</param>
        /// <param name="inputs">the inputs to run predictions on</param>
        /// <param name="minValue">return only results that have at least this value</param>
        /// <param name="maxConcepts">the maximum number of concepts to return</param>
        /// <returns>a new WorkflowPredictRequest instance</returns>
        WorkflowBatchPredictRequest WorkflowPredict(string workflowID,
            IEnumerable<IClarifaiInput> inputs, decimal? minValue = null, int? maxConcepts = null);

        /// <summary>
        /// Gets all models.
        /// </summary>
        /// <returns>a new GetModelsRequest instance</returns>
        GetModelsRequest GetModels();

        /// <summary>
        /// Retrieves an instance of a <see cref="Model{T}"/> using model ID.
        /// </summary>
        /// <typeparam name="T">the model type</typeparam>
        /// <param name="modelID">the model ID</param>
        /// <param name="modelVersionID">the model version ID (optional) - if skipped, the latest
        /// model version data will be retrieved</param>
        /// <returns>a new GetModelRequest instance</returns>
        GetModelRequest<T> GetModel<T>(string modelID,
            string modelVersionID = null) where T : IPrediction;

        /// <summary>
        /// Retrieves an instance of a <see cref="ModelVersion"/> using model ID and version ID.
        /// </summary>
        /// <param name="modelID">the model ID</param>
        /// <param name="versionID">the version ID</param>
        /// <returns>a new GetModelVersionsRequest instance</returns>
        GetModelVersionRequest GetModelVersion(string modelID, string versionID);

        /// <summary>
        /// Returns all <see cref="ModelVersion"/> associated with a certain <see cref="Model{T}"/>.
        /// </summary>
        /// <param name="modelID">the model ID</param>
        /// <returns>a new GetModelVersionsRequest instance</returns>
        GetModelVersionsRequest GetModelVersions(string modelID);

        /// <summary>
        /// Deletes a specific model version.
        /// </summary>
        /// <param name="modelID">the model ID</param>
        /// <param name="versionID">the version ID of a model</param>
        /// <returns>a new instance of DeleteModelVersionRequest</returns>
        DeleteModelVersionRequest DeleteModelVersion(string modelID, string versionID);

        /// <summary>
        /// Creates a new model.
        /// </summary>
        /// <param name="modelID">the model ID</param>
        /// <param name="name">the model name</param>
        /// <param name="concepts"></param>
        /// <param name="areConceptsMutuallyExclusive"></param>
        /// <param name="isEnvironmentClosed">is environment closed</param>
        /// <param name="language">the language</param>
        /// <returns>a new model creation request</returns>
        CreateModelRequest CreateModel(string modelID, string name = null,
            IEnumerable<Concept> concepts = null, bool? areConceptsMutuallyExclusive = null,
            bool? isEnvironmentClosed = null, string language = null);

        /// <summary>
        /// Creates a new model that is not (necessarily) a model for Concepts = ConceptModel).
        /// </summary>
        /// <param name="modelID">the model ID</param>
        /// <param name="name">the model name</param>
        /// <param name="outputInfo">the output info</param>
        /// <returns>a new model creation request</returns>
        CreateModelGenericRequest<T> CreateModelGeneric<T>(string modelID,
            string name = null, IOutputInfo outputInfo = null) where T : IPrediction;

        /// <summary>
        /// Modifies a model.
        /// </summary>
        /// <param name="modelID">the model ID</param>
        /// <param name="action">the modification action</param>
        /// <param name="name">the model name</param>
        /// <param name="concepts">the concepts update the model with</param>
        /// <param name="areConceptsMutuallyExclusive">are concepts mutually exclusive</param>
        /// <param name="isEnvironmentClosed">is environment closed</param>
        /// <param name="language">the language</param>
        /// <returns>a new model modification request</returns>
        ModifyModelRequest ModifyModel(string modelID, ModifyAction action = null,
            string name = null, IEnumerable<Concept> concepts = null,
            bool? areConceptsMutuallyExclusive = null, bool? isEnvironmentClosed = null,
            string language = null);

        /// <summary>
        /// Deletes a model.
        /// </summary>
        /// <param name="modelID">the model ID</param>
        /// <returns>a new model deletion request</returns>
        DeleteModelRequest DeleteModel(string modelID);

        /// <summary>
        /// Deletes all custom models.
        /// </summary>
        /// <returns>a new DeleteAllModelsRequest instance</returns>
        DeleteAllModelsRequest DeleteAllModels();

        /// <summary>
        /// Trains a model.
        /// </summary>
        /// <param name="modelID">the model ID</param>
        /// <returns>a new model train request</returns>
        TrainModelRequest<T> TrainModel<T>(string modelID) where T : IPrediction;

        /// <summary>
        /// Runs a model evaluation to test model's version performance using cross validation.
        /// </summary>
        /// <param name="modelID">the model ID</param>
        /// <param name="versionID">the version ID</param>
        /// <returns>a new ModelEvaluationRequest instance</returns>
        ModelEvaluationRequest ModelEvaluation(string modelID, string versionID);

        /// <summary>
        /// Returns the model's inputs.
        /// </summary>
        /// <param name="modelID">the model ID</param>
        /// <param name="versionID">the model's version ID</param>
        /// <returns>a new GetModelInputsRequest instance</returns>
        GetModelInputsRequest GetModelInputs(string modelID, string versionID = null);

        /// <summary>
        /// Search all the models by name and type of model.
        /// </summary>
        /// <param name="name">
        /// the model name - use "*" for any name, or search by part of a name like "celeb*"
        /// </param>
        /// <param name="modelType">the model type</param>
        /// <returns>a new SearchModelsRequest instance</returns>
        SearchModelsRequest SearchModels(string name, ModelType modelType = null);

        /// <summary>
        /// Searches for inputs.
        /// </summary>
        /// <param name="searchBys">the search clauses</param>
        /// <returns>a new SearchInputsRequest instance</returns>
        SearchInputsRequest SearchInputs(params SearchBy[] searchBys);

        /// <summary>
        /// Searches for inputs.
        /// </summary>
        /// <param name="searchClauses">the search clauses</param>
        /// <param name="language">the language</param>
        /// <returns>a new SearchInputsRequest instance</returns>
        SearchInputsRequest SearchInputs(IEnumerable<SearchBy> searchClauses,
            string language = null);
    }
}
