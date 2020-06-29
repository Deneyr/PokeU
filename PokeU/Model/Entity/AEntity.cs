using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.Model.Entity
{
    public abstract class AEntity: IEntity
    {
        public bool Persistent
        {
            get
            {
                return true;
            }
        }

        public Vector2f Position
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
            get;
            protected set;
        }

        public int HitHigh
        {
            get;
            protected set;
        }

        public AEntity(int positionX, int positionY, int positionZ, int hitBaseX, int hitBaseY, int hitHigh)
        {
            this.Position = new Vector2f(positionX, positionY);

            this.Altitude = positionZ;

            this.HitBase = new Vector2i(hitBaseX, hitBaseY);

            this.HitHigh = hitHigh;
        }

        public void UpdateLogic(LandWorld world, Time deltaTime)
        {
            // Nothing to do by default.
        }
    }
}
