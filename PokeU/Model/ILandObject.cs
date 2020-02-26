using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.Model
{
    public interface ILandObject
    {
        Vector2i Position
        {
            get;
        }

        int Altitude
        {
            get;
        }

    }
}
