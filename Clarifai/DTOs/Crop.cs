using System;
using System.Collections.Generic;
using Clarifai.Internal.GRPC;
using Google.Protobuf.Collections;
using Newtonsoft.Json.Linq;

namespace Clarifai.DTOs
{
    /// <summary>
    /// Crop / bounding box. Crop points are percentages from the edge.
    /// E.g. top of 0.2 means the cropped input will start 20% down from the original edge.
    /// </summary>
    public class Crop
    {
        public decimal Top { get; }
        public decimal Left { get; }
        public decimal Bottom { get; }
        public decimal Right { get; }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="top">top</param>
        /// <param name="left">left</param>
        /// <param name="bottom">bottom</param>
        /// <param name="right">right</param>
        public Crop(decimal top, decimal left, decimal bottom, decimal right)
        {
            Top = top;
            Left = left;
            Bottom = bottom;
            Right = right;
        }

        [Obsolete]
        public JArray SerializeAsArray()
        {
            return new JArray(new [] {Top, Left, Bottom, Right});
        }

        [Obsolete]
        public JObject SerializeAsObject()
        {
            return new JObject(
                new JProperty("top_row", Top),
                new JProperty("left_col", Left),
                new JProperty("bottom_row", Bottom),
                new JProperty("right_col", Right));
        }

        public List<float> GrpcSerializeAsArray()
        {
            return new List<float>{(float) Top, (float) Left, (float) Bottom, (float) Right};
        }

        public BoundingBox GrpcSerializeAsObject()
        {
            return new BoundingBox
            {
                TopRow = (float) Top,
                LeftCol = (float) Left,
                BottomRow = (float) Bottom,
                RightCol = (float) Right,
            };
        }

        [Obsolete]
        public static Crop Deserialize(dynamic jsonObject)
        {
            if (jsonObject is JArray)
            {
                return new Crop(
                    top: (decimal) jsonObject[0],
                    left: (decimal) jsonObject[1],
                    bottom: (decimal) jsonObject[2],
                    right: (decimal) jsonObject[3]);
            }
            else
            {
                return new Crop(
                    (decimal) jsonObject.top_row,
                    (decimal) jsonObject.left_col,
                    (decimal) jsonObject.bottom_row,
                    (decimal) jsonObject.right_col);
            }
        }

        public static Crop GrpcDeserialize(RepeatedField<float> imageCrop)
        {
            Console.WriteLine("imageCrop:");
            Console.WriteLine(imageCrop);
            return new Crop(
                top: (decimal) imageCrop[0],
                left: (decimal) imageCrop[1],
                bottom: (decimal) imageCrop[2],
                right: (decimal) imageCrop[3]);
        }

        public static Crop GrpcDeserialize(BoundingBox box)
        {
            return new Crop(
                (decimal) box.TopRow,
                (decimal) box.LeftCol,
                (decimal) box.BottomRow,
                (decimal) box.RightCol);
        }

        public override bool Equals(object obj)
        {
            return obj is Crop crop &&
                   Top == crop.Top &&
                   Left == crop.Left &&
                   Bottom == crop.Bottom &&
                   Right == crop.Right;
        }

        public override int GetHashCode()
        {
            var hashCode = -481391125;
            hashCode = hashCode * -1521134295 + Top.GetHashCode();
            hashCode = hashCode * -1521134295 + Left.GetHashCode();
            hashCode = hashCode * -1521134295 + Bottom.GetHashCode();
            hashCode = hashCode * -1521134295 + Right.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return $"[Crop: (top: {Top}, left: {Left}, bottom: {Bottom}, right: {Right})]";
        }
    }
}
