using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.Model.Entity
{
    public interface IEntityUpdatable
    {
        void UpdateLogic(LandWorld world, IEntity owner, Time deltaTime);
    }
}
