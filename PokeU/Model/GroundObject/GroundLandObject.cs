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
    }

    public enum LandType
    {
        GROUND = 0,
        GRASS = 1,
        SAND = 2,
        STONE = 3
    }
}
