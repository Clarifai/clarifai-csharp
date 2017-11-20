using System;
using System.Collections.Generic;
using System.Linq;
using Clarifai.DTOs.Predictions;

namespace Clarifai.DTOs.Models
{
    public class ModelType
    {
        public static ModelType Color =>
            new ModelType("color", typeof(ColorModel), typeof(Color));
        public static ModelType Concept =>
            new ModelType("concept", typeof(ConceptModel), typeof(Concept));
        public static ModelType Demographics =>
            new ModelType("facedetect-demographics", typeof(DemographicsModel), typeof(Demographics));
        public static ModelType Embedding =>
            new ModelType("embed", typeof(EmbeddingModel), typeof(Embedding));
        public static ModelType FaceConcepts =>
            new ModelType("facedetect-identity",
            typeof(FaceConceptsModel), typeof(FaceConcepts));
        public static ModelType FaceDetection =>
            new ModelType("facedetect", typeof(FaceDetectionModel), typeof(FaceDetection));
        public static ModelType FaceEmbedding =>
            new ModelType("detect-embed", typeof(FaceEmbeddingModel), typeof(FaceEmbedding));
        public static ModelType Focus =>
            new ModelType("focus", typeof(FocusModel), typeof(Focus));
        public static ModelType Logo =>
            new ModelType("detection", typeof(LogoModel), typeof(Logo));
        public static ModelType Video =>
            new ModelType("video", typeof(VideoModel), typeof(Frame));

        public string TypeExt { get; }
        public Type Model { get; }
        public Type Prediction { get; }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="typeExt">the type extension</param>
        public ModelType(string typeExt, Type model, Type prediction)
        {
            TypeExt = typeExt;
            Model = model;
            Prediction = prediction;
        }

        public override bool Equals(object obj)
        {
            return obj is ModelType type &&
                   TypeExt == type.TypeExt;
        }

        public override int GetHashCode()
        {
            return 1672282492 + EqualityComparer<string>.Default.GetHashCode(TypeExt);
        }

        public override string ToString()
        {
            return $"[ModelType: {TypeExt}]";
        }

        public static ModelType DetermineModelType(string typeExt)
        {
            var modelTypes = new List<ModelType>
            {
                Color, Concept, Demographics, Embedding, FaceConcepts, FaceDetection, FaceEmbedding,
                Focus, Logo, Video
            };
            var query = modelTypes.Where(mt => mt.TypeExt == typeExt).ToList();
            if (!query.Any()) return null;
            ModelType modelType = query.Single();
            return modelType;
        }
    }
}
