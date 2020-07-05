using SFML.System;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.Model.Entity
{
    public abstract class AEntity: IEntity
    {
        public virtual bool Persistent
        {
            get
            {
                return true;
            }
        }

        public Vector2i Position
        {
            get
            {
                return new Vector2i((int) this.Rect.Left, (int) this.Rect.Top);
            }
            set
            {
                this.Rect = new RectangleF(value.X, value.Y, this.Rect.Width, this.Rect.Height);
            }
        }

        public Vector2f TruePosition
        {
            get
            {
                return new Vector2f(this.Position.X + this.Offset.X, this.Position.Y + this.Offset.Y);
            }
        }

        public Vector2f Offset
        {
            get;
            set;
        }

        public int Altitude
        {
            get;
            set;
        }

        //public Vector2i HitBase
        //{
        //    get;
        //    protected set;
        //}

        public int HitHigh
        {
            get;
            protected set;
        }

        public RectangleF Rect
        {
            get;
            protected set;
        }

        public AEntity(int positionX, int positionY, int positionZ, int hitBaseX, int hitBaseY, int hitHigh)
        {
            this.Altitude = positionZ;

            this.Rect = new RectangleF(positionX, positionY, hitBaseX, hitBaseY);

            //this.HitBase = new Vector2i(hitBaseX, hitBaseY);

            this.HitHigh = hitHigh;

            this.Offset = new Vector2f(0, 0);
        }

        public void UpdateLogic(LandWorld world, Time deltaTime)
        {
            // Nothing to do by default.
        }
    }
}
