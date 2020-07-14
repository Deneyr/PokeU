using PokeU.Model.Entity;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.View.Entity2D
{
    public class PlayerEntity2D: AEntity2D
    {
        public PlayerEntity2D(IObject2DFactory factory, PlayerEntity playerEntity) :
            base()
        {
            Texture texture = factory.GetTextureByIndex(0);

            this.ObjectSprite = new Sprite(texture, new IntRect(elementIndex * 2 * MainWindow.MODEL_TO_VIEW, 0 * MainWindow.MODEL_TO_VIEW, 2 * MainWindow.MODEL_TO_VIEW, 2 * MainWindow.MODEL_TO_VIEW));

            this.ObjectSprite.Scale = new Vector2f(0.5f, 0.5f);

            this.Position = new Vector2f(landObject.Position.X, landObject.Position.Y);
        }
    }
}
