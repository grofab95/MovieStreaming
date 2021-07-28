namespace MovieStreaming.Common.Messages
{
    public class IncrementPlayCountMessage
    {
        public string MovieTitle { get; private set; }

        public IncrementPlayCountMessage(string title)
        {
            MovieTitle = title;
        }
    }
}
