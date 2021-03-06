﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pokora.Utils;

namespace Pokora.GameMechanisms
{
    public class Table : IDisposable
    {
        public Table(int smallBlind, int bigBlind, double initialCash, int seatsCount, INotifier notifier)
        {
            _initialSmallBlind = smallBlind;
            _initialBigBlind = bigBlind;
            SmallBlind = smallBlind;
            BigBlind = bigBlind;
            InitialCash = initialCash;
            SeatsCount = seatsCount;
            _notifier = notifier;
            Players = new List<Player>();
            _satPlayers = new List<Player>();
            _deck = new Deck();
        }

        public int SmallBlind { get; private set; }
        public int BigBlind { get; private set; }
        public int SeatsCount { get; }
        public double InitialCash { get; private set; }
        private IList<Player> _satPlayers;
        public IList<Player> Players { get; set; }
        private int _dealerIndex;
        private Deck _deck;
        private Game _game;
        private INotifier _notifier;
        public int GameCount { get; set; }
        private int _initialSmallBlind;
        private int _initialBigBlind;

        public Game CurrentGame => _game;

        public virtual void Join(string userName, IPlayerController controller)
        {
            if (Players.Count >= SeatsCount)
                throw new Exception($"Table is full - {userName} cannot join");

            if (Players.Any(p => p.Name == userName))
                throw new Exception($"{userName} is already in the room");

            //if (userName == "Tommy")
            //{
            //    InitialCash = 0;
            //}

            //if (userName == "Ratchet")
            //{
            //    InitialCash = 10;
            //}

            //if (userName == "Corail")
            //{
            //    InitialCash = 2000;
            //}


            var player = new Player(userName, InitialCash, controller, _notifier, this);
            Players.Add(player);
            _satPlayers.Add(player);

            player.ActionGiven += Player_ActionGiven;
        }

        private void Player_ActionGiven(PlayerAction playerAction)
        {
            _game.TransmitAction(playerAction);
        }

        public Player Start()
        {
            GameCount++;
            if (Players.Count < SeatsCount)
                throw new Exception($"Table is not full yet. Need {SeatsCount - Players.Count} more player(s).");
            Players.Shuffle();
            var dealerPlayer = SetupDealer();
            _game = new Game(SmallBlind, BigBlind, _deck, _notifier);
            _game.Start(Players, dealerPlayer);

            while (!EvalTableEnd())
            {
                RestartGame();
            }

            var winner = Players.Single(p => p.Cash > 0);
            winner.WinTable();

            return winner;
        }

        private void RestartGame()
        {
            try
            {

                GameCount++;
                var nextDealer = GetNextDealer();
                foreach (var player in Players)
                {
                    if (player.Cash == 0)
                        player.LoseTable();
                }

                if (GameCount % 10 == 0)
                {
                    SmallBlind = SmallBlind * 2;
                    BigBlind = BigBlind * 2;

                    //Console.WriteLine($"Small Blind :{SmallBlind}");
                }
                Players = Players.Where(p => p.Cash > 0).ToList();
                _game = new Game(SmallBlind, BigBlind, _deck, _notifier);
                _game.Start(Players, nextDealer);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        private Player SetupDealer()
        {
            _dealerIndex = StaticRandom.Rand(SeatsCount);
            _notifier.DealerPlaced(Players[_dealerIndex].Name);
            return Players[_dealerIndex];
        }

        private Player GetNextDealer()
        {
            var count = 0;
            Player nextDealer = null;
            do
            {
                count++;
                nextDealer = Players[(_dealerIndex + count) % Players.Count];
            } while (nextDealer.Cash == 0);

            _notifier.DealerPlaced(nextDealer.Name);
            _dealerIndex = Players.IndexOf(nextDealer);
            return nextDealer;
        }

        private bool EvalTableEnd()
        {
            return Players.Count(p => p.Cash > 0) == 1;
        }

        public void Dispose()
        {
            foreach (var player in _satPlayers)
            {
                player.Dispose();
            }
        }
    }
}
