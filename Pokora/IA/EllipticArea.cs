using System;
using System.Collections.Generic;
using System.Text;

namespace Pokora.IA
{
    public class EllipticArea
    {
        public EllipticArea(double u = 0, double v = 0, double r = 0)
        {
            U = u;
            V = v;
            R = r;
        }


        public double U { get; set; }
        public double V { get; set; }
        public double R { get; set; }
        public double A { get; set; }
        public double B { get; set; }
    }
}
