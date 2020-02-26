using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace PokeU.Model
{
    public class LandLayer : ILandLayer
    {
        protected int altitudeMin;

        protected int altitudeMax;

        protected IntRect area;

        protected List<ILandObject[,]> landObjectsArray;

        public LandLayer(int altitudeMin, int altitudeMax, IntRect area)
        {
            this.altitudeMin = altitudeMin;

            this.altitudeMax = altitudeMax;

            this.area = area;

            this.landObjectsArray = new List<ILandObject[,]>();

            for(int z = 0; z < altitudeMax - altitudeMin + 1; z++)
            {
                ILandObject[,] landObjectsArray = new ILandObject[area.Height, area.Width];

                for (int i = 0; i < landObjectsArray.GetLength(0); i++)
                {
                    for (int j = 0; j < landObjectsArray.GetLength(1); j++)
                    {
                        landObjectsArray[i, j] = null;
                    }
                }

                this.landObjectsArray.Add(landObjectsArray);
            }
        }

        public void AddLandObject(ILandObject landObject, int i, int j)
        {
            this.landObjectsArray[landObject.Altitude][i, j] = landObject;
        }

        public List<ILandObject> GetLandObjectsAtAltitude(int altitude)
        {
            List<ILandObject> listLandObject = new List<ILandObject>();

            ILandObject[,] landObjectsArray = this.landObjectsArray[altitude - this.altitudeMin];

            for (int i = 0; i < landObjectsArray.GetLength(0); i++)
            {
                for (int j = 0; j < landObjectsArray.GetLength(1); j++)
                {
                    if (landObjectsArray[i, j] != null)
                    {
                        listLandObject.Add(landObjectsArray[i, j]);
                    }
                }
            }

            return listLandObject;
        }

        public virtual ILandLayer GetSubLandLayer(int altitudeMin, int altitudeMax)
        {
            LandLayer landLayer = new LandLayer(altitudeMin, altitudeMax, this.area);

            for (int i = 0; i < altitudeMax - altitudeMin; i++)
            {
                int thisIndex = i + altitudeMin - this.altitudeMin;

                landLayer.landObjectsArray.Add(this.landObjectsArray[thisIndex]);
            }

            return landLayer;
        }
    }
}
