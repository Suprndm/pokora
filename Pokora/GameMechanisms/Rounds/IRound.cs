using System;
using System.Collections.Generic;

namespace Pokora.GameMechanisms.Rounds
{
    public interface IRound
    {
        Player Start(IList<Player> players);
        void HandleAction(PlayerAction playerAction);
    }
}
