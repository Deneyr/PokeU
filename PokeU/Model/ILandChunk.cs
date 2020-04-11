using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.Model
{
    public interface ILandChunk: IObject
    { 
        HashSet<Type> TypesInChunk
        {
            get;
        }

        IntRect Area
        {
            get;
        }

        int AltitudeMin
        {
            get;
        }

        int AltitudeMax
        {
            get;
        }

        LandCase[,] GetLandObjectsAtAltitude(int altitude);

        ILandChunk GetSubLandChunk(int altitudeMin, int altitudeMax);
    }
}
