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
    public class GroundLayerGenerator : ALandLayerGenerator
    {
        public GroundLayerGenerator() :
            base("ground")
        {
            this.InitializeGenerator();
        }

        protected override void AddEpicenterLayer(int influenceRadius, DigressionMethod digressionMethod, int nbMaxPoints, float pointPowerMin, float pointPowerMax)
        {
            this.epicenterLayersList.Add(new EpicenterLayer(influenceRadius, digressionMethod, nbMaxPoints, pointPowerMin, pointPowerMax));
        }

        protected override void InitializeGenerator()
        {
            this.AddEpicenterLayer(256, DigressionMethod.SMOOTH, 5, 0, 1);

            this.AddEpicenterLayer(128, DigressionMethod.SMOOTH, 20, 0, 1);

            this.AddEpicenterLayer(64, DigressionMethod.SQUARE_ACC, 80, 0, 1);
        }

        public override int GenerateLandLayer(WorldGenerator worldGenerator, ILandChunk landChunk, IntRect area, int seed, int minAltitude, int maxAltitude)
        {
            return seed;
        }
    }
}
