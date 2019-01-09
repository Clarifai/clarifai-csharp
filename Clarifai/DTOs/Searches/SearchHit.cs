using Clarifai.DTOs.Inputs;
using Clarifai.Internal.GRPC;

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

        public static SearchHit GrpcDeserialize(Hit hit)
        {
            return new SearchHit((decimal) hit.Score, ClarifaiInput.GrpcDeserialize(hit.Input));
        }
    }
}
