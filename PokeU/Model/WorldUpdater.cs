using PokeU.Model.Entity;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.Model
{
    public class WorldUpdater: IUpdatable, IDisposable
    {
        private Dictionary<PlayerEntity, bool> playerEntityToAdded;

        private HashSet<PlayerEntity> playerEntitiesUpdated;

        private Vector2f worldResolution;

        public Vector2f WorldResolution
        {
            get
            {
                return this.worldResolution;
            }
            set
            {
                if(this.worldResolution != value)
                {
                    this.worldResolution = value;
                }
            }
        }

        public WorldUpdater()
        {
            this.playerEntityToAdded = new Dictionary<PlayerEntity, bool>();
            this.playerEntitiesUpdated = new HashSet<PlayerEntity>();

            this.worldResolution = new Vector2f(0, 0);
        }

        public void AddPlayer(PlayerEntity playerEntity)
        {
            this.playerEntityToAdded.Add(playerEntity, false);

            this.playerEntitiesUpdated.Add(playerEntity);
        }

        public void OnEntityAddedToManager(ILandChunk landChunk, IEntity entity)
        {
            if (entity is PlayerEntity 
                && this.playerEntityToAdded.ContainsKey(entity as PlayerEntity))
            {
                this.playerEntityToAdded[entity as PlayerEntity] = true;
            }
        }

        public void OnEntityRemovedToManager(ILandChunk landChunk, IEntity entity)
        {
            if (entity is PlayerEntity
                && this.playerEntityToAdded.ContainsKey(entity as PlayerEntity))
            {
                this.playerEntityToAdded[entity as PlayerEntity] = false;
            }
        }

        public void Dispose()
        {
            
        }

        public void UpdateLogic(LandWorld world, Time deltaTime)
        {
            foreach(PlayerEntity playerEntity in this.playerEntitiesUpdated)
            {
                world.OnFocusAreaChanged(playerEntity.TruePosition, this.worldResolution);
            }

            foreach (KeyValuePair<PlayerEntity, bool> playerEntityEntry in this.playerEntityToAdded)
            {
                if (playerEntityEntry.Value == false)
                {
                    ILandChunk landChunk = world.GetLandChunkAt(playerEntityEntry.Key.Position.X, playerEntityEntry.Key.Position.Y);

                    if(landChunk != null)
                    {
                        world.EntityManager.AddEntity(playerEntityEntry.Key, landChunk);
                    }
                }
            }

            this.playerEntitiesUpdated.Clear();
        }
    }
}
