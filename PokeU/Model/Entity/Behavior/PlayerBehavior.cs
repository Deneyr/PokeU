using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokeU.Model.Entity.Ability;
using SFML.System;
using SFML.Window;

namespace PokeU.Model.Entity.Behavior
{
    public class PlayerBehavior : ABehavior
    {
        public override void UpdateLogic(LandWorld world, IEntity owner, Time deltaTime)
        {
            if (world.EntityManager.IsBookingStillValid(owner) == false)
            {
                WalkKinematicAbility walkKinematicAbility = owner.GetAbilityById("kinematic.walk") as WalkKinematicAbility;

                if (Keyboard.IsKeyPressed(Keyboard.Key.Z))
                {
                    walkKinematicAbility.SetSteering(world, owner, MovingDirection.ABOVE);
                }
                else if (Keyboard.IsKeyPressed(Keyboard.Key.S))
                {
                    walkKinematicAbility.SetSteering(world, owner, MovingDirection.UNDER);
                }

                if (Keyboard.IsKeyPressed(Keyboard.Key.D))
                {
                    walkKinematicAbility.SetSteering(world, owner, MovingDirection.RIGHT);
                }
                else if (Keyboard.IsKeyPressed(Keyboard.Key.Q))
                {
                    walkKinematicAbility.SetSteering(world, owner, MovingDirection.LEFT);
                }
            }
        }
    }
}
