using System.Collections.Generic;
using System.Linq;

namespace Clarifai.DTOs.Predictions
{
    public class FaceConcepts : IPrediction
    {
        public string TYPE => "faceConcepts";

        public string ID { get; }

        public Crop Crop { get; }

        public List<Concept> Concepts { get; }

        private FaceConcepts(string id, Crop crop, List<Concept> concepts)
        {
            ID = id;
            Crop = crop;
            Concepts = concepts;
        }

        public static FaceConcepts Deserialize(dynamic jsonObject)
        {
            var concepts = new List<Concept>();
            foreach (dynamic concept in jsonObject.data.face.identity.concepts)
            {
                concepts.Add(Concept.Deserialize(concept));
            }
            return new FaceConcepts(
                (string)jsonObject.id,
                DTOs.Crop.Deserialize(jsonObject.region_info.bounding_box),
                concepts);
        }

        /// <summary>
        /// Deserializes this object from a gRPC object.
        /// </summary>
        /// <param name="faceConcepts">the gRPC object</param>
        /// <returns>a new instance of this class</returns>
        public static FaceConcepts GrpcDeserialize(Internal.GRPC.Region faceConcepts)
        {
            List<Concept> concepts = faceConcepts.Data.Face.Identity.Concepts
                .Select(Concept.GrpcDeserialize)
                .ToList();
            return new FaceConcepts(
                faceConcepts.Id,
                Crop.GrpcDeserialize(faceConcepts.RegionInfo.BoundingBox),
                concepts);
        }

        public override bool Equals(object obj)
        {
            return obj is FaceConcepts concepts &&
                   EqualityComparer<Crop>.Default.Equals(Crop, concepts.Crop) &&
                   EqualityComparer<List<Concept>>.Default.Equals(Concepts, concepts.Concepts);
        }

        public override int GetHashCode()
        {
            var hashCode = -1453176561;
            hashCode = hashCode * -1521134295 + EqualityComparer<Crop>.Default.GetHashCode(Crop);
            hashCode = hashCode * -1521134295 +
                       EqualityComparer<List<Concept>>.Default.GetHashCode(Concepts);
            return hashCode;
        }

        public override string ToString()
        {
            return $"[FaceConcepts: (crop: {Crop}, concepts: {Concepts})]";
        }
    }
}
