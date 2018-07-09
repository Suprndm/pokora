using System.Collections.Generic;
using System.Linq;
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
            _eventManager.RaiseEvent($"Table {smallBlind}/{bigBlind} created");
        }

        public void DealerPlaced(string playerName)
        {
            _eventManager.RaiseEvent($"{playerName} is the dealer");
        }

        public void PlayerJoined(string playerName, double cash)
        {
            _eventManager.RaiseEvent($"{playerName} joined the table with {cash}€");
        }

        public void PlayerWin(string playerName)
        {
            _eventManager.RaiseEvent($"{playerName} won the table");
        }

        public void PlayerLose(string playerName)
        {
            _eventManager.RaiseEvent($"{playerName} lost the table");
        }

        public void PlayerCashChanged(string playerName, double amount)
        {
            _eventManager.RaiseEvent($"{playerName} cash is now {amount}€");
        }

        public void PlayerBidChanged(string playerName, double amount)
        {
            _eventManager.RaiseEvent($"{playerName} bid is now {amount}€");
        }

        public void PlayerStateChanged(string playerName, PlayerState playerState)
        {
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
            _eventManager.RaiseEvent($"{name} started");
        }

        public void RoundEnded(string name)
        {
            _eventManager.RaiseEvent($"{name} ended");
        }

        public void PlayersWinPots(IList<KeyValuePair<Player, CardCombination>> winners, Pot pot)
        {
            if (winners.Count == 1 && winners[0].Value == null)
            {
                _eventManager.RaiseEvent($"{winners[0].Key.Name} won {pot.Amount / winners.Count} from the pot because every one else fold");
            }
            else
            {
                for (int i = 0; i < winners.Count; i++)
                {
                    _eventManager.RaiseEvent($"{winners[i].Key.Name} won {pot.Amount / winners.Count} from the pot with a {winners[i].Value.Type} : {CardsBuilder.BuildStringFromCards(winners[i].Value.Cards)}");
                }
            }
        }

        public void PlayerTurnBegin(Player player, IList<PlayerAction> playerActions)
        {
            _eventManager.RaiseEvent($"It is now {player.Name}'s turn \n$ { player.Name} -your actions: { string.Join(" | ", playerActions.Select(a => $"{a.State} {a.Lower} - {a.Highest}"))} \n your choice :");
        }

        public void PlayerTurnBegin(string playerName)
        {
            _eventManager.RaiseEvent($"It is now {playerName}'s turn");
        }

        public void PlayerTurnEnd(string playerName)
        {
            _eventManager.RaiseEvent($"{playerName}'s turn is over");
        }

        public void PlayerActionDone(string playerName, PlayerAction action)
        {
            _eventManager.RaiseEvent($"{playerName} {action.State} for {action.Amount}");
        }

        public void PlayerCardsReceived(string playerName, PlayerHand playerHand)
        {
            _eventManager.RaiseEvent($"{playerName} received new hand");
        }

        public void CardRevealed(Card card)
        {
            _eventManager.RaiseEvent($"{card.BuildStringFromCard()} has been drawn");
        }

        public void GameStartedWith(IList<string> playersInGame)
        {
            _eventManager.RaiseEvent($"a new game starts with{string.Join(" and ", playersInGame)}");
        }

        public void GameEnded()
        {
            _eventManager.RaiseEvent($"game ended");
        }

        public void CardsCleared()
        {
            _eventManager.RaiseEvent($"cards regrouped");
        }

        public void DeckShuffled()
        {
            _eventManager.RaiseEvent($"deck shuffled");
        }

        public void SpinAndGoWonBy(Player player)
        {
            _eventManager.RaiseEvent($"Spin&Go won by {player.Name}");
        }
    }
}
