using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Pokora.IA;
using Pokora.Utils;

namespace Pokora.Tests
{
    public class EllipseTests
    {
        [Test]
        public void Test()
        {
            var ellipse = new EllipticArea(0.3, 0.5, 0.5);
            ellipse.Angle =Math.PI/4;
            ellipse.A = 0.5;
            ellipse.B = 0.1;

            var result = MathHelper.GetEllipticDistance(ellipse, 0.5, 0.7);
        }
    }
}
