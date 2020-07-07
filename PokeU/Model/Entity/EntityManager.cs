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

        private IntRect area;

        private QuadTreeRect<IEntity> entitiesArea;

        private Dictionary<IEntity, ILandChunk> entitiesToChunks;

        private Dictionary<IEntity, BookingEntity> entitiesToBooking;


        public EntityManager()
        {
            this.entitiesArea = new QuadTreeRect<IEntity>();

            this.entitiesToChunks = new Dictionary<IEntity, ILandChunk>();

            this.entitiesToBooking = new Dictionary<IEntity, BookingEntity>();

            this.area = new IntRect();
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

        public bool IsBookingStillValid(IEntity entity)
        {
            return this.entitiesToBooking.ContainsKey(entity);
        }

        public IEnumerable<IEntity> GetEntitiesInCase(int x, int y, int z)
        {
            return this.GetEntitiesInArea(x, y, z, 1, 1, 1);
        }

        public IEnumerable<IEntity> GetEntitiesInArea(int x, int y, int z,
            int hitX, int hitY, int hitZ)
        {
            IntRect areaBounding = new IntRect(x, y, hitX, hitY);

            List<IEntity> result = new List<IEntity>();
            if(this.area.Contains(x, y))
            {
                List<IEntity> listEntities = this.entitiesArea.GetObjects(new System.Drawing.RectangleF(x, y, hitX, hitY));

                foreach(IEntity entity in listEntities)
                {
                    if((entity.Altitude + entity.HitHigh <= z || entity.Altitude >= z + hitZ) == false)
                    {
                        IntRect entityBounding = new IntRect(entity.Position.X, entity.Position.Y, entity.HitBase.X, entity.HitBase.Y);

                        if (areaBounding.Intersects(entityBounding))
                        {
                            result.Add(entity);
                        }
                    }
                }
            }

            return result;
        }

        public bool MoveEntity(IEntity entity, int x, int y)
        {
            if (this.entitiesToBooking.ContainsKey(entity))
            {
                BookingEntity bookingEntity = this.entitiesToBooking[entity];

                ILandChunk landChunkFrom = this.entitiesToChunks[entity];
                ILandChunk landChunkTo = this.entitiesToChunks[bookingEntity];

                if(landChunkFrom == null || landChunkTo == null)
                {
                    throw new Exception("Error during entity moving, chunk from or to is null");
                }

                this.RemoveEntity(bookingEntity);
                if (landChunkFrom != landChunkTo)
                {
                    landChunkFrom.EntitiesInChunk.Remove(entity);
                    landChunkTo.EntitiesInChunk.Add(entity);

                    this.entitiesToChunks.Remove(entity);
                    this.entitiesToChunks.Add(entity, landChunkTo);
                }

                entity.Position = new Vector2i(x, y);
                this.entitiesArea.Move(entity);

                return true;
            }

            return false;
        }

        public void OnChunkAdded(ILandChunk landChunk)
        {
            foreach(IEntity entity in landChunk.EntitiesInChunk)
            {
                this.entitiesArea.Add(entity);
                this.entitiesToChunks.Add(entity, landChunk);
            }
        }

        public void OnChunkRemoved(ILandChunk landChunk)
        {
            foreach (IEntity entity in landChunk.EntitiesInChunk)
            {
                this.entitiesArea.Remove(entity);

                this.entitiesToChunks.Remove(entity);

                if (this.entitiesToBooking.ContainsKey(entity))
                {
                    this.RemoveEntity(entity);
                }
            }
        }

        public void AddEntity(IEntity entity, ILandChunk landChunk)
        {
            this.entitiesArea.Add(entity);
            this.entitiesToChunks.Add(entity, landChunk);

            landChunk.EntitiesInChunk.Add(entity);
        }

        public void RemoveEntity(IEntity entity)
        {
            this.entitiesArea.Remove(entity);

            ILandChunk lEntityChunk = this.entitiesToChunks[entity];
            lEntityChunk.EntitiesInChunk.Remove(entity);
            this.entitiesToChunks.Remove(entity);

            if (this.entitiesToBooking.ContainsKey(entity))
            {
                BookingEntity bookingEntity = this.entitiesToBooking[entity];
                if (this.entitiesToChunks[bookingEntity] != lEntityChunk)
                {
                    this.RemoveEntity(this.entitiesToBooking[entity]);
                }
            }

            if(entity is BookingEntity)
            {
                BookingEntity bookingEntity = entity as BookingEntity;
                this.entitiesToBooking.Remove(bookingEntity.Owner);
            }
        }

        public void OnAllChunksUpdated(LandWorld world)
        {
            this.area = world.CurrentChunksArea;
        }
    }
}
