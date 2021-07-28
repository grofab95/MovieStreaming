namespace MovieStreaming.Common.Messages
{
    public class PlayMovieMessage
    {
        public string MovieTitle { get; private set; }
        public int UserId { get; private set; }

        public PlayMovieMessage(string title, int id)
        {
            MovieTitle = title;
            UserId = id;
        }
    }
}
