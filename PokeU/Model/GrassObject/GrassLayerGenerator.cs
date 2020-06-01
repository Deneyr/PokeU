using PokeU.LandGenerator.EpicenterData;
using PokeU.Model.GroundObject;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.Model.GrassObject
{
    public class GrassLayerGenerator: ALandLayerGenerator
    {
        private int[,] grassArea;

        public GrassLayerGenerator() :
            base("grass")
        {
            this.InitializeGenerator();
        }

        protected override void InitializeGenerator()
        {

        }

        public override ILandLayer GenerateLandLayer(WorldGenerator worldGenerator, IntRect area, int minAltitude, int maxAltitude)
        {
            ALandLayerGenerator altitudeLandLayerGenerator = worldGenerator.Generators["altitude"];

            LandLayer groundLandLayer = new LandLayer(minAltitude, maxAltitude, area);

            bool[,] subArea = new bool[3, 3];

            bool isThereGrass = false;

            this.ConstructGrassArea(worldGenerator, area);

            for (int i = 0; i < area.Height; i++)
            {
                for (int j = 0; j < area.Width; j++)
                {
                    int altitude = altitudeLandLayerGenerator.GetComputedPowerAt(j, i);

                    if (altitude > -2 && altitude < 6)
                    {
                        this.GetComputedLandType(area, i, j, out GrassType grassType, out GrassType secondType, out LandTransition landTransition);

                        LandCase landCase = null;
                        if (grassType != GrassType.NONE)
                        {
                            GroundLandObject groundLandObject = new GrassLandObject(area.Left + j, area.Top + i, altitude, grassType);

                            groundLandLayer.InitializeLandCase(i, j, altitude);
                            landCase = groundLandLayer.GetLandCase(i, j, altitude);

                            landCase.AddLandGround(groundLandObject);

                            isThereGrass = true;
                        }

                        if (secondType != grassType && secondType != GrassType.NONE)
                        {
                            GroundLandObject secondGroundLandObject = new GrassLandObject(area.Left + j, area.Top + i, 0, secondType);
                            secondGroundLandObject.SetTransition(landTransition);

                            groundLandLayer.InitializeLandCase(i, j, altitude);
                            landCase = groundLandLayer.GetLandCase(i, j, altitude);

                            landCase.AddLandGround(secondGroundLandObject);

                            isThereGrass = true;
                        }
                    }
                }
            }

            if (isThereGrass)
            {
                groundLandLayer.AddTypeInLayer(typeof(GrassLandObject));
            }

            return groundLandLayer;
        }

        private void ConstructGrassArea(WorldGenerator worldGenerator, IntRect area)
        {
            ALandLayerGenerator altitudeLandLayerGenerator = worldGenerator.Generators["altitude"];

            ALandLayerGenerator groundLandLayerGenerator = worldGenerator.Generators["ground"];

            grassArea = new int[area.Height + 4, area.Width + 4];

            for (int i = -2; i < area.Height + 2; i++)
            {
                for (int j = -2; j < area.Width + 2; j++)
                {
                    int altitude = altitudeLandLayerGenerator.GetComputedPowerAt(j, i);
                    int power = groundLandLayerGenerator.GetComputedPowerAt(j, i);

                    int currentValue = -1;
                    int grassType = (int)this.GetGrassTypeFromPower(power);
                    if (altitude > -2 && altitude < 6)
                    {
                        if (altitude > 4)
                        {
                            if (grassType == 2 || grassType == 3)
                            {
                                currentValue = grassType;
                            }
                        }
                        else if (altitude > 1)
                        {
                            if (altitude > 2 && grassType == 0)
                            {
                                currentValue = 1;
                            }
                            else
                            {
                                currentValue = grassType;
                            }
                        }
                        else if (altitude < 2)
                        {
                            if (grassType == 0)
                            {
                                currentValue = 0;
                            }
                            else
                            {
                                currentValue = -1;
                            }
                        }
                    }

                    grassArea[i + 2, j + 2] = currentValue;
                }
            }

            for (int i = 0; i < area.Height + 2; i++)
            {
                for (int j = 0; j < area.Width + 2; j++)
                {
                    this.powerArea[i + 1, j + 1] = this.NeedToFillGrassAt(area, i - 1, j - 1);
                }
            }
        }

        protected int NeedToFillGrassAt(
            IntRect area,
            int i, int j)
        {
            bool[,] subAreaBool = new bool[3, 3];
            int[,] subAreaInt = new int[3, 3];

            int maxValue = int.MinValue;
            int minValue = int.MaxValue;
            for (int y = -1; y < 2; y++)
            {
                for (int x = -1; x < 2; x++)
                {
                    int altitude = this.grassArea[i + y + 2, j + x + 2];

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

        protected virtual GrassType GetGrassTypeFromPower(float power)
        {
            return (GrassType)Math.Max(Math.Min(3, power), 0);
        }

        private void GetComputedLandType(
            IntRect area,
            int i, int j,
            out GrassType grassType,
            out GrassType secondType,
            out LandTransition landtransition)
        {
            bool[,] subAreaBool = new bool[3, 3];
            int[,] subAreaInt = new int[3, 3];

            int maxValue = int.MinValue;
            int minValue = int.MaxValue;
            for (int y = -1; y < 2; y++)
            {
                for (int x = -1; x < 2; x++)
                {
                    int currentValue = this.GetComputedPowerAt(j + x, i + y);

                    maxValue = Math.Max(maxValue, currentValue);

                    minValue = Math.Min(minValue, currentValue);

                    subAreaInt[y + 1, x + 1] = currentValue;
                }
            }

            grassType = (GrassType)subAreaInt[1, 1];
            landtransition = LandTransition.NONE;
            secondType = grassType;

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

                landtransition = ALandLayerGenerator.GetLandTransitionFrom(ref subAreaBool);

                if (landtransition != LandTransition.NONE)
                {
                    secondType = (GrassType)maxValue;
                }
            }
        }
    }
}
