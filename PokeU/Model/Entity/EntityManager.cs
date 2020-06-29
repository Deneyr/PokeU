using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace PokeU.Model.Entity
{
    public class EntityManager: IUpdatable
    {
        private Dictionary<IntRect, List<IEntity>> entities;

        public EntityManager()
        {
            this.entities = new Dictionary<IntRect, List<IEntity>>();
        }


        public void AddEntity(IntRect area, IEntity entity)
        {
            List<IEntity> entitiesInChunk;
            if (this.entities.ContainsKey(area) == false)
            {
                entitiesInChunk = new List<IEntity>();
                this.entities.Add(area, entitiesInChunk);
            }
            else
            {
                entitiesInChunk = this.entities[area];
            }

            if (entity != null)
            {
                entitiesInChunk.Add(entity);
            }
        }

        public void RemoveChunk(IntRect area)
        {
            if (this.entities.ContainsKey(area))
            {
                this.entities.Remove(area);
            }
        }

        public void UpdateLogic(LandWorld world, Time deltaTime)
        {
            foreach (KeyValuePair<IntRect, List<IEntity>> entries in this.entities)
            {
                if (world.IsChunkActive(entries.Key))
                {
                    foreach (IEntity entity in entries.Value)
                    {
                        entity.UpdateLogic(world, deltaTime);
                    }
                }
            }
        }

        public void MoveEntity(LandWorld world, IEntity entity, float x, float y, int z)
        {
            ILandChunk landChunkFrom = world.GetLandChunkAt((int)entity.Position.X, (int)entity.Position.Y);
            ILandChunk landChunkTo = world.GetLandChunkAt((int) x, (int) y);

            if (landChunkTo != null)
            {
                if (landChunkFrom != landChunkTo)
                {

                }
            }
        }

        public void OnChunkAdded(ILandChunk chunk)
        {
            if (this.entities.ContainsKey(chunk.Area))
            {
                throw new Exception("Add an existing chunk in Entity Manager");
            }

            List<IEntity> entitiesInChunk = new List<IEntity>();
            this.entities.Add(chunk.Area, entitiesInChunk);

            foreach (IEntity entity in chunk.EntitiesInChunk)
            {
                entitiesInChunk.Add(entity);
            }
        }

        public void OnChunkRemoved(ILandChunk chunk)
        {
            if (this.entities.ContainsKey(chunk.Area) == false)
            {
                throw new Exception("Remove a non-existing chunk in Entity Manager");
            }

            this.entities.Remove(chunk.Area);
        }
    }
}
