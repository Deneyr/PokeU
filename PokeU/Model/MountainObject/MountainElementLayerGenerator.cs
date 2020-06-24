using PokeU.LandGenerator.EpicenterData;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.Model.MountainObject
{
    public class MountainElementLayerGenerator : ALandLayerGenerator
    {
        public MountainElementLayerGenerator() :
            base("elementMountain")
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
            ALandLayerGenerator mountainLandLayerGenerator = worldGenerator.Generators["mountain"];

            ALandLayerGenerator altitudeLandLayerGenerator = worldGenerator.Generators["altitude"];

            ALandLayerGenerator cliffLandLayerGenerator = worldGenerator.Generators["cliff"];

            ALandLayerGenerator elementLandLayerGenerator = worldGenerator.Generators["element"];

            bool isThereMountainElement = false;

            Random random = new Random(seed);

            for (int i = 0; i < area.Height; i++)
            {
                for (int j = 0; j < area.Width; j++)
                {
                    int altitude = altitudeLandLayerGenerator.GetComputedPowerAt(j, i);

                    int altitudeOffset = cliffLandLayerGenerator.GetComputedPowerAt(j, i);

                    int elementIndex = random.Next(0, 5);

                    int power = this.GetElementIndexFromPower(elementLandLayerGenerator.GetComputedPowerAt(j, i));

                    MountainType mountainType = (MountainType)mountainLandLayerGenerator.GetComputedPowerAt(j, i);

                    if(mountainType == MountainType.PROJECTING)
                    {
                        if(elementIndex == 3 && random.Next(0, 3) > 0)
                        {
                            elementIndex = random.Next(0, 3);
                        }
                    }
                    else if(mountainType == MountainType.ROUGH)
                    {
                        if (elementIndex < 3 && random.Next(0, 3) > 0)
                        {
                            elementIndex = 3;
                        }
                    }

                    if (power >= 2 && random.Next(0, 3) > 0 && altitudeOffset == 0 && mountainType != MountainType.NONE)
                    {
                        MountainElementLandObject mountainElement = new MountainElementLandObject(area.Left + j, area.Top + i, altitude, mountainType, elementIndex);

                        LandCase landCase = landChunk.GetLandCase(i, j, altitude);

                        landCase.LandOverGround = mountainElement;

                        isThereMountainElement = true;
                    }
                }
            }

            if (isThereMountainElement)
            {
                landChunk.AddTypeInChunk(typeof(MountainElementLandObject));
            }

            return random.Next();
        }

        protected virtual int GetElementIndexFromPower(float power)
        {
            int index = 13 - (int)Math.Max(Math.Min(13, power % 14), 0);

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
