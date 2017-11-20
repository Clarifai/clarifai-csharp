using Clarifai.DTOs.Inputs;

namespace Clarifai.DTOs.Searches
{
    public class SearchHit
    {
        public decimal Score { get; }
        public IClarifaiInput Input { get; }

        private SearchHit(decimal score, ClarifaiInput input)
        {
            Score = score;
            Input = input;
        }

        public static SearchHit Deserialize(dynamic jsonObject)
        {
            return new SearchHit((decimal)jsonObject.score,
                ClarifaiInput.Deserialize(jsonObject.input));
        }
    }
}
