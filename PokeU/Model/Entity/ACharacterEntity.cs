using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.Model.Entity
{
    public class ACharacterEntity : ALifeformEntity
    {
        public ACharacterEntity(int positionX, int positionY, int positionZ)
            : base(positionX, positionY, positionZ, 1, 1, 1)
        {
            this.InitializeLifeformEntity(100);
        }
    }
}
