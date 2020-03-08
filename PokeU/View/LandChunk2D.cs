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
    public class LandChunk2D : AObject2D
    {
        private List<List<ILandObject2D>[,]> landObjects2DLayers;

        private int altitudeMin;

        private int altitudeMax;      

        private int width;

        private int height;

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

        public LandChunk2D(LandWorld2D landWorld2D, ILandChunk landChunk, int altitudeMin, int altitudeMax):
            base()
        {
            this.altitudeMin = Math.Max(landChunk.AltitudeMin, altitudeMin);

            this.altitudeMax = Math.Min(landChunk.AltitudeMax, altitudeMax);

            this.Width = landChunk.Area.Width;

            this.Height = landChunk.Area.Height;

            this.landObjects2DLayers = new List<List<ILandObject2D>[,]>();

            for(int z = 0; z < this.altitudeMax - this.altitudeMin + 1; z++)
            {
                List<IObject2D> listobject2Ds = new List<IObject2D>();

                List<ILandObject>[,] landObjects = landChunk.GetLandObjectsAtAltitude(this.altitudeMin + z);
                List<ILandObject2D>[,] landObject2Ds = new List<ILandObject2D>[landChunk.Area.Height, landChunk.Area.Width];


                for (int i = 0; i < landChunk.Area.Height; i++)
                {
                    for (int j = 0; j < landChunk.Area.Width; j++)
                    {
                        List<ILandObject2D> listLandObject2Ds = new List<ILandObject2D>();

                        if (landObjects[i, j] != null)
                        {
                            foreach (ILandObject landObject in landObjects[i, j])
                            {
                                ILandObject2D landObject2D = LandWorld2D.MappingObjectModelView[landObject.GetType()].CreateObject2D(landWorld2D, landObject) as ILandObject2D;

                                listLandObject2Ds.Add(landObject2D);
                            }
                        }

                        landObject2Ds[i, j] = listLandObject2Ds;
                    }
                }

                this.landObjects2DLayers.Add(landObject2Ds);
            }

            this.Position = new Vector2f(landChunk.Area.Left, landChunk.Area.Top);
        }

        public override void DrawIn(RenderWindow window, ref FloatRect boundsView)
        {
            foreach (List<ILandObject2D>[,] landObject2DsArray in this.landObjects2DLayers)
            {
                for (int i = 0; i < landObject2DsArray.GetLength(0); i++)
                {
                    for (int j = 0; j < landObject2DsArray.GetLength(1); j++)
                    {
                        FloatRect bounds = new FloatRect(this.Position.X + j * MainWindow.MODEL_TO_VIEW, this.Position.Y + i * MainWindow.MODEL_TO_VIEW, MainWindow.MODEL_TO_VIEW, MainWindow.MODEL_TO_VIEW);

                        /*if (bounds.Left < boundsView.Left + boundsView.Width
                            && bounds.Left + bounds.Width > boundsView.Left
                            && bounds.Top < boundsView.Top + boundsView.Height
                            && bounds.Top + bounds.Height > boundsView.Top)
                        {*/
                        if(bounds.Intersects(boundsView))
                        { 
                            List<ILandObject2D> landObjectsList = landObject2DsArray[i, j];

                            foreach (ILandObject2D object2D in landObjectsList)
                            {
                                object2D.DrawIn(window, ref boundsView);
                            }
                        }
                    }
                }
            }
        }

        public override void Dispose()
        {
            foreach (List<ILandObject2D>[,] landObject2DsArray in this.landObjects2DLayers)
            {
                for (int i = 0; i < landObject2DsArray.GetLength(0); i++)
                {
                    for (int j = 0; j < landObject2DsArray.GetLength(1); j++)
                    {
                        List<ILandObject2D> landObjectsList = landObject2DsArray[i, j];

                        foreach (ILandObject2D object2D in landObjectsList)
                        {
                            object2D.Dispose();
                        }

                        landObjectsList.Clear();
                    }
                }
            }

            this.landObjects2DLayers.Clear();
        }
    }
}
