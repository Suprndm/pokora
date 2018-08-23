﻿using System;
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
            _displayer.IsDisabled = false;
            _displayer.SetConsoleDisplayState(true);
            _eventManager.EventReceived += _eventManager_EventReceived;

            Parallel.ForEach(Enumerable.Range(0, 10000000),new ParallelOptions { MaxDegreeOfParallelism = 1}, (count) =>
            {
                Learner.Instance.GenerateNewElipticAreas();

                Console.WriteLine($"New SET for iteration :{count}");
                try
                {

                    double totalPaid = 0;
                    double totalEarn = 0;

                    var wins = new Dictionary<User, int>();

                    var users = new List<User>
            {
                new User(60)
                {
                    Name = "Tommy",
                    Controller = new Clever2Controller()
                },
                new User(60)
                {
                    Name = "Ratchet",
                    Controller = new Clever1Controller()
                },
                new User(60)
                {
                    Name = "Corail",
                    Controller = new ProbalisticController()
                },
            };

                     wins.Add(users[0], 0);
                     wins.Add(users[1], 0);
                     wins.Add(users[2], 0);

                    _displayer.SetupDisplay(users);

                    var spinAngGoCount = 0;
                    SpinAndGoGame spinAndGoGame = null;
                    do
                    {
                        spinAngGoCount++;
                        //Console.WriteLine(spinAngGoCount + " ");

                        spinAndGoGame = new SpinAndGoGame(1, _consoleNotifier, spinAngGoCount);
                        _displayer.SetupGameDisplay(spinAndGoGame);

                        spinAndGoGame.Setup(users);


                        var winner = spinAndGoGame.LaunchAsync().Result;

                        wins[winner]++;

                        winner.Earn(spinAndGoGame.Prize);

                        totalEarn += spinAndGoGame.Prize;
                        totalPaid += spinAndGoGame.Fee * 3;

                    } while (users.All(user => user.Cash - spinAndGoGame.Fee >= 0));

                    //_displayer.SetConsoleDisplayState(false);
                    _displayer.UpdateDisplay();

                    //Console.WriteLine($"TotalPaid : {totalPaid}");
                    //Console.WriteLine($"TotalEarn : {totalEarn}");
                    //Console.WriteLine($"Rake : {totalEarn / totalPaid * 100} %");
                    Console.WriteLine($"Iteration {count} : Winrates : {string.Join(" | ", wins.ToList().Select(kvp => $"{kvp.Key.Name}: {(double)kvp.Value / spinAngGoCount * 100}%"))}");
                    Learner.Instance.SaveTableResults((double)wins.Single(win => win.Key == users[2]).Value / spinAngGoCount * 100);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                if (count % 5000 == 0)
                {
                    Learner.Instance.DumpResults(count);
                }

            });


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
