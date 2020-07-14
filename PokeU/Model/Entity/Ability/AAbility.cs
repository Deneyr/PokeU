using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace PokeU.Model.Entity.Ability
{
    public abstract class AAbility : IAbility
    {
        public virtual void UpdateLogic(LandWorld world, IEntity owner, Time deltaTime)
        {
            // to override
        }

        public virtual void Dispose()
        {

        }
    }
}
