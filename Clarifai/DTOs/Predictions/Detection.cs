using System;
using System.Collections.Generic;

namespace Clarifai.DTOs.Predictions
{
    public class Detection : IPrediction
    {
        public string TYPE => "detection";

        public Crop Crop { get; }

        public List<Concept> Concepts { get; }

        private Detection(Crop crop, List<Concept> concepts)
        {
            Crop = crop;
            Concepts = concepts;
        }

        public static Detection Deserialize(dynamic jsonObject)
        {
            var concepts = new List<Concept>();
            if (jsonObject.data.concepts != null)
            {
                foreach (dynamic concept in jsonObject.data.concepts)
                {
                    concepts.Add(Concept.Deserialize(concept));
                }
            }

            return new Detection(DTOs.Crop.Deserialize(jsonObject.region_info.bounding_box), concepts);
        }

        public override bool Equals(object obj)
        {
            return obj is Detection detection &&
                   EqualityComparer<Crop>.Default.Equals(Crop, detection.Crop) &&
                   EqualityComparer<List<Concept>>.Default.Equals(Concepts, detection.Concepts);
        }

        public override int GetHashCode()
        {
            var hashCode = -1453176561;
            hashCode = hashCode * -1521134295 + EqualityComparer<Crop>.Default.GetHashCode(Crop);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Concept>>.Default.GetHashCode(Concepts);
            return hashCode;
        }

        public override string ToString()
        {
            return $"[Detection: (crop: {Crop}, concepts: {Concepts})]";
        }
    }
}