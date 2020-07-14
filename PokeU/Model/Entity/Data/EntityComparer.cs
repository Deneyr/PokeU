using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.Model.Entity.Data
{
    public class EntityComparer : IComparer<IEntity>
    {
        public int Compare(IEntity x, IEntity y)
        {
            if(x.Altitude > y.Altitude)
            {
                return 1;
            }
            else if(x.Altitude < y.Altitude)
            {
                return -1;
            }
            else if(x.Position.Y < y.Position.Y)
            {
                return 1;
            }
            else if (x.Position.Y > y.Position.Y)
            {
                return -1;
            }

            return 0;
        }
    }
}
