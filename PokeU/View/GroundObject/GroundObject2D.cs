using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokeU.Model.GroundObject;
using SFML.Graphics;
using SFML.System;

namespace PokeU.View.GroundObject
{
    public class GroundObject2D : ILandObject2D
    {
        private static readonly Texture[] TEXT_LIST;

        static GroundObject2D()
        {
            TEXT_LIST = new Texture[]
            {
                new Texture(@"Autotiles\Red cave highlight.png"),
                new Texture(@"Autotiles\Brown cave sand.png"),
                new Texture(@"Autotiles\Snow cave highlight.png"),
                new Texture(@"Autotiles\White cave highlight.png")
            };
        }

        public Sprite ObjectSprite
        {
            get;
            protected set;
        }

        public Vector2f Position
        {
            get
            {
                return this.ObjectSprite.Position;
            }

            set
            {
                this.ObjectSprite.Position = value * MainWindow.MODEL_TO_VIEW;
            }
        }

        public GroundObject2D(GroundLandObject landObject, Vector2i chunkPosition)
        {
            this.ObjectSprite = new Sprite(TEXT_LIST[(int)landObject.Type], new IntRect(2 * MainWindow.MODEL_TO_VIEW, 4 * MainWindow.MODEL_TO_VIEW, 2 * MainWindow.MODEL_TO_VIEW, 2 * MainWindow.MODEL_TO_VIEW));

            this.ObjectSprite.Scale = new Vector2f(0.5f, 0.5f);

            this.Position = new Vector2f(landObject.Position.X - chunkPosition.X, landObject.Position.Y - chunkPosition.Y);          
        }

    }
}
