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
        protected List<List<ILandObject>[,]> landObjectsArray;

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

            this.landObjectsArray = new List<List<ILandObject>[,]>();

            this.AltitudeMin = altitudeMin;
            this.AltitudeMax = altitudeMax;
        }

        public void ComputeObjectsArray(List<ILandLayer> landLayerLists)
        {
            this.landObjectsArray = new List<List<ILandObject>[,]>();

            for (int z = 0; z < this.AltitudeMax - this.AltitudeMin + 1; z++)
            {
                List<ILandObject>[,] currentArray = new List<ILandObject>[this.Area.Height, this.Area.Width];
                for (int i = 0; i < Area.Height; i++)
                {
                    for (int j = 0; j < Area.Width; j++)
                    {
                        List<ILandObject> listLandObject = new List<ILandObject>();

                        foreach (ILandLayer landLayer in landLayerLists)
                        {
                            ILandObject landObject = landLayer.GetLandObjectAtCoord(i, j, this.AltitudeMin + z);
                            if (landObject != null)
                            {
                                listLandObject.Add(landObject);
                            }
                        }

                        currentArray[i, j] = null;
                        if (listLandObject.Any())
                        {
                            currentArray[i, j] = listLandObject;
                        }
                    }
                }

                this.landObjectsArray.Add(currentArray);
            }
        }

        public List<ILandObject>[,] GetLandObjectsAtAltitude(int altitude)
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

            return landChunk;
        }
    }
}
