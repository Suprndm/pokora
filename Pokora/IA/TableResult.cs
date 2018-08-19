using System.Collections.Generic;
using Pokora.GameMechanisms;

namespace Pokora.IA
{
    public class TableResult
    {
        public double WinRate { get; set; }
        public IDictionary<PlayerState, double> VariableSet { get; set; }
    }
}
