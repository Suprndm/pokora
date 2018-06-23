using System;
using System.Collections.Generic;
using System.Text;

namespace Pokora
{
    public class PlayerHand
    {
        public PlayerHand(Card card1, Card card2)
        {
            Card2 = card2;
            Card1 = card1;
        }

        public Card Card1 { get; }
        public Card Card2 { get; }
    }
}
