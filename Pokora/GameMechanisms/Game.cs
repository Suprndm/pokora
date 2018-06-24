using System;
using System.Collections.Generic;
using System.Linq;
using Pokora.GameMechanisms.Rounds;
using Pokora.Poker;

namespace Pokora.GameMechanisms
{
    public class Game
    {
        public int SmallBlind { get; }
        public int BigBlind { get; }
        public IList<Player> Players { get; set; }
        public Deck Deck { get; }
        public TableCards Cards { get; }
        private IList<Pot> _pots;
        private IList<IRound> _rounds;
        private IRound _currentRound;
        private readonly INotifier _notifier;
        public event Action GameEnded;
        private CombinationEvaluator _combinationEvaluator;

        public Game(int smallBlind, int bigBlind, Deck deck, INotifier notifier)
        {
            SmallBlind = smallBlind;
            BigBlind = bigBlind;
            Deck = deck;
            _notifier = notifier;
            Cards = new TableCards(_notifier);
            _pots = new List<Pot>();
            _combinationEvaluator = new CombinationEvaluator();
            _rounds = new List<IRound>()
            {
                new HandsRound(smallBlind, bigBlind, deck, Cards,_notifier),
                new FlopRound(smallBlind, bigBlind, deck, Cards,_notifier),
                new FourStraightRound(smallBlind, bigBlind, deck, Cards,_notifier),
                new RiverRound(smallBlind, bigBlind, deck, Cards, _notifier)
            };
        }

        public void Start(IList<Player> players, Player dealer)
        {
            _notifier.GameStartedWith(players.Select(p => p.Name).ToList());
            Deck.Regroup();
            Deck.Shuffle();
            _notifier.DeckShuffled();

            OrderPlayers(players, dealer);
            _currentRound = _rounds[0];
            StartRound();
        }

        public void TransmitAction(PlayerAction playerAction)
        {
            _currentRound.HandleAction(playerAction);
        }

        private void StartRound()
        {
            _currentRound.RoundEnded += _currentRound_RoundEnded;
            _currentRound.Start(Players);
        }

        private void _currentRound_RoundEnded(Player lastPlayerToPlay)
        {
            _currentRound.RoundEnded -= _currentRound_RoundEnded;

            HandlePots();

            if (EvalGameEnd())
            {
                ResolvePots();


                _notifier.GameEnded();
                GameEnded?.Invoke();
            }
            else
            {
                ReorderPlayers(lastPlayerToPlay);
                ResetPlayersState();
                PickNextRound();
                StartRound();
            }
        }

        private bool EvalGameEnd()
        {
            if (Players.Count(player => player.State != PlayerState.Fold) == 1 || _rounds.Last() == _currentRound)
                return true;
            else return false;
        }

        private void ResetPlayersState()
        {
            foreach (var player in Players)
            {
                player.State = PlayerState.None;
            }
        }

        private void PickNextRound()
        {
            var indexOfCurrentRound = _rounds.IndexOf(_currentRound);
            _currentRound = _rounds[indexOfCurrentRound + 1];
        }

        private void HandlePots()
        {
            foreach (var previousPot in _pots)
            {
                foreach (var player in previousPot.Participants.ToList().Where(p => p.State == PlayerState.Fold))
                {
                    previousPot.RemoveParticipant(player);
                }
            }

            var allInPlayers = Players.Where(p => p.State == PlayerState.AllIn).OrderBy(p => p.Bid).ToList();
            var playersToCompute = Players.ToList();
            foreach (var allInPlayer in allInPlayers)
            {
                var sidePot = new Pot();
                var bid = allInPlayer.TakeAllBid();
                sidePot.Earn(bid);
                sidePot.DeclareParticipant(allInPlayer);
                playersToCompute.Remove(allInPlayer);
                foreach (var player in playersToCompute)
                {
                    bid = player.TakePartOfTheBid(bid);
                    sidePot.Earn(bid);
                    if (player.State != PlayerState.Fold)
                        sidePot.DeclareParticipant(player);
                }

                _pots.Add(sidePot);
            }

            var pot = new Pot();
            foreach (var player in playersToCompute)
            {
                var bid = player.TakeAllBid();
                pot.Earn(bid);
                if (player.State != PlayerState.Fold)
                    pot.DeclareParticipant(player);
            }

            _pots.Add(pot);

            _notifier.PotsUpdated(_pots);
        }

        private void ReorderPlayers(Player lastPlayerToPlay)
        {
            var indexOfLastPlayer = Players.IndexOf(lastPlayerToPlay);
            var nextPlayer = Players[(indexOfLastPlayer + 1) % Players.Count];

            while (nextPlayer.State == PlayerState.AllIn || nextPlayer.State == PlayerState.Fold)
            {
                indexOfLastPlayer++;
                nextPlayer = Players[(indexOfLastPlayer + 1) % Players.Count];
            }

            Players = Players.Where(p => p.State != PlayerState.AllIn && p.State != PlayerState.Fold).ToList();
            OrderPlayers(Players, nextPlayer);
        }

        private void OrderPlayers(IList<Player> players, Player dealer)
        {
            var dealerIndex = players.IndexOf(dealer);
            var firstPlayerIndex = dealerIndex;
            Players = new List<Player>();
            Players.Add(Players[firstPlayerIndex]);

            for (int i = 1; i < players.Count; i++)
            {
                var playerIndex = (dealerIndex + i) % players.Count;
                Players.Add(Players[playerIndex]);
            }
        }

        private void ResolvePots()
        {
            if (Players.Count(player => player.State != PlayerState.Fold) == 1)
            {
                var winner = Players.Single(player => player.State != PlayerState.Fold);
                foreach (var pot in _pots)
                {
                    pot.DeclareWinners(new List<Player>() { winner });
                    _notifier.PlayersWinPots(new List<string> { winner.Name }, pot, null);
                }
            }
            else
            {
                foreach (var pot in _pots)
                {
                    var results = new Dictionary<Player, CardCombination>();
                    foreach (var player in pot.Participants)
                    {
                        var playerCardsCombination = new List<Card>()
                        {
                            player.Hand.Card1,
                            player.Hand.Card2,
                        };

                        playerCardsCombination = playerCardsCombination.Concat(Cards.Cards).ToList();

                        results.Add(player, _combinationEvaluator.EvaluateCards(playerCardsCombination));
                    }

                    var resultList = results.OrderByDescending(result => result.Value.Score).ToList();

                    var winners = new List<Player>();
                    var winnersHands = new List<CardCombination>();
                    winners.Add(resultList[0].Key);
                    winnersHands.Add(resultList[0].Value);
                    for (int i = 1; i < results.Count - 1; i++)
                    {
                        if (resultList[i].Value.Score == resultList[0].Value.Score)
                        {
                            winners.Add(resultList[i].Key);
                            winnersHands.Add(resultList[i].Value);

                        }
                    }

                    pot.DeclareWinners(winners);
                    _notifier.PlayersWinPots(winners.Select(p=>p.Name).ToList(), pot, winnersHands);
                }
            }
        }
    }
}
