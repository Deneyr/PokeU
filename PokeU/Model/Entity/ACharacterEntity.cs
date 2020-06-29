using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.Model.Entity
{
    public class ACharacterEntity : AEntity
    {
        public ACharacterEntity(int positionX, int positionY, int positionZ, int hitBaseX, int hitBaseY, int hitHigh)
            : base(positionX, positionY, positionZ, hitBaseX, hitBaseY, hitHigh)
        {
        }
    }
}
