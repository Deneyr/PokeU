using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace PokeU.Model.Entity.Behavior
{
    public abstract class ABehavior : IBehavior
    {
        public virtual void Dispose()
        {

        }

        public abstract void UpdateLogic(LandWorld world, IEntity owner, Time deltaTime);
    }
}
