using PokeU.Model.Loader;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.Model.Entity.Ability
{
    public class WalkKinematicAbility : AKinematicAbility
    {
        private float speed;

        public WalkKinematicAbility(float speed)
        {
            this.speed = speed;
        }

        protected override bool BookPosition(LandWorld world, IEntity entity, Vector2i position, int altitude)
        {
            //while(altitude <= LandChunkLoader.ALTITUDE_RANGE && world.GetLandCaseAt(position.X, position.Y, altitude).LandWall != null)
            //{
            //    altitude++;
            //}

            altitude = world.GetAltitudeAt(position.X, position.Y);

            if (altitude <= LandChunkLoader.ALTITUDE_RANGE)
            {
                return base.BookPosition(world, entity, position, altitude);
            }

            return false;
        }

        public override void UpdateLogic(LandWorld world, IEntity owner, Time deltaTime)
        {
            base.UpdateLogic(world, owner, deltaTime);

            if (world.EntityManager.IsBookingStillValid(owner))
            {
                BookingEntity bookEntity = world.EntityManager.GetBookingEntityFor(owner);

                Vector2i offsetPosition = bookEntity.Position - owner.Position;
                int offsetAltitude = bookEntity.Altitude - owner.Altitude;

                this.elapsedMovingTime += deltaTime.AsMilliseconds() / 1000f;

                Vector2f elapOffsetPosition = new Vector2f(this.elapsedMovingTime * this.speed * Math.Sign(offsetPosition.X), this.elapsedMovingTime * this.speed * Math.Sign(offsetPosition.Y));
                float elapOffsetAltitude = this.elapsedMovingTime * this.speed * Math.Sign(offsetAltitude);


                bool reachedX = false;
                bool reachedY = false;
                bool reachedAltitude = false;
                if (Math.Abs(elapOffsetPosition.X) > Math.Abs(offsetPosition.X))
                {
                    elapOffsetPosition.X = offsetPosition.X;
                    reachedX = true;
                }

                if (Math.Abs(elapOffsetPosition.Y) > Math.Abs(offsetPosition.Y))
                {
                    elapOffsetPosition.Y = offsetPosition.Y;
                    reachedY = true;
                }

                if (Math.Abs(elapOffsetAltitude) > Math.Abs(offsetAltitude))
                {
                    elapOffsetAltitude = offsetAltitude;
                    reachedAltitude = true;
                }

                if(reachedX && reachedY && reachedAltitude)
                {
                    world.EntityManager.MoveEntity(owner, bookEntity.Position.X, bookEntity.Position.Y, bookEntity.Altitude);
                }
                else
                {
                    owner.SetOffsetPosition(elapOffsetPosition.X, elapOffsetPosition.Y, elapOffsetAltitude);
                }
            }
        }

    }
}
