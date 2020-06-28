using PokeU.Model;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.LandGenerator.EpicenterData
{
    public abstract class ALandLayerGenerator
    {

        private static int[,] CENTER_MATRIX = new int[,]
        {
            {1, 2, 1},
            {2, 0, 2},
            {1, 2, 1}
        };

        private static int[,] HORIZONTAL_MATRIX = new int[,]
        {
            {2, 2, 2},
            {1, 0, 1},
            {2, 2, 2}
        };

        private static int[,] VERTICAL_MATRIX = new int[,]
        {
            {2, 1, 2},
            {2, 0, 2},
            {2, 1, 2}
        };

        private static int[,] BEND_BOT_LEFT_MATRIX = new int[,]
        {
            {2, 1, 2},
            {0, 0, 1},
            {1, 0, 2}
        };

        private static int[,] BEND_TOP_LEFT_MATRIX = new int[,]
        {
            {1, 0, 2},
            {0, 0, 1},
            {2, 1, 2}
        };

        private static int[,] BEND_TOP_RIGHT_MATRIX = new int[,]
        {
            {2, 0, 1},
            {1, 0, 0},
            {2, 1, 2}
        };

        private static int[,] BEND_BOT_RIGHT_MATRIX = new int[,]
        {
            {2, 1, 2},
            {1, 0, 0},
            {2, 0, 1}
        };

        ///

        private static int[,] RIGHT_MATRIX = new int[,]
        {
            {2, 0, 0},
            {1, 0, 0},
            {2, 0, 0}
        };

        private static int[,] BOT_MATRIX = new int[,]
        {
            {2, 1, 2},
            {0, 0, 0},
            {0, 0, 0}
        };

        private static int[,] LEFT_MATRIX = new int[,]
        {
            {0, 0, 2},
            {0, 0, 1},
            {0, 0, 2}
        };

        private static int[,] TOP_MATRIX = new int[,]
        {
            {0, 0, 0},
            {0, 0, 0},
            {2, 1, 2}
        };

        //
        private static int[,] TOP_RIGHT_MATRIX = new int[,]
        {
            {0, 0, 0},
            {0, 0, 0},
            {1, 0, 0}
        };

        private static int[,] BOT_RIGHT_MATRIX = new int[,]
        {
            {1, 0, 0},
            {0, 0, 0},
            {0, 0, 0}
        };

        private static int[,] BOT_LEFT_MATRIX = new int[,]
        {
            {0, 0, 1},
            {0, 0, 0},
            {0, 0, 0}
        };

        private static int[,] TOP_LEFT_MATRIX = new int[,]
        {
            {0, 0, 0},
            {0, 0, 0},
            {0, 0, 1}
        };

        //
        private static int[,] BOT_INT_RIGHT_MATRIX = new int[,]
        {
            {2, 1, 2},
            {1, 0, 0},
            {2, 0, 0}
        };

        private static int[,] BOT_INT_LEFT_MATRIX = new int[,]
        {
            {2, 1, 2},
            {0, 0, 1},
            {0, 0, 2}
        };

        private static int[,] TOP_INT_LEFT_MATRIX = new int[,]
        {
            {0, 0, 2},
            {0, 0, 1},
            {2, 1, 2}
        };

        private static int[,] TOP_INT_RIGHT_MATRIX = new int[,]
        {
            {2, 0, 0},
            {1, 0, 0},
            {2, 1, 2}
        };


        protected List<EpicenterLayer> epicenterLayersList;

        protected int[,] powerArea;

        public ALandLayerGenerator(string name)
        {
            this.Name = name;

            this.epicenterLayersList = new List<EpicenterLayer>();
        }

        public string Name
        {
            get;
            protected set;
        }

        protected abstract void InitializeGenerator();

        protected virtual void AddEpicenterLayer(int influenceRadius, DigressionMethod digressionMethod, int nbMaxPoints, float pointPowerMin, float pointPowerMax)
        {
            this.epicenterLayersList.Add(new EpicenterLayer(influenceRadius, digressionMethod, nbMaxPoints, pointPowerMin, pointPowerMax));
        }

        protected virtual void AddEpicenterDensityLayer(int influenceRadius, DigressionMethod digressionMethod, int nbMaxPoints, float pointPowerMin, float pointPowerMax)
        {
            this.epicenterLayersList.Add(new EpicenterDensityLayer(influenceRadius, digressionMethod, nbMaxPoints, pointPowerMin, pointPowerMax));
        }

        public virtual void GenerateEpicenterLayer(int seed, IntRect area)
        {
            int i = 0;
            foreach (EpicenterLayer layer in this.epicenterLayersList)
            {
                layer.GenerateEpicenterPoints(seed * this.Name.GetHashCode() * i, area);
                i++;
            }

            this.powerArea = new int[area.Height + 4, area.Width + 4];

            if (this.epicenterLayersList.Count > 0)
            {
                bool[,] subArea = new bool[3, 3];

                for (i = 0; i < area.Height + 4; i++)
                {
                    for (int j = 0; j < area.Width + 4; j++)
                    {
                        this.powerArea[i, j] = this.NeedToFillAt(area, i - 2, j - 2);
                    }
                }
            }
        }

        private int NeedToFillAt(
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
                    int altitude = (int)this.GetPowerAt(new Vector2f(area.Left + j + x, area.Top + i + y));

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

            if(needToFill)
            {
                return maxValue;
            }
            return subAreaInt[1, 1];
        }

        public abstract int GenerateLandLayer(WorldGenerator worldGenerator, ILandChunk landChunk, IntRect area, int seed, int minAltitude, int maxAltitude);

        public virtual float GetPowerAt(Vector2f position)
        {
            float powerResult = 0;

            foreach(EpicenterLayer layer in this.epicenterLayersList)
            {
                powerResult += layer.GetPowerAt(position);
            }

            return powerResult; 
        }

        public virtual int GetComputedPowerAt(int j, int i)
        {
            return this.powerArea[i + 2, j + 2];
        }

        private static bool MatchMatrix(ref bool[,] array, ref int[,] matrix)
        {
            bool result = true;
            for(int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    switch(matrix[i, j])
                    {
                        case 0:
                            result &= array[i, j] == false;
                            break;
                        case 1:
                            result &= array[i, j];
                            break;
                    }
                }
            }

            return result;
        }

        protected static void AssignGround(
            ILandChunk landChunk,
            int i, int j,
            int altitude, int altitudeOffset, 
            ILandObject mainLandObject, ILandObject secondLandObject,
            bool onlyGround = false)
        {
            if (mainLandObject != null || secondLandObject != null)
            {
                if (altitudeOffset == 0)
                {
                    landChunk.InitializeLandCase(i, j, altitude);
                    LandCase landCase = landChunk.GetLandCase(i, j, altitude);

                    if (mainLandObject != null)
                    {
                        landCase.AddLandGround(mainLandObject);
                    }

                    if (secondLandObject != null)
                    {
                        landCase.AddLandGround(secondLandObject);
                    }
                }
                else
                {
                    for (int z = 0; z < altitudeOffset; z++)
                    {
                        landChunk.InitializeLandCase(i, j, altitude + z);
                        LandCase landCase = landChunk.GetLandCase(i, j, altitude + z);
                        //if (landCase == null)
                        //{
                        //    throw new Exception("if offset > 0, case must be already initialized thanks to CliffLayerGenerator !");
                        //}

                        if (mainLandObject != null)
                        {
                            ILandObject cloneMainObject = mainLandObject.Clone();
                            cloneMainObject.Altitude = altitude + z;

                            landCase.AddLandGround(cloneMainObject);
                        }

                        if (secondLandObject != null)
                        {
                            ILandObject cloneSecondObject = secondLandObject.Clone();
                            cloneSecondObject.Altitude = altitude + z;

                            landCase.AddLandGround(cloneSecondObject);
                        }

                        if (onlyGround == false && landCase.LandWall != null)
                        {
                            if (mainLandObject != null)
                            {
                                ILandObject cloneMainObject = mainLandObject.Clone(landCase.LandWall.LandTransition);
                                if (cloneMainObject != null)
                                {
                                    cloneMainObject.Altitude = altitude + z;

                                    landCase.AddLandGroundOverWall(cloneMainObject);
                                }
                            }

                            if (secondLandObject != null)
                            {
                                ILandObject cloneSecondObject = secondLandObject.Clone(landCase.LandWall.LandTransition);
                                if (cloneSecondObject != null)
                                {
                                    cloneSecondObject.Altitude = altitude + z;

                                    landCase.AddLandGroundOverWall(cloneSecondObject);
                                }
                            }
                        }
                    }
                }
            }
        }

        //protected static void GenerateGroundOverCliff(LandCase mainLandCase, ILandObject mainLandObject)
        //{
        //    if (mainLandCase.LandWall != null)
        //    {
        //        ILandObject landObjectOverWall = mainLandObject.Clone(mainLandCase.LandWall.LandTransition);

        //        if (landObjectOverWall != null)
        //        {
        //            mainLandCase.AddLandGroundOverWall(landObjectOverWall);
        //        }
        //        else
        //        {
        //            throw new Exception("Ground clone creation failed !");
        //        }
        //    }
        //}

        public static bool NeedToFill(ref bool[,] array)
        {
            if (MatchMatrix(ref array, ref HORIZONTAL_MATRIX)
                || MatchMatrix(ref array, ref VERTICAL_MATRIX)
                || MatchMatrix(ref array, ref CENTER_MATRIX)
                || MatchMatrix(ref array, ref BEND_BOT_LEFT_MATRIX)
                || MatchMatrix(ref array, ref BEND_BOT_RIGHT_MATRIX)
                || MatchMatrix(ref array, ref BEND_TOP_LEFT_MATRIX)
                || MatchMatrix(ref array, ref BEND_TOP_RIGHT_MATRIX))
                //|| MatchMatrix(ref array, ref CENTER_LEFT_MATRIX))
            {
                return true;
            }
            return false;
        }

        public static LandTransition GetLandTransitionFrom(ref bool[,] array)
        {

            if (MatchMatrix(ref array, ref BOT_RIGHT_MATRIX))
            {
                return LandTransition.BOT_RIGHT;
            }
            else if (MatchMatrix(ref array, ref BOT_LEFT_MATRIX))
            {
                return LandTransition.BOT_LEFT;
            }
            else if (MatchMatrix(ref array, ref TOP_LEFT_MATRIX))
            {
                return LandTransition.TOP_LEFT;
            }
            else if (MatchMatrix(ref array, ref TOP_RIGHT_MATRIX))
            {
                return LandTransition.TOP_RIGHT;
            }

            else if (MatchMatrix(ref array, ref BOT_INT_RIGHT_MATRIX))
            {
                return LandTransition.BOT_INT_RIGHT;
            }
            else if (MatchMatrix(ref array, ref BOT_INT_LEFT_MATRIX))
            {
                return LandTransition.BOT_INT_LEFT;
            }
            else if (MatchMatrix(ref array, ref TOP_INT_LEFT_MATRIX))
            {
                return LandTransition.TOP_INT_LEFT;
            }
            else if (MatchMatrix(ref array, ref TOP_INT_RIGHT_MATRIX))
            {
                return LandTransition.TOP_INT_RIGHT;
            }

            else if (MatchMatrix(ref array, ref RIGHT_MATRIX))
            {
                return LandTransition.RIGHT;
            }
            else if (MatchMatrix(ref array, ref BOT_MATRIX))
            {
                return LandTransition.BOT;
            }
            else if (MatchMatrix(ref array, ref LEFT_MATRIX))
            {
                return LandTransition.LEFT;
            }
            else if (MatchMatrix(ref array, ref TOP_MATRIX))
            {
                return LandTransition.TOP;
            }

            return LandTransition.NONE;
            /*
            // Sides check
            if (array[1,0])
            {
                if (array[0,1])
                {
                    return LandTransition.BOT_INT_RIGHT;
                }
                else if (array[2,1])
                {
                    return LandTransition.TOP_INT_RIGHT;
                }
                else
                {
                    return LandTransition.RIGHT;
                }
            }

            if (array[0,1])
            {
                if (array[1,0])
                {
                    return LandTransition.BOT_INT_RIGHT;
                }
                else if (array[1,2])
                {
                    return LandTransition.BOT_INT_LEFT;
                }
                else
                {
                    return LandTransition.BOT;
                }
            }

            if (array[1,2])
            {
                if (array[0,1])
                {
                    return LandTransition.BOT_INT_LEFT;
                }
                else if (array[2,1])
                {
                    return LandTransition.TOP_INT_LEFT;
                }
                else
                {
                    return LandTransition.LEFT;
                }
            }

            if (array[2,1])
            {
                if (array[1,0])
                {
                    return LandTransition.TOP_INT_RIGHT;
                }
                else if (array[1,2])
                {
                    return LandTransition.TOP_INT_LEFT;
                }
                else
                {
                    return LandTransition.TOP;
                }
            }

            //Corners check
            if (array[0,0])
            {
                return LandTransition.TOP_LEFT;
            }

            if (array[0,2])
            {
                return LandTransition.TOP_RIGHT;
            }

            if (array[2,2])
            {
                return LandTransition.BOT_RIGHT;
            }

            if (array[2,0])
            {
                return LandTransition.BOT_LEFT;
            }

            return LandTransition.WHOLE; */
        }
    }
}
