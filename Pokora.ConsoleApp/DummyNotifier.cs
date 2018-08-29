using System;
using System.Collections.Generic;
using System.Text;
using Pokora.GameMechanisms;

namespace Pokora.ConsoleApp
{
    public class DummyNotifier : INotifier
    {
        public void TableCreated(int smallBlind, int bigBlind)
        {

        }

        public void DealerPlaced(string playerName)
        {

        }

        public void PlayerJoined(string playerName, double cash)
        {

        }

        public void PlayerWin(string playerName)
        {

        }

        public void PlayerLose(string playerName)
        {

        }

        public void PlayerCashChanged(string playerName, double amount)
        {

        }

        public void PlayerBidChanged(string playerName, double amount)
        {

        }

        public void PlayerStateChanged(string playerName, PlayerState playerState)
        {

        }

        public void PlayerAvailableActionsChanged(string playerName, IList<PlayerAction> actions)
        {

        }

        public void PotsUpdated(IList<Pot> pots)
        {

        }

        public void RoundStarted(string name)
        {

        }

        public void RoundEnded(string name)
        {

        }

        public void PlayersWinPots(IList<KeyValuePair<Player, CardCombination>> winners, Pot pot)
        {

        }

        public void PlayerTurnBegin(Player player, IList<PlayerAction> playerActions)
        {

        }

        public void PlayerTurnEnd(string playerName)
        {

        }

        public void PlayerActionDone(string playerName, PlayerAction action)
        {

        }

        public void PlayerCardsReceived(string playerName, PlayerHand playerHand)
        {

        }

        public void CardRevealed(Card card)
        {

        }

        public void GameStartedWith(IList<string> playersInGame)
        {

        }

        public void GameEnded()
        {

        }

        public void CardsCleared()
        {

        }

        public void DeckShuffled()
        {

        }

        public void SpinAndGoWonBy(Player player)
        {

        }
    }
}
