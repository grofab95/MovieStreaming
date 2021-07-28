using Akka.Actor;
using Serilog;
using Serilog.Events;
using System;

namespace MovieStreaming.Remote
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    .CreateLogger();

            Console.WriteLine("Creating MovieStreamingActorSystem in remote process");
            using (var system = ActorSystem.Create("MovieStreamingActorSystem"))
            {
                Console.ReadLine();
            }
        }
    }
}
