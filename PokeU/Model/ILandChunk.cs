using PokeU.Model.Entity;
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

        HashSet<IEntity> EntitiesInChunk
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

        void InitializeLandCase(int i, int j, int z);

        LandCase GetLandCase(int i, int j, int z);

        void AddTypeInChunk(Type type);

        ILandChunk GetSubLandChunk(int altitudeMin, int altitudeMax);

        int GetAltitudeAt(int i, int j);

        void SetAltitudeAt(int i, int j, int altitude);
    }
}
