namespace Clarifai.Solutions
{
    public class Solutions
    {
        public Moderation.Moderation Moderation { get; }

        public Solutions(string apiKey)
        {
            Moderation = new Moderation.Moderation(apiKey);
        }
    }
}
