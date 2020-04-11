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

        //private LandType secondLandType;

        public LandType Type
        {
            get
            {
                return this.landType;
            }
        }

        //public LandType SecondType
        //{
        //    get
        //    {
        //        return this.secondLandType;
        //    }
        //}

        public GroundLandObject(int positionX, int positionY, int positionZ, LandType landType): 
            base(positionX, positionY, positionZ)
        {
            this.landType = landType;

            // this.secondLandType = this.landType;
        }

        public void SetTransition(LandTransition landTransition)
        {
            // this.secondLandType = secondLandType;

            this.LandTransition = landTransition;
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
