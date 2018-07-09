using System.Linq;
using System.Threading.Tasks;
using Pokora.GameMechanisms;

namespace Pokora.ConsoleApp.PlayerControllers
{
    class AllinController : BaseController
    {
        public override void NotifyTurn()
        {
            SendAction(AvailableActions.Any(a => a.State == PlayerState.AllIn)
                ? new PlayerAction(Player, PlayerState.AllIn, 0, 0,
                    AvailableActions.Single(action => action.State == PlayerState.AllIn).Amount)
                : new PlayerAction(Player, PlayerState.Fold, 0, 0));
        }
    }
}
