using System;
using System.Collections.Generic;
using System.Text;
using Pokora.GameMechanisms;

namespace Pokora.ConsoleApp.Display
{
    public class TableView
    {
        public string Cards { get; set; }
        public string Round { get; set; }
        private IList<PotView> Pots { get; set; }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
