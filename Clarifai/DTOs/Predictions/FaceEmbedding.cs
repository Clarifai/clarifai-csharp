using System.Collections.Generic;
using System.Linq;

namespace Clarifai.DTOs.Predictions
{
    public class FaceEmbedding : IPrediction
    {
        public string TYPE => "face-embedding";

        public Crop Crop { get; }

        public List<Embedding> Embeddings { get; }

        private FaceEmbedding(Crop crop, List<Embedding> embeddings)
        {
            Crop = crop;
            Embeddings = embeddings;
        }

        public static FaceEmbedding Deserialize(dynamic jsonObject)
        {
            var embeddings = new List<Embedding>();
            foreach (dynamic embedding in jsonObject.data.embeddings)
            {
                embeddings.Add(Embedding.Deserialize(embedding));
            }
            return new FaceEmbedding(
                DTOs.Crop.Deserialize(jsonObject.region_info.bounding_box),
                embeddings);
        }

        /// <summary>
        /// Deserializes this object from a gRPC object.
        /// </summary>
        /// <param name="faceEmbedding">the gRPC object</param>
        /// <returns>a new instance of this class</returns>
        public static FaceEmbedding GrpcDeserialize(Internal.GRPC.Region faceEmbedding)
        {
            List<Embedding> embeddings = faceEmbedding.Data.Embeddings
                .Select(Embedding.GrpcDeserialize)
                .ToList();
            return new FaceEmbedding(
                DTOs.Crop.GrpcDeserialize(faceEmbedding.RegionInfo.BoundingBox),
                embeddings);
        }

        public override bool Equals(object obj)
        {
            return obj is FaceEmbedding embedding &&
                   EqualityComparer<Crop>.Default.Equals(Crop, embedding.Crop) &&
                   EqualityComparer<List<Embedding>>.Default.Equals(Embeddings,
                       embedding.Embeddings);
        }

        public override int GetHashCode()
        {
            var hashCode = 1745160450;
            hashCode = hashCode * -1521134295 + EqualityComparer<Crop>.Default.GetHashCode(Crop);
            hashCode = hashCode * -1521134295 +
                       EqualityComparer<List<Embedding>>.Default.GetHashCode(Embeddings);
            return hashCode;
        }

        public override string ToString()
        {
            return $"[FaceEmbedding: (crop: {Crop})]";
        }
    }
}
