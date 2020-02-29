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
        public GroundLayerGenerator()
        {
            this.InitializeGenerator();
        }

        protected override void InitializeGenerator()
        {
            this.AddEpicenterLayer(256, DigressionMethod.SMOOTH, 5, 1);

            this.AddEpicenterLayer(128, DigressionMethod.LINEAR, 20, 1);

            this.AddEpicenterLayer(64, DigressionMethod.SQUARE_DEC, 50, 2);
        }

        public override ILandLayer GenerateLandLayer(WorldGenerator worldGenerator, IntRect area, int minAltitude, int maxAltitude)
        {
            LandLayer groundLandLayer = new LandLayer(minAltitude, maxAltitude, area);

            for(int i = 0; i < area.Height; i++)
            {
                for (int j = 0; j < area.Width; j++)
                {
                    float power = this.GetPowerAt(new Vector2f(area.Left + j, area.Top + i));

                    LandType landType = (LandType) Math.Max(Math.Min(3, power / 2), 0);

                    groundLandLayer.AddLandObject(new GroundLandObject(area.Left + j, area.Top + i, 0, landType), i, j);
                }
            }

            return groundLandLayer;
        }
    }
}
