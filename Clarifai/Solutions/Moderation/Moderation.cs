using Clarifai.API;
using Clarifai.DTOs.Inputs;
using Clarifai.Solutions.Moderation.API.Requests.Inputs;
using Clarifai.Solutions.Moderation.API.Requests.Models;

namespace Clarifai.Solutions.Moderation
{
    public class Moderation
    {
        private readonly IClarifaiHttpClient _httpClient;

        public Moderation(string apiKey)
        {
            _httpClient = new ClarifaiHttpClient(apiKey, "https://api.clarifai-moderation.com");
        }

        public ModerationPredictRequest Predict(string modelID, IClarifaiInput input)
        {
            return new ModerationPredictRequest(_httpClient, modelID, input);
        }

        public GetModerationStatusRequest GetModerationStatus(string inputID)
        {
            return new GetModerationStatusRequest(_httpClient, inputID);
        }
    }
}
