using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokeU.Model.Entity.Data;
using QuadTrees;
using SFML.Graphics;
using SFML.System;

namespace PokeU.Model.Entity
{
    public class EntityManager: IUpdatable
    {
        // private Dictionary<IntRect, List<IEntity>> entities;

        private QuadTreeRect<IEntity> area;

        private Dictionary<IEntity, ILandChunk> entitiesToChunks;

        private Dictionary<IEntity, BookingEntity> entitiesToBooking;


        public EntityManager()
        {
            this.area = new QuadTreeRect<IEntity>();

            this.entitiesToChunks = new Dictionary<IEntity, ILandChunk>();

            this.entitiesToBooking = new Dictionary<IEntity, BookingEntity>();
        }

        public void UpdateLogic(LandWorld world, Time deltaTime)
        {
            foreach (KeyValuePair<IEntity, ILandChunk> entries in this.entitiesToChunks)
            {
                if (world.IsChunkActive(entries.Value.Area))
                {
                    entries.Key.UpdateLogic(world, deltaTime);
                }
            }
        }

        public bool BookPositionForEntity(LandWorld world, IEntity entity, int x, int y, int z)
        {
            if(this.entitiesToBooking.ContainsKey(entity) == false)
            {
                ILandChunk landChunk = world.GetLandChunkAt(x, y);

                if(landChunk != null)
                {
                    BookingEntity bookingEntity = new BookingEntity(entity, x, y, z);

                    this.entitiesToBooking.Add(entity, bookingEntity);

                    this.AddEntity(entity, landChunk);

                    return true;
                }
            }

            return false;
        }

        public bool MoveEntity(LandWorld world, IEntity entity, int x, int y)
        {
            if (this.entitiesToBooking.ContainsKey(entity))
            {
                BookingEntity bookingEntity = this.entitiesToBooking[entity];
                this.RemoveEntity(bookingEntity);

                ILandChunk landChunkFrom = world.GetLandChunkAt(entity.Position.X, entity.Position.Y);
                ILandChunk landChunkTo = world.GetLandChunkAt(x, y);

                if (landChunkTo != null)
                {
                    if (landChunkFrom != landChunkTo)
                    {
                        landChunkFrom.EntitiesInChunk.Remove(entity);
                        landChunkTo.EntitiesInChunk.Add(entity);

                        this.entitiesToChunks.Remove(entity);
                        this.entitiesToChunks.Add(entity, landChunkTo);
                    }

                    entity.Position = new Vector2i(x, y);

                    this.area.Move(entity);

                    return true;
                }
            }

            return false;
        }

        public void OnChunkAdded(ILandChunk chunk)
        {
            foreach(IEntity entity in chunk.EntitiesInChunk)
            {
                this.AddEntity(entity, chunk);
            }
        }

        public void OnChunkRemoved(ILandChunk chunk)
        {
            foreach (IEntity entity in chunk.EntitiesInChunk)
            {
                //this.area.Remove(entity);

                //this.entitiesToChunks.Remove(entity);

                //if (this.entitiesToBooking.ContainsKey(entity))
                //{
                //    this.RemoveEntity(entity);
                //}

                this.RemoveEntity(entity);
            }
        }

        public void AddEntity(IEntity entity, ILandChunk landChunk)
        {
            this.area.Add(entity);
            this.entitiesToChunks.Add(entity, landChunk);
        }

        public void RemoveEntity(IEntity entity)
        {
            this.area.Remove(entity);

            if (this.entitiesToChunks.ContainsKey(entity))
            {
                this.entitiesToChunks[entity].EntitiesInChunk.Remove(entity);
                this.entitiesToChunks.Remove(entity);
            }

            if (this.entitiesToBooking.ContainsKey(entity))
            {
                this.RemoveEntity(this.entitiesToBooking[entity]);
            }

            if(entity is BookingEntity)
            {
                BookingEntity bookingEntity = entity as BookingEntity;
                this.entitiesToBooking.Remove(bookingEntity.Owner);
            }
        }

        public void OnAllChunksUpdated(LandWorld world)
        {
            
        }
    }
}
