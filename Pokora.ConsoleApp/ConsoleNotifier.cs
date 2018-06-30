using System;
using System.Collections.Generic;
using System.Text;
using Pokora.Cards;
using Pokora.ConsoleApp.Display;
using Pokora.GameMechanisms;

namespace Pokora.ConsoleApp
{
    public class ConsoleNotifier : INotifier
    {
        private readonly EventManager _eventManager;
        private readonly PokoraDisplayer _pokoraDisplayer;

        public ConsoleNotifier(EventManager eventManager, PokoraDisplayer pokoraDisplayer)
        {
            _eventManager = eventManager;
            _pokoraDisplayer = pokoraDisplayer;
        }

        public void TableCreated(int smallBlind, int bigBlind)
        {
            _pokoraDisplayer.UpdateDisplay();
            _eventManager.RaiseEvent($"Table {smallBlind}/{bigBlind} created");
        }

        public void DealerPlaced(string playerName)
        {
            _pokoraDisplayer.UpdateDisplay();
            _eventManager.RaiseEvent($"{playerName} is the dealer");
        }

        public void PlayerJoined(string playerName, double cash)
        {
            _pokoraDisplayer.UpdateDisplay();
            _eventManager.RaiseEvent($"{playerName} joined the table with {cash}€");
        }

        public void PlayerWin(string playerName)
        {
            _pokoraDisplayer.UpdateDisplay();
            _eventManager.RaiseEvent($"{playerName} won the table");
        }

        public void PlayerLose(string playerName)
        {
            _pokoraDisplayer.UpdateDisplay();
            _eventManager.RaiseEvent($"{playerName} lost the table");
        }

        public void PlayerCashChanged(string playerName, double amount)
        {
            _pokoraDisplayer.UpdateDisplay();
            _eventManager.RaiseEvent($"{playerName} cash is now {amount}€");
        }

        public void PlayerBidChanged(string playerName, double amount)
        {
            _pokoraDisplayer.UpdateDisplay();
            _eventManager.RaiseEvent($"{playerName} bid is now {amount}€");

        }

        public void PlayerStateChanged(string playerName, PlayerState playerState)
        {
            _pokoraDisplayer.UpdateDisplay();
            _eventManager.RaiseEvent($"{playerName} state is now {playerState}");
        }

        public void PlayerAvailableActionsChanged(string playerName, IList<PlayerAction> actions)
        {
            _pokoraDisplayer.UpdateDisplay();
        }

        public void PotsUpdated(IList<Pot> pots)
        {
            _pokoraDisplayer.UpdateDisplay();
        }

        public void RoundStarted(string name)
        {
            _pokoraDisplayer.UpdateDisplay();
            _eventManager.RaiseEvent($"{name} started");
        }

        public void RoundEnded(string name)
        {
            _pokoraDisplayer.UpdateDisplay();
            _eventManager.RaiseEvent($"{name} ended");
        }

        public void PlayersWinPots(IList<string> playerNames, Pot pot, IList<CardCombination> combinations)
        {
            _pokoraDisplayer.UpdateDisplay();
            _eventManager.RaiseEvent($"{string.Join(" and ",playerNames)} won the pot of {pot.Amount}");
        }

        public void PlayerTurnBegin(string playerName)
        {
            _pokoraDisplayer.UpdateDisplay();
            _eventManager.RaiseEvent($"It is now {playerName}'s turn");
        }

        public void PlayerTurnEnd(string playerName)
        {
            _pokoraDisplayer.UpdateDisplay();
            _eventManager.RaiseEvent($"{playerName}'s turn is over");
        }

        public void PlayerActionDone(string playerName, PlayerAction action)
        {
            _pokoraDisplayer.UpdateDisplay();
            _eventManager.RaiseEvent($"{playerName} {action.State} for {action.Amount}");
        }

        public void PlayerCardsReceived(string playerName, PlayerHand playerHand)
        {
            _pokoraDisplayer.UpdateDisplay();
            _eventManager.RaiseEvent($"{playerName} received new hand");

        }

        public void CardRevealed(Card card)
        {
            _pokoraDisplayer.UpdateDisplay();
            _eventManager.RaiseEvent($"{card.BuildStringFromCard()} has been drawn");
        }

        public void GameStartedWith(IList<string> playersInGame)
        {
            _eventManager.RaiseEvent($"a new game starts with{string.Join(" and ", playersInGame)}");
        }

        public void GameEnded()
        {
            _pokoraDisplayer.UpdateDisplay();
            _eventManager.RaiseEvent($"game ended");
        }

        public void CardsCleared()
        {
            _pokoraDisplayer.UpdateDisplay();
            _eventManager.RaiseEvent($"cards regrouped");
        }

        public void DeckShuffled()
        {
            _pokoraDisplayer.UpdateDisplay();
            _eventManager.RaiseEvent($"deck shuffled");
        }
    }
}
