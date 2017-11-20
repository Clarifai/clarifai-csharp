using System.Collections.Generic;

namespace Clarifai.DTOs.Predictions
{
    public class FaceDetection : IPrediction
    {
        public string TYPE => "faceDetection";

        public Crop Crop { get; }

        private FaceDetection(Crop crop)
        {
            Crop = crop;
        }

        public static FaceDetection Deserialize(dynamic jsonObject)
        {
            return new FaceDetection(DTOs.Crop.Deserialize(jsonObject.region_info.bounding_box));
        }

        public override bool Equals(object obj)
        {
            return obj is FaceDetection detection &&
                   EqualityComparer<Crop>.Default.Equals(Crop, detection.Crop);
        }

        public override int GetHashCode()
        {
            return 793696463 + EqualityComparer<Crop>.Default.GetHashCode(Crop);
        }

        public override string ToString()
        {
            return $"[FaceDetection: (crop: {Crop})]";
        }
    }
}
