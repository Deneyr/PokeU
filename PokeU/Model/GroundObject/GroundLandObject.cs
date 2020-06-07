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
        public LandType Type
        {
            get;
            protected set;
        }

        public GroundLandObject(int positionX, int positionY, int positionZ, LandType landType): 
            base(positionX, positionY, positionZ)
        {
            this.Type = landType;
        }

        public void SetTransition(LandTransition landTransition)
        {
            this.LandTransition = landTransition;
        }

        public override ILandObject Clone(LandTransition wallLandTransition)
        {
            LandTransition landTransitionOverWall = this.GetLandTransitionOverWall(wallLandTransition);

            if (landTransitionOverWall != LandTransition.NONE)
            {
                GroundLandObject groundLandObject = new GroundLandObject(this.Position.X, this.Position.Y, this.Altitude, this.Type);
                groundLandObject.SetTransition(landTransitionOverWall);

                return groundLandObject;
            }
            return null;
        }

        public override ILandObject Clone()
        {
            GroundLandObject groundLandObject = new GroundLandObject(this.Position.X, this.Position.Y, this.Altitude, this.Type);
            groundLandObject.SetTransition(this.LandTransition);

            return groundLandObject;
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
