using System;
using System.Collections.Generic;
using Clarifai.API.Requests.Concepts;
using Clarifai.API.Requests.Feedbacks;
using Clarifai.API.Requests.Inputs;
using Clarifai.API.Requests.Models;
using Clarifai.DTOs.Feedbacks;
using Clarifai.DTOs.Inputs;
using Clarifai.DTOs.Models;
using Clarifai.DTOs.Models.OutputsInfo;
using Clarifai.DTOs.Predictions;
using Clarifai.DTOs.Searches;
using Clarifai.Exceptions;
using Newtonsoft.Json.Linq;

namespace Clarifai.API
{
    /// <inheritdoc />
    public class ClarifaiClient : IClarifaiClient
    {
        public IClarifaiHttpClient HttpClient { get; }

        /// <inheritdoc />
        public PublicModels PublicModels { get; }

        public Solutions.Solutions Solutions { get; }

        /// <summary>
        /// Ctor.
        /// </summary>
        public ClarifaiClient() : this(GetEnvVar())
        { }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="apiKey">your Clarifai API key</param>
        public ClarifaiClient(string apiKey) : this(new ClarifaiHttpClient(apiKey))
        { }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="httpClient">the Clarifai http client</param>
        public ClarifaiClient(IClarifaiHttpClient httpClient)
        {
            HttpClient = httpClient;
            PublicModels = new PublicModels(httpClient);
            Solutions = new Solutions.Solutions(httpClient.ApiKey);
        }

        /// <inheritdoc />
        public GetConceptsRequest GetConcepts()
        {
            return new GetConceptsRequest(HttpClient);
        }

        /// <inheritdoc />
        public GetConceptRequest GetConcept(string conceptID)
        {
            return new GetConceptRequest(HttpClient, conceptID);
        }

        /// <inheritdoc />
        public AddConceptsRequest AddConcepts(params Concept[] concepts)
        {
            return new AddConceptsRequest(HttpClient, concepts);
        }

        /// <inheritdoc />
        public AddConceptsRequest AddConcepts(IEnumerable<Concept> concepts)
        {
            return new AddConceptsRequest(HttpClient, concepts);
        }

        /// <inheritdoc />
        public ModifyConceptsRequest ModifyConcepts(params Concept[] concepts)
        {
            return new ModifyConceptsRequest(HttpClient, concepts);
        }

        /// <inheritdoc />
        public ModifyConceptsRequest ModifyConcepts(IEnumerable<Concept> concepts)
        {
            return new ModifyConceptsRequest(HttpClient, concepts);
        }

        /// <inheritdoc />
        public SearchConceptsRequest SearchConcepts(string query, string language = null)
        {
            return new SearchConceptsRequest(HttpClient, query, language);
        }

        /// <inheritdoc />
        public GetInputsRequest GetInputs()
        {
            return new GetInputsRequest(HttpClient);
        }

        /// <inheritdoc />
        public GetInputRequest GetInput(string inputID)
        {
            return new GetInputRequest(HttpClient, inputID);
        }

        /// <inheritdoc />
        public AddInputsRequest AddInputs(params IClarifaiInput[] inputs)
        {
            return new AddInputsRequest(HttpClient, inputs);
        }

        /// <inheritdoc />
        public AddInputsRequest AddInputs(IEnumerable<IClarifaiInput> inputs)
        {
            return new AddInputsRequest(HttpClient, inputs);
        }

        /// <inheritdoc />
        public ModifyInputRequest ModifyInput(string inputID, ModifyAction action,
            IEnumerable<Concept> positiveConcepts = null,
            IEnumerable<Concept> negativeConcepts = null,
            IEnumerable<RegionFeedback> regionFeedbacks = null)
        {
            return new ModifyInputRequest(HttpClient, inputID, action, positiveConcepts,
                negativeConcepts, regionFeedbacks);
        }

        /// <inheritdoc />
        public ModifyInputMetadataRequest ModifyInputMetadata(string inputID, JObject metadata)
        {
            return new ModifyInputMetadataRequest(HttpClient, inputID, metadata);
        }

        /// <inheritdoc />
        public DeleteInputsRequest DeleteInputs(params string[] inputIDs)
        {
            return new DeleteInputsRequest(HttpClient, inputIDs);
        }

        /// <inheritdoc />
        public DeleteInputsRequest DeleteInputs(IEnumerable<string> inputIDs)
        {
            return new DeleteInputsRequest(HttpClient, inputIDs);
        }

        /// <inheritdoc />
        public DeleteAllInputsRequest DeleteAllInputs()
        {
            return new DeleteAllInputsRequest(HttpClient);
        }

        /// <inheritdoc />
        public GetInputsStatusRequest GetInputsStatus()
        {
            return new GetInputsStatusRequest(HttpClient);
        }

        /// <inheritdoc />
        public PredictRequest<T> Predict<T>(string modelID, IClarifaiInput input,
            string modelVersionID = null, string language = null, decimal? minValue = null,
            int? maxConcepts = null, IEnumerable<Concept> selectConcepts = null)
            where T : IPrediction
        {
            return new PredictRequest<T>(HttpClient, modelID, input, modelVersionID, language, minValue,
                maxConcepts, selectConcepts);
        }

        /// <inheritdoc />
        public BatchPredictRequest<T> Predict<T>(string modelID, IEnumerable<IClarifaiInput> inputs,
            string modelVersionID = null, string language = null, decimal? minValue = null,
            int? maxConcepts = null, IEnumerable<Concept> selectConcepts = null)
            where T : IPrediction
        {
            return new BatchPredictRequest<T>(HttpClient, modelID, inputs, modelVersionID, language, minValue,
                maxConcepts, selectConcepts);
        }

