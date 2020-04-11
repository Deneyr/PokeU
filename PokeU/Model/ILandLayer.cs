using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.Model
{
    public interface ILandLayer: IObject
    {
        HashSet<Type> TypesInLayer
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

        void InitializeLandCase(int i, int j, int z);

        LandCase GetLandCase(int i, int j, int z);

        void AddTypeInLayer(Type type);
    }
}
