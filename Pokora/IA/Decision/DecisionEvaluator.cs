using System.Collections.Generic;
using System.Linq;
using Pokora.GameMechanisms;
using Pokora.Utils;

namespace Pokora.IA.Decision
{
    public class DecisionEvaluator
    {
        public DecisionEvaluator()
        {
        }

        public PlayerAction Decide(IList<PlayerAction> actions, double quality, double cashCriticality, bool useElipse, IDictionary<PlayerState, EllipticArea>  areas)
        {
            var decisions = new List<Decision>();
            foreach (var playerAction in actions)
            {
                double interest = 0;
                double amount = 0;

                var elipticArea = areas[playerAction.State];

                switch (playerAction.State)
                {
                    case PlayerState.Fold:
                        break;
                    case PlayerState.Call:
                        amount = playerAction.Amount;
                        break;
                    case PlayerState.Check:
                        break;
                    case PlayerState.Bet:
                        amount =  playerAction.Lower*2;
                        break;
                    case PlayerState.Raise:
                        amount = playerAction.Lower * 2;
                        break;
                    case PlayerState.AllIn:
                        amount = playerAction.Amount;
                        break;
                }

                if (useElipse)
                {
                    interest = ComputeDistanceRatioFromElipse(quality, cashCriticality, elipticArea);
                }
                else
                {
                    interest = ComputeDistanceRatioFromCircleArea(quality, cashCriticality, elipticArea);

                }

                decisions.Add(
                    new Decision()
                    {
                        Action = new PlayerAction(playerAction.Player, playerAction.State, playerAction.Lower,
                            playerAction.Highest, amount),
                        Interest = interest
                    });
            }

            return decisions.OrderByDescending(decision => decision.Interest).First().Action;
        }

        public double ComputeDistanceRatioFromCircleArea(double quality, double criticality, EllipticArea area)
        {
            var distance = MathHelper.Distance(quality, criticality, area.U, area.V);

            if (distance > area.R) return 0;
            else
            {
                return 1 - distance / area.R;
            }
        }

        public double ComputeDistanceRatioFromElipse(double quality, double criticality, EllipticArea area)
        {
            var distance = MathHelper.GetEllipticDistance(area, quality, criticality);

            if (distance > 1) return 0;
            else
            {
                return 1 - distance;
            }
        }


    }
}
