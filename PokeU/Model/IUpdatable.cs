using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.Model
{
    public interface IUpdatable
    {
        void UpdateLogic(LandWorld world, Time deltaTime);
    }
}
