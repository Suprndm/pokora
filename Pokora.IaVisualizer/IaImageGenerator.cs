using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Pokora.GameMechanisms;
using Pokora.IA;
using Pokora.Utils;

namespace Pokora.IaVisualizer
{
    public class IaImageGenerator
    {
        public void GenerateIaImage(TableResult tableResult)
        {
            var image = new Bitmap(1000, 1000);
            for (int i = 0; i < 1000; i++)
            {
                for (int j = 0; j < 1000; j++)
                {
                    var quality = (double)i / 1000;
                    var risk = (double)(1000 - j) / 1000;

                    Dictionary<PlayerState, double> distances = new Dictionary<PlayerState, double>();

                    foreach (PlayerState playerState in (PlayerState[])Enum.GetValues(typeof(PlayerState)))
                    {
                        if (playerState != PlayerState.None)
                        {
                            var distance = ComputeDistanceRatioFromElipse(quality, risk, tableResult.EllipticAreas[playerState]);
                            distances.Add(playerState, distance);
                        }
                    }

                    var bestState = distances.OrderByDescending(d => d.Value).First();

                    var color = GetColorByType(bestState.Key);

                    image.SetPixel(i, j, Color.FromArgb((int)(color.R * bestState.Value), (int)(color.G * bestState.Value), (int)(color.B * bestState.Value)));

                }
            }

            image.Save($"{tableResult.Name}-{Guid.NewGuid()}.png");


        }


        public double ComputeDistanceRatioFromElipse(double quality, double criticality, EllipticArea area)
        {
            var distance = MathHelper.GetEllipticDistance(area, quality, criticality);

            if (distance > 1) return 0;
            else
            {
                return 1 - distance;
            }
        }

        private Color GetColorByType(PlayerState playerState)
        {
            switch (playerState)
            {
                case PlayerState.Fold:
                    return Color.FromArgb(255, 0, 0);
                case PlayerState.Call:
                    return Color.FromArgb(0, 255, 0);

                case PlayerState.Check:
                    return Color.FromArgb(0, 0, 255);

                case PlayerState.Bet:
                    return Color.FromArgb(255, 255, 0);

                case PlayerState.Raise:
                    return Color.FromArgb(255, 0, 255);

                case PlayerState.AllIn:
                    return Color.FromArgb(255, 255, 255);
                default: throw new ArgumentException();
            }

        }
    }
}
