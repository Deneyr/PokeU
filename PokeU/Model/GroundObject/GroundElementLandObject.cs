using PokeU.Model.LandInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.Model.GroundObject
{
    public class GroundElementLandObject : ALandObject, ILandOverGround
    {
        public LandType LandType
        {
            get;
            private set;
        }

        public int ElementIndex
        {
            get;
            private set;
        }

        public GroundElementLandObject(int positionX, int positionY, int positionZ, LandType landType, int elementIndex) :
            base(positionX, positionY, positionZ)
        {
            this.LandType = landType;
            this.ElementIndex = elementIndex;
        }

        public override ILandObject Clone(LandTransition wallLandTransition)
        {
            if (wallLandTransition != LandTransition.NONE)
            {
                GroundElementLandObject groundLandObject = new GroundElementLandObject(this.Position.X, this.Position.Y, this.Altitude, this.LandType, this.ElementIndex);

                return groundLandObject;
            }
            return null;
        }

        public override ILandObject Clone()
        {
            GroundElementLandObject groundLandObject = new GroundElementLandObject(this.Position.X, this.Position.Y, this.Altitude, this.LandType, this.ElementIndex);

            return groundLandObject;
        }
    }
}
