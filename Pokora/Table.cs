using System;
using System.Collections.Generic;
using System.Linq;

namespace Pokora
{
    public class Table
    {
        public Table(int smallBlind, int bigBlind, double initialCash, int seatsCount)
        {
            SmallBlind = smallBlind;
            BigBlind = bigBlind;
            InitialCash = initialCash;
            SeatsCount = seatsCount;
            Players = new List<Player>();
            _roundOrderedPlayers = new List<Player>();
            _pots = new List<Pot>();
            _deck = new Deck();
            _cards = new TableCards();
            _combinationEvaluator = new CombinationEvaluator();
        }

        public int SmallBlind { get; }
        public int BigBlind { get; }
        public int SeatsCount { get; }
        public double InitialCash { get; }

        public IList<Player> Players { get; set; }
        private IList<Player> _roundOrderedPlayers;
        private int _dealerIndex;
        private IList<Pot> _pots;
        private Player _currentPlayer;
        private GameState _gameState;
        private Deck _deck;
        private TableCards _cards;
        private CombinationEvaluator _combinationEvaluator;

        public void Join(string userName, IPlayerController controller)
        {
            if (Players.Count >= SeatsCount)
                throw new Exception($"Table is full - {userName} cannot join");

            if (Players.Any(p => p.Name == userName))
                throw new Exception($"{userName} is already in the room");

            var player = new Player(userName, InitialCash, controller);
            Players.Add(player);

            player.ActionGiven += Player_ActionGiven;
        }

        private void Player_ActionGiven(PlayerAction playerAction)
        {
            HandlePlayerAction(playerAction);
            ComputePlayersActions();

            if (EvalGameEnd())
            {
                ResolveGame();
            }
            else if (EvalTurnsEnd())
            {
                ResolveTurn();
                AdvanceTurn();
            }
            else
            {
                WaitForNextPlayerAction();
            }
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
                    player.Pay(playerAction.Amount); break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Start()
        {
            if (Players.Count < SeatsCount)
                throw new Exception($"Table is not full yet. Need {SeatsCount - Players.Count} more player(s).");

            SetupDealer();
            OrderPlayers();
            PayBlinds();
            SetupGame();
            ComputePlayersActions();
            StartTurn();
        }

        private void SetupGame()
        {
            _deck.Shuffle();

            foreach (var player in _roundOrderedPlayers)
            {
                player.GiveHand(_deck.Draw(), _deck.Draw());
            }

            _gameState = GameState.Initial;
        }

        private void SetupDealer()
        {
            _dealerIndex = Randomizer.Instance.Random.Next(SeatsCount);
        }

        private void StartTurn()
        {
            var currentPlayer = _roundOrderedPlayers.First();
            currentPlayer.StartTurn();
            _currentPlayer = currentPlayer;
        }

        private void OrderPlayers()
        {
            var firstPlayerIndex = _dealerIndex;

            _roundOrderedPlayers.Clear();
            _roundOrderedPlayers.Add(Players[firstPlayerIndex]);

            for (int i = 1; i < SeatsCount; i++)
            {
                var playerIndex = (_dealerIndex + i) % SeatsCount;
                _roundOrderedPlayers.Add(Players[playerIndex]);
            }
        }

        private void PayBlinds()
        {
            _roundOrderedPlayers[_roundOrderedPlayers.Count - 2].Pay(SmallBlind);
            _roundOrderedPlayers[_roundOrderedPlayers.Count - 1].Pay(BigBlind);
        }

        private void ComputePlayersActions()
        {
            var maxBid = _roundOrderedPlayers.Max(player => player.Bid);
            foreach (var player in _roundOrderedPlayers)
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
                        availableActions.Add(new PlayerAction(player, PlayerState.Call, maxBid - player.Bid, maxBid - player.Bid));
                        availableActions.Add(new PlayerAction(player, PlayerState.Raise, (maxBid - player.Bid) * 2, player.Cash - 1));
                        availableActions.Add(new PlayerAction(player, PlayerState.AllIn, player.Cash, player.Cash));
                    }
                    else
                    {
                        availableActions.Add(new PlayerAction(player, PlayerState.Check, 0, 0));
                        availableActions.Add(new PlayerAction(player, PlayerState.Bet, SmallBlind, player.Cash - 1));
                        availableActions.Add(new PlayerAction(player, PlayerState.AllIn, player.Cash, player.Cash));
                    }
                }

