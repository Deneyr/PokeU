using PokeU.LandGenerator.EpicenterData;
using SFML.Graphics;
using System;

namespace PokeU.Model.GroundObject
{
    public class GroundElementLayerGenerator : ALandLayerGenerator
    {
        public GroundElementLayerGenerator() :
            base("elementGround")
        {
            this.InitializeGenerator();
        }

        protected override void InitializeGenerator()
        {
            //this.AddEpicenterLayer(30, DigressionMethod.CIRCLE, 80, 2, 15);

            //this.AddEpicenterLayer(20, DigressionMethod.CIRCLE, 70, 5, 20);

            //this.AddEpicenterLayer(32, DigressionMethod.CIRCLE, 70, 5, 40);
        }

        public override int GenerateLandLayer(WorldGenerator worldGenerator, ILandChunk landChunk, IntRect area, int seed, int minAltitude, int maxAltitude)
        {
            ALandLayerGenerator groundLandLayerGenerator = worldGenerator.Generators["defaultGround"];

            ALandLayerGenerator altitudeLandLayerGenerator = worldGenerator.Generators["altitude"];

            ALandLayerGenerator cliffLandLayerGenerator = worldGenerator.Generators["cliff"];

            ALandLayerGenerator elementLandLayerGenerator = worldGenerator.Generators["element"];

            bool isThereSandElement = false;

            Random random = new Random(seed);

            for (int i = 0; i < area.Height; i++)
            {
                for (int j = 0; j < area.Width; j++)
                {
                    int altitude = altitudeLandLayerGenerator.GetComputedPowerAt(j, i);

                    int altitudeOffset = cliffLandLayerGenerator.GetComputedPowerAt(j, i);

                    int elementIndex = random.Next(0, 8);

                    int power = this.GetElementPower(elementLandLayerGenerator.GetComputedPowerAt(j, i));

                    LandType landType = (LandType)groundLandLayerGenerator.GetComputedPowerAt(j, i);

                    if (landType == LandType.SAND)
                    {
                        if (power >= 8 && random.Next(0, 3) > 0 && altitudeOffset == 0)
                        {
                            float trueAltitude = altitudeLandLayerGenerator.GetPowerAt(new SFML.System.Vector2f(area.Left + j, area.Top + i));

                            if((elementIndex == 1 || elementIndex == 3) 
                                && trueAltitude > 0.6 && random.Next(0, 4) > 0)
                            {
                                elementIndex -= 1; 
                            }

                            GroundElementLandObject groundElement = new GroundElementLandObject(area.Left + j, area.Top + i, altitude, landType, elementIndex);

                            LandCase landCase = landChunk.GetLandCase(i, j, altitude);

                            landCase.LandOverGround = groundElement;

                            isThereSandElement = true;
                        }
                    }
                }
            }

            if (isThereSandElement)
            {
                landChunk.AddTypeInChunk(typeof(GroundElementLandObject));
            }

            return random.Next();
        }

        protected virtual int GetElementPower(float power)
        {
            int index = (int)Math.Max(Math.Min(20, power % 21), 0);

            if (index < 10)
            {
                index = -1;
            }
            else
            {
                index -= 10;
            }

            return index;
        }
    }
}
