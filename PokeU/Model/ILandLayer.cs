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
        int AltitudeMin
        {
            get;
        }

        int AltitudeMax
        {
            get;
        }

        void AddLandObject(ILandObject landObject, int i, int j);

        ILandObject GetLandObjectAtCoord(int i, int j, int z);
    }
}
