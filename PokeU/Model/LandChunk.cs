using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokeU.Model.Entity;
using SFML.Graphics;
using SFML.System;

namespace PokeU.Model
{
    public class LandChunk : ILandChunk
    {
        protected List<LandCase[,]> landObjectsArray;

        protected sbyte[,] altitudeArray;

        protected HashSet<Type> typesInChunk;

        protected HashSet<IEntity> entitiesInChunk;

        public HashSet<Type> TypesInChunk
        {
            get
            {
                return this.typesInChunk;
            }
        }

        public HashSet<IEntity> EntitiesInChunk
        {
            get
            {
                return this.entitiesInChunk;
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
            this.altitudeArray = new sbyte[this.Area.Height, this.Area.Width];

            this.entitiesInChunk = new HashSet<IEntity>();

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

        public void AddEntity(IEntity entity)
        {
            this.entitiesInChunk.Add(entity);
        }

        public void AddTypeInChunk(Type type)
        {
            this.typesInChunk.Add(type);
        }

        public LandCase[,] GetLandObjectsAtAltitude(int altitude)
        {
            return this.landObjectsArray[altitude - this.AltitudeMin];
        }

        public int GetAltitudeAt(int i, int j)
        {
            return this.altitudeArray[i, j];
        }

        public void SetAltitudeAt(int i, int j, int altitude)
        {
            this.altitudeArray[i, j] = (sbyte)altitude;
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
