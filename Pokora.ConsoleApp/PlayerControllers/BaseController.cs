using System;
using System.Collections.Generic;
using Pokora.GameMechanisms;

namespace Pokora.ConsoleApp.PlayerControllers
{
    public abstract class BaseController : IPlayerController
    {
        protected Player Player;
        protected Table Table;
        protected IList<PlayerAction> AvailableActions;

        public event Action<PlayerAction> ActionReceived;

        protected virtual void SendAction(PlayerAction action)
        {
            ActionReceived?.Invoke(action);
        }

        public abstract void NotifyTurn();

        public void SendAvailableActions(IList<PlayerAction> actions)
        {
            AvailableActions = actions;
        }

        public void LinkPlayer(Player player, Table table)
        {
            Table = table;
            Player = player;
        }
    }
}