                player.GetAvailableActions(availableActions);
            }
        }

        private bool EvalGameEnd()
        {
            if (_roundOrderedPlayers.Count(player => player.State != PlayerState.Fold) == 1)
                return true;
            else return false;
        }

        private bool EvalTurnsEnd()
        {
            var maxBid = _roundOrderedPlayers.Max(player => player.Bid);
            if (!_roundOrderedPlayers.Any(player => player.Bid < maxBid
                                                    && player.State != PlayerState.AllIn && player.State != PlayerState.Fold)
                && _roundOrderedPlayers.Any(player => player.State == PlayerState.None))
            {
                return true;
            }

            return false;
        }

        private void ExecuteFlop()
        {
            _gameState = GameState.Flop;
            _deck.Burn();
            _cards.Reveal(_deck.Draw());
            _cards.Reveal(_deck.Draw());
            _cards.Reveal(_deck.Draw());
        }

        private void ExecuteFourStraights()
        {
            _gameState = GameState.FourStraight;
            _deck.Burn();
            _cards.Reveal(_deck.Draw());

        }

        private void ExecuteRiver()
        {
            _cards.Reveal(_deck.Draw());
            _deck.Burn();
            _gameState = GameState.River;
        }

        private void AdvanceTurn()
        {
            switch (_gameState)
            {
                case GameState.Initial:
                    ExecuteFlop();
                    break;
                case GameState.Flop:
                    ExecuteFourStraights();
                    break;
                case GameState.FourStraight:
                    ExecuteRiver();
                    break;
                case GameState.River:
                    ResolveGame();
                    return;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            WaitForNextPlayerAction();
        }

        private void WaitForNextPlayerAction()
        {
            var nextPlayer =
                _roundOrderedPlayers[(_roundOrderedPlayers.IndexOf(_currentPlayer) + 1) % _roundOrderedPlayers.Count];
            while (nextPlayer.State == PlayerState.Fold || nextPlayer.State == PlayerState.AllIn)
            {
                nextPlayer =
                    _roundOrderedPlayers[(_roundOrderedPlayers.IndexOf(_currentPlayer) + 1) % _roundOrderedPlayers.Count];
            }

            _currentPlayer = nextPlayer;
            nextPlayer.StartTurn();
        }

        private void ResolveTurn()
        {
            foreach (var player in _roundOrderedPlayers)
            {
                Pot pot;
                if (_pots.Count == 0)
                {
                    pot = new Pot();
                    _pots.Add(pot);
                }
                else
                {
                    pot = _pots[0];
                }

                var bid = player.TakeAllBid();
                if (bid > 0)
                {
                    pot.Amount += bid;
                    if (!pot.Participants.Contains(player) && player.State != PlayerState.Fold)
                    {
                        pot.DeclareParticipant(player);
                    }
                }
            }
        }

        private void ResolveGame()
        {
            if (_roundOrderedPlayers.Count(player => player.State != PlayerState.Fold) == 1)
            {
               var winner = _roundOrderedPlayers.Single(player => player.State != PlayerState.Fold);
                winner.Earn(_pots[0].Amount);
            }
            else
            {
                var results = new Dictionary<Player, CardCombination>();
                foreach (var player in _roundOrderedPlayers.Where(player=>player.State!=PlayerState.Fold))
                {
                    var playerCardsCombination = new List<Card>()
                    {
                        player.Hand.Card1,
                        player.Hand.Card2,
                    };

                    playerCardsCombination = playerCardsCombination.Concat(_cards.Cards).ToList();

                    results.Add(player, _combinationEvaluator.EvaluateCards(playerCardsCombination));
                }

                var winner = results.OrderByDescending(result => result.Value.Score).First().Key;
                winner.Earn(_pots[0].Amount);
            }
        }
    }
}