        /// <inheritdoc />
        public WorkflowPredictRequest WorkflowPredict(string workflowID, IClarifaiInput inputs,
            decimal? minValue = null, int? maxConcepts = null)
        {
            return new WorkflowPredictRequest(HttpClient, workflowID, inputs, minValue, maxConcepts);
        }

        /// <inheritdoc />
        public WorkflowBatchPredictRequest WorkflowPredict(string workflowID,
            IEnumerable<IClarifaiInput> inputs, decimal? minValue = null, int? maxConcepts = null)
        {
            return new WorkflowBatchPredictRequest(HttpClient, workflowID, inputs, minValue, maxConcepts);
        }

        /// <inheritdoc />
        public GetModelsRequest GetModels()
        {
            return new GetModelsRequest(HttpClient);
        }

        /// <inheritdoc />
        public GetModelRequest<T> GetModel<T>(string modelID) where T : IPrediction
        {
            return new GetModelRequest<T>(HttpClient, modelID);
        }

        /// <inheritdoc />
        public GetModelVersionRequest GetModelVersion(string modelID, string versionID)
        {
            return new GetModelVersionRequest(HttpClient, modelID, versionID);
        }

        /// <inheritdoc />
        public GetModelVersionsRequest GetModelVersions(string modelID)
        {
            return new GetModelVersionsRequest(HttpClient, modelID);
        }

        /// <inheritdoc />
        public DeleteModelVersionRequest DeleteModelVersion(string modelID, string versionID)
        {
            return new DeleteModelVersionRequest(HttpClient, modelID, versionID);
        }

        /// <inheritdoc />
        public DeleteAllModelsRequest DeleteAllModels()
        {
            return new DeleteAllModelsRequest(HttpClient);
        }

        /// <inheritdoc />
        public CreateModelRequest CreateModel(string modelID, string name = null,
            IEnumerable<Concept> concepts = null, bool? areConceptsMutuallyExclusive = null,
            bool? isEnvironmentClosed = null, string language = null)
        {
            return new CreateModelRequest(HttpClient, modelID, name, concepts,
                areConceptsMutuallyExclusive, isEnvironmentClosed, language);
        }

        /// <inheritdoc />
        public CreateModelGenericRequest<T> CreateModelGeneric<T>(string modelID,
            string name = null, IOutputInfo outputInfo = null) where T : IPrediction
        {
            return new CreateModelGenericRequest<T>(HttpClient, modelID, name, outputInfo);
        }

        /// <inheritdoc />
        public ModifyModelRequest ModifyModel(string modelID, ModifyAction action = null,
            string name = null, IEnumerable<Concept> concepts = null,
            bool? areConceptsMutuallyExclusive = null, bool? isEnvironmentClosed = null,
            string language = null)
        {
            return new ModifyModelRequest(HttpClient, modelID, action, name, concepts,
                areConceptsMutuallyExclusive, isEnvironmentClosed, language);
        }

        /// <inheritdoc />
        public DeleteModelRequest DeleteModel(string modelID)
        {
            return new DeleteModelRequest(HttpClient, modelID);
        }

        /// <inheritdoc />
        public TrainModelRequest<T> TrainModel<T>(string modelID) where T : IPrediction
        {
            return new TrainModelRequest<T>(HttpClient, modelID);
        }

        /// <inheritdoc />
        public ModelEvaluationRequest ModelEvaluation(string modelID, string versionID)
        {
            return new ModelEvaluationRequest(HttpClient, modelID, versionID);
        }

        /// <inheritdoc />
        public GetModelInputsRequest GetModelInputs(string modelID, string versionID = null)
        {
            return new GetModelInputsRequest(HttpClient, modelID, versionID);
        }

        /// <inheritdoc />
        public SearchModelsRequest SearchModels(string name, ModelType modelType = null)
        {
            return new SearchModelsRequest(HttpClient, name, modelType);
        }

        /// <inheritdoc />
        public SearchInputsRequest SearchInputs(params SearchBy[] searchBys)
        {
            return new SearchInputsRequest(HttpClient, searchBys);
        }

        /// <inheritdoc />
        public SearchInputsRequest SearchInputs(IEnumerable<SearchBy> searchClauses,
            string language = null)
        {
            return new SearchInputsRequest(HttpClient, searchClauses, language);
        }

        /// <inheritdoc />
        public ModelFeedbackRequest ModelFeedback(string modelID, string imageUrl, string inputID,
            string outputID, string endUserID, string sessionID,
            IEnumerable<ConceptFeedback> concepts = null,
            IEnumerable<RegionFeedback> regions = null)
        {
            return new ModelFeedbackRequest(HttpClient, modelID, imageUrl, inputID, outputID, endUserID,
                sessionID, concepts, regions);
        }

        /// <inheritdoc />
        public SearchesFeedbackRequest SearchesFeedback(string inputID, string searchID,
            string endUserID, string sessionID)
        {
            return new SearchesFeedbackRequest(HttpClient, inputID, searchID, endUserID, sessionID);
        }

        /// <summary>
        /// Retrieves the Clarifai API key from the environment.
        /// </summary>
        /// <returns>the Clarifai API key</returns>
        /// <exception cref="ClarifaiException">throws if the env. var. doesn't exist</exception>
        private static string GetEnvVar()
        {
            var apiKey = Environment.GetEnvironmentVariable("CLARIFAI_API_KEY");
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ClarifaiException(
                    "Either set a CLARIFAI_API_KEY environment variable or provide the API " +
                    "key in an constructor argument.");
            }
            return apiKey;
        }
    }
}
