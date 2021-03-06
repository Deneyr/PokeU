﻿using PokeU.LandGenerator.EpicenterData;
using PokeU.Model.GroundObject;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.Model.GrassObject
{
    public class GrassLayerGenerator: ALandLayerGenerator
    {
        private int[,] grassArea;

        public GrassLayerGenerator() :
            base("grass")
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

            bool isThereGrass = false;

            this.ConstructGrassArea(worldGenerator, area);

            for (int i = 0; i < area.Height; i++)
            {
                for (int j = 0; j < area.Width; j++)
                {
                    int altitude = altitudeLandLayerGenerator.GetComputedPowerAt(j, i);

                    int altitudeOffset = cliffLandLayerGenerator.GetComputedPowerAt(j, i);

                    if ((altitude > -2 || (altitude == -2 && altitudeOffset > 0))
                        && altitude < 7)
                    {
                        LandCreationHelper.GetComputedLandType(this, area, i, j, out int grassTypeInt, out int secondTypeInt, out LandTransition landTransition, out LandTransition secondLandTransition);
                        //this.GetComputedLandType(area, i, j, out GrassType grassType, out GrassType secondType, out LandTransition landTransition, out LandTransition secondLandTransition);

                        GrassType grassType = (GrassType)grassTypeInt;
                        GrassType secondType = (GrassType)secondTypeInt;

                        GroundLandObject groundLandObject = null;
                        GroundLandObject secondGroundLandObject = null;

                        if (grassType != GrassType.NONE)
                        {
                            groundLandObject = new GrassLandObject(area.Left + j, area.Top + i, altitude, grassType);
                            groundLandObject.SetTransition(landTransition);

                            isThereGrass = true;
                        }

                        if (secondType != grassType && secondType != GrassType.NONE)
                        {
                            secondGroundLandObject = new GrassLandObject(area.Left + j, area.Top + i, 0, secondType);
                            secondGroundLandObject.SetTransition(secondLandTransition);

                            isThereGrass = true;
                        }

                        bool onlyGround = altitude == 6 && altitudeOffset > 0;
                        AssignGround(landChunk, i, j, altitude, altitudeOffset, groundLandObject, secondGroundLandObject, onlyGround);
                    }
                }
            }

            if (isThereGrass)
            {
                landChunk.AddTypeInChunk(typeof(GrassLandObject));
            }

            return seed;
        }

        private void ConstructGrassArea(WorldGenerator worldGenerator, IntRect area)
        {
            ALandLayerGenerator altitudeLandLayerGenerator = worldGenerator.Generators["altitude"];

            ALandLayerGenerator groundLandLayerGenerator = worldGenerator.Generators["ground"];

            this.grassArea = new int[area.Height + 4, area.Width + 4];

            for (int i = -2; i < area.Height + 2; i++)
            {
                for (int j = -2; j < area.Width + 2; j++)
                {
                    int altitude = altitudeLandLayerGenerator.GetComputedPowerAt(j, i);
                    int power = groundLandLayerGenerator.GetComputedPowerAt(j, i);

                    int currentValue = -1;
                    int grassType = (int)this.GetGrassTypeFromPower(power);

                    if (altitude > 6)
                    {
                        currentValue = -1;
                    }
                    else if (altitude > 4)
                    {
                        if (grassType == 2 || grassType == 3)
                        {
                            currentValue = grassType;
                        }
                    }
                    else if (altitude > 1)
                    {
                        if (altitude > 2 && grassType == 0)
                        {
                            currentValue = 1;
                        }
                        else
                        {
                            currentValue = grassType;
                        }
                    }
                    else if (altitude > -2)
                    {
                        if (grassType == 0)
                        {
                            currentValue = 0;
                        }
                        else
                        {
                            currentValue = -1;
                        }
                    }
                    else
                    {
                        currentValue = -1;
                    }

                    this.grassArea[i + 2, j + 2] = currentValue;
                }
            }

            for (int i = 0; i < area.Height + 2; i++)
            {
                for (int j = 0; j < area.Width + 2; j++)
                {
                    this.powerArea[i + 1, j + 1] = LandCreationHelper.NeedToFillLandAt(this.grassArea, area, i - 1, j - 1);
                    //this.powerArea[i + 1, j + 1] = this.NeedToFillGrassAt(area, i - 1, j - 1);
                }
            }
        }

        //protected int NeedToFillGrassAt(
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
            //            int altitude = this.grassArea[i + y + 2, j + x + 2];

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

        protected virtual GrassType GetGrassTypeFromPower(float power)
        {
            return (GrassType)Math.Max(Math.Min(3, power), 0);
        }

        //private void GetComputedLandType(
        //    IntRect area,
        //    int i, int j,
        //    out GrassType grassType,
        //    out GrassType secondType,
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

        //    grassType = (GrassType)subAreaInt[1, 1];
        //    landtransition = LandTransition.NONE;
        //    secondLandtransition = LandTransition.NONE;
        //    secondType = grassType;

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

        //                    if(subAreaInt[y, x] != -1)
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
        //            secondType = (GrassType)maxValue;
        //        }

        //        if(subAreaInt[1, 1] == -1 && primaryType != -1)
        //        {
        //            grassType = (GrassType)primaryType;

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
