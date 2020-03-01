using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace PokeU.Model.GroundObject
{
    public class GroundLandObject: ALandObject
    {
        private LandType landType;

        private LandType secondLandType;

        public LandType Type
        {
            get
            {
                return this.landType;
            }
        }

        public LandType SecondType
        {
            get
            {
                return this.secondLandType;
            }
        }

        public GroundLandObject(int positionX, int positionY, int positionZ, LandType landType): 
            base(positionX, positionY, positionZ)
        {
            this.landType = landType;

            this.secondLandType = this.landType;
        }

        public void SetSecondLandType(LandType secondLandType, LandTransition landTransition)
        {
            this.secondLandType = secondLandType;

            this.LandTransition = landTransition;
        }
    }

    public enum LandType
    {
        GROUND = 0,
        GRASS = 1,
        SAND = 2,
        STONE = 3
    }
}
