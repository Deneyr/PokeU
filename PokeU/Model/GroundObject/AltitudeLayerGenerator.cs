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
            this.AddEpicenterLayer(512, DigressionMethod.SMOOTH, 4, -5, 5);

            this.AddEpicenterLayer(256, DigressionMethod.SMOOTH, 5, -2, 2);

            this.AddEpicenterLayer(128, DigressionMethod.LINEAR, 20, -1, 1);

            this.AddEpicenterLayer(64, DigressionMethod.SQUARE_DEC, 50, -1, 1);
        }

        public override float GetPowerAt(Vector2f position)
        {
            float powerResult = base.GetPowerAt(position);

            powerResult = Math.Max(-this.altitudeRange, Math.Min(this.altitudeRange, powerResult));

            return powerResult;
        }

        public override ILandLayer GenerateLandLayer(WorldGenerator worldGenerator, IntRect area, int minAltitude, int maxAltitude)
        {
            return null;
        }
    }
}
