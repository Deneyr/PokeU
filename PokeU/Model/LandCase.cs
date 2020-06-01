using PokeU.Model.LandInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.Model
{
    public class LandCase: IObject
    {
        private List<ILandObject> landGroundOverWallList;

        private ILandObject landWater;

        private ILandObject landOverWall;

        private ILandObject landWall;

        private ILandObject landOverGround;

        private List<ILandObject> landGroundList;

        public bool IsValid
        {
            get
            {
                if(this.landWater != null 
                    || this.landWall != null)
                {
                    return true;
                }

                if(this.landGroundList.Count > 0)
                {
                    return true;
                }

                return false;
            }
        }

        public bool IsOnlyWater
        {
            get
            {
                if(this.landWall != null
                    || this.landGroundList.Count > 0)
                {
                    return false;
                }

                return this.landWater != null;
            }
        }

        public List<ILandObject> LandGroundOverWallList
        {
            get
            {
                return this.landGroundOverWallList;
            }
        }

        public ILandObject LandWater
        {
            get
            {
                return this.landWater;
            }

            set
            {
                if (value == null || value is ILandWater)
                {
                    this.landWater = value;
                }
            }
        }

        public ILandObject LandOverWall
        {
            get
            {
                return this.landOverWall;
            }

            set
            {
                if (value == null || value is ILandOverWall)
                {
                    this.landOverWall = value;
                }
            }
        }

        public ILandObject LandWall
        {
            get
            {
                return this.landWall;
            }
            set
            {
                if (value == null || value is ILandWall)
                {
                    this.landWall = value;
                }
            }
        }

        public ILandObject LandOverGround
        {
            get
            {
                return this.landWall;
            }
            set
            {
                if (value == null || value is ILandOverGround)
                {
                    this.landOverGround = value;
                }
            }
        }

        public List<ILandObject> LandGroundList
        {
            get
            {
                return this.landGroundList;
            }
        }

        public LandCase()
        {
            this.landGroundOverWallList = new List<ILandObject>();

            this.landWater = null;

            this.landOverWall = null;

            this.landWall = null;

            this.landOverGround = null;

            this.landGroundList = new List<ILandObject>();
        }

        public void AddLandGroundOverWall(ILandObject landGround)
        {
            if (landGround != null && landGround is ILandGround)
            {
                if (landGround.LandTransition == LandTransition.NONE)
                {
                    this.landGroundOverWallList.Clear();
                }

                this.landGroundOverWallList.Add(landGround);
            }
        }

        public void AddLandGround(ILandObject landGround)
        {
            if (landGround != null && landGround is ILandGround)
            {
                if(landGround.LandTransition == LandTransition.NONE)
                {
                    this.landGroundList.Clear();
                }

                this.landGroundList.Add(landGround);
            }
        }

        //public void AppendTypes(HashSet<Type> typeSet)
        //{
        //    foreach (ILandObject landGroundOverWallObject in this.landGroundOverWallList)
        //    {
        //        typeSet.Add(landGroundOverWallObject.GetType());
        //    }

        //    if (this.LandWater != null)
        //    {
        //        typeSet.Add(this.LandWater.GetType());
        //    }

        //    if (this.LandOverGround != null)
        //    {
        //        typeSet.Add(this.LandOverGround.GetType());
        //    }

        //    if (this.LandWall != null)
        //    {
        //        typeSet.Add(this.LandWall.GetType());
        //    }

        //    if (this.LandOverWall != null)
        //    {
        //        typeSet.Add(this.LandOverWall.GetType());
        //    }

        //    foreach (ILandObject landGroundObject in this.landGroundList)
        //    {
        //        typeSet.Add(landGroundObject.GetType());
        //    }
        //}

        public List<ILandObject> GetLandObjects()
        {
            List<ILandObject> landObjectsList = new List<ILandObject>();

            foreach (ILandObject landGroundOverWallObject in this.landGroundOverWallList)
            {
                landObjectsList.Add(landGroundOverWallObject);
            }

            if (this.LandWater != null)
            {
                landObjectsList.Add(this.LandWater);
            }

            if (this.LandOverGround != null)
            {
                landObjectsList.Add(this.LandOverGround);
            }

            if (this.LandWall != null)
            {
                landObjectsList.Add(this.LandWall);
            }

            if (this.LandOverWall != null)
            {
                landObjectsList.Add(this.LandOverWall);
            }

            foreach (ILandObject landGroundObject in this.landGroundList)
            {
                landObjectsList.Add(landGroundObject);
            }

            return landObjectsList;
        }
    }
}
