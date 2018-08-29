using System.Collections.Generic;
using System.Linq;
using Pokora.GameMechanisms;
using Pokora.Poker;

namespace Pokora.ConsoleApp.PlayerControllers
{
    public class AllinController : BaseController
    {
        public override PlayerAction Play(IList<PlayerAction> actions)
        {
            return (actions.Any(a => a.State == PlayerState.AllIn)
                ? new PlayerAction(Player, PlayerState.AllIn, 0, 0,
                    actions.Single(action => action.State == PlayerState.AllIn).Amount)
                : new PlayerAction(Player, PlayerState.Fold, 0, 0));
        }
    }
}
