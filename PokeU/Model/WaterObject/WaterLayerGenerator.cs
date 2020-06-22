using PokeU.LandGenerator.EpicenterData;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.Model.WaterObject
{
    public class WaterLayerGenerator : ALandLayerGenerator
    {
        //private int[,] altitudeArea;

        public WaterLayerGenerator() :
            base("water")
        {
            this.InitializeGenerator();
        }

        protected override void InitializeGenerator()
        {

        }

        private void ConstructAltitudeArea(WorldGenerator worldGenerator, IntRect area)
        {
            ALandLayerGenerator altitudeLandLayerGenerator = worldGenerator.Generators["altitude"];

            //this.powerArea = new int[area.Height + 2, area.Width + 2];
            for (int i = -1; i < area.Height + 1; i++)
            {
                for (int j = -1; j < area.Width + 1; j++)
                {
                    int altitude = altitudeLandLayerGenerator.GetComputedPowerAt(j, i);

                    if (altitude == 0)
                    {
                        //double preAltitude = altitudeLandLayerGenerator.GetPowerAt(new Vector2f(area.Left + j, area.Top + i));

                        altitude = this.NeedToFillSandAt(worldGenerator, area, i, j);

                        altitude = Math.Max(0, altitude);
                    }

                    this.powerArea[i + 1, j + 1] = altitude;
                }
            }
        }

        private int NeedToFillSandAt(
            WorldGenerator worldGenerator,
            IntRect area,
            int i, int j)
        {
            ALandLayerGenerator altitudeLandLayerGenerator = worldGenerator.Generators["altitude"];

            bool[,] subAreaBool = new bool[3, 3];
            int[,] subAreaInt = new int[3, 3];

            int maxValue = int.MinValue;
            int minValue = int.MaxValue;
            for (int y = -1; y < 2; y++)
            {
                for (int x = -1; x < 2; x++)
                {
                    double preAltitude = altitudeLandLayerGenerator.GetPowerAt(new Vector2f(area.Left + j + x, area.Top + i + y));

                    int altitude = (int)Math.Round(preAltitude);

                    maxValue = Math.Max(maxValue, altitude);

                    minValue = Math.Min(minValue, altitude);

                    subAreaInt[y + 1, x + 1] = altitude;
                }
            }

            bool needToFill = false;
            if (subAreaInt[1, 1] != maxValue)
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

                needToFill = ALandLayerGenerator.NeedToFill(ref subAreaBool);
            }

            if (needToFill)
            {
                return maxValue;
            }
            return subAreaInt[1, 1];
        }

        public override int GenerateLandLayer(WorldGenerator worldGenerator, ILandChunk landChunk, IntRect area, int seed, int minAltitude, int maxAltitude)
        {
            ALandLayerGenerator altitudeLandLayerGenerator = worldGenerator.Generators["altitude"];

            bool isThereWater = false;

            this.ConstructAltitudeArea(worldGenerator, area);

            for (int i = 0; i < area.Height; i++)
            {
                for (int j = 0; j < area.Width; j++)
                {
                    int altitude = this.powerArea[i + 1, j + 1];

                    int[,] subAreaInt = new int[3, 3];
                    int maxLocalAltitude = int.MinValue;

                    maxLocalAltitude = this.GetComputedMatrix(i, j, ref subAreaInt);

                    for (int z = altitude; z <= 0; z++)
                    {
                        this.GetComputedLandType(area, ref subAreaInt, maxLocalAltitude, out LandTransition landTransition);

                        WaterLandObject waterLandObject = new WaterLandObject(area.Left + j, area.Top + i, z);
                        waterLandObject.SetLandTransition(landTransition);

                        landChunk.InitializeLandCase(i, j, z);
                        landChunk.GetLandCase(i, j, z).LandWater = waterLandObject;
                        isThereWater = true;

                        subAreaInt[1, 1]++;
                    }
                }
            }

            if (isThereWater)
            {
                landChunk.AddTypeInChunk(typeof(WaterLandObject));
            }

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
                    int altitude = this.powerArea[i + y + 1, j + x + 1];

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

                landtransition = LandTransitionHelper.ReverseLandTransition(landtransition);
            }
        }
    }
}
