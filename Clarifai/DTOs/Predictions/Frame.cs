using System.Collections.Generic;

namespace Clarifai.DTOs.Predictions
{
    public class Frame : IPrediction
    {
        public string TYPE => "frame";

        public int Index { get; }

        public long Time { get; }

        public List<Concept> Concepts { get; }

        private Frame(int index, long time, List<Concept> concepts)
        {
            Index = index;
            Time = time;
            Concepts = concepts;
        }

        public static Frame Deserialize(dynamic jsonObject)
        {
            var concepts = new List<Concept>();
            foreach (dynamic concept in jsonObject.data.concepts)
            {
                concepts.Add(Concept.Deserialize(concept));
            }
            return new Frame(
                (int)jsonObject.frame_info.index,
                (long)jsonObject.frame_info.time,
                concepts);
        }

        public override bool Equals(object obj)
        {
            return obj is Frame frame &&
                   Index == frame.Index &&
                   Time == frame.Time &&
                   EqualityComparer<List<Concept>>.Default.Equals(Concepts, frame.Concepts);
        }

        public override int GetHashCode()
        {
            var hashCode = -1744060065;
            hashCode = hashCode * -1521134295 + Index.GetHashCode();
            hashCode = hashCode * -1521134295 + Time.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Concept>>.Default.GetHashCode(Concepts);
            return hashCode;
        }

        public override string ToString()
        {
            return $"[Frame: (index: {Index}, time: {Time})]";
        }
    }
}
