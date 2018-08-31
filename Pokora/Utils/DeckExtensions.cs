using System;
using System.Collections.Generic;
using System.Linq;

namespace Pokora.Utils
{
    public static class DeckExtensions
    {
        public static void Shuffle<T>(this IList<T> cards)
        {
            //Fisher–Yates shuffle en.wikipedia.org/wiki/Fisher–Yates_shuffle
            //for i from 0 to n−2 do
            //j ← random integer such that i ≤ j < n
            //exchange a[i] and a[j]
            int count = cards.Count;
            int[] deck = new int[count];
            for (byte i = 0; i < count; i++)
                deck[i] = i;
            Random rand = new Random();
            // next(max) Returns a non-negative random integer that is less than the specified maximum.
            for (byte i = 0; i <= count - 2; i++)
            {
                int j = rand.Next(count - i);
                if (j > 0)
                {
                    int curVal = deck[i];
                    deck[i] = deck[i + j];
                    deck[i + j] = curVal;
                }
            }
            //suffle the other directinon to be sure 
            //for i from n−1 downto 1 do
            //j ← random integer such that 0 ≤ j ≤ i
            //exchange a[j] and a[i]
            for (int i = count - 1; i >= 1; i--)
            {
                int j = rand.Next(i + 1);
                if (j != i)
                {
                    int curVal = deck[i];
                    deck[i] = deck[j];
                    deck[i] = curVal;
                }
            }

            var temp = cards.ToList();

            for (int i = 0; i < cards.Count; i++)
            {
                cards[i] = temp[deck[i]];
            }
        }

        public static void LightShuffle<T>(this IList<T> cards)
        {
            for (int i = cards.Count - 1; i > 0; i--)
            {
                int j = StaticRandom.Rand(i + 1);
                var t = cards[i];
                cards[i] = cards[j];
                cards[j] = t;
            }
        }
    }
}
