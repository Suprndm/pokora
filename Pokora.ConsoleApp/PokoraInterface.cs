using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pokora.ConsoleApp.Configuration;
using Pokora.ConsoleApp.Display;
using Pokora.ConsoleApp.PlayerControllers;
using Pokora.IA;
using Pokora.SpinAndGo;

namespace Pokora.ConsoleApp
{
    public class PokoraInterface
    {
        private readonly Logger _logger;
        private readonly PokoraDisplayer _displayer;
        private IDictionary<User, int> _wins;

        private SpinAndGoGame _spinAndGoGame;

        private readonly ConsoleNotifier _consoleNotifier;
        private readonly EventManager _eventManager;
        private readonly int _simulationCount = 1000;

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
            _displayer.SetConsoleDisplayState(false);
            _eventManager.EventReceived += _eventManager_EventReceived;

            for (int i = 0; i < 100000; i++)
            {

                Learner.Instance.GenerateNewVariableSet();

                Console.WriteLine($"New SET for iteration :{i}");
                try
                {

                    double totalPaid = 0;
                    double totalEarn = 0;

                    _wins = new Dictionary<User, int>();

                    var users = new List<User>
            {
                new User(100)
                {
                    Name = "Tommy",
                    Controller = new Clever2Controller()
                },
                new User(100)
                {
                    Name = "Ratchet",
                    Controller = new Clever1Controller()
                },
                new User(100)
                {
                    Name = "Corail",
                    Controller = new ProbalisticController()
                },
            };

                    _wins.Add(users[0], 0);
                    _wins.Add(users[1], 0);
                    _wins.Add(users[2], 0);

                    _displayer.SetupDisplay(users);
          
                    var spinAngGoCount = 0;
                    do
                    {
                        spinAngGoCount++;
                        //Console.WriteLine(spinAngGoCount + " ");

                        _spinAndGoGame = new SpinAndGoGame(1, _consoleNotifier, spinAngGoCount);
                        _displayer.SetupGameDisplay(_spinAndGoGame);

                        _spinAndGoGame.Setup(users);

                        await _logger.LogMessageAsync("Setting up interface");

                        var winner = await _spinAndGoGame.LaunchAsync();

                        _wins[winner]++;

                        winner.Earn(_spinAndGoGame.Prize);

                        totalEarn += _spinAndGoGame.Prize;
                        totalPaid += _spinAndGoGame.Fee * 3;

                    } while (users.All(user => user.Cash - _spinAndGoGame.Fee >= 0) && spinAngGoCount < _simulationCount);

                    _displayer.SetConsoleDisplayState(false);
                    _displayer.UpdateDisplay();

                    Console.WriteLine($"TotalPaid : {totalPaid}");
                    Console.WriteLine($"TotalEarn : {totalEarn}");
                    Console.WriteLine($"Rake : {totalEarn / totalPaid * 100} %");
                    Console.WriteLine($"Winrates : {string.Join(" | ", _wins.ToList().Select(kvp => $"{kvp.Key.Name}: {(double)kvp.Value / spinAngGoCount * 100}%"))}");
                    Learner.Instance.SaveTableResults((double)_wins.Single(win => win.Key == users[2]).Value / spinAngGoCount * 100);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                if (i % 500 == 0)
                {
                    Learner.Instance.DumpResults(i);
                }
            }

            Learner.Instance.DumpResults(0);
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
