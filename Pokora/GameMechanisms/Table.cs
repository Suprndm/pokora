using System;
using System.Collections.Generic;
using System.Linq;

namespace Pokora.GameMechanisms
{
    public class Table
    {
        public Table(int smallBlind, int bigBlind, double initialCash, int seatsCount, INotifier notifier)
        {
            SmallBlind = smallBlind;
            BigBlind = bigBlind;
            InitialCash = initialCash;
            SeatsCount = seatsCount;
            _notifier = notifier;
            Players = new List<Player>();
            _deck = new Deck();
        }

        public int SmallBlind { get; }
        public int BigBlind { get; }
        public int SeatsCount { get; }
        public double InitialCash { get; }

        public IList<Player> Players { get; set; }
        private int _dealerIndex;
        private Deck _deck;
        private Game _game;
        private INotifier _notifier;


        public void Join(string userName, IPlayerController controller)
        {
            if (Players.Count >= SeatsCount)
                throw new Exception($"Table is full - {userName} cannot join");

            if (Players.Any(p => p.Name == userName))
                throw new Exception($"{userName} is already in the room");

            var player = new Player(userName, InitialCash, controller, _notifier);
            Players.Add(player);

            player.ActionGiven += Player_ActionGiven;
        }

        private void Player_ActionGiven(PlayerAction playerAction)
        {
            _game.TransmitAction(playerAction);
        }

        public void Start()
        {
            if (Players.Count < SeatsCount)
                throw new Exception($"Table is not full yet. Need {SeatsCount - Players.Count} more player(s).");

            var dealerPlayer = SetupDealer();
            _game = new Game(SmallBlind, BigBlind, _deck, _notifier);
            _game.Start(Players, dealerPlayer);

            _game.GameEnded += _game_GameEnded;
        }

        private void _game_GameEnded()
        {
            _game.GameEnded -= _game_GameEnded;
            if (EvalTableEnd())
            {
                var winner = Players.Single(p => p.Cash > 0);
                winner.WinTable();
            }
            else
            {
                var nextDealer = GetNextDealer();
                foreach (var player in Players)
                {
                    if(player.Cash == 0)
                        player.LoseTable();
                }

                Players = Players.Where(p => p.Cash > 0).ToList();
                _game = new Game(SmallBlind, BigBlind, _deck, _notifier);
                _game.Start(Players, nextDealer);

                _game.GameEnded += _game_GameEnded;
            }
        }

        private Player SetupDealer()
        {
            _dealerIndex = Randomizer.Instance.Random.Next(SeatsCount);
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
            } while (nextDealer.Cash > 0);

            _notifier.DealerPlaced(nextDealer.Name);

            return nextDealer;
        }

        private bool EvalTableEnd()
        {
            return Players.Count(p => p.Cash > 0) == 1;
        }
    }
}
