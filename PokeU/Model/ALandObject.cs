using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace PokeU.Model
{
    public abstract class ALandObject : ILandObject
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

        public abstract ILandObject CreateLandObjectOverWall(LandTransition wallLandTransition);

        public LandTransition GetLandTransitionOverWall(LandTransition wallLandTransition)
        {
            return LandTransitionHelper.IntersectionLandTransition(wallLandTransition, this.LandTransition);

            //switch (wallLandTransition)
            //{
            //    case LandTransition.NONE:
            //        return LandTransition.NONE;
            //    case LandTransition.RIGHT:
            //        if(this.LandTransition == LandTransition.TOP
            //            || this.LandTransition == LandTransition.BOT_INT_RIGHT
            //            || this.LandTransition == LandTransition.TOP_RIGHT)
            //        {
            //            return LandTransition.TOP_RIGHT;
            //        }
            //        else if(this.LandTransition == LandTransition.BOT
            //            || this.LandTransition == LandTransition.TOP_INT_RIGHT
            //            || this.LandTransition == LandTransition.BOT_RIGHT)
            //        {
            //            return LandTransition.BOT_RIGHT;
            //        }
            //        break;
            //    case LandTransition.LEFT:
            //        if (this.LandTransition == LandTransition.TOP
            //            || this.LandTransition == LandTransition.BOT_INT_LEFT
            //            || this.LandTransition == LandTransition.TOP_LEFT)
            //        {
            //            return LandTransition.TOP_LEFT;
            //        }
            //        else if (this.LandTransition == LandTransition.BOT
            //            || this.LandTransition == LandTransition.TOP_INT_LEFT
            //            || this.LandTransition == LandTransition.BOT_LEFT)
            //        {
            //            return LandTransition.BOT_LEFT;
            //        }
            //        break;
            //    case LandTransition.TOP:
            //        if (this.LandTransition == LandTransition.RIGHT
            //            || this.LandTransition == LandTransition.BOT_INT_LEFT
            //            || this.LandTransition == LandTransition.TOP_LEFT)
            //        {
            //            return LandTransition.TOP_RIGHT;
            //        }
            //        else if (this.LandTransition == LandTransition.BOT
            //            || this.LandTransition == LandTransition.TOP_INT_LEFT
            //            || this.LandTransition == LandTransition.BOT_LEFT)
            //        {
            //            return LandTransition.BOT_LEFT;
            //        }
            //        break;
            //    case LandTransition.BOT:
            //        return LandTransition.TOP;
            //    case LandTransition.TOP_LEFT:
            //        return LandTransition.TOP_INT_LEFT;
            //    case LandTransition.BOT_LEFT:
            //        return LandTransition.BOT_INT_LEFT;
            //    case LandTransition.TOP_RIGHT:
            //        return LandTransition.TOP_INT_RIGHT;
            //    case LandTransition.BOT_RIGHT:
            //        return LandTransition.BOT_INT_RIGHT;
            //    case LandTransition.TOP_INT_LEFT:
            //        return LandTransition.TOP_LEFT;
            //    case LandTransition.BOT_INT_LEFT:
            //        return LandTransition.BOT_LEFT;
            //    case LandTransition.TOP_INT_RIGHT:
            //        return LandTransition.TOP_RIGHT;
            //    case LandTransition.BOT_INT_RIGHT:
            //        return LandTransition.BOT_RIGHT;
            //}

            //return wallLandTransition;
        }

        public static LandTransition InverseLandTransition(LandTransition landTransition)
        {
            return LandTransitionHelper.ReverseLandTransition(landTransition);

            //switch (landTransition)
            //{
            //    case LandTransition.NONE:
            //        return LandTransition.NONE;
            //    case LandTransition.RIGHT:
            //        return LandTransition.LEFT;
            //    case LandTransition.LEFT:
            //        return LandTransition.RIGHT;
            //    case LandTransition.TOP:
            //        return LandTransition.BOT;
            //    case LandTransition.BOT:
            //        return LandTransition.TOP;
            //    case LandTransition.TOP_LEFT:
            //        return LandTransition.TOP_INT_LEFT;
            //    case LandTransition.BOT_LEFT:
            //        return LandTransition.BOT_INT_LEFT;
            //    case LandTransition.TOP_RIGHT:
            //        return LandTransition.TOP_INT_RIGHT;
            //    case LandTransition.BOT_RIGHT:
            //        return LandTransition.BOT_INT_RIGHT;
            //    case LandTransition.TOP_INT_LEFT:
            //        return LandTransition.TOP_LEFT;
            //    case LandTransition.BOT_INT_LEFT:
            //        return LandTransition.BOT_LEFT;
            //    case LandTransition.TOP_INT_RIGHT:
            //        return LandTransition.TOP_RIGHT;
            //    case LandTransition.BOT_INT_RIGHT:
            //        return LandTransition.BOT_RIGHT;
            //}

            //return LandTransition.NONE;
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
