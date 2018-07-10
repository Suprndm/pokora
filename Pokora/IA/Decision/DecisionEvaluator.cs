using System.Collections.Generic;
using System.Linq;

namespace Pokora.IA.Decision
{
    public class DecisionEvaluator
    {
        public PlayerAction Decide(IList<PlayerAction> actions, double quality, double risk)
        {
            return actions.First();
        }
    }
}
