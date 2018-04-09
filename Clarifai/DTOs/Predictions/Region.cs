using System.Collections.Generic;

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
    }
}
