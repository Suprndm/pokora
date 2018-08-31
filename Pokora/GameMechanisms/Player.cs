using System;
using System.Collections.Generic;
using System.Linq;

namespace Pokora.GameMechanisms
{
    public class Player : IDisposable
    {
        private readonly INotifier _notifier;
        public event Action<PlayerAction> ActionGiven;
        private readonly Guid _guid;
        private readonly Table _table;

        public Player(string name, double cash, IPlayerController controller, INotifier notifier, Table table)
        {
            _guid = Guid.NewGuid();

            _notifier = notifier;
            _table = table;
            Name = name;
            Cash = cash;
            Controller = controller;
            Controller.LinkPlayer(this, table);
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

        public bool IsOut()
        {
            if (State == PlayerState.Fold) return true;
            if (State == PlayerState.AllIn) return true;

            return false;
        }

        public bool IsRoundOver(double maxBid)
        {
            if (State == PlayerState.Fold) return true;
            if (State == PlayerState.AllIn) return true;
            if (Bid == maxBid && State != PlayerState.None) return true;

            return false;
        }

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

            if (Cash <= amount)
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
                var part = Bid;
                Bid = 0;
                return part;
            }
        }

        private void ValidateAmount(double amount)
        {
            if (amount <= 0) throw new Exception($"Amount should be positive - current : {amount}");
        }

        private void Controller_ActionReceived(PlayerAction action)
        {


            if (_isPlayerTurn == false) throw new Exception($"It is not {Name}'s turn");

            if (_playerAvailableActions.All(a => a.State != action.State))
                throw new Exception($"{action} is not available for {Name}({_guid})");

            ActionGiven?.Invoke(action);
        }

        public PlayerAction StartTurn(IList<PlayerAction> actions)
        {
            _isPlayerTurn = true;
            _notifier.PlayerTurnBegin(this, actions);
            return Controller.Play(actions);
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

        public override string ToString()
        {
            return Name;
        }

        public void Dispose()
        {
        }
    }
}
