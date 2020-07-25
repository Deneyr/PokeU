using PokeU.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.View.Entity2D
{
    public abstract class ACharacterEntity2D : AEntity2D
    {
        protected ACharacterEntity characterEntity;

        public ACharacterEntity2D(IObject2DFactory factory, ACharacterEntity characterEntity):
            base()
        {
            characterEntity.PositionUpdated += this.OnPositionUpdated;

            this.characterEntity = characterEntity;
        }

        protected virtual void OnPositionUpdated(IEntity obj)
        {
            this.Position = obj.TruePosition;
        }

        protected enum CharacterAnimation
        {
            IDLE_TOP = 0,
            IDLE_BOT = 1,
            IDLE_RIGHT = 2,
            IDLE_LEFT = 3,
            WALK_TOP = 4,
            WALK_BOT = 5,
            WALK_RIGHT = 6,
            WALK_LEFT = 7
        }

        public override void Dispose()
        {
            this.characterEntity.PositionUpdated += this.OnPositionUpdated;
        }
    }
}
