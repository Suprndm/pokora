using System.Collections.Generic;
using System.Linq;
using Pokora.GameMechanisms;

namespace Pokora.IA.Decision
{
    public class DecisionEvaluator
    {
        public PlayerAction Decide(IList<PlayerAction> actions, double quality, double cashCriticality)
        {
            var decisions = new List<Decision>();
            var variableSet = Learner.Instance.GetVariableSet();

            foreach (var playerAction in actions)
            {
                double interest = 0;
                double amount = 0;


                var threshold = variableSet[playerAction.State];

                switch (playerAction.State)
                {
                    case PlayerState.Fold:
                        if (quality <= threshold)
                            interest = threshold;
                        break;
                    case PlayerState.Call:
                        if (quality > threshold)
                            interest = threshold;
                        amount = playerAction.Amount;
                        break;
                    case PlayerState.Check:
                        if (quality > threshold)
                            interest = threshold;
                        break;
                    case PlayerState.Bet:
                        if (quality > threshold)
                            interest = threshold;
                        amount = quality * playerAction.Highest;
                        if (amount <= playerAction.Lower)
                            amount = playerAction.Lower;
                        break;
                    case PlayerState.Raise:
                        if (quality > threshold)
                            interest = threshold;
                        amount = quality * playerAction.Highest;
                        if (amount <= playerAction.Lower)
                            amount = playerAction.Lower;
                        break;
                    case PlayerState.AllIn:
                        if (quality > threshold)
                            interest = threshold;
                        break;
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
    }
}
