using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace PokeU.Model
{
    public class LandChunk : ILandChunk
    {
        protected List<LandCase[,]> landObjectsArray;

        protected HashSet<Type> typesInChunk;

        public HashSet<Type> TypesInChunk
        {
            get
            {
                return this.typesInChunk;
            }
        }

        public IntRect Area
        {
            get;
            protected set;
        }

        public int AltitudeMin
        {
            get;
            protected set;
        }

        public int AltitudeMax
        {
            get;
            protected set;
        }

        public LandChunk(int altitudeMin, int altitudeMax, IntRect area)
        {
            this.Area = area;

            this.landObjectsArray = new List<LandCase[,]>();

            this.typesInChunk = new HashSet<Type>();

            this.AltitudeMin = altitudeMin;
            this.AltitudeMax = altitudeMax;

            for (int z = 0; z < this.AltitudeMax - this.AltitudeMin + 1; z++)
            {
                LandCase[,] currentArray = new LandCase[this.Area.Height, this.Area.Width];

                for (int i = 0; i < Area.Height; i++)
                {
                    for (int j = 0; j < Area.Width; j++)
                    {
                        currentArray[i, j] = null;
                    }
                }

                this.landObjectsArray.Add(currentArray);
            }
        }

        public void InitializeLandCase(int i, int j, int z)
        {
            if (this.landObjectsArray[z - this.AltitudeMin][i, j] == null)
            {
                this.landObjectsArray[z - this.AltitudeMin][i, j] = new LandCase();
            }
        }

        public LandCase GetLandCase(int i, int j, int z)
        {
            return this.landObjectsArray[z - this.AltitudeMin][i, j];
        }

        public void AddTypeInChunk(Type type)
        {
            this.typesInChunk.Add(type);
        }

        //public void ComputeObjectsArray(List<ILandLayer> landLayerLists)
        //{
        //    this.landObjectsArray = new List<LandCase[,]>();

        //    this.typesInChunk.Clear();
        //    foreach (ILandLayer landLayer in landLayerLists)
        //    {
        //        this.typesInChunk.UnionWith(landLayer.TypesInLayer);
        //    }

        //    for (int z = 0; z < this.AltitudeMax - this.AltitudeMin + 1; z++)
        //    {
        //        LandCase[,] currentArray = new LandCase[this.Area.Height, this.Area.Width];
        //        for (int i = 0; i < Area.Height; i++)
        //        {
        //            for (int j = 0; j < Area.Width; j++)
        //            {
        //                currentArray[i, j] = null;

        //                foreach (ILandLayer landLayer in landLayerLists)
        //                {
        //                    LandCase landLayerCase = landLayer.GetLandCase(i, j, this.AltitudeMin + z);

        //                    if (landLayerCase != null)
        //                    {
        //                        if(currentArray[i, j] == null)
        //                        {
        //                            currentArray[i, j] = new LandCase();
        //                        }

        //                        foreach (ILandObject landGroundOverWallObject in landLayerCase.LandGroundOverWallList)
        //                        {
        //                            currentArray[i, j].AddLandGroundOverWall(landGroundOverWallObject);
        //                        }

        //                        if (landLayerCase.LandWater != null)
        //                        {
        //                            currentArray[i, j].LandWater = landLayerCase.LandWater;
        //                        }

        //                        if (landLayerCase.LandOverGround != null)
        //                        {
        //                            currentArray[i, j].LandOverGround = landLayerCase.LandOverGround;
        //                        }

        //                        if (landLayerCase.LandWall != null)
        //                        {
        //                            currentArray[i, j].LandWall = landLayerCase.LandWall;
        //                        }

        //                        if (landLayerCase.LandOverWall != null)
        //                        {
        //                            currentArray[i, j].LandOverWall = landLayerCase.LandOverWall;
        //                        }

        //                        foreach (ILandObject landGroundObject in landLayerCase.LandGroundList)
        //                        {
        //                            currentArray[i, j].AddLandGround(landGroundObject);
        //                        }
        //                    }
        //                }

        //                //if(currentArray[i, j] != null)
        //                //{
        //                //    if (currentArray[i, j].LandGroundOverWallList.Count == 0 
        //                //        && currentArray[i, j].LandWall != null
        //                //        && currentArray[i, j].LandGroundList.Count > 0)
        //                //    {
        //                //        foreach(ILandObject landObject in currentArray[i, j].LandGroundList)
        //                //        {
        //                //            ILandObject landObjectOverWall = landObject.CreateLandObjectOverWall(currentArray[i, j].LandWall.LandTransition);

        //                //            if (landObjectOverWall != null)
        //                //            {
        //                //                currentArray[i, j].AddLandGroundOverWall(landObjectOverWall);
        //                //            }
        //                //        }
        //                //    }
        //                //}
        //            }
        //        }

        //        this.landObjectsArray.Add(currentArray);
        //    }
        //}

        public LandCase[,] GetLandObjectsAtAltitude(int altitude)
        {
            return this.landObjectsArray[altitude - this.AltitudeMin];
        }

        public ILandChunk GetSubLandChunk(int altitudeMin, int altitudeMax)
        {
            LandChunk landChunk = new LandChunk(altitudeMin, altitudeMax, this.Area);

            for (int i = 0; i < altitudeMax - altitudeMin; i++)
            {
                int thisIndex = i + altitudeMin - this.AltitudeMin;

                landChunk.landObjectsArray.Add(this.landObjectsArray[thisIndex]);
            }

            landChunk.typesInChunk = new HashSet<Type>(this.typesInChunk);

            return landChunk;
        }
    }
}
