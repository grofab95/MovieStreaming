using Akka.Actor;
using Akka.Event;
using MovieStreaming.Common.Messages;
using System;

namespace MovieStreaming.Common.Actors
{
    public class UserActor : ReceiveActor
    {
        private string _currentlyWatching;
        private string _userId;
        private ILoggingAdapter _logger = Context.GetLogger();

        public UserActor(int userId)
        {
            _userId = $"[User: {userId}] ";
             _logger.Debug(_userId + "Creating a UserActor");
             _logger.Debug(_userId + "Setting initial behaviour to stopped");
            Stopped();
        }

        private void Playing()
        {
            Receive<PlayMovieMessage>(msg => _logger.Warning(_userId + "Cannot start playing another movie before stopping existing one"));
            Receive<StopMovieMessage>(msg => StopPlayingCurrentMovie());

             _logger.Debug(_userId + "UserActor has now become Playing");
        }

        private void Stopped()
        {
            Receive<PlayMovieMessage>(msg => StartPlayingMovie(msg.MovieTitle));
            Receive<StopMovieMessage>(msg => _logger.Warning(_userId + "Cannot stop if nothing is playing"));

             _logger.Debug(_userId + "UserActor has now become Stopped");
        }

        private void StartPlayingMovie(string movieTitle)
        {
            _currentlyWatching = movieTitle;
            _logger.Info(_userId + $"User is currently watching {movieTitle}");

            var message = new IncrementPlayCountMessage(movieTitle);
            Context.ActorSelection("/user/Playback/PlaybackStatistics/MoviePlayCounter").Tell(message);

            Become(Playing);
        }

        private void StopPlayingCurrentMovie()
        {
            _logger.Info(_userId + $"User stopping {_currentlyWatching}");
            _currentlyWatching = null;
            Become(Stopped);
        }

        protected override void PreStart()
        {
             _logger.Debug(_userId + "UserActor PreStart");
        }

        protected override void PostStop()
        {
             _logger.Debug(_userId + "UserActor PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
             _logger.Debug(_userId + $"UserActor PreRestart because: {reason}");

            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
             _logger.Debug(_userId + $"UserActor PostRestart because: {reason}");

            base.PostRestart(reason);
        }
    }
}
