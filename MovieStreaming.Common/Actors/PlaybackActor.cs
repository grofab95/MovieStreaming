using Akka.Actor;
using Akka.Event;
using MovieStreaming.Common.Messages;
using Serilog;
using System;

namespace MovieStreaming.Common.Actors
{
    public class PlaybackActor : ReceiveActor
    {
        private ILoggingAdapter _logger = Context.GetLogger();

        public PlaybackActor()
        {
            Context.ActorOf(Props.Create<UserCoordinatorActor>(), "UserCoordinator");
            Context.ActorOf(Props.Create<PlaybackStatisticsActor>(), "PlaybackStatistics");
        }

        protected override void PreStart()
        {
             _logger.Debug("PlaybackActor PreStart");
        }

        protected override void PostStop()
        {
             _logger.Debug("PlaybackActor PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
             _logger.Debug("PlaybackActor PreRestart because: " + reason);
            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
             _logger.Debug("PlaybackActor PostRestart because: " + reason);
            base.PostRestart(reason);
        }

        private void HandlePlayMovieMessage(PlayMovieMessage message)
        {
              _logger.Debug($"Title: {message.MovieTitle}, id: {message.UserId}");
        }
    }
}
