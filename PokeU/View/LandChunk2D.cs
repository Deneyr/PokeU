using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokeU.Model;
using PokeU.Model.GroundObject;
using PokeU.View.GroundObject;
using SFML.Graphics;
using SFML.System;

namespace PokeU.View
{
    public class LandChunk2D : IObject2D
    {
        private List<List<IObject2D>> listLayersLandObject;

        private int altitudeMin;

        private int altitudeMax;

        //private RenderTexture renderTexture;

        private int width;

        private int height;

        private Sprite sprite;

        public Sprite ObjectSprite
        {
            get
            {
                return this.sprite;
            }
        }

        public Vector2f Position
        {
            get
            {
                return this.ObjectSprite.Position;
            }

            protected set
            {
                this.ObjectSprite.Position = value * MainWindow.MODEL_TO_VIEW;
            }
        }

        public int Width
        {
            get
            {
                return this.width;
            }

            set
            {
                this.width = value * MainWindow.MODEL_TO_VIEW;
            }
        }

        public int Height
        {
            get
            {
                return this.height;
            }

            set
            {
                this.height = value * MainWindow.MODEL_TO_VIEW;
            }
        }

        public LandChunk2D(ILandChunk landChunk)
        {
            this.altitudeMin = landChunk.AltitudeMin;

            this.altitudeMax = landChunk.AltitudeMax;

            this.Width = landChunk.Area.Width;

            this.Height = landChunk.Area.Height;

            //this.renderTexture = new RenderTexture((uint)this.Width, (uint)this.Height);

            this.sprite = new Sprite();

            this.listLayersLandObject = new List<List<IObject2D>>();

            for(int i = 0; i < altitudeMax - altitudeMin + 1; i++)
            {
                List<IObject2D> listobject2Ds = new List<IObject2D>();

                List<ILandObject> landObjects = landChunk.GetLandObjectsAtAltitude(altitudeMin + i);

                foreach(GroundLandObject landObject in landObjects)
                {
                    IObject2D object2D = LandWorld2D.MappingObjectModelView[landObject.GetType()].CreateObject2D(landObject);

                    listobject2Ds.Add(object2D);
                }

                this.listLayersLandObject.Add(listobject2Ds);
            }

            this.Position = new Vector2f(landChunk.Area.Left, landChunk.Area.Top);
        }

        public void DrawIn(RenderWindow window)
        {
            foreach (List<IObject2D> listObject2D in this.listLayersLandObject)
            {
                foreach (IObject2D object2D in listObject2D)
                {
                    object2D.DrawIn(window);
                }
            }
        }

        public void Dispose()
        {
            foreach (List<IObject2D> listObject2D in this.listLayersLandObject)
            {
                foreach (IObject2D object2D in listObject2D)
                {
                    object2D.Dispose();
                }

                listObject2D.Clear();
            }

            this.listLayersLandObject.Clear();
        }
    }
}
