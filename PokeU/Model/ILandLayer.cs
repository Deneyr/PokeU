using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.Model
{
    public interface ILandLayer
    {
        void AddLandObject(ILandObject landObject, int i, int j);

        List<ILandObject> GetLandObjectsAtAltitude(int altitude);

        ILandLayer GetSubLandLayer(int altitudeMin, int altitudeMax);
    }
}
