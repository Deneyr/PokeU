﻿using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.Model.Entity.Ability
{
    public abstract class AKinematicAbility: AAbility
    {
        private MovingState movingState;

        public event Action<AKinematicAbility> MovingStateUpdated;

        public float elapsedMovingTime;

        public MovingState MovingState
        {
            get
            {
                return this.movingState;
            }
            protected set
            {
                if(this.movingState != value)
                {
                    this.movingState = value;

                    this.NotifyMovingStateUpdated();
                }
            }
        }

        public Vector2i DesiredPosition
        {
            get;
            protected set;
        }

        public int DesiredAltitude
        {
            get;
            protected set;
        }

        public bool TryToReach
        {
            get;
            protected set;
        }

        public AKinematicAbility()
        {
            this.DesiredPosition = new Vector2i();
            this.TryToReach = false;
            this.MovingState = MovingState.NONE;
            this.elapsedMovingTime = 0;
        }

        public virtual bool SetSteering(LandWorld world, IEntity entity, MovingDirection direction)
        {
            Vector2i nextPosition = entity.Position;
            int nextAltitude = entity.Altitude;

            switch (direction)
            {
                case MovingDirection.UP:
                    nextPosition = new Vector2i(entity.Position.X, entity.Position.Y - 1);
                    break;
                case MovingDirection.DOWN:
                    nextPosition = new Vector2i(entity.Position.X, entity.Position.Y + 1);
                    break;
                case MovingDirection.RIGHT:
                    nextPosition = new Vector2i(entity.Position.X + 1, entity.Position.Y);
                    break;
                case MovingDirection.LEFT:
                    nextPosition = new Vector2i(entity.Position.X - 1, entity.Position.Y);
                    break;
                case MovingDirection.ABOVE:
                    nextAltitude += 1;
                    break;
                case MovingDirection.UNDER:
                    nextAltitude -= 1;
                    break;
            }

            if(this.BookPosition(world, entity, nextPosition, nextAltitude))
            {
                return true;
            }

            return false;
        }

        public virtual bool SetSteering(LandWorld world, IEntity entity, Vector2i desiredPosition, int altitude)
        {
            this.DesiredPosition = desiredPosition;
            this.DesiredAltitude = altitude;
            this.TryToReach = true;

            return true;
        }

        protected virtual bool BookPosition(LandWorld world, IEntity entity, Vector2i position, int altitude)
        {
            if (this.IsPositionValid(world, entity, position, altitude))
            {
                if(world.EntityManager.BookPositionForEntity(world, entity, position.X, position.Y, altitude))
                {
                    if(position.X != entity.Position.X
                        && (position.Y == entity.Position.Y && altitude == entity.Altitude))
                    {
                        if(position.X > entity.Position.X)
                        {
                            this.MovingState = MovingState.MOVE_DOWN;
                        }
                        else
                        {
                            this.MovingState = MovingState.MOVE_UP;
                        }
                    }
                    else if(position.Y != entity.Position.Y
                        && (position.X == entity.Position.X && altitude == entity.Altitude))
                    {
                        if (position.Y > entity.Position.Y)
                        {
                            this.MovingState = MovingState.MOVE_RIGHT;
                        }
                        else
                        {
                            this.MovingState = MovingState.MOVE_LEFT;
                        }
                    }
                    else if(altitude != entity.Altitude
                        && (position.X == entity.Position.X && position.Y == entity.Position.Y))
                    {
                        if (altitude > entity.Altitude)
                        {
                            this.MovingState = MovingState.ABOVE;
                        }
                        else
                        {
                            this.MovingState = MovingState.UNDER;
                        }
                    }
                    else
                    {
                        this.MovingState = MovingState.TELEPORT;
                    }

                    this.elapsedMovingTime = 0;

                    return true;
                }
            }
            return false;
        }

        public override void UpdateLogic(LandWorld world, IEntity owner, Time deltaTime)
        {
            this.UpdateDesiredPosition(world, owner);

            if (world.EntityManager.IsBookingStillValid(owner) == false)
            {
                switch (this.MovingState)
                {
                    case MovingState.MOVE_UP:
                        this.MovingState = MovingState.IDLE_UP;
                        break;
                    case MovingState.MOVE_DOWN:
                        this.MovingState = MovingState.IDLE_DOWN;
                        break;
                    case MovingState.MOVE_RIGHT:
                        this.MovingState = MovingState.IDLE_RIGHT;
                        break;
                    case MovingState.MOVE_LEFT:
                        this.MovingState = MovingState.IDLE_LEFT;
                        break;
                }

                if (owner.OffsetPosition.X != 0 || owner.OffsetPosition.X != 0 || owner.OffsetAltitude != 0)
                {
                    owner.SetOffsetPosition(0, 0, 0);
                }
            }
        }

        private void NotifyMovingStateUpdated()
        {
            if(this.MovingStateUpdated != null)
            {
                this.MovingStateUpdated(this);
            }
        }

        protected virtual void UpdateDesiredPosition(LandWorld world, IEntity owner)
        {
            if (this.TryToReach)
            {

                int offsetX = this.DesiredPosition.X - owner.Position.X;
                int offsetY = this.DesiredPosition.Y - owner.Position.Y;
                int offsetZ = this.DesiredAltitude - owner.Altitude;

                Vector2i vectX = new Vector2i(owner.Position.X + offsetX, 0);
                Vector2i vectY = new Vector2i(0, owner.Position.Y + offsetY);

                if (this.IsPositionValid(world, owner, vectX, owner.Altitude))
                {
                    this.BookPosition(world, owner, vectX, owner.Altitude);
                }
                else if (this.IsPositionValid(world, owner, vectY, owner.Altitude))
                {
                    this.BookPosition(world, owner, vectY, owner.Altitude);
                }
                else if (this.IsPositionValid(world, owner, owner.Position, owner.Altitude + offsetZ))
                {
                    this.BookPosition(world, owner, owner.Position, owner.Altitude + offsetZ);
                }

                if (world.EntityManager.IsBookingStillValid(owner))
                {
                    BookingEntity bookingEntity = world.EntityManager.GetBookingEntityFor(owner);

                    if(bookingEntity.Position == this.DesiredPosition)
                    {
                        this.TryToReach = false;
                    }
                }

                //if (this.DesiredPosition.Equals(owner.Position))
                //{
                //    this.TryToReach = false;
                //}
                //else
                //{
                //    int offsetX = this.DesiredPosition.X - owner.Position.X;
                //    int offsetY = this.DesiredPosition.Y - owner.Position.Y;
                //    int offsetZ = this.DesiredAltitude - owner.Altitude;

                //    Vector2i vectX = new Vector2i(owner.Position.X + offsetX, 0);
                //    Vector2i vectY = new Vector2i(0, owner.Position.Y + offsetY);

                //    if (this.IsPositionValid(world, owner, vectX, owner.Altitude))
                //    {
                //        this.BookPosition(world, owner, vectX, owner.Altitude);
                //    }
                //    else if(this.IsPositionValid(world, owner, vectY, owner.Altitude))
                //    {
                //        this.BookPosition(world, owner, vectY, owner.Altitude);
                //    }
                //    else if (this.IsPositionValid(world, owner, owner.Position, owner.Altitude + offsetZ))
                //    {
                //        this.BookPosition(world, owner, owner.Position, owner.Altitude + offsetZ);
                //    }
                //}
            }
        }

        protected virtual bool IsPositionValid(LandWorld world, IEntity entity, Vector2i position, int altitude)
        {
            LifeAbility lifeAbility = entity.GetAbilityById("life") as LifeAbility;
            if (lifeAbility != null && lifeAbility.CurrentHP <= 0)
            {
                return false;
            }

            if(entity.Position.Equals(position) && entity.Altitude == altitude)
            {
                return false;
            }

            if(world.EntityManager.IsBookingStillValid(entity))
            {
                return false;
            }

            if(world.EntityManager.GetEntitiesInCase(position.X, position.Y, altitude).Any())
            {
                return false;
            }

            return true;
        }

    }

    public enum MovingDirection
    {
        UP,
        DOWN,
        RIGHT,
        LEFT,
        ABOVE,
        UNDER,
    }

    public enum MovingState
    {
        IDLE_UP,
        IDLE_DOWN,
        IDLE_RIGHT,
        IDLE_LEFT,
        MOVE_UP,
        MOVE_DOWN,
        MOVE_RIGHT,
        MOVE_LEFT,
        ABOVE,
        UNDER,
        TELEPORT,
        NONE
    }
}
