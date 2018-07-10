using System.Collections.Generic;
using Pokora.GameMechanisms;

namespace Pokora.IA
{
    public interface IQualityEvaluator
    {
        double EvalQualityScore(PlayerHand playerHand, IList<Card> tableCards);
    }
}
