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
        private Vector2i position;

        private Vector2i hitBase;

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
                return this.position;
            }
            set
            {
                this.position = value;

                this.UpdateRect();
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

        public Vector2i HitBase
        {
            get
            {
                return this.hitBase;
            }

            protected set
            {
                this.hitBase = value;

                this.UpdateRect();
            }
        }

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

        private void UpdateRect()
        {
            this.Rect = new RectangleF(this.position.X, this.position.Y, this.hitBase.X, this.hitBase.Y);
        }

        public void UpdateLogic(LandWorld world, Time deltaTime)
        {
            // Nothing to do by default.
        }
    }
}
