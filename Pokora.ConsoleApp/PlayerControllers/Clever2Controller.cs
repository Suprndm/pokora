using System.Collections.Generic;
using System.Linq;
using Pokora.GameMechanisms;
using Pokora.Poker;

namespace Pokora.ConsoleApp.PlayerControllers
{
    public class Clever2Controller : BaseController
    {
        private CombinationEvaluator _combinationEvaluator;

        public Clever2Controller()
        {
            _combinationEvaluator = new CombinationEvaluator();
        }
        public override PlayerAction Play(IList<PlayerAction> actions)
        {
            var maxBid = Table.Players.Max(player => player.Bid);

            var debtRatio = maxBid / (Player.Cash + Player.Bid);

            var combination = _combinationEvaluator.EvaluateCards(new List<Card> { Player.Hand.Card1, Player.Hand.Card2 });
            if (debtRatio > 0.5 && combination.Score >= 200)
            {
                return (new PlayerAction(Player, PlayerState.AllIn, 0, 0,
                    actions.Single(action => action.State == PlayerState.AllIn).Amount));
            }
             if(debtRatio> 0.5)
            {
                return (new PlayerAction(Player, PlayerState.Fold, 0, 0));
            }
            if(debtRatio <= 0.2)
            {
                if (actions.Any(action => action.State == PlayerState.Call))
                {
                    return (new PlayerAction(Player, PlayerState.Call, 0, 0,
                        actions.Single(action => action.State == PlayerState.Call).Amount));
                }
                else if (actions.Any(action => action.State == PlayerState.Bet))
                {
                    return (new PlayerAction(Player, PlayerState.Bet, 0, 0,
                        actions.Single(action => action.State == PlayerState.Bet).Lower*2));
                }
            }

            return (new PlayerAction(Player, PlayerState.Fold, 0, 0));
        }
    }
}
