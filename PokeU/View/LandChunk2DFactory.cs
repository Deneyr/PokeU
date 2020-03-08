using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokeU.Model;

namespace PokeU.View
{
    public class LandChunk2DFactory : AObject2DFactory
    {
        public override IObject2D CreateObject2D(LandWorld2D landWorld2D, IObject obj)
        {
            ILandChunk landChunk = obj as ILandChunk;

            return new LandChunk2D(landWorld2D, landChunk, landWorld2D.CurrentAltitude - LandWorld2D.LOADED_ALTITUDE_RANGE, landWorld2D.CurrentAltitude + LandWorld2D.LOADED_ALTITUDE_RANGE);
        }
    }
}
