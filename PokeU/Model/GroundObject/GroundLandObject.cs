using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokeU.Model.LandInterface;
using SFML.System;

namespace PokeU.Model.GroundObject
{
    public class GroundLandObject: ALandObject, ILandGround
    {
        private LandType landType;

        public LandType Type
        {
            get
            {
                return this.landType;
            }
        }

        public GroundLandObject(int positionX, int positionY, int positionZ, LandType landType): 
            base(positionX, positionY, positionZ)
        {
            this.landType = landType;
        }

        public void SetTransition(LandTransition landTransition)
        {
            this.LandTransition = landTransition;
        }

        public override ILandObject CreateLandObjectOverWall(LandTransition wallLandTransition)
        {
            LandTransition landTransitionOverWall = this.GetLandTransitionOverWall(wallLandTransition);

            if (landTransitionOverWall != LandTransition.NONE)
            {
                GroundLandObject groundLandObject = new GroundLandObject(this.Position.X, this.Position.Y, this.Altitude, this.landType);
                groundLandObject.SetTransition(landTransitionOverWall);

                return groundLandObject;
            }
            return null;
        }
    }

    public enum LandType
    {
        GROUND = 0,
        SAND = 1,
        GRASS = 2,
        STONE = 3,
        SNOW = 4
    }
}
