using System;

namespace Pokora
{
    public sealed class Randomizer
    {
        private static readonly Lazy<Randomizer> Lazy =
            new Lazy<Randomizer>(() => new Randomizer());

        public static Randomizer Instance => Lazy.Value;
        public Random Random { get; }

        private Randomizer()
        {
            Random = new Random();
        }
    }
}