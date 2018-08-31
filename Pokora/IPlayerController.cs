using System;
using System.Collections.Generic;
using System.Text;
using Pokora.GameMechanisms;

namespace Pokora
{
    public interface IPlayerController
    {
        PlayerAction Play(IList<PlayerAction> actions);
        void LinkPlayer(Player player, Table table);
    }
}
