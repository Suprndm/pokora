using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pokora.ConsoleApp.Configuration;
using Pokora.ConsoleApp.Display;
using Pokora.ConsoleApp.PlayerControllers;
using Pokora.SpinAndGo;

namespace Pokora.ConsoleApp
{
    public class PokoraInterface
    {
        private readonly Logger _logger;
        private readonly PokoraDisplayer _displayer;


        private SpinAndGoGame _spinAndGoGame;

        private readonly ConsoleNotifier _consoleNotifier;
        private readonly EventManager _eventManager;
        private readonly int _simulationCount = 10000;

        public PokoraInterface(
            Logger logger,
            PokoraDisplayer displayer,
            ConsoleNotifier consoleNotifier,
            EventManager eventManager)
        {
            _logger = logger;
            _displayer = displayer;
            _consoleNotifier = consoleNotifier;
            _eventManager = eventManager;
        }



        public async Task Start()
        {

            double totalPaid = 0;
            double totalEarn = 0;

            var users = new List<User>
            {
                new User(1000)
                {
                    Name = "Tommy",
                    Controller = new AllinController()
                },
                new User(1000)
                {
                    Name = "Ratchet",
                    Controller = new AllinController()
                },
                new User(1000)
                {
                    Name = "Corail",
                    Controller = new AllinController()
                },
            };

            _displayer.SetupDisplay(users);
            _displayer.SetConsoleDisplayState(false);
            _eventManager.EventReceived += _eventManager_EventReceived;
            var spinAngGoCount = 0;
            do
            {
                spinAngGoCount++;
                Console.WriteLine(spinAngGoCount + " ");

                _spinAndGoGame = new SpinAndGoGame(1, _consoleNotifier, spinAngGoCount);
                _displayer.SetupGameDisplay(_spinAndGoGame);

                _spinAndGoGame.Setup(users);

                await _logger.LogMessageAsync("Setting up interface");

                var winner = await _spinAndGoGame.LaunchAsync();

                winner.Earn(_spinAndGoGame.Prize);

                totalEarn += _spinAndGoGame.Prize;
                totalPaid += _spinAndGoGame.Fee *3;

            } while (users.All(user => user.Cash - _spinAndGoGame.Fee >= 0) && spinAngGoCount < _simulationCount);

            _displayer.SetConsoleDisplayState(true);
            _displayer.UpdateDisplay();

            Console.WriteLine($"TotalPaid : {totalPaid}");
            Console.WriteLine($"TotalEarn : {totalEarn}");
            Console.WriteLine($"Rake : {totalEarn/ totalPaid *100} %");
        }

        public void Setup(ConsoleNotifier notifier)
        {

        }

        private void _eventManager_EventReceived(string eventMessage)
        {
            _displayer.PushEvent(eventMessage);
        }
    }
}
