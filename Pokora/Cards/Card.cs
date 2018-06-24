using System;

namespace Pokora
{
    public struct Card   
    {
        public Card(int value, int color)
        {
            Value = value;
            Color = color;
        }

        public int Value { get;}
        public int Color { get;}
    }
}
