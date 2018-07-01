using System;
using System.Threading.Tasks;
using Monithor.Client;
using Monithor.Definitions;
using Pokora.ConsoleApp.Display;
using Unity;
using Unity.Lifetime;

namespace Pokora.ConsoleApp
{
    public static class Bootstrapper
    {
        public static async Task<IUnityContainer> RegisterConfiguration()
        {
            var emitter = new MonithorEmitter("https://monithor.azurewebsites.net/thorhub", "Pokora Console");

            bool connectionEstablished = false;

            emitter.ConnectionStatusChanged += async (status) =>
            {
                if (status == ConnectionStatus.Healthy)
                {
                    connectionEstablished = true;
                    await emitter.Trace(MessageLevel.Informational, MessageType.Fonctional,
                        "", "Pokora monithor connected",
                        "");

                }
            };

            emitter.Connect();

            while (!connectionEstablished)
            {
                Console.WriteLine("trying to connect to monithor");
                await Task.Delay(1000);
            }

           
            var container = new UnityContainer();
            var logger = new Logger(emitter);

            container.RegisterInstance(logger, new ContainerControlledLifetimeManager());

            var eventManager = new EventManager();
            container.RegisterInstance(eventManager, new ContainerControlledLifetimeManager());

            var displayer = new PokoraDisplayer();
            container.RegisterInstance(displayer, new ContainerControlledLifetimeManager());

            return container;
        } 
    }
}
