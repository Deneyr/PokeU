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
            ALandLayerGenerator altitudeLandLayerGenerator = worldGenerator.Generators["ground"];

            LandLayer altitudeLandLayer = new LandLayer(minAltitude, maxAltitude, area);

            bool[,] subArea = new bool[3, 3];

            for (int i = 0; i < area.Height; i++)
            {
                for (int j = 0; j < area.Width; j++)
                {
                    int altitude = (int)this.GetPowerAt(new Vector2f(area.Left + j, area.Top + i));

                    this.GetLandType(area, i, j, out LandTransition landTransition);

                    if (landTransition != LandTransition.NONE)
                    {
                        AltitudeLandObject altitudeLandObject = new AltitudeLandObject(area.Left + j, area.Top + i, altitude, LandType.GRASS);
                        altitudeLandLayer.AddLandObject(altitudeLandObject, i, j);

                        altitudeLandObject.SetLandTransition(landTransition);
                    }
                }
            }

            return altitudeLandLayer;
        }

        private void GetLandType(
            IntRect area,
            int i, int j,
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
                    int altitude = (int)this.GetPowerAt(new Vector2f(area.Left + j + x, area.Top + i + y));

                    maxValue = Math.Max(maxValue, altitude);

                    minValue = Math.Min(minValue, altitude);

                    subAreaInt[y + 1, x + 1] = altitude;
                }
            }

            landtransition = LandTransition.NONE;

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
            }
        }
    }
}
