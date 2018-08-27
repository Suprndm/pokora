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


        public async Task StartSoloGame()
        {
            _eventManager.EventReceived += _eventManager_EventReceived;

            try
            {

                double totalPaid = 0;
                double totalEarn = 0;
                var wins = new Dictionary<User, int>();
                var users = new List<User>
                {
                    new User(20)
                    {
                        Name = "Tommy",
                        Controller =new ProbalisticController(Learner.Instance.GetJsonAreas("{\"WinRate\":70.5,\"EllipticAreas\":{\"Fold\":{\"Angle\":3.9968039870670142,\"U\":0.861,\"V\":0.124,\"R\":0.072,\"A\":0.961,\"B\":0.246},\"Check\":{\"Angle\":1.0122909661567112,\"U\":0.748,\"V\":0.055,\"R\":0.032,\"A\":0.739,\"B\":0.704},\"Call\":{\"Angle\":0.36651914291880922,\"U\":0.6,\"V\":0.868,\"R\":0.082,\"A\":0.656,\"B\":0.542},\"Bet\":{\"Angle\":5.0789081233034983,\"U\":0.28,\"V\":0.671,\"R\":0.121,\"A\":0.032,\"B\":0.688},\"Raise\":{\"Angle\":3.8048177693476379,\"U\":0.41,\"V\":0.538,\"R\":0.297,\"A\":0.629,\"B\":0.973},\"AllIn\":{\"Angle\":4.1887902047863905,\"U\":0.034,\"V\":0.726,\"R\":0.342,\"A\":0.903,\"B\":0.259}}}"), true)
                    },
                    new User(20)
                    {
                        Name = "Ratchet",
                        Controller =new ProbalisticController(Learner.Instance.GetJsonAreas("{\"WinRate\":70.5,\"EllipticAreas\":{\"Fold\":{\"Angle\":3.9968039870670142,\"U\":0.861,\"V\":0.124,\"R\":0.072,\"A\":0.961,\"B\":0.246},\"Check\":{\"Angle\":1.0122909661567112,\"U\":0.748,\"V\":0.055,\"R\":0.032,\"A\":0.739,\"B\":0.704},\"Call\":{\"Angle\":0.36651914291880922,\"U\":0.6,\"V\":0.868,\"R\":0.082,\"A\":0.656,\"B\":0.542},\"Bet\":{\"Angle\":5.0789081233034983,\"U\":0.28,\"V\":0.671,\"R\":0.121,\"A\":0.032,\"B\":0.688},\"Raise\":{\"Angle\":3.8048177693476379,\"U\":0.41,\"V\":0.538,\"R\":0.297,\"A\":0.629,\"B\":0.973},\"AllIn\":{\"Angle\":4.1887902047863905,\"U\":0.034,\"V\":0.726,\"R\":0.342,\"A\":0.903,\"B\":0.259}}}"), true)
                    },
                    new User(20)
                    {
                        Name = "Corail",
                        Controller = new ConsolePlayerController()
                    },
                };

                wins.Add(users[0], 0);
                wins.Add(users[1], 0);
                wins.Add(users[2], 0);

                _displayer.IsSoloGame = true;
                _displayer.SetupDisplay(users);

                var spinAngGoCount = 0;
                SpinAndGoGame spinAndGoGame = null;
                do
                {
                    spinAngGoCount++;

                    spinAndGoGame = new SpinAndGoGame(1, _consoleNotifier, spinAngGoCount);
                    _displayer.SetupGameDisplay(spinAndGoGame);
                    spinAndGoGame.Setup(users.ToList());


                    var winner = spinAndGoGame.LaunchAsync();
                    if (wins.ContainsKey(winner))
                    {
                        wins[winner]++;
                    }

                    winner.Earn(spinAndGoGame.Prize);

                    totalEarn += spinAndGoGame.Prize;
                    totalPaid += spinAndGoGame.Fee * 3;
                } while (users.All(user => user.Cash - spinAndGoGame.Fee >= 0));

                _displayer.UpdateDisplay();

                Console.WriteLine($"TotalPaid : {totalPaid}");
                Console.WriteLine($"TotalEarn : {totalEarn}");
                Console.WriteLine($"Rake : {totalEarn / totalPaid * 100} %");
                Console.WriteLine($": Winrates : {string.Join(" | ", wins.ToList().Select(kvp => $"{kvp.Key.Name}: {(double)kvp.Value / spinAngGoCount * 100}%"))}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
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
                                Name = "PCE1.4",
                                Controller =new ProbalisticController(Learner.Instance.GetJsonAreas("{\"WinRate\":58.199999999999996,\"EllipticAreas\":{\"Fold\":{\"Angle\":4.4854961776254267,\"U\":0.057,\"V\":0.455,\"R\":0.45,\"A\":0.801,\"B\":0.327},\"Check\":{\"Angle\":2.4260076602721181,\"U\":0.762,\"V\":0.261,\"R\":0.383,\"A\":0.71,\"B\":0.488},\"Call\":{\"Angle\":1.0821041362364843,\"U\":0.19,\"V\":0.03,\"R\":0.299,\"A\":0.232,\"B\":0.745},\"Bet\":{\"Angle\":6.0737457969402664,\"U\":0.665,\"V\":0.186,\"R\":0.24,\"A\":0.252,\"B\":0.567},\"Raise\":{\"Angle\":1.3264502315156903,\"U\":0.207,\"V\":0.671,\"R\":0.193,\"A\":0.037,\"B\":0.007},\"AllIn\":{\"Angle\":4.4680428851054836,\"U\":0.7,\"V\":0.195,\"R\":0.373,\"A\":0.539,\"B\":0.984}}}"), true)
                            },
                            //new User(200000)
                            //{
                            //    Name = "PCE1.5",
                            //    Controller =new ProbalisticController(Learner.Instance.GetJsonAreas("{\"WinRate\":55.95,\"EllipticAreas\":{\"Fold\":{\"Angle\":0.94247779607693793,\"U\":0.419,\"V\":0.746,\"R\":0.463,\"A\":0.613,\"B\":0.54},\"Check\":{\"Angle\":0.0,\"U\":0.181,\"V\":0.258,\"R\":0.497,\"A\":0.402,\"B\":0.868},\"Call\":{\"Angle\":5.0614548307835552,\"U\":0.522,\"V\":0.194,\"R\":0.439,\"A\":0.647,\"B\":0.875},\"Bet\":{\"Angle\":2.1293016874330819,\"U\":0.155,\"V\":0.355,\"R\":0.258,\"A\":0.842,\"B\":0.097},\"Raise\":{\"Angle\":0.10471975511965977,\"U\":0.821,\"V\":0.78,\"R\":0.319,\"A\":0.982,\"B\":0.672},\"AllIn\":{\"Angle\":6.1784655520599268,\"U\":0.526,\"V\":0.511,\"R\":0.145,\"A\":0.909,\"B\":0.567}}}"), true)
                            //},
                            new User(200000)
                            {
                                Name = "Clever 2",
                                Controller =new Clever2Controller()
                            },
                            new User(200000)
                            {
                                Name = "Clever 22",
                                Controller =new Clever2Controller()
                            },
                            new User(200000)
                            {
                                Name = "Clever 222",
                                Controller =new Clever2Controller()
                            },
                            //new User(200000)
                            //{
                            //    Name = "PCE1.2",
                            //    Controller =new ProbalisticController(Learner.Instance.GetJsonAreas("{\"WinRate\":62.3,\"EllipticAreas\":{\"Fold\":{\"Angle\":0.62831853071795862,\"U\":0.711,\"V\":0.815,\"R\":0.107,\"A\":0.506,\"B\":0.894},\"Check\":{\"Angle\":0.715584993317675,\"U\":0.995,\"V\":0.425,\"R\":0.07,\"A\":0.91,\"B\":0.693},\"Call\":{\"Angle\":6.0039326268604931,\"U\":0.68,\"V\":0.13,\"R\":0.425,\"A\":0.884,\"B\":0.694},\"Bet\":{\"Angle\":0.052359877559829883,\"U\":0.553,\"V\":0.079,\"R\":0.087,\"A\":0.891,\"B\":0.665},\"Raise\":{\"Angle\":5.2883476335428181,\"U\":0.816,\"V\":0.718,\"R\":0.219,\"A\":0.963,\"B\":0.718},\"AllIn\":{\"Angle\":2.9146998508305306,\"U\":0.052,\"V\":0.88,\"R\":0.374,\"A\":0.544,\"B\":0.942}}}"), true)
                            //},
                            new User(200000)
                            {
                                Name = "PCE1.3",
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
                                Name = "PCE1.4_bis",
                                Controller =new ProbalisticController(Learner.Instance.GetJsonAreas("{\"WinRate\":58.199999999999996,\"EllipticAreas\":{\"Fold\":{\"Angle\":4.4854961776254267,\"U\":0.057,\"V\":0.455,\"R\":0.45,\"A\":0.801,\"B\":0.327},\"Check\":{\"Angle\":2.4260076602721181,\"U\":0.762,\"V\":0.261,\"R\":0.383,\"A\":0.71,\"B\":0.488},\"Call\":{\"Angle\":1.0821041362364843,\"U\":0.19,\"V\":0.03,\"R\":0.299,\"A\":0.232,\"B\":0.745},\"Bet\":{\"Angle\":6.0737457969402664,\"U\":0.665,\"V\":0.186,\"R\":0.24,\"A\":0.252,\"B\":0.567},\"Raise\":{\"Angle\":1.3264502315156903,\"U\":0.207,\"V\":0.671,\"R\":0.193,\"A\":0.037,\"B\":0.007},\"AllIn\":{\"Angle\":4.4680428851054836,\"U\":0.7,\"V\":0.195,\"R\":0.373,\"A\":0.539,\"B\":0.984}}}"), true)
                            },
                            //new User(200000)
                            //{
                            //    Name = "PCE1.5_bis",
                            //    Controller =new ProbalisticController(Learner.Instance.GetJsonAreas("{\"WinRate\":55.95,\"EllipticAreas\":{\"Fold\":{\"Angle\":0.94247779607693793,\"U\":0.419,\"V\":0.746,\"R\":0.463,\"A\":0.613,\"B\":0.54},\"Check\":{\"Angle\":0.0,\"U\":0.181,\"V\":0.258,\"R\":0.497,\"A\":0.402,\"B\":0.868},\"Call\":{\"Angle\":5.0614548307835552,\"U\":0.522,\"V\":0.194,\"R\":0.439,\"A\":0.647,\"B\":0.875},\"Bet\":{\"Angle\":2.1293016874330819,\"U\":0.155,\"V\":0.355,\"R\":0.258,\"A\":0.842,\"B\":0.097},\"Raise\":{\"Angle\":0.10471975511965977,\"U\":0.821,\"V\":0.78,\"R\":0.319,\"A\":0.982,\"B\":0.672},\"AllIn\":{\"Angle\":6.1784655520599268,\"U\":0.526,\"V\":0.511,\"R\":0.145,\"A\":0.909,\"B\":0.567}}}"), true)
                            //},
                            new User(200000)
                            {
                                Name = "Clever 2_bis",
                                Controller =new Clever2Controller()
                            },
                            new User(200000)
                            {
                                Name = "Clever 22_bis",
                                Controller =new Clever2Controller()
                            },
                            new User(200000)
                            {
                                Name = "Clever 222_bis",
                                Controller =new Clever2Controller()
                            },
                            //new User(200000)
                            //{
                            //    Name = "PCE1.2_bis",
                            //    Controller =new ProbalisticController(Learner.Instance.GetJsonAreas("{\"WinRate\":62.3,\"EllipticAreas\":{\"Fold\":{\"Angle\":0.62831853071795862,\"U\":0.711,\"V\":0.815,\"R\":0.107,\"A\":0.506,\"B\":0.894},\"Check\":{\"Angle\":0.715584993317675,\"U\":0.995,\"V\":0.425,\"R\":0.07,\"A\":0.91,\"B\":0.693},\"Call\":{\"Angle\":6.0039326268604931,\"U\":0.68,\"V\":0.13,\"R\":0.425,\"A\":0.884,\"B\":0.694},\"Bet\":{\"Angle\":0.052359877559829883,\"U\":0.553,\"V\":0.079,\"R\":0.087,\"A\":0.891,\"B\":0.665},\"Raise\":{\"Angle\":5.2883476335428181,\"U\":0.816,\"V\":0.718,\"R\":0.219,\"A\":0.963,\"B\":0.718},\"AllIn\":{\"Angle\":2.9146998508305306,\"U\":0.052,\"V\":0.88,\"R\":0.374,\"A\":0.544,\"B\":0.942}}}"), true)
                            //},
                             new User(200000)
                             {
                                 Name = "PCE1.3_bis",
                                 Controller =new ProbalisticController(Learner.Instance.GetJsonAreas("{\"WinRate\":70.5,\"EllipticAreas\":{\"Fold\":{\"Angle\":3.9968039870670142,\"U\":0.861,\"V\":0.124,\"R\":0.072,\"A\":0.961,\"B\":0.246},\"Check\":{\"Angle\":1.0122909661567112,\"U\":0.748,\"V\":0.055,\"R\":0.032,\"A\":0.739,\"B\":0.704},\"Call\":{\"Angle\":0.36651914291880922,\"U\":0.6,\"V\":0.868,\"R\":0.082,\"A\":0.656,\"B\":0.542},\"Bet\":{\"Angle\":5.0789081233034983,\"U\":0.28,\"V\":0.671,\"R\":0.121,\"A\":0.032,\"B\":0.688},\"Raise\":{\"Angle\":3.8048177693476379,\"U\":0.41,\"V\":0.538,\"R\":0.297,\"A\":0.629,\"B\":0.973},\"AllIn\":{\"Angle\":4.1887902047863905,\"U\":0.034,\"V\":0.726,\"R\":0.342,\"A\":0.903,\"B\":0.259}}}"), true)
                             },
                            new User(200000)
                            {
                                Name = "Delphine_bis",
                                Controller =new ProbalisticController(Learner.Instance.GetGoodAreas())
                            },
                    };

                    var areas = Learner.Instance.GenerateNewElipticAreas();
                    try
                    {

                        double totalPaid = 0;
                        double totalEarn = 0;

                        var wins = new Dictionary<User, int>();

                        var corail = new User(200000)
                        {
                            Name = "Corail",
                            //Controller = new ProbalisticController(Learner.Instance.GetJsonAreas("{\"WinRate\":58.199999999999996,\"EllipticAreas\":{\"Fold\":{\"Angle\":4.4854961776254267,\"U\":0.057,\"V\":0.455,\"R\":0.45,\"A\":0.801,\"B\":0.327},\"Check\":{\"Angle\":2.4260076602721181,\"U\":0.762,\"V\":0.261,\"R\":0.383,\"A\":0.71,\"B\":0.488},\"Call\":{\"Angle\":1.0821041362364843,\"U\":0.19,\"V\":0.03,\"R\":0.299,\"A\":0.232,\"B\":0.745},\"Bet\":{\"Angle\":6.0737457969402664,\"U\":0.665,\"V\":0.186,\"R\":0.24,\"A\":0.252,\"B\":0.567},\"Raise\":{\"Angle\":1.3264502315156903,\"U\":0.207,\"V\":0.671,\"R\":0.193,\"A\":0.037,\"B\":0.007},\"AllIn\":{\"Angle\":4.4680428851054836,\"U\":0.7,\"V\":0.195,\"R\":0.373,\"A\":0.539,\"B\":0.984}}}"), true)
                            Controller = new ProbalisticController(areas, true)
                        };

                        wins.Add(corail, 0);


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


        public async Task WhosTheBest()
        {
            _displayer.IsDisabled = true;
            _displayer.SetConsoleDisplayState(false);
            _eventManager.EventReceived += _eventManager_EventReceived;
            Parallel.ForEach(Enumerable.Range(1, 10000000), new ParallelOptions { MaxDegreeOfParallelism = 4 }, (count) =>
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
                                Controller =new ProbalisticController(Learner.Instance.GetGoodAreas())
                                //Controller =new ProbalisticController(Learner.Instance.GetJsonAreas("{\"WinRate\":62.3,\"EllipticAreas\":{\"Fold\":{\"Angle\":0.62831853071795862,\"U\":0.711,\"V\":0.815,\"R\":0.107,\"A\":0.506,\"B\":0.894},\"Check\":{\"Angle\":0.715584993317675,\"U\":0.995,\"V\":0.425,\"R\":0.07,\"A\":0.91,\"B\":0.693},\"Call\":{\"Angle\":6.0039326268604931,\"U\":0.68,\"V\":0.13,\"R\":0.425,\"A\":0.884,\"B\":0.694},\"Bet\":{\"Angle\":0.052359877559829883,\"U\":0.553,\"V\":0.079,\"R\":0.087,\"A\":0.891,\"B\":0.665},\"Raise\":{\"Angle\":5.2883476335428181,\"U\":0.816,\"V\":0.718,\"R\":0.219,\"A\":0.963,\"B\":0.718},\"AllIn\":{\"Angle\":2.9146998508305306,\"U\":0.052,\"V\":0.88,\"R\":0.374,\"A\":0.544,\"B\":0.942}}}"), true)
                            },
                            new User(200000)
                            {
                                Name = "Corail",
                                Controller =new ProbalisticController(Learner.Instance.GetJsonAreas("{\"WinRate\":50.55,\"EllipticAreas\":{\"Fold\":{\"Angle\":4.6251225177849733,\"U\":0.308,\"V\":0.833,\"R\":0.489,\"A\":0.871,\"B\":0.842},\"Check\":{\"Angle\":0.36651914291880922,\"U\":0.911,\"V\":0.297,\"R\":0.479,\"A\":0.419,\"B\":0.221},\"Call\":{\"Angle\":1.1344640137963142,\"U\":0.435,\"V\":0.246,\"R\":0.085,\"A\":0.014,\"B\":0.91},\"Bet\":{\"Angle\":4.39822971502571,\"U\":0.987,\"V\":0.981,\"R\":0.425,\"A\":0.845,\"B\":0.36},\"Raise\":{\"Angle\":1.902408884673819,\"U\":0.725,\"V\":0.676,\"R\":0.464,\"A\":0.543,\"B\":0.704},\"AllIn\":{\"Angle\":4.6251225177849733,\"U\":0.407,\"V\":0.642,\"R\":0.159,\"A\":0.96,\"B\":0.947}}}"), true)
                            },
                };

                    wins.Add(users[0], 0);
                    wins.Add(users[1], 0);
                    wins.Add(users[2], 0);


                    var spinAngGoCount = 0;
                    SpinAndGoGame spinAndGoGame = null;
                    do
                    {
                        spinAngGoCount++;

                        spinAndGoGame = new SpinAndGoGame(1, _consoleNotifier, spinAngGoCount);

                        spinAndGoGame.Setup(users.ToList());


                        var winner = spinAndGoGame.LaunchAsync();

                        wins[winner]++;

                        winner.Earn(spinAndGoGame.Prize);

                        totalEarn += spinAndGoGame.Prize;
                        totalPaid += spinAndGoGame.Fee * 3;

                    } while (users.All(user => user.Cash - spinAndGoGame.Fee >= 0) && spinAngGoCount < 1000);

                    Console.WriteLine($"Iteration {count} : Winrates : {string.Join(" | ", wins.ToList().Select(kvp => $"{kvp.Key.Name}: {(double)kvp.Value / spinAngGoCount * 100}%"))}");

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });


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
