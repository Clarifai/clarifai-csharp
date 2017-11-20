using System.Collections.Generic;

namespace Clarifai.DTOs.Predictions
{
    public class Embedding : IPrediction
    {
        public string TYPE => "embedding";

        public int NumDimensions { get; }

        private readonly decimal[] _vector;
        /// <summary>
        /// A copy is returned so the original array cannot be modified.
        /// </summary>
        public decimal[] Vector => (decimal[]) _vector.Clone();

        private Embedding(int numDimensions, decimal[] vector)
        {
            NumDimensions = numDimensions;
            _vector = vector;
        }

        public static Embedding Deserialize(dynamic jsonObject)
        {
            int numDimensions = jsonObject.num_dimensions;
            var vector = new decimal[numDimensions];
            for (int i = 0; i < jsonObject.vector.Count; i++)
            {
                vector[i] = jsonObject.vector[i];
            }
            return new Embedding(numDimensions, vector);
        }

        public override bool Equals(object obj)
        {
            return obj is Embedding embedding &&
                   NumDimensions == embedding.NumDimensions &&
                   EqualityComparer<decimal[]>.Default.Equals(_vector, embedding._vector);
        }

        public override int GetHashCode()
        {
            var hashCode = -1938963465;
            hashCode = hashCode * -1521134295 + NumDimensions.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<decimal[]>.Default.GetHashCode(_vector);
            return hashCode;
        }

        public override string ToString()
        {
            return $"[Embedding: (vector: {_vector})]";
        }
    }
}
