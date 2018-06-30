using System;
using System.Collections.Generic;
using System.Linq;

namespace Pokora.GameMechanisms
{
    public class Player
    {
        private readonly INotifier _notifier;
        public event Action<PlayerAction> ActionGiven;
        public Player(string name, double cash, IPlayerController controller, INotifier notifier)
        {
            _notifier = notifier;
            Name = name;
            Cash = cash;
            Controller = controller;
            Controller.LinkPlayer(this);
            Controller.ActionReceived += Controller_ActionReceived;
        }

        public string Name { get; }

        public double Cash
        {
            get => _cash;
            private set
            {
                _cash = value;
                _notifier.PlayerCashChanged(Name, _cash);
            }
        }

        public double Bid
        {
            get => _bid;
            private set
            {
                _bid = value;
                _notifier.PlayerBidChanged(Name, _bid);
            }
        }

        public PlayerState State
        {
            get => _state;
            set
            {
                _state = value;
                _notifier.PlayerStateChanged(Name, State);
            }
        }

        public IPlayerController Controller { get; }

        public PlayerHand Hand
        {
            get => _hand;
            private set
            {
                _hand = value;
                _notifier.PlayerCardsReceived(Name, _hand);
            }
        }

        private IList<PlayerAction> _playerAvailableActions;
        private bool _isPlayerTurn;
        private PlayerState _state;
        private double _bid;
        private double _cash;
        private PlayerHand _hand;

        public void LoseTable()
        {
            _notifier.PlayerLose(Name);
        }

        public void WinTable()
        {
            _notifier.PlayerWin(Name);
        }

        public void Pay(double amount)
        {
            ValidateAmount(amount);

            if (Cash < amount)
            {
                State = PlayerState.AllIn;
                Bid += Cash;
                Cash = 0;
            }
            else
            {
                Cash += -amount;
                Bid += amount;
            }
        }

        public void Earn(double amount)
        {
            ValidateAmount(amount);
            Cash += amount;
        }

        public double TakeAllBid()
        {
            var currentBid = Bid;

            Bid = 0;

            return currentBid;
        }

        public double TakePartOfTheBid(double amount)
        {
            if (Bid >= amount)
            {
                Bid += -amount;
                return amount;
            }
            else
            {
                Bid = 0;
                return Bid;
            }
        }

        private void ValidateAmount(double amount)
        {
            if (amount <= 0) throw new Exception($"Amount should be positive - current : {amount}");
        }

        public void GetAvailableActions(IList<PlayerAction> availableActions)
        {
            _playerAvailableActions = availableActions;
            Controller.SendAvailableActions(_playerAvailableActions);
            _notifier.PlayerAvailableActionsChanged(Name, availableActions);
        }

        private void Controller_ActionReceived(PlayerAction action)
        {
            if (_isPlayerTurn == false) throw new Exception($"It is not {Name}'s turn");

            if (_playerAvailableActions.All(a => a.State != action.State))
                throw new Exception($"{action} is not available for {Name}");

            ActionGiven?.Invoke(action);
        }

        public void StartTurn()
        {
            _isPlayerTurn = true;
            _notifier.PlayerTurnBegin(Name);
            Controller.NotifyTurn();
        }

        public void EndTurn()
        {
            _isPlayerTurn = false;
            _notifier.PlayerTurnEnd(Name);
        }

        public void GiveHand(Card card1, Card card2)
        {
            Hand = new PlayerHand(card1, card2);
        }
    }
}
