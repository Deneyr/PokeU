using PokeU.LandGenerator.EpicenterData;
using PokeU.Model.GrassObject;
using PokeU.Model.GroundObject;
using PokeU.Model.WaterObject;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PokeU.Model.Loader
{
    public class LandChunkLoader
    {
        private static readonly int ALTITUDE_RANGE = 32;

        WorldGenerator worldGenerator;

        //private List<List<LandChunkContainer>> landChunkArea;
        private Dictionary<IntRect, LandChunkContainer> pendingLandChunks;

        private Thread mainThread;

        private Mutex mainMutex;

        private volatile bool isRunning;

        public event Action<List<Tuple<LandChunkContainer, ILandChunk>>> LandChunksImported;


        public LandChunkLoader()
        {
            this.worldGenerator = new WorldGenerator(12, new Vector2f(0, 1f / 128), new SFML.System.Vector2f(0, 0));

            this.worldGenerator.AddGenerator(0, new AltitudeLayerGenerator(ALTITUDE_RANGE));

            this.worldGenerator.AddGenerator(1, new CliffLayerGenerator());

            this.worldGenerator.AddGenerator(2, new DefaultGroundLayerGenerator());

            this.worldGenerator.AddGenerator(3, new GroundLayerGenerator());

            this.worldGenerator.AddGenerator(4, new WaterLayerGenerator());

            this.worldGenerator.AddGenerator(5, new GrassLayerGenerator());

            this.pendingLandChunks = new Dictionary<IntRect, LandChunkContainer>();

            this.mainMutex = new Mutex();

            this.isRunning = true;

            this.mainThread = new Thread(new ThreadStart(this.Run));
            this.mainThread.Start();
        }

        public void Run()
        {
            while (this.isRunning)
            {
                this.mainMutex.WaitOne();

                IEnumerable<LandChunkContainer> containersToImport = this.pendingLandChunks.Values.ToList();

                this.mainMutex.ReleaseMutex();

                List<Tuple<LandChunkContainer, ILandChunk>> containerImported = new List<Tuple<LandChunkContainer, ILandChunk>>();
                foreach (LandChunkContainer container in containersToImport)
                {
                    // TODO: Add file importation

                    this.worldGenerator.GenerateEpicenterChunk(container.Area);

                    LandChunk landChunk = this.worldGenerator.GenerateLandChunk(container.Area, -ALTITUDE_RANGE, ALTITUDE_RANGE);

                    containerImported.Add(new Tuple<LandChunkContainer, ILandChunk>(container, landChunk));
                }

                this.NotifyLandChunkImported(containerImported);

                this.mainMutex.WaitOne();

                foreach (LandChunkContainer container in containersToImport)
                {
                    this.pendingLandChunks.Remove(container.Area);
                }

                this.mainMutex.ReleaseMutex();

                Thread.Sleep(100);
            }
        }

        public bool IsLandChunkLoading(IntRect chunkArea)
        {
            this.mainMutex.WaitOne();

            bool lResult = this.pendingLandChunks.ContainsKey(chunkArea);

            this.mainMutex.ReleaseMutex();

            return lResult;
        }

        public void RequestChunk(List<LandChunkContainer> landChunkContainers)
        {
            this.mainMutex.WaitOne();

            foreach(LandChunkContainer container in landChunkContainers)
            {
                this.pendingLandChunks.Add(container.Area, container);
            }

            this.mainMutex.ReleaseMutex();
        }
        /*
        public void ReleaseChunks(List<LandChunkContainer> containers)
        {
            this.mainMutex.WaitOne();

            foreach (LandChunkContainer container in containers)
            {
                if (container.LandChunk != null)
                {
                    if (this.pendingChunksToImport.ContainsKey(container.LandChunk.Area))
                    {
                        this.pendingChunksToImport.Remove(container.LandChunk.Area);
                    }

                    this.pendingChunksToRelease.Add(container.LandChunk.Area, container);
                }
            }

            this.mainMutex.ReleaseMutex();
        }*/

        public void StopThread()
        {
            this.isRunning = false;
        }
 
        private void NotifyLandChunkImported(List<Tuple<LandChunkContainer, ILandChunk>> containersImported)
        {
            if (this.LandChunksImported != null)
            {
                this.LandChunksImported(containersImported);
            }
        }
    }

    public class LandChunkContainer
    {
        public LandChunkContainer(IntRect area)
        {
            this.Area = area;

            this.LandChunk = null;
        }

        public IntRect Area
        {
            get;
            protected set;
        }

        public ILandChunk LandChunk
        {
            get;
            set;
        }
    }
}
