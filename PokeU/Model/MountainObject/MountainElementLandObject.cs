using PokeU.Model.LandInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.Model.MountainObject
{
    public class MountainElementLandObject : ALandObject, ILandOverGround
    {
        public MountainType LandMountainType
        {
            get;
            private set;
        }

        public int ElementIndex
        {
            get;
            private set;
        }

        public MountainElementLandObject(int positionX, int positionY, int positionZ, MountainType mountainType, int elementIndex) :
            base(positionX, positionY, positionZ)
        {
            this.LandMountainType = mountainType;
            this.ElementIndex = elementIndex;
        }

        public override ILandObject Clone(LandTransition wallLandTransition)
        {
            if (wallLandTransition != LandTransition.NONE)
            {
                MountainElementLandObject mountainLandObject = new MountainElementLandObject(this.Position.X, this.Position.Y, this.Altitude, this.LandMountainType, this.ElementIndex);

                return mountainLandObject;
            }
            return null;
        }

        public override ILandObject Clone()
        {
            MountainElementLandObject mountainLandObject = new MountainElementLandObject(this.Position.X, this.Position.Y, this.Altitude, this.LandMountainType, this.ElementIndex);

            return mountainLandObject;
        }
    }
}
