using PokeU.LandGenerator.EpicenterData;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.Model.GroundObject
{
    public class CliffLayerGenerator : ALandLayerGenerator
    {
        public CliffLayerGenerator() :
            base("cliff")
        {
            this.InitializeGenerator();
        }

        protected override void InitializeGenerator()
        {

        }

        public override int GenerateLandLayer(WorldGenerator worldGenerator, ILandChunk landChunk, IntRect area, int seed, int minAltitude, int maxAltitude)
        {
            ALandLayerGenerator altitudeLandLayerGenerator = worldGenerator.Generators["altitude"];

            bool[,] subArea = new bool[3, 3];

            this.powerArea = new int[area.Height + 4, area.Width + 4];

            for (int i = 0; i < area.Height; i++)
            {
                for (int j = 0; j < area.Width; j++)
                {
                    int[,] subAreaInt = new int[3, 3];
                    int maxLocalAltitude = int.MinValue;

                    maxLocalAltitude = this.GetComputedMatrix(altitudeLandLayerGenerator, i, j, ref subAreaInt);

                    int diffAltitude = maxLocalAltitude - subAreaInt[1, 1];

                    this.powerArea[i + 2, j + 2] = diffAltitude;

                    for (int offset = 0; offset < diffAltitude; offset++)
                    {
                        this.GetComputedLandType(area, ref subAreaInt, maxLocalAltitude, out LandTransition landTransition);

                        if (landTransition != LandTransition.NONE)
                        {
                            AltitudeLandObject altitudeLandObject = new AltitudeLandObject(area.Left + j, area.Top + i, subAreaInt[1, 1], LandType.GRASS);

                            landChunk.InitializeLandCase(i, j, subAreaInt[1, 1]);
                            landChunk.GetLandCase(i, j, subAreaInt[1, 1]).LandWall = altitudeLandObject;

                            altitudeLandObject.SetLandTransition(landTransition);
                        }

                        subAreaInt[1, 1]++;
                    }
                }
            }

            landChunk.AddTypeInChunk(typeof(AltitudeLandObject));

            return seed;
        }

        protected int GetComputedMatrix(ALandLayerGenerator altitudeLandLayerGenerator, int i, int j, ref int[,] subAreaInt)
        {
            int maxValue = int.MinValue;
            int minValue = int.MaxValue;

            for (int y = -1; y < 2; y++)
            {
                for (int x = -1; x < 2; x++)
                {
                    int altitude = altitudeLandLayerGenerator.GetComputedPowerAt(j + x, i + y);

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
                        if (subAreaInt[y, x] <= subAreaInt[1, 1])
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
