using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokeU.Model;
using PokeU.Model.GroundObject;
using SFML.Graphics;
using SFML.System;

namespace PokeU.View.GroundObject
{
    public class GroundObject2DFactory : AObject2DFactory
    {

        protected override void InitializeFactory()
        {
            this.texturesPath.Add(@"Autotiles\Red cave highlight.png");
            this.texturesPath.Add(@"Autotiles\Brown cave sand2.png");
            this.texturesPath.Add(@"Autotiles\Snow cave highlight2.png");
            this.texturesPath.Add(@"Autotiles\Brick path2.png");

            base.InitializeFactory();
        }

        public override IObject2D CreateObject2D(IObject obj)
        {
            GroundLandObject groundLandObject = obj as GroundLandObject;

            if (groundLandObject != null)
            {
                return new GroundObject2D(this, groundLandObject);
            }
            return null;
        }
    }
}
