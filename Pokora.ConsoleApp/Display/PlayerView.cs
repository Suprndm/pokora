using System;
using System.Collections.Generic;
using System.Text;

namespace Pokora.ConsoleApp.Display
{
    public class PlayerView
    {
        public string Name { get; set; }
        public int Cash { get; set; }
        public int Bid { get; set; }
        public string Blind { get; set; }
        public string Hand { get; set; }

        public override string ToString()
        {
            return $"{Name}:{Cash} |{Hand}| - {Blind} ->{Bid}";
        }
    }
}
