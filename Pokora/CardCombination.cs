using System;
using System.Collections.Generic;
using System.Text;

namespace Pokora
{
    public class CardCombination
    {
        public CardCombination(CombinationType type, IReadOnlyCollection<Card> cards, double score)
        {
            Type = type;
            Cards = cards;
            Score = score;
        }

        public CombinationType Type { get;  }
        public IReadOnlyCollection<Card> Cards { get;}
        public double Score { get; }
    }
}
