using PokeU.LandGenerator.EpicenterData;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.Model.MountainObject
{
    public class MountainLayerGenerator : ALandLayerGenerator
    {
        private int[,] mountainArea;

        public MountainLayerGenerator() :
            base("mountain")
        {
            this.InitializeGenerator();
        }

        protected override void InitializeGenerator()
        {

        }

        public override void GenerateLandLayer(WorldGenerator worldGenerator, ILandChunk landChunk, IntRect area, int minAltitude, int maxAltitude)
        {
            ALandLayerGenerator altitudeLandLayerGenerator = worldGenerator.Generators["altitude"];

            ALandLayerGenerator cliffLandLayerGenerator = worldGenerator.Generators["cliff"];

            bool[,] subArea = new bool[3, 3];

            bool isThereMountain = false;

            this.ConstructMountainArea(worldGenerator, area);

            for (int i = 0; i < area.Height; i++)
            {
                for (int j = 0; j < area.Width; j++)
                {
                    int altitude = altitudeLandLayerGenerator.GetComputedPowerAt(j, i);

                    int altitudeOffset = cliffLandLayerGenerator.GetComputedPowerAt(j, i);

                    if ((altitude > 6 || (altitude == 6 && altitudeOffset > 0))
                        && altitude < 23)
                    {

                        LandCreationHelper.GetComputedLandType(this, area, i, j, out int mountainTypeInt, out int secondTypeInt, out LandTransition landTransition, out LandTransition secondLandTransition);
                        //this.GetComputedLandType(area, i, j, out MountainType mountainType, out MountainType secondType, out LandTransition landTransition, out LandTransition secondLandTransition);

                        MountainType mountainType = (MountainType)mountainTypeInt;
                        MountainType secondType = (MountainType)secondTypeInt;

                        MountainLandObject groundLandObject = null;
                        MountainLandObject secondGroundLandObject = null;

                        if (mountainType != MountainType.NONE)
                        {
                            groundLandObject = new MountainLandObject(area.Left + j, area.Top + i, altitude, mountainType);
                            groundLandObject.SetTransition(landTransition);

                            isThereMountain = true;
                        }

                        if (secondType != mountainType && secondType != MountainType.NONE)
                        {
                            secondGroundLandObject = new MountainLandObject(area.Left + j, area.Top + i, 0, secondType);
                            secondGroundLandObject.SetTransition(secondLandTransition);

                            isThereMountain = true;
                        }

                        bool onlyGround = altitude == 22 && altitudeOffset > 0;
                        AssignGround(landChunk, i, j, altitude, altitudeOffset, groundLandObject, secondGroundLandObject, onlyGround);
                    }
                }
            }

            if (isThereMountain)
            {
                landChunk.AddTypeInChunk(typeof(MountainLandObject));
            }
        }

        private void ConstructMountainArea(WorldGenerator worldGenerator, IntRect area)
        {
            ALandLayerGenerator altitudeLandLayerGenerator = worldGenerator.Generators["altitude"];

            ALandLayerGenerator groundLandLayerGenerator = worldGenerator.Generators["ground"];

            this.mountainArea = new int[area.Height + 4, area.Width + 4];

            for (int i = -2; i < area.Height + 2; i++)
            {
                for (int j = -2; j < area.Width + 2; j++)
                {
                    int altitude = altitudeLandLayerGenerator.GetComputedPowerAt(j, i);
                    int power = groundLandLayerGenerator.GetComputedPowerAt(j, i);

                    int currentValue = -1;
                    int mountainType = (int)this.GetMountainTypeFromPower(power);

                    if(altitude > 22)
                    {
                        currentValue = -1;
                    }
                    if (altitude > 18)
                    {
                        if(mountainType == 0)
                        {
                            currentValue = 1;
                        }
                        else
                        {
                            currentValue = mountainType;
                        }
                    }
                    else if (altitude > 8)
                    {
                        currentValue = mountainType;
                    }
                    else if (altitude > 6)
                    {
                        if (mountainType == 1)
                        {
                            currentValue = -1;
                        }
                        else
                        {
                            currentValue = mountainType;
                        }
                    }
                    else
                    {
                        currentValue = -1;
                    }

                    this.mountainArea[i + 2, j + 2] = currentValue;
                }
            }

            for (int i = 0; i < area.Height + 2; i++)
            {
                for (int j = 0; j < area.Width + 2; j++)
                {
                    this.powerArea[i + 1, j + 1] = LandCreationHelper.NeedToFillLandAt(this.mountainArea, area, i - 1, j - 1);
                    //this.powerArea[i + 1, j + 1] = this.NeedToFillMountainAt(area, i - 1, j - 1);
                }
            }
        }

        //protected int NeedToFillMountainAt(
        //    IntRect area,
        //    int i, int j)
        //{
        //    bool[,] subAreaBool = new bool[3, 3];
        //    int[,] subAreaInt = new int[3, 3];

        //    int maxValue = int.MinValue;
        //    int minValue = int.MaxValue;
        //    for (int y = -1; y < 2; y++)
        //    {
        //        for (int x = -1; x < 2; x++)
        //        {
        //            int altitude = this.mountainArea[i + y + 2, j + x + 2];

        //            maxValue = Math.Max(maxValue, altitude);

        //            minValue = Math.Min(minValue, altitude);

        //            subAreaInt[y + 1, x + 1] = altitude;
        //        }
        //    }

        //    bool needToFill = false;
        //    if (subAreaInt[1, 1] != maxValue)
        //    {
        //        for (int y = 0; y < 3; y++)
        //        {
        //            for (int x = 0; x < 3; x++)
        //            {
        //                if (subAreaInt[y, x] != maxValue)
        //                {
        //                    subAreaBool[y, x] = false;
        //                }
        //                else
        //                {
        //                    subAreaBool[y, x] = true;
        //                }
        //            }
        //        }

        //        needToFill = ALandLayerGenerator.NeedToFill(ref subAreaBool);
        //    }

        //    if (needToFill)
        //    {
        //        return maxValue;
        //    }
        //    return subAreaInt[1, 1];
        //}

        protected virtual MountainType GetMountainTypeFromPower(float power)
        {
            return (MountainType)Math.Max(Math.Min(1, (power % 4) - 2), -1);
        }

        //private void GetComputedLandType(
        //    IntRect area,
        //    int i, int j,
        //    out MountainType mountainType,
        //    out MountainType secondType,
        //    out LandTransition landtransition,
        //    out LandTransition secondLandtransition)
        //{
        //    bool[,] subAreaBool = new bool[3, 3];
        //    int[,] subAreaInt = new int[3, 3];

        //    int maxValue = int.MinValue;
        //    int minValue = int.MaxValue;
        //    for (int y = -1; y < 2; y++)
        //    {
        //        for (int x = -1; x < 2; x++)
        //        {
        //            int currentValue = this.GetComputedPowerAt(j + x, i + y);

        //            maxValue = Math.Max(maxValue, currentValue);

        //            minValue = Math.Min(minValue, currentValue);

        //            subAreaInt[y + 1, x + 1] = currentValue;
        //        }
        //    }

        //    mountainType = (MountainType)subAreaInt[1, 1];
        //    landtransition = LandTransition.NONE;
        //    secondLandtransition = LandTransition.NONE;
        //    secondType = mountainType;

        //    if (subAreaInt[1, 1] != maxValue)
        //    {

        //        int primaryType = -1;
        //        for (int y = 0; y < 3; y++)
        //        {
        //            for (int x = 0; x < 3; x++)
        //            {
        //                if (subAreaInt[y, x] != maxValue)
        //                {
        //                    subAreaBool[y, x] = false;

        //                    if (subAreaInt[y, x] != -1)
        //                    {
        //                        primaryType = subAreaInt[y, x];
        //                    }
        //                }
        //                else
        //                {
        //                    subAreaBool[y, x] = true;
        //                }
        //            }
        //        }

        //        secondLandtransition = ALandLayerGenerator.GetLandTransitionFrom(ref subAreaBool);

        //        if (secondLandtransition != LandTransition.NONE)
        //        {
        //            secondType = (MountainType)maxValue;
        //        }

        //        if (subAreaInt[1, 1] == -1 && primaryType != -1)
        //        {
        //            mountainType = (MountainType)primaryType;

        //            for (int y = 0; y < 3; y++)
        //            {
        //                for (int x = 0; x < 3; x++)
        //                {
        //                    if (subAreaInt[y, x] != primaryType)
        //                    {
        //                        subAreaBool[y, x] = false;
        //                    }
        //                    else
        //                    {
        //                        subAreaBool[y, x] = true;
        //                    }
        //                }
        //            }

        //            landtransition = ALandLayerGenerator.GetLandTransitionFrom(ref subAreaBool);
        //        }
        //    }
        //}
    }
}
