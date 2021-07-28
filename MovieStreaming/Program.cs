using Akka.Actor;
using MovieStreaming.Common.Actors;
using MovieStreaming.Common.Messages;
using Serilog;
using Serilog.Events;
using System;

namespace MovieStreaming
{
    class Program
    {
        private static ActorSystem MovieStreamingActorSystem;

        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    .CreateLogger();

            Console.WriteLine("Creating MovieStreamingActorSystem");
            MovieStreamingActorSystem = ActorSystem.Create("MovieStreamingActorSystem");
            MovieStreamingActorSystem.ActorOf(Props.Create<PlaybackActor>(), "Playback");
            do
            {
                try
                {
                     Console.WriteLine("");
                     Console.WriteLine("enter a command and hit enter");

                    var command = Console.ReadLine();

                    if (command.StartsWith("play"))
                    {
                        var userId = int.Parse(command.Split(',')[1]);
                        var movieTitile = command.Split(',')[2];

                        var message = new PlayMovieMessage(movieTitile, userId);
                        MovieStreamingActorSystem.ActorSelection("/user/Playback/UserCoordinator").Tell(message);
                    }

                    if (command.StartsWith("stop"))
                    {
                        var userId = int.Parse(command.Split(',')[1]);

                        var message = new StopMovieMessage(userId);
                        MovieStreamingActorSystem.ActorSelection("/user/Playback/UserCoordinator").Tell(message);
                    }

                    if (command == "exit")
                    {
                        MovieStreamingActorSystem.Terminate().Wait();
                        Console.WriteLine("_movieStreamingActorSystem terminated ... ");
                        Console.ReadKey();
                        Environment.Exit(1);
                    }
                }
                catch (Exception ex)
                {
                     Log.Fatal($"ERROR: {ex.Message}");
                }                
            } 
            while (true);            
        }
    }
}
