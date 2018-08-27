using System;
using System.Collections.Concurrent;
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
                    var opponentsPool1 = new ConcurrentBag<User>()
            {
                            new User(200000)
                            {
                                Name = "Tommy",
                                Controller = new Clever1Controller()
                            },
                            new User(200000)
                            {
                                Name = "Ratchet",
                                Controller = new Clever1Controller()
                                //Controller =new ProbalisticController(Learner.Instance.GetGoodAreas())
                            },
                            new User(200000)
                            {
                                Name = "Armand",
                                Controller =new ProbalisticController(Learner.Instance.GetJsonAreas("{\"WinRate\":62.3,\"EllipticAreas\":{\"Fold\":{\"Angle\":0.62831853071795862,\"U\":0.711,\"V\":0.815,\"R\":0.107,\"A\":0.506,\"B\":0.894},\"Check\":{\"Angle\":0.715584993317675,\"U\":0.995,\"V\":0.425,\"R\":0.07,\"A\":0.91,\"B\":0.693},\"Call\":{\"Angle\":6.0039326268604931,\"U\":0.68,\"V\":0.13,\"R\":0.425,\"A\":0.884,\"B\":0.694},\"Bet\":{\"Angle\":0.052359877559829883,\"U\":0.553,\"V\":0.079,\"R\":0.087,\"A\":0.891,\"B\":0.665},\"Raise\":{\"Angle\":5.2883476335428181,\"U\":0.816,\"V\":0.718,\"R\":0.219,\"A\":0.963,\"B\":0.718},\"AllIn\":{\"Angle\":2.9146998508305306,\"U\":0.052,\"V\":0.88,\"R\":0.374,\"A\":0.544,\"B\":0.942}}}"), true)
                            },
                            new User(200000)
                            {
                                Name = "Topaz",
                                Controller =new ProbalisticController(Learner.Instance.GetJsonAreas("{\"WinRate\":70.5,\"EllipticAreas\":{\"Fold\":{\"Angle\":3.9968039870670142,\"U\":0.861,\"V\":0.124,\"R\":0.072,\"A\":0.961,\"B\":0.246},\"Check\":{\"Angle\":1.0122909661567112,\"U\":0.748,\"V\":0.055,\"R\":0.032,\"A\":0.739,\"B\":0.704},\"Call\":{\"Angle\":0.36651914291880922,\"U\":0.6,\"V\":0.868,\"R\":0.082,\"A\":0.656,\"B\":0.542},\"Bet\":{\"Angle\":5.0789081233034983,\"U\":0.28,\"V\":0.671,\"R\":0.121,\"A\":0.032,\"B\":0.688},\"Raise\":{\"Angle\":3.8048177693476379,\"U\":0.41,\"V\":0.538,\"R\":0.297,\"A\":0.629,\"B\":0.973},\"AllIn\":{\"Angle\":4.1887902047863905,\"U\":0.034,\"V\":0.726,\"R\":0.342,\"A\":0.903,\"B\":0.259}}}"), true)
                            },
                            new User(200000)
                            {
                                Name = "Delphine",
                                Controller =new ProbalisticController(Learner.Instance.GetGoodAreas())
                            },
            };

                    var opponentsPool2 = new ConcurrentBag<User>()
            {
                            new User(200000)
                            {
                                Name = "Tommy_2",
                                Controller = new Clever1Controller()
                            },
                            new User(200000)
                            {
                                Name = "Ratchet_2",
                                Controller = new Clever1Controller()
                                //Controller =new ProbalisticController(Learner.Instance.GetGoodAreas())
                            },
                            new User(200000)
                            {
                                Name = "Armand_2",
                                Controller =new ProbalisticController(Learner.Instance.GetJsonAreas("{\"WinRate\":62.3,\"EllipticAreas\":{\"Fold\":{\"Angle\":0.62831853071795862,\"U\":0.711,\"V\":0.815,\"R\":0.107,\"A\":0.506,\"B\":0.894},\"Check\":{\"Angle\":0.715584993317675,\"U\":0.995,\"V\":0.425,\"R\":0.07,\"A\":0.91,\"B\":0.693},\"Call\":{\"Angle\":6.0039326268604931,\"U\":0.68,\"V\":0.13,\"R\":0.425,\"A\":0.884,\"B\":0.694},\"Bet\":{\"Angle\":0.052359877559829883,\"U\":0.553,\"V\":0.079,\"R\":0.087,\"A\":0.891,\"B\":0.665},\"Raise\":{\"Angle\":5.2883476335428181,\"U\":0.816,\"V\":0.718,\"R\":0.219,\"A\":0.963,\"B\":0.718},\"AllIn\":{\"Angle\":2.9146998508305306,\"U\":0.052,\"V\":0.88,\"R\":0.374,\"A\":0.544,\"B\":0.942}}}"), true)
                            },
                            new User(200000)
                            {
                                Name = "Topaz_2",
                                Controller =new ProbalisticController(Learner.Instance.GetJsonAreas("{\"WinRate\":70.5,\"EllipticAreas\":{\"Fold\":{\"Angle\":3.9968039870670142,\"U\":0.861,\"V\":0.124,\"R\":0.072,\"A\":0.961,\"B\":0.246},\"Check\":{\"Angle\":1.0122909661567112,\"U\":0.748,\"V\":0.055,\"R\":0.032,\"A\":0.739,\"B\":0.704},\"Call\":{\"Angle\":0.36651914291880922,\"U\":0.6,\"V\":0.868,\"R\":0.082,\"A\":0.656,\"B\":0.542},\"Bet\":{\"Angle\":5.0789081233034983,\"U\":0.28,\"V\":0.671,\"R\":0.121,\"A\":0.032,\"B\":0.688},\"Raise\":{\"Angle\":3.8048177693476379,\"U\":0.41,\"V\":0.538,\"R\":0.297,\"A\":0.629,\"B\":0.973},\"AllIn\":{\"Angle\":4.1887902047863905,\"U\":0.034,\"V\":0.726,\"R\":0.342,\"A\":0.903,\"B\":0.259}}}"), true)
                            },
                            new User(200000)
                            {
                                Name = "Delphine_2",
                                Controller =new ProbalisticController(Learner.Instance.GetGoodAreas())
                            },
            };

                    var areas = Learner.Instance.GenerateNewElipticAreas();
                    //Console.WriteLine($"New SET for iteration :{count}");
                    try
                    {

                        double totalPaid = 0;
                        double totalEarn = 0;

                        var wins = new Dictionary<User, int>();

                        var corail = new User(200000)
                        {
                            Name = "Corail",
                            Controller = new ProbalisticController(areas, true)
                        };

                        wins.Add(corail, 0);

                        //_displayer.SetupDisplay(users);

                        var spinAngGoCount = 0;
                        SpinAndGoGame spinAndGoGame = null;
                        do
                        {
                            var opponent1 = GetRandomOpponent(opponentsPool1);
                            var opponent2 = GetRandomOpponent(opponentsPool2);
                            var selectedPlayers = new List<User>();
                            selectedPlayers.Add(opponent1);
                            selectedPlayers.Add(opponent2);
                            selectedPlayers.Add(corail);
                            spinAngGoCount++;

                            spinAndGoGame = new SpinAndGoGame(1, _consoleNotifier, spinAngGoCount);
                            //_displayer.SetupGameDisplay(spinAndGoGame);
                            //Console.WriteLine(string.Join("|", selectedPlayers.Select(p => p.Name)));
                            spinAndGoGame.Setup(selectedPlayers.ToList());


                            var winner = spinAndGoGame.LaunchAsync();
                            //Console.WriteLine($"Table { spinAngGoCount}, gameCount : {spinAndGoGame.GetGameCount()}");
                            if (wins.ContainsKey(winner))
                            {
                                wins[winner]++;
                            }

                            winner.Earn(spinAndGoGame.Prize);

                            totalEarn += spinAndGoGame.Prize;
                            totalPaid += spinAndGoGame.Fee * 3;
                        } while (spinAngGoCount < 2000);

                        //_displayer.SetConsoleDisplayState(false);
                        //_displayer.UpdateDisplay();

                        //Console.WriteLine($"TotalPaid : {totalPaid}");
                        //Console.WriteLine($"TotalEarn : {totalEarn}");
                        //Console.WriteLine($"Rake : {totalEarn / totalPaid * 100} %");
                        Console.WriteLine($"Iteration {count} : Winrates : {string.Join(" | ", wins.ToList().Select(kvp => $"{kvp.Key.Name}: {(double)kvp.Value / spinAngGoCount * 100}%"))}");

                        Learner.Instance.SaveTableResults(areas, (double)wins[corail] / spinAngGoCount * 100);
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

        private User GetRandomOpponent(ConcurrentBag<User> opponents)
        {
            var op = opponents.ToArray()[StaticRandom.Rand(opponents.Count)];
            return op;
        }

        private void _eventManager_EventReceived(string eventMessage)
        {
            _displayer.PushEvent(eventMessage);
        }
    }
}
