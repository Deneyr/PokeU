using PokeU.Model;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.View
{
    public class LandCase2D : AObject2D
    {
        private List<ILandObject2D> landGroundOverWallList;

        private ILandObject2D landWater;

        private ILandObject2D landOverWall;

        private ILandObject2D landWall;

        private ILandObject2D landOverGround;

        private List<ILandObject2D> landGroundList;

        public bool DrawOnlyWall
        {
            get;
            set;
        }

        public LandCase2D(LandWorld2D landWorld2D, LandCase landCase)
        {
            this.landGroundOverWallList = new List<ILandObject2D>();

            this.landWater = null;

            this.landOverWall = null;

            this.landWall = null;

            this.landOverGround = null;

            this.landGroundList = new List<ILandObject2D>();

            this.DrawOnlyWall = false;


            foreach (ILandObject landGroundObject in landCase.LandGroundList)
            {
                ILandObject2D landObject2D = LandWorld2D.MappingObjectModelView[landGroundObject.GetType()].CreateObject2D(landWorld2D, landGroundObject) as ILandObject2D;

                this.landGroundList.Add(landObject2D);
            }

            if (landCase.LandOverGround != null)
            {
                ILandObject2D landObject2D = LandWorld2D.MappingObjectModelView[landCase.LandOverGround.GetType()].CreateObject2D(landWorld2D, landCase.LandOverGround) as ILandObject2D;

                this.landOverGround = landObject2D;
            }

            if (landCase.LandWall != null)
            {
                foreach (ILandObject landGroundOverWallObject in landCase.LandGroundOverWallList)
                {
                    ILandObject2D landObject2D = LandWorld2D.MappingObjectModelView[landGroundOverWallObject.GetType()].CreateObject2D(landWorld2D, landGroundOverWallObject) as ILandObject2D;

                    this.landGroundOverWallList.Add(landObject2D);
                }
            }

            if (landCase.LandWater != null)
            {
                ILandObject2D landObject2D = LandWorld2D.MappingObjectModelView[landCase.LandWater.GetType()].CreateObject2D(landWorld2D, landCase.LandWater) as ILandObject2D;

                this.landWater = landObject2D;
            }

            if (landCase.LandWall != null)
            {
                ILandObject2D landObject2D = LandWorld2D.MappingObjectModelView[landCase.LandWall.GetType()].CreateObject2D(landWorld2D, landCase.LandWall) as ILandObject2D;

                this.landWall = landObject2D;
            }

            if (landCase.LandWall != null)
            {
                if (landCase.LandOverWall != null)
                {
                    ILandObject2D landObject2D = LandWorld2D.MappingObjectModelView[landCase.LandOverWall.GetType()].CreateObject2D(landWorld2D, landCase.LandOverWall) as ILandObject2D;

                    this.landOverWall = landObject2D;
                }
            }
        }

        public void SetLandCaseRatio(int level, int maxLevel)
        {
            this.RatioAltitude = ((float) level) / maxLevel;

            foreach (ILandObject2D landGroundObject in this.landGroundList)
            {
                landGroundObject.RatioAltitude = this.RatioAltitude;
            }

            if (this.landOverGround != null)
            {
                this.landOverGround.RatioAltitude = this.RatioAltitude;
            }

            foreach (ILandObject2D landGroundOverWallObject in this.landGroundOverWallList)
            {
                landGroundOverWallObject.RatioAltitude = ((float)level + 1) / maxLevel;
            }

            if (this.landWater != null)
            {
                this.landWater.RatioAltitude = this.RatioAltitude;
            }

            if (this.landWall != null)
            {
                this.landWall.RatioAltitude = this.RatioAltitude;
            }

            if (this.landOverWall != null)
            {
                this.landOverWall.RatioAltitude = this.RatioAltitude;
            }
        }

        public bool IsValid
        {
            get
            {
                if (this.landWater != null
                    || this.landWall != null)
                {
                    return true;
                }

                if (this.landGroundList.Count > 0)
                {
                    return true;
                }

                return false;
            }
        }

        public override void DrawIn(RenderWindow window, ref FloatRect boundsView)
        {
            if (this.IsValid)
            {
                if (this.DrawOnlyWall == false)
                {
                    foreach (ILandObject2D landGroundObject in this.landGroundList)
                    {
                        landGroundObject.RatioAltitude = this.RatioAltitude;

                        landGroundObject.DrawIn(window, ref boundsView);
                    }
                }

                if (this.landOverGround != null)
                {
                    this.landOverGround.DrawIn(window, ref boundsView);
                }

                foreach (ILandObject2D landGroundOverWallObject in this.landGroundOverWallList)
                {
                    landGroundOverWallObject.DrawIn(window, ref boundsView);
                }

                if (this.landWater != null)
                {
                    this.landWater.DrawIn(window, ref boundsView);
                }

                if (this.landWall != null)
                {
                    this.landWall.DrawIn(window, ref boundsView);
                }

                if (this.landOverWall != null)
                {
                    this.landOverWall.DrawIn(window, ref boundsView);
                }
            }
        }

        public override void Dispose()
        {
            foreach (ILandObject2D landGroundObject in this.landGroundList)
            {
                landGroundObject.Dispose();
            }

            if (this.landOverGround != null)
            {
                this.landOverGround.Dispose();
            }

            foreach (ILandObject2D landGroundOverWallObject in this.landGroundOverWallList)
            {
                landGroundOverWallObject.Dispose();
            }

            if (this.landWater != null)
            {
                this.landWater.Dispose();
            }

            if (this.landWall != null)
            {
                this.landWall.Dispose();
            }

            if (this.landOverWall != null)
            {
                this.landOverWall.Dispose();
            }

            base.Dispose();
        }
    }
}
