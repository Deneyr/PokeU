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
        private List<LandCase2D[,]> landObjects2DLayers;

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

        public LandChunk2D(LandWorld2D landWorld2D, ILandChunk landChunk)
        {
            this.altitudeMin = int.MaxValue;

            this.altitudeMax = int.MinValue;

            this.Width = landChunk.Area.Width;

            this.Height = landChunk.Area.Height;

            this.landObjects2DLayers = new List<LandCase2D[,]>();

            this.UpdateCurrentAltitude(landWorld2D, landChunk, landWorld2D.CurrentAltitude);

            //for(int z = 0; z < this.altitudeMax - this.altitudeMin + 1; z++)
            //{
            //    List<IObject2D> listobject2Ds = new List<IObject2D>();

            //    LandCase[,] landCases = landChunk.GetLandObjectsAtAltitude(this.altitudeMin + z);
            //    LandCase2D[,] landObject2Ds = new LandCase2D[landChunk.Area.Height, landChunk.Area.Width];


            //    for (int i = 0; i < landChunk.Area.Height; i++)
            //    {
            //        for (int j = 0; j < landChunk.Area.Width; j++)
            //        {
            //            if (landCases[i, j] != null)
            //            {
            //                landObject2Ds[i, j] = LandWorld2D.MappingObjectModelView[typeof(LandCase)].CreateObject2D(landWorld2D, landCases[i, j]) as LandCase2D;

            //                if (z + this.altitudeMin < this.altitudeMax)
            //                {
            //                    landObject2Ds[i, j].UpdateOverLandCase(landChunk.GetLandObjectsAtAltitude(this.altitudeMin + z + 1)[i, j]);
            //                }
            //                if (z > 0)
            //                {
            //                    landObject2Ds[i, j].UpdateUnderLandCase(landChunk.GetLandObjectsAtAltitude(this.altitudeMin + z - 1)[i, j]);
            //                }

            //                landObject2Ds[i, j].SetLandCaseRatio(this.altitudeMin + z, LandWorld2D.LOADED_ALTITUDE_RANGE);
            //            }
            //            else
            //            {
            //                landObject2Ds[i, j] = null;
            //            }
            //        }
            //    }

            //    this.landObjects2DLayers.Add(landObject2Ds);
            //}

            this.Position = new Vector2f(landChunk.Area.Left, landChunk.Area.Top);
        }

        public int UpdateCurrentAltitude(LandWorld2D landWorld2D, ILandChunk landChunk, int newAltitude)
        {
            int trueCurrentAltitude = Math.Max(Math.Min(newAltitude, landChunk.AltitudeMax), landChunk.AltitudeMin);

            int altitudeMin = Math.Max(landChunk.AltitudeMin, trueCurrentAltitude - LandWorld2D.LOADED_ALTITUDE_RANGE);

            int altitudeMax = Math.Min(landChunk.AltitudeMax, trueCurrentAltitude + LandWorld2D.LOADED_ALTITUDE_RANGE);

            int AltitudesMinToRemove = Math.Min(this.altitudeMax + 1, altitudeMin) - this.altitudeMin;

            if (this.landObjects2DLayers.Count > 0)
            {
                for (int i = 0; i < AltitudesMinToRemove; i++)
                {
                    this.landObjects2DLayers.RemoveAt(0);

                    //Console.WriteLine("Remove altitude : " + (this.altitudeMin + i));
                }

                int AltitudesMaxToRemove = this.altitudeMax - Math.Max(this.altitudeMin - 1, altitudeMax);
                for (int i = 0; i < AltitudesMaxToRemove; i++)
                {
                    this.landObjects2DLayers.RemoveAt(this.landObjects2DLayers.Count - 1);

                    //Console.WriteLine("Remove altitude : " + (this.altitudeMax - i));
                }


                int supLimit = Math.Min(this.altitudeMin, altitudeMax + 1);
                int AltitudesMinToAdd = supLimit - altitudeMin;
                for (int i = 0; i < AltitudesMinToAdd; i++)
                {
                    LandCase2D[,] landObject2Ds = new LandCase2D[landChunk.Area.Height, landChunk.Area.Width];

                    this.CreateAltitude2D(landWorld2D, landChunk, supLimit - i - 1, ref landObject2Ds);

                    this.landObjects2DLayers.Insert(0, landObject2Ds);

                    //Console.WriteLine("Add altitude : " + (supLimit - i - 1));
                }
            }

            int infLimit = Math.Max(this.altitudeMax, altitudeMin - 1);
            int AltitudesMaxToAdd = altitudeMax - infLimit;
            for (int i = 0; i < AltitudesMaxToAdd; i++)
            {
                LandCase2D[,] landObject2Ds = new LandCase2D[landChunk.Area.Height, landChunk.Area.Width];

                this.CreateAltitude2D(landWorld2D, landChunk, infLimit + i + 1, ref landObject2Ds);

                this.landObjects2DLayers.Add(landObject2Ds);

                //Console.WriteLine("Add altitude : " + (infLimit + i + 1));
            }

            this.altitudeMin = altitudeMin;

            this.altitudeMax = altitudeMax;

            int z = 0;
            foreach (LandCase2D[,] landCases in this.landObjects2DLayers)
            {
                for (int i = 0; i < landChunk.Area.Height; i++)
                {
                    for (int j = 0; j < landChunk.Area.Width; j++)
                    {
                        if (landCases[i, j] != null)
                        {
                            landCases[i, j].SetLandCaseRatio((this.altitudeMin + z) - trueCurrentAltitude, LandWorld2D.LOADED_ALTITUDE_RANGE);
                        }
                    }
                }
                z++;
            }

            return trueCurrentAltitude;
        }

        public override void DrawIn(RenderWindow window, ref FloatRect boundsView)
        {
            LandCase2D[,] layer2D = this.landObjects2DLayers.FirstOrDefault();

            if (layer2D == null)
            {
                return;
            }

            for (int i = 0; i < layer2D.GetLength(0); i++)
            {
                for (int j = 0; j < layer2D.GetLength(1); j++)
                {
                    FloatRect bounds = new FloatRect(this.Position.X + j * MainWindow.MODEL_TO_VIEW, this.Position.Y + i * MainWindow.MODEL_TO_VIEW, MainWindow.MODEL_TO_VIEW, MainWindow.MODEL_TO_VIEW);

                    /*if (bounds.Left < boundsView.Left + boundsView.Width
                        && bounds.Left + bounds.Width > boundsView.Left
                        && bounds.Top < boundsView.Top + boundsView.Height
                        && bounds.Top + bounds.Height > boundsView.Top)
                    {*/
                    if(bounds.Intersects(boundsView))
                    {
                        bool firstCaseDrawn = false; 

                        foreach (LandCase2D[,] landObject2DsArray in this.landObjects2DLayers)
                        {
                            LandCase2D landObjectsList = landObject2DsArray[i, j];

                            if (landObjectsList != null)
                            {
                                landObjectsList.DrawIn(window, ref boundsView);

                                firstCaseDrawn = landObjectsList.IsValid;
                            }
                        }
                    }
                }
            }
        }

        public override void Dispose()
        {
            foreach (LandCase2D[,] landObject2DsArray in this.landObjects2DLayers)
            {
                for (int i = 0; i < landObject2DsArray.GetLength(0); i++)
                {
                    for (int j = 0; j < landObject2DsArray.GetLength(1); j++)
                    {
                        LandCase2D landObjectsList = landObject2DsArray[i, j];

                        if (landObjectsList != null)
                        {
                            landObjectsList.Dispose();
                        }
                    }
                }
            }

            this.landObjects2DLayers.Clear();
        }

        private void CreateAltitude2D(LandWorld2D landWorld2D, ILandChunk landChunk, int altitude, ref LandCase2D[,] landObject2Ds)
        {
            List<IObject2D> listobject2Ds = new List<IObject2D>();

            LandCase[,] landCases = landChunk.GetLandObjectsAtAltitude(altitude);

            LandCase[,] landCasesUp = null;
            LandCase[,] landCasesDown = null;
            if (altitude < landChunk.AltitudeMax)
            {
                landCasesUp = landChunk.GetLandObjectsAtAltitude(altitude + 1);
            }
            if (altitude > landChunk.AltitudeMin)
            {
                landCasesDown = landChunk.GetLandObjectsAtAltitude(altitude - 1);
            }


            for (int i = 0; i < landChunk.Area.Height; i++)
            {
                for (int j = 0; j < landChunk.Area.Width; j++)
                {
                    if (landCases[i, j] != null
                        && (landCases[i, j].IsOnlyWater == false || landCasesUp == null || landCasesUp[i, j] == null || landCasesUp[i, j].LandWater == null))
                    {
                        landObject2Ds[i, j] = LandWorld2D.MappingObjectModelView[typeof(LandCase)].CreateObject2D(landWorld2D, landCases[i, j]) as LandCase2D;

                        if (landCasesUp != null)
                        {
                            landObject2Ds[i, j].UpdateOverLandCase(landCasesUp[i, j]);
                        }
                        if (landCasesDown != null)
                        {
                            landObject2Ds[i, j].UpdateUnderLandCase(landCasesDown[i, j]);
                        }
                    }
                    else
                    {
                        landObject2Ds[i, j] = null;
                    }
                }
            }
        }
    }
}
