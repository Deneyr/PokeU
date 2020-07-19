using PokeU.Model;
using PokeU.Model.Entity;
using PokeU.Model.GrassObject;
using PokeU.Model.GroundObject;
using PokeU.Model.MountainObject;
using PokeU.Model.WaterObject;
using PokeU.View.Entity2D;
using PokeU.View.GrassObject;
using PokeU.View.GroundObject;
using PokeU.View.MountainObject;
using PokeU.View.ResourcesManager;
using PokeU.View.WaterObject;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.View
{
    public class LandWorld2D
    {
        public static readonly int LOADED_ALTITUDE_RANGE = 5;

        public static readonly Dictionary<Type, IObject2DFactory> MappingObjectModelView;

        public static readonly TextureManager TextureManager;

        private Entity2DManager entity2DManager;

        private Dictionary<ILandChunk, LandChunk2D> landChunksDictionary;

        private ChunkResourcesLoader chunkResourcesLoader;

        private int currentAltitude;

        static LandWorld2D()
        {
            TextureManager = new TextureManager();

            MappingObjectModelView = new Dictionary<Type, IObject2DFactory>();

            // Land objects

            MappingObjectModelView.Add(typeof(GroundLandObject), new GroundObject2DFactory());
            MappingObjectModelView.Add(typeof(GroundElementLandObject), new GroundElementObject2DFactory());
            MappingObjectModelView.Add(typeof(AltitudeLandObject), new AltitudeObject2DFactory());
            MappingObjectModelView.Add(typeof(WaterLandObject), new WaterObject2DFactory());
            MappingObjectModelView.Add(typeof(MountainLandObject), new MountainObject2DFactory());
            MappingObjectModelView.Add(typeof(MountainElementLandObject), new MountainElementObject2DFactory());
            MappingObjectModelView.Add(typeof(GrassLandObject), new GrassObject2DFactory());
            MappingObjectModelView.Add(typeof(GrassElementLandObject), new GrassElementObject2DFactory());

            // Entity objects

            MappingObjectModelView.Add(typeof(PlayerEntity), new PlayerEntity2DFactory());

            MappingObjectModelView.Add(typeof(LandChunk), new LandChunk2DFactory());
            MappingObjectModelView.Add(typeof(LandCase), new LandCase2DFactory());

            foreach (IObject2DFactory factory in MappingObjectModelView.Values)
            {
                TextureManager.TextureLoaded += factory.OnTextureLoaded;
                TextureManager.TextureUnloaded += factory.OnTextureUnloaded;
            }
        }

        public LandWorld2D(LandWorld landWorld)
        {
            this.landChunksDictionary = new Dictionary<ILandChunk, LandChunk2D>();

            this.chunkResourcesLoader = new ChunkResourcesLoader();

            this.entity2DManager = new Entity2DManager(this);
            landWorld.EntityManager.EntityAdded += this.entity2DManager.OnEntityAdded;
            landWorld.EntityManager.EntityRemoved += this.entity2DManager.OnEntityRemoved;

            this.currentAltitude = 0;

            landWorld.ChunkAdded += OnChunkAdded;
            landWorld.ChunkRemoved += OnChunkRemoved;
        }

        public int CurrentAltitude
        {
            get
            {
                return this.currentAltitude;
            }
            set
            {
                int trueCurrentAltitude = 0;

                foreach(KeyValuePair<ILandChunk, LandChunk2D> landChunkPair in this.landChunksDictionary)
                {
                    trueCurrentAltitude = landChunkPair.Value.UpdateCurrentAltitude(this, landChunkPair.Key, value);
                }

                this.currentAltitude = trueCurrentAltitude;
            }
        }

        public void DrawIn(RenderWindow window, ref FloatRect boundsView)
        {
            //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            //sw.Start();

            foreach (LandChunk2D landChunk2D in this.landChunksDictionary.Values)
            {
                FloatRect bounds = new FloatRect(landChunk2D.Position, new SFML.System.Vector2f(landChunk2D.Width, landChunk2D.Height));

                if (bounds.Intersects(boundsView))
                {
                    landChunk2D.DrawIn(window, ref boundsView);
                }
            }

            this.entity2DManager.DrawIn(window, ref boundsView);

            //sw.Stop();

            //Console.WriteLine("time consume = " + sw.Elapsed);
        }

        private void OnChunkAdded(ILandChunk obj)
        {
            this.chunkResourcesLoader.LoadChunkResources(this, obj);

            IObject2DFactory landChunk2DFactory = LandWorld2D.MappingObjectModelView[obj.GetType()];

            this.landChunksDictionary.Add(obj, landChunk2DFactory.CreateObject2D(this, obj) as LandChunk2D);
        }


        private void OnChunkRemoved(ILandChunk obj)
        {
            this.chunkResourcesLoader.UnloadChunkResources(this, obj);

            this.landChunksDictionary[obj].Dispose();

            this.landChunksDictionary.Remove(obj);
        }

        public void Dispose(LandWorld landWorld)
        {
            landWorld.EntityManager.EntityAdded -= this.entity2DManager.OnEntityAdded;
            landWorld.EntityManager.EntityRemoved -= this.entity2DManager.OnEntityRemoved;

            landWorld.ChunkAdded -= OnChunkAdded;
            landWorld.ChunkRemoved -= OnChunkRemoved;
        }
    }
}
