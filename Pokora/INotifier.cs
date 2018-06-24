using System;
using System.Collections.Generic;
using System.Text;
using Pokora.GameMechanisms;

namespace Pokora
{
    public interface INotifier
    {
        void TableCreated(int smallBlind, int bigBlind);
        void DealerPlaced(string playerName);
        void PlayerJoined(string playerName, double cash);
        void PlayerWin(string playerName);
        void PlayerLose(string playerName);
        void PlayerCashChanged(string playerName, double amount);
        void PlayerBidChanged(string playerName, double amount);
        void PlayerStateChanged(string playerName, PlayerState playerState);
        void PlayerAvailableActionsChanged(string playerName, IList<PlayerAction> actions);
        void PotsUpdated(IList<Pot> pots);
        void RoundStarted(string name);
        void RoundEnded(string name);
        void PlayersWinPots(IList<string> playerNames, Pot pot, IList<CardCombination> combinations);
        void PlayerTurnBegin(string playerName);
        void PlayerTurnEnd(string playerName);
        void PlayerActionDone(string playerName, PlayerAction action);
        void PlayerCardsReceived(string playerName, PlayerHand playerHand);
        void CardRevealed(Card card);
        void GameStartedWith(IList<string> playersInGame);
        void GameEnded();
        void CardsCleared();
        void DeckShuffled();
    }
}
