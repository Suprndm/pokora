using System;
using System.Collections.Generic;
using System.Text;

namespace Pokora
{
    public interface IPlayerController
    {
       event Action<PlayerAction> ActionReceived;
        void NotifyTurn();
        void SendAvailableActions(IList<PlayerAction> actions);
    }
}
