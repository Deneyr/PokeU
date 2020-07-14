using PokeU.Model.Entity.Ability;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.Model.Entity
{
    public class ALifeformEntity : AEntity
    {
        public void InitializeLifeformEntity(int MaxHP)
        {
            LifeAbility lifeModel = new LifeAbility(MaxHP);

            this.idToAbilities.Add("life", lifeModel);
        }

        public ALifeformEntity(int positionX, int positionY, int positionZ, int hitBaseX, int hitBaseY, int hitHigh) 
            : base(positionX, positionY, positionZ, hitBaseX, hitBaseY, hitHigh)
        {
        }
    }
}
