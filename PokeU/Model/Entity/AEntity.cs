using PokeU.Model.Entity.Ability;
using PokeU.Model.Entity.Behavior;
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
        private Vector2i hitBase;

        protected Dictionary<string, IAbility> idToAbilities;

        protected Dictionary<string, IBehavior> idToBehaviors;

        public event Action<IEntity> PositionUpdated;

        public virtual bool Persistent
        {
            get
            {
                return true;
            }
        }

        public Vector2i Position
        {
            get;
            protected set;
        }

        public Vector2f TruePosition
        {
            get
            {
                return new Vector2f(this.Position.X + this.OffsetPosition.X, this.Position.Y + this.OffsetPosition.Y);
            }
        }

        public float TrueAltitude
        {
            get
            {
                return this.Altitude + this.OffsetAltitude;
            }
        }

        public Vector2f OffsetPosition
        {
            get;
            protected set;
        }

        public int Altitude
        {
            get;
            protected set;
        }

        public float OffsetAltitude
        {
            get;
            protected set;
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
            this.Position = new Vector2i(positionX, positionY);
            this.Altitude = positionZ;

            this.OffsetPosition = new Vector2f(0, 0);
            this.OffsetAltitude = 0;

            this.Rect = new RectangleF(positionX, positionY, hitBaseX, hitBaseY);

            this.HitHigh = hitHigh;

            this.idToAbilities = new Dictionary<string, IAbility>();
            this.idToBehaviors = new Dictionary<string, IBehavior>();
        }

        private void UpdateRect()
        {
            this.Rect = new RectangleF(this.Position.X, this.Position.Y, this.hitBase.X, this.hitBase.Y);
        }

        public IAbility GetAbilityById(string id)
        {
            return this.idToAbilities[id];
        }

        public IBehavior GetBehaviorById(string id)
        {
            return this.idToBehaviors[id];
        }

        public void SetPosition(int x, int y, int z)
        {
            if(this.Position.X != x || this.Position.Y != y || this.Altitude != z)
            {
                this.Position = new Vector2i(x, y);
                this.Altitude = z;

                this.OffsetPosition = new Vector2f(0, 0);
                this.OffsetAltitude = 0;

                this.UpdateRect();

                this.NotifyPositionUpdated();
            }
        }

        public void SetOffsetPosition(float x, float y, float z)
        {
            if (this.OffsetPosition.X != x || this.OffsetPosition.Y != y || this.OffsetAltitude != z)
            {
                this.OffsetPosition = new Vector2f(x, y);
                this.OffsetAltitude = z;

                this.NotifyPositionUpdated();
            }
        }

        public void UpdateLogic(LandWorld world, Time deltaTime)
        {
            foreach(IAbility ability in this.idToAbilities.Values)
            {
                ability.UpdateLogic(world, this, deltaTime);
            }

            foreach (IBehavior behavior in this.idToBehaviors.Values)
            {
                behavior.UpdateLogic(world, this, deltaTime);
            }
        }

        private void NotifyPositionUpdated()
        {
            if(this.PositionUpdated != null)
            {
                this.PositionUpdated(this);
            }
        }

        public void Dispose()
        {
            foreach(IAbility ability in this.idToAbilities.Values)
            {
                ability.Dispose();
            }

            this.idToAbilities.Clear();
        }
    }
}
