using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.Model.GroundObject
{
    public class AltitudeLandObject: ALandObject
    {
        private LandType landType;

        public LandType Type
        {
            get
            {
                return this.landType;
            }
        }

        public AltitudeLandObject(int positionX, int positionY, int positionZ, LandType landType) :
            base(positionX, positionY, positionZ)
        {
            this.landType = landType;

        }

        public void SetLandTransition(LandTransition landTransition)
        {
            this.LandTransition = landTransition;
        }
    }
}
