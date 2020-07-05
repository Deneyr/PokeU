using QuadTrees.QTreeRect;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.Model.Entity
{
    public interface IEntity: IObject, IUpdatable, IRectQuadStorable
    {
        bool Persistent
        {
            get;
        }

        Vector2f TruePosition
        {
            get;
        }

        Vector2i Position
        {
            get;
            set;
        }

        Vector2f Offset
        {
            get;
            set;
        }

        int Altitude
        {
            get;
            set;
        }

        //Vector2i HitBase
        //{
        //    get;
        //}

        int HitHigh
        {
            get;
        }
    }
}
