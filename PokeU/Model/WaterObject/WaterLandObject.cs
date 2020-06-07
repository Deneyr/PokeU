using PokeU.Model.LandInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.Model.WaterObject
{
    public class WaterLandObject : ALandObject, ILandWater
    {
        public WaterLandObject(int positionX, int positionY, int positionZ) : base(positionX, positionY, positionZ)
        {
        }

        public void SetLandTransition(LandTransition landTransition)
        {
            this.LandTransition = landTransition;
        }

        public override ILandObject Clone(LandTransition wallLandTransition)
        {
            return null;
        }

        public override ILandObject Clone()
        {
            WaterLandObject waterLandObject = new WaterLandObject(this.Position.X, this.Position.Y, this.Altitude);
            waterLandObject.SetLandTransition(this.LandTransition);

            return waterLandObject;
        }
    }
}
