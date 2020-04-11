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
    public class GroundLayerGenerator2 : ALandLayerGenerator
    {
        public GroundLayerGenerator2():
            base("ground")
        {
            this.InitializeGenerator();
        }

        protected override void InitializeGenerator()
        {
            this.AddEpicenterLayer(256, DigressionMethod.SMOOTH, 5, 1, 1);

            this.AddEpicenterLayer(128, DigressionMethod.LINEAR, 20, 1, 1);

            this.AddEpicenterLayer(64, DigressionMethod.SQUARE_DEC, 50, 2, 2);
        }

        public override ILandLayer GenerateLandLayer(WorldGenerator worldGenerator, IntRect area, int minAltitude, int maxAltitude)
        {
            ALandLayerGenerator altitudeLandLayerGenerator = worldGenerator.Generators["altitude"];

            LandLayer groundLandLayer = new LandLayer(minAltitude, maxAltitude, area);

            bool[,] subArea = new bool[3, 3];

            for (int i = 0; i < area.Height; i++)
            {
                for (int j = 0; j < area.Width; j++)
                {
                    this.GetLandType(area, i, j, out LandType landType, out LandType secondType, out LandTransition landTransition);

                    int altitude = altitudeLandLayerGenerator.GetComputedPowerAt(j, i);
                    LandCase landCase = groundLandLayer.GetLandCase(i, j, altitude);

                    GroundLandObject groundLandObject = new GroundLandObject(area.Left + j, area.Top + i, altitude, landType);

                    groundLandLayer.InitializeLandCase(i, j, altitude);
                    landCase.AddLandGround(groundLandObject);

                    if (secondType != landType)
                    {
                        GroundLandObject secondGroundLandObject = new GroundLandObject(area.Left + j, area.Top + i, altitude, landType);
                        secondGroundLandObject.SetTransition(landTransition);

                        landCase.AddLandGround(secondGroundLandObject);
                    }
                }
            }

            groundLandLayer.AddTypeInLayer(typeof(GroundLandObject));

            return groundLandLayer;
        }

        protected virtual LandType GetLandTypeFromPower(float power)
        {
            return (LandType)Math.Max(Math.Min(3, power / 2), 0);
        }

        private void GetLandType(
            IntRect area,
            int i, int j,
            out LandType landType,
            out LandType secondType,
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
                    float power = this.GetComputedPowerAt(j + x, i + y);

                    int currentValue = (int)this.GetLandTypeFromPower(power);

                    maxValue = Math.Max(maxValue, currentValue);

                    minValue = Math.Min(minValue, currentValue);

                    subAreaInt[y + 1, x + 1] = currentValue;
                }
            }

            landType = (LandType)subAreaInt[1, 1];
            landtransition = LandTransition.NONE;
            secondType = landType;

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
                    secondType = (LandType)maxValue;
                }
            }
        }
    }
}
