using PokeU.Model;
using PokeU.Model.Entity;
using PokeU.Model.Entity.Data;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.View.Entity2D
{
    public class Entity2DManager
    {
        private SortedDictionary<IEntity, AEntity2D> entitiesToEntities2D;

        private WeakReference<LandWorld2D> landWorld2D;

        public Entity2DManager(LandWorld2D owner)
        {
            this.entitiesToEntities2D = new SortedDictionary<IEntity, AEntity2D>(new EntityComparer());

            this.landWorld2D = new WeakReference<LandWorld2D>(owner);
        }

        public void DrawIn(RenderWindow window, ref FloatRect boundsView)
        {
            foreach(AEntity2D entity2D in this.entitiesToEntities2D.Values)
            {
                entity2D.DrawIn(window, ref boundsView);
            }
        }

        public void OnEntityAdded(ILandChunk landChunk, IEntity entity)
        {
            IObject2DFactory entityFactory = LandWorld2D.MappingObjectModelView[entity.GetType()];

            if (this.landWorld2D.TryGetTarget(out LandWorld2D world2D))
            {
                this.entitiesToEntities2D.Add(entity, entityFactory.CreateObject2D(world2D, entity) as AEntity2D);
            }
        }

        public void OnEntityRemoved(ILandChunk landChunk, IEntity entity)
        {
            if (this.entitiesToEntities2D.ContainsKey(entity))
            {
                this.entitiesToEntities2D.Remove(entity);
            }
        }

    }
}
