namespace MovieStreaming.Common.Messages
{
    public class StopMovieMessage
    {
        public int UserId { get; set; }

        public StopMovieMessage(int userId)
        {
            UserId = userId;
        }
    }
}
