using PokeU.Model.GroundObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.Model.GrassObject
{
    public class GrassLandObject : GroundLandObject
    {
        public GrassType LandGrassType
        {
            get;
            private set;
        }

        public GrassLandObject(int positionX, int positionY, int positionZ, GrassType grassType) :
            base(positionX, positionY, positionZ, LandType.GRASS)
        {
            this.LandGrassType = grassType;
        }

        public override ILandObject CreateLandObjectOverWall(LandTransition wallLandTransition)
        {
            LandTransition landTransitionOverWall = this.GetLandTransitionOverWall(wallLandTransition);

            if (landTransitionOverWall != LandTransition.NONE)
            {
                GrassLandObject grassLandObject = new GrassLandObject(this.Position.X, this.Position.Y, this.Altitude, this.LandGrassType);
                grassLandObject.SetTransition(landTransitionOverWall);

                return grassLandObject;
            }
            return null;
        }
    }

    public enum GrassType
    {
        NONE = -1,
        DRY = 0,
        LIGHT = 1,
        NORMAL = 2,
        BUSHY = 3
    }
}
