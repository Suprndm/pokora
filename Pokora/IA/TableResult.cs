using System.Collections.Generic;
using Pokora.GameMechanisms;

namespace Pokora.IA
{
    public class TableResult
    {
        public string Name { get; set; }
        public double WinRate { get; set; }
        public IDictionary<PlayerState, EllipticArea> EllipticAreas { get; set; }
    }
}
