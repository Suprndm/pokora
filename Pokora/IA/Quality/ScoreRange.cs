using System;
using System.Collections.Generic;
using System.Text;

namespace Pokora.IA.Quality
{
    public struct ScoreRange
    {
        public ScoreRange(int cardsCount, double min, double max)
        {
            CardsCount = cardsCount;
            Min = min;
            Max = max;
        }

        public int CardsCount { get; }
        public double Min { get;}
        public double Max { get;}
    }
}
