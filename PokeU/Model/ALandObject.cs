using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace PokeU.Model
{
    public class ALandObject : ILandObject
    {
        public Vector2i Position
        {
            get;
            protected set;
        }

        public int Altitude
        {
            get;
            protected set;
        }

        public ALandObject(int positionX, int positionY, int positionZ)
        {
            this.Position = new Vector2i(positionX, positionY);

            this.Altitude = positionZ;
        }
    }
}
