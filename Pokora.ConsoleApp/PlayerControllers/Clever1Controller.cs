using System.Collections.Generic;
using System.Linq;
using Pokora.GameMechanisms;
using Pokora.Poker;

namespace Pokora.ConsoleApp.PlayerControllers
{
    public class Clever1Controller : BaseController
    {
        private CombinationEvaluator _combinationEvaluator;

        public Clever1Controller()
        {
            _combinationEvaluator = new CombinationEvaluator();
        }

        public override void NotifyTurn()
        {
            var combination = _combinationEvaluator.EvaluateCards(new List<Card> { Player.Hand.Card1, Player.Hand.Card2 });
            if (combination.Score >= 200)
            {
                SendAction(new PlayerAction(Player, PlayerState.AllIn, 0, 0,
                    AvailableActions.Single(action => action.State == PlayerState.AllIn).Amount));
            }
            else
            {
                SendAction(new PlayerAction(Player, PlayerState.Fold, 0, 0));
            }
        }
    }
}
