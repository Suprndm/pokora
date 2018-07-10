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

        public override void NotifyTurn()
        {
            var maxBid = Table.Players.Max(player => player.Bid);

            var debtRatio = maxBid / (Player.Cash + Player.Bid);

            var combination = _combinationEvaluator.EvaluateCards(new List<Card> { Player.Hand.Card1, Player.Hand.Card2 });
            if (debtRatio > 0.5 && combination.Score >= 200)
            {
                SendAction(new PlayerAction(Player, PlayerState.AllIn, 0, 0,
                    AvailableActions.Single(action => action.State == PlayerState.AllIn).Amount));
            }
            else if(debtRatio> 0.5)
            {
                SendAction(new PlayerAction(Player, PlayerState.Fold, 0, 0));
            }
            else if(debtRatio <= 0.2)
            {
                if (AvailableActions.Any(action => action.State == PlayerState.Call))
                {
                    SendAction(new PlayerAction(Player, PlayerState.Call, 0, 0,
                        AvailableActions.Single(action => action.State == PlayerState.Call).Amount));
                }
                else if (AvailableActions.Any(action => action.State == PlayerState.Bet))
                {
                    SendAction(new PlayerAction(Player, PlayerState.Bet, 0, 0,
                        AvailableActions.Single(action => action.State == PlayerState.Bet).Lower*2));
                }
 
            }
            else
            {
                SendAction(new PlayerAction(Player, PlayerState.Fold, 0, 0));
            }
        }
    }
}
