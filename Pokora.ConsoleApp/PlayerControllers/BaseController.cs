using System;
using System.Collections.Generic;
using Pokora.GameMechanisms;

namespace Pokora.ConsoleApp.PlayerControllers
{
    public abstract class BaseController : IPlayerController
    {
        protected Player Player;
        protected Table Table;

        public abstract PlayerAction Play(IList<PlayerAction> actions);

        public void LinkPlayer(Player player, Table table)
        {
            Table = table;
            Player = player;
        }
    }
}
