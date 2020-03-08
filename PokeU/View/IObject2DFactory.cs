using PokeU.Model;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.View
{
    public interface IObject2DFactory
    {
        IObject2D CreateObject2D(LandWorld2D landWorld2D, IObject obj);

        Dictionary<string, Texture> Resources
        {
            get;
        }

        Texture GetTextureByIndex(int index);

        void OnTextureLoaded(string path, Texture texture);

        void OnTextureUnloaded(string path);
    }
}
