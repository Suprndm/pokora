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
                                //Controller =new ProbalisticController(Learner.Instance.GetGoodAreas())
                            },
                            new User(200000)
                            {
                                Name = "Ratchet",
                                //Controller = new Clever2Controller()
                                //Controller =new ProbalisticController(Learner.Instance.GetGoodAreas())
                                Controller =new ProbalisticController(Learner.Instance.GetJsonAreas("{\"WinRate\":62.3,\"EllipticAreas\":{\"Fold\":{\"Angle\":0.62831853071795862,\"U\":0.711,\"V\":0.815,\"R\":0.107,\"A\":0.506,\"B\":0.894},\"Check\":{\"Angle\":0.715584993317675,\"U\":0.995,\"V\":0.425,\"R\":0.07,\"A\":0.91,\"B\":0.693},\"Call\":{\"Angle\":6.0039326268604931,\"U\":0.68,\"V\":0.13,\"R\":0.425,\"A\":0.884,\"B\":0.694},\"Bet\":{\"Angle\":0.052359877559829883,\"U\":0.553,\"V\":0.079,\"R\":0.087,\"A\":0.891,\"B\":0.665},\"Raise\":{\"Angle\":5.2883476335428181,\"U\":0.816,\"V\":0.718,\"R\":0.219,\"A\":0.963,\"B\":0.718},\"AllIn\":{\"Angle\":2.9146998508305306,\"U\":0.052,\"V\":0.88,\"R\":0.374,\"A\":0.544,\"B\":0.942}}}"), true)
                            },
                            new User(200000)
                            {
                                Name = "Corail",
                                //Controller =new ProbalisticController(Learner.Instance.GetJsonAreas("{\"WinRate\":62.3,\"EllipticAreas\":{\"Fold\":{\"Angle\":0.62831853071795862,\"U\":0.711,\"V\":0.815,\"R\":0.107,\"A\":0.506,\"B\":0.894},\"Check\":{\"Angle\":0.715584993317675,\"U\":0.995,\"V\":0.425,\"R\":0.07,\"A\":0.91,\"B\":0.693},\"Call\":{\"Angle\":6.0039326268604931,\"U\":0.68,\"V\":0.13,\"R\":0.425,\"A\":0.884,\"B\":0.694},\"Bet\":{\"Angle\":0.052359877559829883,\"U\":0.553,\"V\":0.079,\"R\":0.087,\"A\":0.891,\"B\":0.665},\"Raise\":{\"Angle\":5.2883476335428181,\"U\":0.816,\"V\":0.718,\"R\":0.219,\"A\":0.963,\"B\":0.718},\"AllIn\":{\"Angle\":2.9146998508305306,\"U\":0.052,\"V\":0.88,\"R\":0.374,\"A\":0.544,\"B\":0.942}}}"), true)
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

                    if (count % 500 == 0)
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
