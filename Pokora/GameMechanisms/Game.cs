using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public IList<Pot> Pots { get; }
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
            Pots = new List<Pot>();
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
            Task.Factory.StartNew(() =>
            {
                try
                {
                    _notifier.GameStartedWith(players.Select(p => p.Name).ToList());

                    Deck.Regroup();
                    Deck.Shuffle();


                    _notifier.DeckShuffled();

                    OrderPlayers(players, dealer);
                    ResetPlayersState();

                    _currentRound = _rounds[0];
                    StartRound();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
           
            });
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
            foreach (var previousPot in Pots)
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
                if (allInPlayer.Bid > 0)
                {
                    var sidePot = new Pot();
                    var allinBid = allInPlayer.TakeAllBid();
                    sidePot.Earn(allinBid);
                    sidePot.DeclareParticipant(allInPlayer);
                    playersToCompute.Remove(allInPlayer);
                    foreach (var player in playersToCompute)
                    {
                        var bid = player.TakePartOfTheBid(allinBid);
                        sidePot.Earn(bid);
                        if (player.State != PlayerState.Fold)
                            sidePot.DeclareParticipant(player);
                    }

                    Pots.Add(sidePot);
                }
            }

            var pot = new Pot();
            foreach (var player in playersToCompute.Where(p => p.Bid > 0))
            {
                var bid = player.TakeAllBid();
                pot.Earn(bid);
                if (player.State != PlayerState.Fold)
                    pot.DeclareParticipant(player);
            }

            if (pot.Participants.Count > 0)
            {
                var equivalantPot = Pots.FirstOrDefault(p =>
                    p.Participants.All(participant => pot.Participants.Contains(participant)));
                if (equivalantPot != null)
                {
                    equivalantPot.Earn(pot.Amount);
                }
                else
                {
                    Pots.Add(pot);
                }
            }


            _notifier.PotsUpdated(Pots);
        }

        private void ReorderPlayers(Player lastPlayerToPlay)
        {
            if (Players.All(p => p.State == PlayerState.AllIn || p.State == PlayerState.Fold))
                return;

            var indexOfLastPlayer = Players.IndexOf(lastPlayerToPlay);
            var nextPlayer = Players[(indexOfLastPlayer + 1) % Players.Count];

            while (nextPlayer.State == PlayerState.AllIn || nextPlayer.State == PlayerState.Fold)
            {
                indexOfLastPlayer++;
                nextPlayer = Players[(indexOfLastPlayer + 1) % Players.Count];
            }

         //   Players = Players.Where(p => p.State != PlayerState.AllIn && p.State != PlayerState.Fold).ToList();
            OrderPlayers(Players, nextPlayer);
        }

        private void OrderPlayers(IList<Player> players, Player dealer)
        {
            var dealerIndex = players.IndexOf(dealer);
            var firstPlayerIndex = dealerIndex;
            Players = new List<Player>();
            Players.Add(players[firstPlayerIndex]);

            for (int i = 1; i < players.Count; i++)
            {
                var playerIndex = (dealerIndex + i) % players.Count;
                Players.Add(players[playerIndex]);
            }
        }

        private void ResolvePots()
        {
            if (Players.Count(player => player.State != PlayerState.Fold) == 1)
            {
                var playerToWin = Players.Single(player => player.State != PlayerState.Fold);
                var winner = new List<KeyValuePair<Player, CardCombination>>()
                {
                    new KeyValuePair<Player, CardCombination>(playerToWin, null)
                };

                foreach (var pot in Pots)
                {
                    pot.DeclareWinners(winner);
                    _notifier.PlayersWinPots(winner, pot);
                }
            }
            else
            {
                foreach (var pot in Pots)
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

                    var winners = new List<KeyValuePair<Player,CardCombination>>();
                    winners.Add(resultList[0]);
                    if (results.Count > 1)
                    {
                        for (int i = 1; i < results.Count; i++)
                        {
                            if (resultList[i].Value.Score == resultList[0].Value.Score)
                            {
                                winners.Add(resultList[i]);
                            }
                        }
                    }

                    pot.DeclareWinners(winners);
                    _notifier.PlayersWinPots(winners, pot);
                }
            }
        }
    }
}
