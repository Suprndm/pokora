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
            _displayer.IsDisabled = true;
            _displayer.SetConsoleDisplayState(false);
            _eventManager.EventReceived += _eventManager_EventReceived;
            Parallel.ForEach(Enumerable.Range(1, 10000000), new ParallelOptions { MaxDegreeOfParallelism = 16 }, (count) =>
                {
                    var areas = Learner.Instance.GenerateNewElipticAreas();
                    //Console.WriteLine($"New SET for iteration :{count}");
                    try
                    {

                        double totalPaid = 0;
                        double totalEarn = 0;

                        var wins = new Dictionary<User, int>();

                        var users = new List<User>
                        {
                            new User(200000)
                            {
                                Name = "Tommy",
                                Controller = new Clever2Controller()
                            },
                            new User(200000)
                            {
                                Name = "Ratchet",
                                Controller =new ProbalisticController(Learner.Instance.GetJsonAreas("{\"WinRate\":62.3,\"EllipticAreas\":{\"Fold\":{\"U\":0.89,\"V\":0.838,\"R\":0.442,\"A\":0.585,\"B\":0.891},\"Check\":{\"U\":0.922,\"V\":0.976,\"R\":0.213,\"A\":0.52,\"B\":0.39},\"Call\":{\"U\":0.797,\"V\":0.907,\"R\":0.012,\"A\":0.23,\"B\":0.204},\"Bet\":{\"U\":0.342,\"V\":0.001,\"R\":0.473,\"A\":0.035,\"B\":0.144},\"Raise\":{\"U\":0.604,\"V\":0.214,\"R\":0.088,\"A\":0.774,\"B\":0.779},\"AllIn\":{\"U\":0.296,\"V\":0.435,\"R\":0.336,\"A\":0.493,\"B\":0.264}}}"))
                            },
                            new User(200000)
                            {
                                Name = "Corail",
                                //Controller =new ProbalisticController(Learner.Instance.GetJsonAreas("{\"WinRate\":62.3,\"EllipticAreas\":{\"Fold\":{\"U\":0.89,\"V\":0.838,\"R\":0.442,\"A\":0.585,\"B\":0.891},\"Check\":{\"U\":0.922,\"V\":0.976,\"R\":0.213,\"A\":0.52,\"B\":0.39},\"Call\":{\"U\":0.797,\"V\":0.907,\"R\":0.012,\"A\":0.23,\"B\":0.204},\"Bet\":{\"U\":0.342,\"V\":0.001,\"R\":0.473,\"A\":0.035,\"B\":0.144},\"Raise\":{\"U\":0.604,\"V\":0.214,\"R\":0.088,\"A\":0.774,\"B\":0.779},\"AllIn\":{\"U\":0.296,\"V\":0.435,\"R\":0.336,\"A\":0.493,\"B\":0.264}}}"))
                                Controller =new ProbalisticController(areas, true)
                            },
                };

                        wins.Add(users[0], 0);
                        wins.Add(users[1], 0);
                        wins.Add(users[2], 0);

                        //_displayer.SetupDisplay(users);

                        var spinAngGoCount = 0;
                        SpinAndGoGame spinAndGoGame = null;
                        do
                        {
                            spinAngGoCount++;

                            spinAndGoGame = new SpinAndGoGame(1, _consoleNotifier, spinAngGoCount);
                            //_displayer.SetupGameDisplay(spinAndGoGame);

                            spinAndGoGame.Setup(users.ToList());


                            var winner = spinAndGoGame.LaunchAsync();
                            //Console.WriteLine($"Table { spinAngGoCount}, gameCount : {spinAndGoGame.GetGameCount()}");

                            wins[winner]++;

                            winner.Earn(spinAndGoGame.Prize);

                            totalEarn += spinAndGoGame.Prize;
                            totalPaid += spinAndGoGame.Fee * 3;

                        } while (users.All(user => user.Cash - spinAndGoGame.Fee >= 0) && spinAngGoCount < 1000);

                        //_displayer.SetConsoleDisplayState(false);
                        //_displayer.UpdateDisplay();

                        //Console.WriteLine($"TotalPaid : {totalPaid}");
                        //Console.WriteLine($"TotalEarn : {totalEarn}");
                        //Console.WriteLine($"Rake : {totalEarn / totalPaid * 100} %");
                        Console.WriteLine($"Iteration {count} : Winrates : {string.Join(" | ", wins.ToList().Select(kvp => $"{kvp.Key.Name}: {(double)kvp.Value / spinAngGoCount * 100}%"))}");

                        Learner.Instance.SaveTableResults(areas, (double)wins.Single(win => win.Key == users.Single(u => u.Name == "Corail")).Value / spinAngGoCount * 100);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    if (count % 2000 == 0)
                    {
                        Learner.Instance.DumpResults(count);
                    }

                });


            Learner.Instance.DumpResults(100);
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
