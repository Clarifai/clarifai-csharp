using System.Collections.Generic;
using Clarifai.Internal.GRPC;

namespace Clarifai.DTOs.Predictions
{
    public class Region
    {
        public string ID { get; }
        public Crop Crop { get; }
        public List<Concept> FaceConcepts { get; }

        private Region(string id, Crop crop, List<Concept> faceConcepts)
        {
            ID = id;
            Crop = crop;
            FaceConcepts = faceConcepts;
        }

        public static Region Deserialize(dynamic jsonObject)
        {
            var faceConcepts = new List<Concept>();

            if (jsonObject.data != null)
            {
                var face = jsonObject.data.face;
                if (face.identity != null)
                {
                    foreach (dynamic concept in face.identity.concepts)
                    {
                        faceConcepts.Add(Concept.Deserialize(concept));
                    }
                }
            }

            return new Region(
                (string)jsonObject.id,
                DTOs.Crop.Deserialize(jsonObject.region_info.bounding_box),
                faceConcepts);
        }

        public static Region GrpcDeserialize(Internal.GRPC.Region region)
        {
            var faceConcepts = new List<Concept>();

            if (region.Data != null)
            {
                Face face = region.Data.Face;
                if (face.Identity != null)
                {
                    foreach (Internal.GRPC.Concept concept in face.Identity.Concepts)
                    {
                        faceConcepts.Add(Concept.GrpcDeserialize(concept));
                    }
                }
            }

            return new Region(
                region.Id ,
                DTOs.Crop.GrpcDeserialize(region.RegionInfo.BoundingBox),
                faceConcepts);
        }
    }
}
