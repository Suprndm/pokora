using System;
using System.Threading;

namespace Pokora
{
    public static class StaticRandom
    {
        static int seed = Environment.TickCount;

        static readonly ThreadLocal<Random> random =
            new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref seed)));

        public static int Rand(int i)
        {
            return random.Value.Next(i);
        }
    }
}