using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.View
{
    public interface IObject2D: IDisposable
    {
        Sprite ObjectSprite
        {
            get;
        }

        Vector2f Position
        {
            get;
        }

        void DrawIn(RenderWindow window);
    }
}
