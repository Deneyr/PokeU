using PokeU.Model.Entity.Ability;
using QuadTrees.QTreeRect;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.Model.Entity
{
    public interface IEntity: IObject, IUpdatable, IRectQuadStorable, IDisposable
    {
        bool Persistent
        {
            get;
        }

        Vector2f TruePosition
        {
            get;
        }

        float TrueAltitude
        {
            get;
        }

        Vector2i Position
        {
            get;
        }

        Vector2f OffsetPosition
        {
            get;
        }

        int Altitude
        {
            get;
        }

        float OffsetAltitude
        {
            get;
        }

        Vector2i HitBase
        {
            get;
        }

        int HitHigh
        {
            get;
        }

        IAbility GetAbilityById(string id);

        void SetPosition(int x, int y, int z);

        void SetOffsetPosition(float x, float y, float z);
    }
}
