using System;
using System.Drawing;

namespace Pokora.Utils
{
    public static class MathHelper
    {

        public static float Distance(Point p1, Point p2)
        {
            return (float)Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (double)((p1.Y - p2.Y) * (p1.Y - p2.Y)));
        }

        public static float Distance(double p1X, double p1Y, double p2X, double p2Y)
        {
            return (float)Math.Sqrt((p1X - p2X) * (p1X - p2X) + (double)((p1Y - p2Y) * (p1Y - p2Y)));
        }
    }
}
