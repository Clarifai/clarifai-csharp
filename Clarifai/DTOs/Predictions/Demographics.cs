using System;
using System.Collections.Generic;
using System.Linq;
using Clarifai.Internal.GRPC;

namespace Clarifai.DTOs.Predictions
{
    public class Demographics : IPrediction
    {
        public string TYPE => "demographics";

        public Crop Crop { get; }

        public List<Concept> AgeAppearanceConcepts { get; }

        public List<Concept> GenderAppearanceConcepts { get; }

        public List<Concept> MulticulturalAppearanceConcepts { get; }

        private Demographics(Crop crop, List<Concept> ageAppearanceConcepts,
            List<Concept> genderAppearanceConcepts, List<Concept> multiculturalAppearanceConcepts)
        {
            Crop = crop;
            AgeAppearanceConcepts = ageAppearanceConcepts;
            GenderAppearanceConcepts = genderAppearanceConcepts;
            MulticulturalAppearanceConcepts = multiculturalAppearanceConcepts;
        }

        [Obsolete]
        public static Demographics Deserialize(dynamic jsonObject)
        {
            dynamic face = jsonObject.data.face;

            var ageAppearanceConcepts = new List<Concept>();
            foreach (dynamic concept in face.age_appearance.concepts)
            {
                ageAppearanceConcepts.Add(Concept.Deserialize(concept));
            }

            var genderAppearanceConcepts = new List<Concept>();
            foreach (dynamic concept in face.gender_appearance.concepts)
            {
                genderAppearanceConcepts.Add(Concept.Deserialize(concept));
            }

            var multiculturalAppearanceConcepts = new List<Concept>();
            foreach (dynamic concept in face.multicultural_appearance.concepts)
            {
                multiculturalAppearanceConcepts.Add(Concept.Deserialize(concept));
            }
            return new Demographics(DTOs.Crop.Deserialize(jsonObject.region_info.bounding_box),
                ageAppearanceConcepts, genderAppearanceConcepts, multiculturalAppearanceConcepts);
        }

        /// <summary>
        /// Deserializes this object from a gRPC object.
        /// </summary>
        /// <param name="demographics">the gRPC object</param>
        /// <returns>a new instance of this class</returns>
        public static Demographics GrpcDeserialize(Internal.GRPC.Region demographics)
        {
            Face face = demographics.Data.Face;

            List<Concept> ageAppearanceConcepts = face.AgeAppearance.Concepts
                .Select(Concept.GrpcDeserialize)
                .ToList();

            List<Concept> genderAppearanceConcepts = face.GenderAppearance.Concepts
                .Select(Concept.GrpcDeserialize)
                .ToList();

            List<Concept> multiculturalAppearanceConcepts = face.MulticulturalAppearance.Concepts
                .Select(Concept.GrpcDeserialize)
                .ToList();

            return new Demographics(Crop.GrpcDeserialize(demographics.RegionInfo.BoundingBox),
                ageAppearanceConcepts, genderAppearanceConcepts, multiculturalAppearanceConcepts);
        }

        public override bool Equals(object obj)
        {
            return obj is Demographics demographics &&
                   EqualityComparer<Crop>.Default.Equals(Crop, demographics.Crop) &&
                   EqualityComparer<List<Concept>>.Default.Equals(AgeAppearanceConcepts,
                       demographics.AgeAppearanceConcepts) &&
                   EqualityComparer<List<Concept>>.Default.Equals(GenderAppearanceConcepts,
                       demographics.GenderAppearanceConcepts) &&
                   EqualityComparer<List<Concept>>.Default.Equals(MulticulturalAppearanceConcepts,
                       demographics.MulticulturalAppearanceConcepts);
        }

        public override int GetHashCode()
        {
            var hashCode = -1243036980;
            hashCode = hashCode * -1521134295 + EqualityComparer<Crop>.Default.GetHashCode(Crop);
            hashCode = hashCode * -1521134295 +
                       EqualityComparer<List<Concept>>.Default.GetHashCode(AgeAppearanceConcepts);
            hashCode = hashCode * -1521134295 +
                       EqualityComparer<List<Concept>>.Default
                           .GetHashCode(GenderAppearanceConcepts);
            hashCode = hashCode * -1521134295 +
                       EqualityComparer<List<Concept>>.Default.GetHashCode(
                           MulticulturalAppearanceConcepts);
            return hashCode;
        }

        public override string ToString()
        {
            return $"[Demographics: (crop: {Crop})]";
        }
    }
}
