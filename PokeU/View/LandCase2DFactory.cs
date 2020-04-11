using PokeU.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.View
{
    public class LandCase2DFactory : AObject2DFactory
    {
        public override IObject2D CreateObject2D(LandWorld2D landWorld2D, IObject obj)
        {
            LandCase landCase = obj as LandCase;

            return new LandCase2D(landWorld2D, landCase);
        }
    }
}
