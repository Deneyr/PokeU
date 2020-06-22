using PokeU.LandGenerator.EpicenterData;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.Model.GroundObject
{
    public class AltitudeLayerGenerator : ALandLayerGenerator
    {
        private int altitudeRange;

        public AltitudeLayerGenerator(int altitudeRange) :
            base("altitude")
        {
            this.InitializeGenerator();

            this.altitudeRange = Math.Abs(altitudeRange);
        }

        protected override void InitializeGenerator()
        {
            this.AddEpicenterLayer(256, DigressionMethod.SQUARE_DEC, 4, -20, 20);

            //this.AddEpicenterLayer(256, DigressionMethod.SMOOTH, 10, 0, 10);

            this.AddEpicenterLayer(128, DigressionMethod.LINEAR, 20, 0, 5);

            this.AddEpicenterLayer(16, DigressionMethod.LINEAR, 50, 5, 3);
        }

        public override float GetPowerAt(Vector2f position)
        {
            float powerResult = base.GetPowerAt(position);

            powerResult = Math.Max(-this.altitudeRange, Math.Min(this.altitudeRange, powerResult));

            return powerResult;
        }

        public override int GenerateLandLayer(WorldGenerator worldGenerator, ILandChunk landChunk, IntRect area, int seed, int minAltitude, int maxAltitude)
        {
            return seed;
        }

        protected int GetComputedMatrix(int i, int j, ref int[,] subAreaInt)
        {
            int maxValue = int.MinValue;
            int minValue = int.MaxValue;

            for (int y = -1; y < 2; y++)
            {
                for (int x = -1; x < 2; x++)
                {
                    int altitude = this.GetComputedPowerAt(j + x, i + y);

                    maxValue = Math.Max(maxValue, altitude);

                    minValue = Math.Min(minValue, altitude);

                    subAreaInt[y + 1, x + 1] = altitude;
                }
            }

            return maxValue;
        }

        protected void GetComputedLandType(
            IntRect area,
            ref int[,] subAreaInt,
            int maxValue,
            out LandTransition landtransition)
        {
            bool[,] subAreaBool = new bool[3, 3];

            landtransition = LandTransition.NONE;

            if (subAreaInt[1, 1] < maxValue)
            {
                for (int y = 0; y < 3; y++)
                {
                    for (int x = 0; x < 3; x++)
                    {
                        if (subAreaInt[y, x] != maxValue)
                        {
                            subAreaBool[y, x] = false;
                        }
                        else
                        {
                            subAreaBool[y, x] = true;
                        }
                    }
                }

                landtransition = ALandLayerGenerator.GetLandTransitionFrom(ref subAreaBool);
            }
        }
    }
}
