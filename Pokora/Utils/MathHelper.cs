using System;
using System.Drawing;
using Pokora.IA;

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

        public static double GetEllipticDistance(double x, double y, double xp, double yp, double d, double D,
            double angle)
        {
            var cosa = Math.Cos(angle);
            var sina = Math.Sin(angle);

            var dd = d / 2 * d / 2;
            var DD = D / 2 * D / 2;

            var a = (cosa * (xp - x) + sina * (yp - y)) * (cosa * (xp - x) + sina * (yp - y));
            var b = (sina * (xp - x) - cosa * (yp - y)) * (sina * (xp - x) - cosa * (yp - y));
            var ellipse = (a / dd) + (b / DD);

            return ellipse;
        }

        public static double GetEllipticDistance(EllipticArea ellipse, double quality, double risk)
        {
            return GetEllipticDistance(ellipse.U, ellipse.V, quality, risk, ellipse.A * 2, ellipse.B * 2,
                ellipse.Angle);
        }

    }
}
