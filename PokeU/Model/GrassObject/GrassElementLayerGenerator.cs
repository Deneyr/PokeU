using PokeU.LandGenerator.EpicenterData;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.Model.GrassObject
{
    public class GrassElementLayerGenerator : ALandLayerGenerator
    {
        public GrassElementLayerGenerator() :
            base("elementGrass")
        {
            this.InitializeGenerator();
        }

        protected override void InitializeGenerator()
        {
            this.AddEpicenterLayer(30, DigressionMethod.CIRCLE, 80, 2, 15);

            this.AddEpicenterLayer(20, DigressionMethod.CIRCLE, 70, 5, 20);

            this.AddEpicenterLayer(32, DigressionMethod.CIRCLE, 70, 5, 40);
        }

        public override int GenerateLandLayer(WorldGenerator worldGenerator, ILandChunk landChunk, IntRect area, int seed, int minAltitude, int maxAltitude)
        {
            ALandLayerGenerator grassLandLayerGenerator = worldGenerator.Generators["grass"];

            ALandLayerGenerator altitudeLandLayerGenerator = worldGenerator.Generators["altitude"];

            ALandLayerGenerator cliffLandLayerGenerator = worldGenerator.Generators["cliff"];

            bool isThereGrassElement = false;

            Random random = new Random(seed);

            for (int i = 0; i < area.Height; i++)
            {
                for (int j = 0; j < area.Width; j++)
                {
                    int altitude = altitudeLandLayerGenerator.GetComputedPowerAt(j, i);

                    int altitudeOffset = cliffLandLayerGenerator.GetComputedPowerAt(j, i);

                    int elementIndex = random.Next(0, 12);// 

                    int power = this.GetElementIndexFromPower(this.GetComputedPowerAt(j, i));

                    GrassType grassType = (GrassType)grassLandLayerGenerator.GetComputedPowerAt(j, i);

                    if (power >= 2 && random.Next(0, 3) > 0 && altitudeOffset == 0 && grassType != GrassType.NONE)
                    {
                        GrassElementLandObject grassElement = new GrassElementLandObject(area.Left + j, area.Top + i, altitude, grassType, elementIndex);

                        LandCase landCase = landChunk.GetLandCase(i, j, altitude);

                        landCase.LandOverGround = grassElement;

                        isThereGrassElement = true;
                    }
                }
            }

            if (isThereGrassElement)
            {
                landChunk.AddTypeInChunk(typeof(GrassElementLandObject));
            }

            return random.Next();
        }

        protected virtual int GetElementIndexFromPower(float power)
        {
            int index = (int) Math.Max(Math.Min(31, power % 32), 0);

            if(index < 10)
            {
                index = -1;
            }
            else
            {
                index -= 20;
            }

            return index;
        }
    }
}
