using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Unity;

namespace Pokora.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().Wait();
            Console.Read();
        }

        static async Task MainAsync()
        {
            try
            {

                IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", true, true)
                    .Build();
                var parallelism = int.Parse(config["Settings:Parallelism"]);

                var container = await Bootstrapper.RegisterConfiguration();
                var pokoraInterface = container.Resolve<PokoraInterface>();
                var notifier = container.Resolve<ConsoleNotifier>();
                await pokoraInterface.Start(parallelism);

                Console.WriteLine("Game finished ?");
                Console.Read();
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occured will setting up application: {e}");
                Console.ReadKey(true);
            }
        }
    }
}
