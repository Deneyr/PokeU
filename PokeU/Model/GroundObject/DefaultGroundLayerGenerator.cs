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
    public class DefaultGroundLayerGenerator : ALandLayerGenerator
    {
        public DefaultGroundLayerGenerator() :
            base("defaultGround")
        {
            this.InitializeGenerator();
        }

        protected override void InitializeGenerator()
        {

        }

        public override int GenerateLandLayer(WorldGenerator worldGenerator, ILandChunk landChunk, IntRect area, int seed, int minAltitude, int maxAltitude)
        {
            ALandLayerGenerator altitudeLandLayerGenerator = worldGenerator.Generators["altitude"];

            ALandLayerGenerator cliffLandLayerGenerator = worldGenerator.Generators["cliff"];

            bool[,] subArea = new bool[3, 3];

            for (int i = 0; i < area.Height; i++)
            {
                for (int j = 0; j < area.Width; j++)
                {                    
                    this.GetComputedLandType(altitudeLandLayerGenerator, area, i, j, out LandType landType, out LandType secondType, out LandTransition landTransition);

                    int altitude = altitudeLandLayerGenerator.GetComputedPowerAt(j, i);

                    int altitudeOffset = cliffLandLayerGenerator.GetComputedPowerAt(j, i);

                    GroundLandObject groundLandObject = new GroundLandObject(area.Left + j, area.Top + i, altitude, landType);

                    GroundLandObject secondGroundLandObject = new GroundLandObject(area.Left + j, area.Top + i, altitude, secondType);
                    secondGroundLandObject.SetTransition(landTransition);


                    AssignGround(landChunk, i, j, altitude, altitudeOffset, groundLandObject, secondGroundLandObject);
                    //for (int z = 0; z < altitudeOffset - 1; z++)
                    //{
                    //    LandCase landCase = landChunk.GetLandCase(i, j, altitude + z);

                    //    GroundLandObject groundLandObject = new GroundLandObject(area.Left + j, area.Top + i, altitude, landType);

                    //    GenerateGroundOverCliff(landCase, groundLandObject);
                    //}

                    //if (secondType != landType)
                    //{
                    //    //landChunk.InitializeLandCase(i, j, altitude + z);
                    //    LandCase secondLandCase = landChunk.GetLandCase(i, j, altitude + altitudeOffset - 1);

                    //    GroundLandObject secondGroundLandObject = new GroundLandObject(area.Left + j, area.Top + i, altitude + altitudeOffset - 1, secondType);
                    //    secondGroundLandObject.SetTransition(landTransition);

                    //    secondLandCase.AddLandGroundOverWall(secondGroundLandObject);
                    //}
                    //else
                    //{
                    //    LandCase landCase = landChunk.GetLandCase(i, j, altitude + altitudeOffset - 1);

                    //    GroundLandObject groundLandObject = new GroundLandObject(area.Left + j, area.Top + i, altitude + altitudeOffset - 1, landType);

                    //    GenerateGroundOverCliff(landCase, groundLandObject);
                    //}
                }
            }

            landChunk.AddTypeInChunk(typeof(GroundLandObject));

            return seed;
        }

        protected virtual LandType GetLandTypeFromPower(float power)
        {
            LandType landType = LandType.GROUND;

            if (power < -1)
            {
                landType = LandType.GROUND;
            }
            else if (power < 2)
            {
                landType = LandType.SAND;
            }
            /*else if (power < 10)
            {
                landType = LandType.GRASS;
            }*/
            else if (power < 20)
            {
                landType = LandType.STONE;
            }
            else
            {
                landType = LandType.SNOW;
            }

            return landType;
        }

        private void GetComputedLandType(
            ALandLayerGenerator altitudeLandLayerGenerator,
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
                    float power = altitudeLandLayerGenerator.GetComputedPowerAt(j + x, i + y);

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
