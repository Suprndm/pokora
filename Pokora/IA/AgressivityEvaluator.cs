using System;
using System.Collections.Generic;
using System.Text;
using Pokora.GameMechanisms;

namespace Pokora.IA
{
    public class AgressivityEvaluator
    {
        public double EvaluateAggressivity(IList<Player> otherPlayers)
        {
            double totalAggressivity = 0;
            foreach (var otherPlayer in otherPlayers)
            {
                totalAggressivity += EvaluateAggressivity(otherPlayer);
            }

            totalAggressivity = totalAggressivity / otherPlayers.Count;


            return totalAggressivity;
        }

        public double EvaluateAggressivity(Player player)
        {
            double totalInvestedRatio = 0;
            foreach (var playedAction in player.PlayedActions)
            {
                if (playedAction.Highest > 0)
                    totalInvestedRatio += playedAction.Amount / playedAction.Highest;
            }

            var averageInvestedRatio = totalInvestedRatio / player.PlayedActions.Count;

            return averageInvestedRatio;
        }
    }
}
