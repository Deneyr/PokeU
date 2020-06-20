using PokeU.LandGenerator.EpicenterData;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.Model
{
    public static class LandCreationHelper
    {
        
        public static int NeedToFillLandAt(
            int[,] mountainArea,
            IntRect area,
            int i, int j)
        {
            bool[,] subAreaBool = new bool[3, 3];
            int[,] subAreaInt = new int[3, 3];

            int maxValue = int.MinValue;
            int minValue = int.MaxValue;
            for (int y = -1; y < 2; y++)
            {
                for (int x = -1; x < 2; x++)
                {
                    int altitude = mountainArea[i + y + 2, j + x + 2];

                    maxValue = Math.Max(maxValue, altitude);

                    minValue = Math.Min(minValue, altitude);

                    subAreaInt[y + 1, x + 1] = altitude;
                }
            }

            bool needToFill = false;
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

                needToFill = ALandLayerGenerator.NeedToFill(ref subAreaBool);
            }

            if (needToFill)
            {
                return maxValue;
            }
            return subAreaInt[1, 1];
        }

        public static void GetComputedLandType(
            ALandLayerGenerator generator,
            IntRect area,
            int i, int j,
            out int mountainType,
            out int secondType,
            out LandTransition landtransition,
            out LandTransition secondLandtransition)
        {
            bool[,] subAreaBool = new bool[3, 3];
            int[,] subAreaInt = new int[3, 3];

            int maxValue = int.MinValue;
            int minValue = int.MaxValue;
            for (int y = -1; y < 2; y++)
            {
                for (int x = -1; x < 2; x++)
                {
                    int currentValue = generator.GetComputedPowerAt(j + x, i + y);

                    maxValue = Math.Max(maxValue, currentValue);

                    minValue = Math.Min(minValue, currentValue);

                    subAreaInt[y + 1, x + 1] = currentValue;
                }
            }

            mountainType = subAreaInt[1, 1];
            landtransition = LandTransition.NONE;
            secondLandtransition = LandTransition.NONE;
            secondType = mountainType;

            if (subAreaInt[1, 1] != maxValue)
            {

                int primaryType = -1;
                for (int y = 0; y < 3; y++)
                {
                    for (int x = 0; x < 3; x++)
                    {
                        if (subAreaInt[y, x] != maxValue)
                        {
                            subAreaBool[y, x] = false;

                            if (subAreaInt[y, x] != -1)
                            {
                                primaryType = subAreaInt[y, x];
                            }
                        }
                        else
                        {
                            subAreaBool[y, x] = true;
                        }
                    }
                }

                secondLandtransition = ALandLayerGenerator.GetLandTransitionFrom(ref subAreaBool);

                if (secondLandtransition != LandTransition.NONE)
                {
                    secondType = maxValue;
                }

                if (subAreaInt[1, 1] == -1 && primaryType != -1)
                {
                    mountainType = primaryType;

                    for (int y = 0; y < 3; y++)
                    {
                        for (int x = 0; x < 3; x++)
                        {
                            if (subAreaInt[y, x] != primaryType)
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
}
