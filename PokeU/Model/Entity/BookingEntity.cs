using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.Model.Entity
{
    public class BookingEntity : AEntity
    {
        public override bool Persistent
        {
            get
            {
                return false;
            }
        }

        public IEntity Owner
        {
            get;
            private set;
        }

        public BookingEntity(IEntity owner, int x, int y, int z) 
            : base(x, y, z, (int) owner.Rect.Width, (int) owner.Rect.Height, owner.HitHigh)
        {
            this.Owner = owner;
        }
    }
}
