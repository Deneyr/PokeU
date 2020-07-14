using PokeU.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.View.Entity2D
{
    public abstract class AEntity2D: AObject2D
    {
        public virtual void OnEntityMoved(IEntity entity)
        {

        }
    }
}
