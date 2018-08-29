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

        public override PlayerAction Play(IList<PlayerAction> actions)
        {
            var combination = _combinationEvaluator.EvaluateCards(new List<Card> { Player.Hand.Card1, Player.Hand.Card2 });
            if (combination.Score >= 200)
            {
                return (new PlayerAction(Player, PlayerState.AllIn, 0, 0,
                    actions.Single(action => action.State == PlayerState.AllIn).Amount));
            }
            else
            {
                return (new PlayerAction(Player, PlayerState.Fold, 0, 0));
            }
        }
    }
}
