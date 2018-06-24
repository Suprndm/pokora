using System;
using System.Collections.Generic;

namespace Pokora.GameMechanisms.Rounds
{
    public interface IRound
    {
        event Action<Player> RoundEnded;
        void Start(IList<Player> players);
        void HandleAction(PlayerAction playerAction);
    }
}
