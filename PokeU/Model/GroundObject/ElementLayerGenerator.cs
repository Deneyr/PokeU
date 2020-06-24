using PokeU.LandGenerator.EpicenterData;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.Model.GroundObject
{
    public class ElementLayerGenerator : ALandLayerGenerator
    {
        public ElementLayerGenerator() :
            base("element")
        {
            this.InitializeGenerator();
        }

        protected override void InitializeGenerator()
        {
            this.AddEpicenterLayer(30, DigressionMethod.CIRCLE, 80, 2, 10);

            this.AddEpicenterLayer(20, DigressionMethod.CIRCLE, 70, 5, 15);

            this.AddEpicenterLayer(32, DigressionMethod.CIRCLE, 70, 5, 30);
        }

        public override int GenerateLandLayer(WorldGenerator worldGenerator, ILandChunk landChunk, IntRect area, int seed, int minAltitude, int maxAltitude)
        {
            return seed;
        }
    }
}
