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
        protected List<ILandLayer> landLayersList;

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

        public LandChunk(IntRect area)
        {
            this.Area = area;

            this.landLayersList = new List<ILandLayer>();

            this.AltitudeMin = 0;
            this.AltitudeMax = 0;
        }

        public void AddLandLayer(ILandLayer layer)
        {
            this.landLayersList.Add(layer);

            if(layer.AltitudeMin < this.AltitudeMin)
            {
                this.AltitudeMin = layer.AltitudeMin;
            }

            if (layer.AltitudeMax > this.AltitudeMax)
            {
                this.AltitudeMax = layer.AltitudeMax;
            }
        }

        public List<ILandObject> GetLandObjectsAtAltitude(int altitude)
        {
            List<ILandObject> landObjectsResult = new List<ILandObject>();

            foreach(ILandLayer layer in this.landLayersList)
            {
                landObjectsResult.AddRange(layer.GetLandObjectsAtAltitude(altitude));
            }

            return landObjectsResult;
        }

        public ILandChunk GetSubLandChunk(int altitudeMin, int altitudeMax)
        {
            LandChunk landChunkResult = new LandChunk(this.Area);

            foreach (ILandLayer layer in this.landLayersList)
            {
                landChunkResult.AddLandLayer(layer.GetSubLandLayer(altitudeMin, altitudeMax));
            }

            return landChunkResult;
        }
    }
}
