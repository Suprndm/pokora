using System;
using System.Collections.Generic;
using System.Linq;

namespace Pokora
{
    public class Player
    {
        public event Action<PlayerAction> ActionGiven;
        public Player(string name, double cash, IPlayerController controller)
        {
            Name = name;
            Cash = cash;
            Controller = controller;

            Controller.ActionReceived += Controller_ActionReceived;
        }


        public string Name { get; }
        public double Cash { get; private set; }
        public double Bid { get; private set; }
        public  PlayerState State { get; set; }
        public IPlayerController Controller { get; }
        public PlayerHand Hand { get; private set; }

        private IList<PlayerAction> _playerAvailableActions;
        private bool _isPlayerTurn;

        public void Pay(double amount)
        {
            ValidateAmount(amount);

            if (Cash<amount)
                throw new Exception($"Cannot pay {amount}. Current balance is {Cash}");

            Cash += -amount;

            Bid += amount;
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

        private void ValidateAmount(double amount)
        {
            if(amount<=0) throw new Exception($"Amount should be positive - current : {amount}");
        }

        public void GetAvailableActions(IList<PlayerAction> availableActions)
        {
            _playerAvailableActions = availableActions;
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
            Controller.NotifyTurn();
        }

        public void EndTurn()
        {
            _isPlayerTurn = false;
        }

        public void GiveHand(Card card1, Card card2)
        {
            Hand = new PlayerHand(card1, card2);
        }
    }
}
