using Clarifai.API;

namespace Clarifai.DTOs.Models
{
    /// <summary>
    /// A collection of already existing models provided by the API for immediate use.
    /// </summary>
    public class PublicModels
    {
        /// <summary>
        /// Color model recognizes dominant colors on an input.
        /// </summary>
        public ColorModel ColorModel { get; }

        /// <summary>
        /// Apparel model recognizes clothing, accessories, ant other fashion-related items.
        /// </summary>
        public ConceptModel ApparelModel { get; }

        /// <summary>
        /// Food model recognizes food items and dishes, down to the ingredient level.
        /// </summary>
        public ConceptModel FoodModel { get; }

        /// <summary>
        /// General model predicts most generally.
        /// </summary>
        public ConceptModel GeneralModel { get; }

        /// <summary>
        /// Landscape quality model predicts the quality of a landscape image.
        /// </summary>
        public ConceptModel LandscapeQualityModel { get; }

        /// <summary>
        /// Moderation model predicts inputs such as safety, gore, nudity, etc.
        /// </summary>
        public ConceptModel ModerationModel { get; }

        /// <summary>
        /// NSFW model identifies different levels of nudity.
        /// </summary>
        public ConceptModel NsfwModel { get; }

        /// <summary>
        /// Portrait quality model predicts the quality of a portrait image.
        /// </summary>
        public ConceptModel PortraitQualityModel { get; }

        /// <summary>
        /// Textures & Patterns model predicts textures and patterns on an image.
        /// </summary>
        public ConceptModel TexturesAndPatternsModel { get; }

        /// <summary>
        /// Travel model recognizes travel and hospitality-related concepts.
        /// </summary>
        public ConceptModel TravelModel { get; }

        /// <summary>
        /// Wedding model recognizes wedding-related concepts bride, groom, flowers, and more.
        /// </summary>
        public ConceptModel WeddingModel { get; }

        /// <summary>
        /// Demographics model predicts the age, gender, and cultural appearance.
        /// </summary>
        public DetectionModel DemographicsModel { get; }

        /// <summary>
        /// General embedding model computes numerical embedding vectors using our General model.
        /// </summary>
        public EmbeddingModel GeneralEmbeddingModel { get; }

        /// <summary>
        /// Celebrity model identifies celebrities that closely resemble detected faces.
        /// </summary>
        public DetectionModel CelebrityModel { get; }

        /// <summary>
        /// Face detection model detects the presence and location of human faces.
        /// </summary>
        public DetectionModel FaceDetectionModel { get; }

        /// <summary>
        /// Face embedding model computes numerical embedding vectors using our Face detection
        /// model.
        /// </summary>
        public FaceEmbeddingModel FaceEmbeddingModel { get; }

        /// <summary>
        /// Logo model detects and identifies brand logos.
        /// </summary>
        public DetectionModel LogoModel { get; }


        public VideoModel ApparelVideoModel { get; }

        public VideoModel FoodVideoModel { get; }

        public VideoModel GeneralVideoModel { get; }

        public VideoModel NsfwVideoModel { get; }

        public VideoModel TravelVideoModel { get; }

        public VideoModel WeddingVideoModel { get; }

        /// <summary>
        /// Ctor.
        /// </summary>
        public PublicModels(IClarifaiHttpClient httpClient)
        {
            ColorModel = new ColorModel(httpClient, "eeed0b6733a644cea07cf4c60f87ebb7");

            ApparelModel = new ConceptModel(httpClient, "e0be3b9d6a454f0493ac3a30784001ff");
            FoodModel = new ConceptModel(httpClient, "bd367be194cf45149e75f01d59f77ba7");
            GeneralModel = new ConceptModel(httpClient, "aaa03c23b3724a16a56b629203edc62c");
            LandscapeQualityModel = new ConceptModel(httpClient,
                "bec14810deb94c40a05f1f0eb3c91403");
            ModerationModel = new ConceptModel(httpClient, "d16f390eb32cad478c7ae150069bd2c6");
            NsfwModel = new ConceptModel(httpClient, "e9576d86d2004ed1a38ba0cf39ecb4b1");
            PortraitQualityModel = new ConceptModel(httpClient, "de9bd05cfdbf4534af151beb2a5d0953");
            TexturesAndPatternsModel = new ConceptModel(httpClient,
                "fbefb47f9fdb410e8ce14f24f54b47ff");
            TravelModel = new ConceptModel(httpClient, "eee28c313d69466f836ab83287a54ed9");
            WeddingModel = new ConceptModel(httpClient, "c386b7a870114f4a87477c0824499348");

            DemographicsModel = new DetectionModel(httpClient,
                "c0c0ac362b03416da06ab3fa36fb58e3");

            GeneralEmbeddingModel = new EmbeddingModel(httpClient,
                "bbb5f41425b8468d9b7a554ff10f8581");

            CelebrityModel = new DetectionModel(httpClient, "e466caa0619f444ab97497640cefc4dc");

            FaceEmbeddingModel = new FaceEmbeddingModel(httpClient,
                "e15d0f873e66047e579f90cf82c9882z");

            FaceDetectionModel = new DetectionModel(httpClient,
                "53e1df302c079b3db8a0a36033ed2d15");

            LogoModel = new DetectionModel(httpClient, "c443119bf2ed4da98487520d01a0b1e3");

            ApparelVideoModel = new VideoModel(httpClient, "e0be3b9d6a454f0493ac3a30784001ff");
            FoodVideoModel = new VideoModel(httpClient, "bd367be194cf45149e75f01d59f77ba7");
            GeneralVideoModel = new VideoModel(httpClient, "aaa03c23b3724a16a56b629203edc62c");
            NsfwVideoModel = new VideoModel(httpClient, "e9576d86d2004ed1a38ba0cf39ecb4b1");
            TravelVideoModel = new VideoModel(httpClient, "eee28c313d69466f836ab83287a54ed9");
            WeddingVideoModel = new VideoModel(httpClient, "c386b7a870114f4a87477c0824499348");
        }
    }
}
