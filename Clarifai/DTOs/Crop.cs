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

        public JObject Serialize()
        {
            return new JObject(
                new JProperty("top_row", Top),
                new JProperty("left_col", Left),
                new JProperty("bottom_row", Bottom),
                new JProperty("right_col", Right));
        }

        public static Crop Deserialize(dynamic jsonObject)
        {
            return new Crop(
                (decimal)jsonObject.top_row,
                (decimal)jsonObject.left_col,
                (decimal)jsonObject.bottom_row,
                (decimal)jsonObject.right_col);
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
