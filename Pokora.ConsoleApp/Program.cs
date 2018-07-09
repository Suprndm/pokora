using System;
using System.Threading.Tasks;
using Unity;

namespace Pokora.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().Wait();
        }

        static async Task MainAsync()
        {
            try
            {
                var container = await Bootstrapper.RegisterConfiguration();
                var pokoraInterface = container.Resolve<PokoraInterface>();
                var notifier = container.Resolve<ConsoleNotifier>();
                await pokoraInterface.Start();

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
