using Akka.Actor;
using Akka.Event;
using MovieStreaming.Common.Exceptions;
using Serilog;
using System;

namespace MovieStreaming.Common.Actors
{
    public class PlaybackStatisticsActor : ReceiveActor
    {
        private ILoggingAdapter _logger = Context.GetLogger();

        public PlaybackStatisticsActor()
        {
            Context.ActorOf(Props.Create<MoviePlayCounterActor>(), "MoviePlayCounter");
        }

        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(
                    ex => 
                    {
                        if (ex is SimulatedCorruptStateException)
                        {
                            _logger.Error("SimulatedCorruptStateException was throw");
                            return Directive.Restart;
                        }
                        if (ex is SimulatedTerribleMovieException)
                        {
                            _logger.Error("SimulatedTerribleMovieException was throw");
                            return Directive.Resume;
                        }

                        return Directive.Restart;
                    }
                );
        }

        protected override void PreStart()
        {
             _logger.Debug("PlaybackStatisticsActor PreStart");
        }

        protected override void PostStop()
        {
             _logger.Debug("PlaybackStatisticsActor PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
             _logger.Debug("PlaybackStatisticsActor PreRestart because: " + reason);
             base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
             _logger.Debug("PlaybackStatisticsActor PostRestart because: " + reason);
             base.PostRestart(reason);
        }
    }
}
