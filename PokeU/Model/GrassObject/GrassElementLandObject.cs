using PokeU.Model.LandInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.Model.GrassObject
{
    public class GrassElementLandObject : ALandObject, ILandOverGround
    {
        public GrassType LandGrassType
        {
            get;
            private set;
        }

        public int ElementIndex
        {
            get;
            private set;
        }

        public GrassElementLandObject(int positionX, int positionY, int positionZ, GrassType grassType, int elementIndex) :
            base(positionX, positionY, positionZ)
        {
            this.LandGrassType = grassType;
            this.ElementIndex = elementIndex;
        }

        public override ILandObject Clone(LandTransition wallLandTransition)
        {
            if (wallLandTransition != LandTransition.NONE)
            {
                GrassElementLandObject grassLandObject = new GrassElementLandObject(this.Position.X, this.Position.Y, this.Altitude, this.LandGrassType, this.ElementIndex);

                return grassLandObject;
            }
            return null;
        }

        public override ILandObject Clone()
        {
            GrassElementLandObject grassLandObject = new GrassElementLandObject(this.Position.X, this.Position.Y, this.Altitude, this.LandGrassType, this.ElementIndex);

            return grassLandObject;
        }
    }
}
