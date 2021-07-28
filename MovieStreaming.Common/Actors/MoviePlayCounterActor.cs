using Akka.Actor;
using Akka.Event;
using MovieStreaming.Common.Exceptions;
using MovieStreaming.Common.Messages;
using Serilog;
using System;
using System.Collections.Generic;

namespace MovieStreaming.Common.Actors
{
    public class MoviePlayCounterActor : ReceiveActor
    {
        private readonly Dictionary<string, int> _moviePlayCounts;
        private ILoggingAdapter _logger = Context.GetLogger();

        public MoviePlayCounterActor()
        {
            _moviePlayCounts = new();
            Receive<IncrementPlayCountMessage>(HandleIncrementMessage);
        }

        private void HandleIncrementMessage(IncrementPlayCountMessage obj)
        {
            if (_moviePlayCounts.ContainsKey(obj.MovieTitle))
            {
                _moviePlayCounts[obj.MovieTitle]++;
            }
            else
            {
                _moviePlayCounts.Add(obj.MovieTitle, 1);
            }

            if (_moviePlayCounts[obj.MovieTitle] > 3)
                throw new SimulatedCorruptStateException();

            if (obj.MovieTitle == "bad")
                throw new SimulatedTerribleMovieException();

            _logger.Info($"MoviePlayCounterActor {obj.MovieTitle} has been watched {_moviePlayCounts[obj.MovieTitle]} times");
        }

        protected override void PreStart()
        {
            _logger.Debug("MoviePlayCounterActor PreStart");
        }

        protected override void PostStop()
        {
            _logger.Debug("MoviePlayCounterActor PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            _logger.Debug("MoviePlayCounterActor PreRestart because: " + reason);
            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            _logger.Debug("MoviePlayCounterActor PostRestart because: " + reason);
            base.PostRestart(reason);
        }
    }
}
