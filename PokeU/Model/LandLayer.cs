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
        protected IntRect area;

        protected List<LandCase[,]> landObjectsArray;

        protected HashSet<Type> typesInLayer;

        public LandLayer(int altitudeMin, int altitudeMax, IntRect area)
        {
            this.AltitudeMin = altitudeMin;

            this.AltitudeMax = altitudeMax;

            this.area = area;

            this.landObjectsArray = new List<LandCase[,]>();

            this.typesInLayer = new HashSet<Type>();

            for(int z = 0; z < altitudeMax - altitudeMin + 1; z++)
            {
                LandCase[,] landObjectsArray = new LandCase[area.Height, area.Width];

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

        public HashSet<Type> TypesInLayer
        {
            get
            {
                return this.typesInLayer;
            }
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

        //public void AddLandObject(ILandObject landObject, int i, int j)
        //{
        //    this.landObjectsArray[landObject.Altitude - this.AltitudeMin][i, j] = landObject;
        //}

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

        public void AddTypeInLayer(Type type)
        {
            this.typesInLayer.Add(type);
        }
    }
}
