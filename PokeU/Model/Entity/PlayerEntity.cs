using PokeU.Model.Entity.Ability;
using PokeU.Model.Entity.Behavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.Model.Entity
{
    public class PlayerEntity : ACharacterEntity
    {
        public PlayerEntity(int positionX, int positionY, int positionZ) : base(positionX, positionY, positionZ)
        {
            WalkKinematicAbility walkKinematicAbility = new WalkKinematicAbility(1f);
            this.idToAbilities.Add("kinematic.walk", walkKinematicAbility);

            PlayerBehavior playerBehavior = new PlayerBehavior();
            this.idToBehaviors.Add("player", playerBehavior);
        }
    }
}
