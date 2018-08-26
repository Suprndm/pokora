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
                    Controller =new ProbalisticController(Learner.Instance.GetGoodAreas())
                },
                new User(200000)
                {
                    Name = "Corail",
                    Controller =new ProbalisticController(Learner.Instance.GetJsonAreas("{\"WinRate\":56.000000000000007,\"EllipticAreas\":{\"Fold\":{\"U\":0.629,\"V\":0.623,\"R\":0.106,\"A\":0.149,\"B\":0.547},\"Check\":{\"U\":0.59,\"V\":0.781,\"R\":0.33,\"A\":0.765,\"B\":0.21},\"Call\":{\"U\":0.715,\"V\":0.669,\"R\":0.288,\"A\":0.918,\"B\":0.272},\"Bet\":{\"U\":0.868,\"V\":0.085,\"R\":0.207,\"A\":0.963,\"B\":0.418},\"Raise\":{\"U\":0.306,\"V\":0.392,\"R\":0.06,\"A\":0.9,\"B\":0.142},\"AllIn\":{\"U\":0.467,\"V\":0.25,\"R\":0.085,\"A\":0.095,\"B\":0.773}}}"))
                    //Controller =new ProbalisticController(Learner.Instance.GenerateNewElipticAreas())
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
