using PokeU.Model;
using PokeU.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.View.Entity2D
{
    public class PlayerEntity2DFactory : AObject2DFactory
    {
        protected override void InitializeFactory()
        {
            this.texturesPath.Add(@"Autotiles\trainer.png");

            base.InitializeFactory();
        }

        public override IObject2D CreateObject2D(LandWorld2D landWorld2D, IObject obj)
        {
            PlayerEntity playerEntity = obj as PlayerEntity;

            if (playerEntity != null)
            {
                return new PlayerEntity2D(this, playerEntity);
            }
            return null;
        }
    }
}
