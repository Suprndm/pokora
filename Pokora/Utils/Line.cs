﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Pokora.Utils
{
    public class Line
    {
        public double A { get; set; }
        public double B { get; set; }

        public double Theta { get; set; }
        public double R { get; set; }

        public Point P1 { get; set; }
        public Point P2 { get; set; }

        public double WhiteRatio { get; set; }

        public Line(Point p1, Point p2)
        {
            P1 = p1;
            P2 = p2;
            A = (double)(p2.Y - p1.Y) / (p2.X - p1.X);
            B = p1.Y - (A * p1.X);
        }
    }
}
