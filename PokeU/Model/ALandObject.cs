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

        public LandTransition LandTransition
        {
            get;
            protected set;
        }

        public ALandObject(int positionX, int positionY, int positionZ)
        {
            this.Position = new Vector2i(positionX, positionY);

            this.Altitude = positionZ;

            this.LandTransition = LandTransition.NONE;
        }
    }

    public enum LandTransition
    {
        NONE,
        RIGHT,
        LEFT,
        TOP,
        BOT,
        TOP_LEFT,
        BOT_LEFT,
        TOP_RIGHT,
        BOT_RIGHT,
        TOP_INT_LEFT,
        BOT_INT_LEFT,
        TOP_INT_RIGHT,
        BOT_INT_RIGHT,

        // To implement
        DIAGONAL_RIGHT,
        DIAGONAL_LEFT
    }
}
