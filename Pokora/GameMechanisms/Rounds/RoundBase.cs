using System;
using System.Collections.Generic;
using System.Linq;

namespace Pokora.GameMechanisms.Rounds
{
    public abstract class RoundBase : IRound
    {
        public int SmallBlind { get; }
        public int BigBlind { get; }
        public IList<Player> Players { get; set; }
        private Player _currentPlayer;
        public Deck Deck { get; }
        public TableCards Cards { get; }

        protected abstract string RoundName { get; }

        private readonly INotifier _notifier;

        protected RoundBase(int smallBlind, int bigBlind, Deck deck, TableCards cards, INotifier notifier)
        {
            SmallBlind = smallBlind;
            BigBlind = bigBlind;
            Deck = deck;
            Cards = cards;
            _notifier = notifier;
        }

        public abstract void Setup();

        public event Action<Player> RoundEnded;

        public void Start(IList<Player> players)
        {
            Players = players;
            ResetPlayersStateIfNeeded();
            _notifier.RoundStarted(RoundName);
            Setup();
            _currentPlayer = Players.First();

            ComputePlayersActions();

            var roundEnds = EvalRoundEnd();

            if (!roundEnds)
            {
                _currentPlayer.StartTurn();
            }
            else
            {
                _notifier.RoundEnded(RoundName);
                RoundEnded?.Invoke(_currentPlayer);
            }
        }

        public void HandleAction(PlayerAction playerAction)
        {
            //Console.ReadKey(true);
            HandlePlayerAction(playerAction);
            _notifier.PlayerActionDone(playerAction.Player.Name, playerAction);
            ComputePlayersActions();
            var roundEnds = EvalRoundEnd();

            if (!roundEnds)
            {
                NextTurn();
            }
            else
            {
                _notifier.RoundEnded(RoundName);
                RoundEnded?.Invoke(_currentPlayer);
            }
        }

        private void ResetPlayersStateIfNeeded()
        {

            foreach (var player in Players.Where(p => p.State != PlayerState.AllIn && p.State != PlayerState.Fold))
            {
                player.State = PlayerState.None;
            }
        }

        private void NextTurn()
        {
            var nextPlayer =
                Players[(Players.IndexOf(_currentPlayer) + 1) % Players.Count];
            while (nextPlayer.State == PlayerState.Fold || nextPlayer.State == PlayerState.AllIn)
            {
                nextPlayer =
                    Players[(Players.IndexOf(nextPlayer) + 1) % Players.Count];
            }

            _currentPlayer = nextPlayer;
            nextPlayer.StartTurn();
        }

        private void HandlePlayerAction(PlayerAction playerAction)
        {
            var player = playerAction.Player;
            switch (playerAction.State)
            {
                case PlayerState.None:
                    throw new Exception("None is an invalid action to Take");
                    break;
                case PlayerState.Fold:
                    break;
                case PlayerState.Call:
                    player.Pay(playerAction.Amount);
                    break;
                case PlayerState.Check:
                    break;
                case PlayerState.Bet:
                    player.Pay(playerAction.Amount);
                    break;
                case PlayerState.Raise:
                    player.Pay(playerAction.Amount); break;
                case PlayerState.AllIn:
                    player.Pay(player.Cash); break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (player.Cash == 0 && playerAction.State != PlayerState.Fold)
            {
                player.State = PlayerState.AllIn;
            }
            else
            {
                player.State = playerAction.State;
            }
        }

        private void ComputePlayersActions()
        {
            var maxBid = Players.Max(player => player.Bid);
            foreach (var player in Players)
            {
                var availableActions = new List<PlayerAction>();
                if (player.State == PlayerState.AllIn || player.State == PlayerState.Fold)
                {

                }
                else
                {
                    availableActions.Add(new PlayerAction(player, PlayerState.Fold, 0, 0));
                    if (player.Bid < maxBid)
                    {
                        availableActions.Add(new PlayerAction(player, PlayerState.Call, maxBid - player.Bid, maxBid - player.Bid, maxBid - player.Bid));
                        if (player.Cash > maxBid - player.Bid)
                        {
                            availableActions.Add(new PlayerAction(player, PlayerState.Raise, (maxBid - player.Bid) * 2, player.Cash));
                        } 

                        availableActions.Add(new PlayerAction(player, PlayerState.AllIn, player.Cash, player.Cash, player.Cash));
                    }
                    else
                    {
                        if (player.Cash == 0)
                        {

                        }
                        availableActions.Add(new PlayerAction(player, PlayerState.Check, 0, 0));
                        availableActions.Add(new PlayerAction(player, PlayerState.Bet, SmallBlind, player.Cash));
                        availableActions.Add(new PlayerAction(player, PlayerState.AllIn, player.Cash, player.Cash));
                    }
                }

                player.GetAvailableActions(availableActions);
            }
        }

        private bool EvalRoundEnd()
        {
            var maxBid = Players.Max(player => player.Bid);
            var playersRoundOverCount = Players.Count(player => player.IsRoundOver(maxBid));
            if (playersRoundOverCount == Players.Count)
                return true;


            if (playersRoundOverCount == Players.Count - 1)
            {
                var lastPlayer = Players.Single(player => !player.IsRoundOver(maxBid));

                if (lastPlayer.State == PlayerState.None && lastPlayer.Bid == maxBid && Players.Count(player=>player.IsOut())== Players.Count - 1)
                    return true;

                if (Players.Count(player =>
                        player.State == PlayerState.Fold || (player.State == PlayerState.AllIn && player.Bid == 0)) ==
                    Players.Count - 1)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
