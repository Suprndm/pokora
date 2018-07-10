using System.Collections.Generic;
using Pokora.GameMechanisms;

namespace Pokora.IA
{
    public class QualityEvaluator : IQualityEvaluator
    {
        private const double maxCardsScore = 4330955887260.6328;
        private const double minCardsScore = 43.96;
        public double EvalQualityScore(PlayerHand playerHand, IList<Card> tableCards)
        {
            return 0;
        }
    }
}
