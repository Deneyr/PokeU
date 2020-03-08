using PokeU.LandGenerator.EpicenterData;
using PokeU.Model.GroundObject;
using PokeU.Model.Loader;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PokeU.Model
{
    public class LandWorld: IDisposable
    {
        private static readonly int CHUNK_SIZE = 64;

        private static readonly int NB_MAX_CACHE_CHUNK = 8;

        private LandChunkLoader landChunkLoader;
        private Mutex mainMutex;

        private Dictionary<IntRect, Tuple<LandChunkContainer, ILandChunk>> pendingLandChunksImported;

        private Dictionary<IntRect, LandChunkContainer> currentLoadedLandChunks;
        private List<LandChunkContainer> landChunksCache;
        private HashSet<LandChunkContainer> landChunksToRemove;

        private List<List<LandChunkContainer>> landChunkArea;
        private IntRect currentChunksArea;

        public event Action<ILandChunk> ChunkAdded;

        public event Action<ILandChunk> ChunkRemoved;

        public event Action<int> AltitudeChanged;

        public LandWorld()
        {
            this.landChunkLoader = new LandChunkLoader();
            this.landChunkLoader.LandChunksImported += this.OnLandChunkImported;

            this.pendingLandChunksImported = new Dictionary<IntRect, Tuple<LandChunkContainer, ILandChunk>>();

            this.currentLoadedLandChunks = new Dictionary<IntRect, LandChunkContainer>();
            this.landChunksCache = new List<LandChunkContainer>();
            this.landChunksToRemove = new HashSet<LandChunkContainer>();

            this.mainMutex = new Mutex();

            this.landChunkArea = new List<List<LandChunkContainer>>();

            this.currentChunksArea = new IntRect(0, 0, 0, 0);
        }

        public void OnFocusAreaChanged(Vector2f areaPosition, Vector2f areaSize, int altitude)
        {
            List<LandChunkContainer> removedChunk = new List<LandChunkContainer>();
            List<LandChunkContainer> addedChunk = new List<LandChunkContainer>();

            IntRect newArea = new IntRect(
                (int)(areaPosition.X - areaSize.X / 2 + 1),
                (int)(areaPosition.Y - areaSize.Y / 2 + 1),
                (int)(areaSize.X + 1),
                (int)(areaSize.Y + 1));

            IntRect newChunksArea = new IntRect(
                (int)Math.Floor(((double)newArea.Left) / CHUNK_SIZE) - 1,
                (int)Math.Floor(((double)newArea.Top) / CHUNK_SIZE) - 1,
                newArea.Width / CHUNK_SIZE + 4,
                newArea.Height / CHUNK_SIZE + 4);

            // Remove out bounds chunks
            int minNbRemove = newChunksArea.Top - this.currentChunksArea.Top;
            int maxNbRemove = (this.currentChunksArea.Top + this.currentChunksArea.Height) - (newChunksArea.Top + newChunksArea.Height);

            if (minNbRemove > 0)
            {
                List<List<LandChunkContainer>> subLandChunkList = this.landChunkArea.GetRange(0, minNbRemove);
                foreach (List<LandChunkContainer> row in subLandChunkList)
                {
                    removedChunk.AddRange(row);

                    Console.WriteLine(row.Count + " up chunk removed");
                }
                landChunkArea.RemoveRange(0, minNbRemove);
            }

            if (maxNbRemove > 0)
            {
                List<List<LandChunkContainer>> subLandChunkList = this.landChunkArea.GetRange(landChunkArea.Count - maxNbRemove, maxNbRemove);
                foreach (List<LandChunkContainer> row in subLandChunkList)
                {
                    removedChunk.AddRange(row);

                    Console.WriteLine(row.Count + " down chunk removed");
                }
                landChunkArea.RemoveRange(landChunkArea.Count - maxNbRemove, maxNbRemove);
            }

            minNbRemove = newChunksArea.Left - this.currentChunksArea.Left;
            maxNbRemove = (this.currentChunksArea.Left + this.currentChunksArea.Width) - (newChunksArea.Left + newChunksArea.Width);

            foreach (List<LandChunkContainer> row in this.landChunkArea)
            {
                if (minNbRemove > 0)
                {
                    removedChunk.AddRange(row.GetRange(0, minNbRemove));
                    row.RemoveRange(0, minNbRemove);

                    Console.WriteLine("left chunk removed");
                }

                if (maxNbRemove > 0)
                {
                    removedChunk.AddRange(row.GetRange(landChunkArea.Count - maxNbRemove, maxNbRemove));
                    row.RemoveRange(landChunkArea.Count - maxNbRemove, maxNbRemove);

                    Console.WriteLine("right chunk removed");
                }
            }

            // Add new chunks
            int minNbAdd = this.currentChunksArea.Left - newChunksArea.Left;
            int maxNbAdd = (newChunksArea.Left + newChunksArea.Width) - (this.currentChunksArea.Left + this.currentChunksArea.Width);

            int i = 0;
            if (minNbAdd > 0)
            {
                foreach (List<LandChunkContainer> row in this.landChunkArea)
                {
                    List<LandChunkContainer> chunksAdded = new List<LandChunkContainer>();
                    for (int j = 0; j < minNbAdd; j++)
                    {
                        IntRect area = new IntRect((newChunksArea.Left + j) * CHUNK_SIZE, (newChunksArea.Top + i) * CHUNK_SIZE, CHUNK_SIZE, CHUNK_SIZE);

                        LandChunkContainer container = new LandChunkContainer(area);

                        chunksAdded.Add(container);
                    }
                    addedChunk.AddRange(chunksAdded);

                    row.InsertRange(0, chunksAdded);

                    Console.WriteLine(chunksAdded.Count + " left chunk added");

                    i++;
                }
            }

            i = 0;
            if (maxNbAdd > 0)
            {
                foreach (List<LandChunkContainer> row in this.landChunkArea)
                {
                    List<LandChunkContainer> chunksAdded = new List<LandChunkContainer>();
                    for (int j = 0; j < maxNbAdd; j++)
                    {
                        IntRect area = new IntRect((newChunksArea.Left + row.Count + j) * CHUNK_SIZE, (newChunksArea.Top + i) * CHUNK_SIZE, CHUNK_SIZE, CHUNK_SIZE);

                        LandChunkContainer container = new LandChunkContainer(area);

                        chunksAdded.Add(container);
                    }

                    addedChunk.AddRange(chunksAdded);

                    row.InsertRange(row.Count, chunksAdded);

                    Console.WriteLine(chunksAdded.Count + " right chunk added");

                    i++;
                }
            }

            minNbAdd = currentChunksArea.Top - newChunksArea.Top;
            maxNbAdd = (newChunksArea.Top + newChunksArea.Height) - (currentChunksArea.Top + currentChunksArea.Height);

            if (minNbAdd > 0)
            {
                for (i = minNbAdd - 1; i >= 0; i--)
                {
                    List<LandChunkContainer> chunksAdded = new List<LandChunkContainer>();

                    for (int j = 0; j < newChunksArea.Width; j++)
                    {
                        IntRect area = new IntRect((newChunksArea.Left + j) * CHUNK_SIZE, (newChunksArea.Top + i) * CHUNK_SIZE, CHUNK_SIZE, CHUNK_SIZE);

                        LandChunkContainer container = new LandChunkContainer(area);

                        chunksAdded.Add(container);
                    }
                    addedChunk.AddRange(chunksAdded);

                    this.landChunkArea.Insert(0, chunksAdded);

                    Console.WriteLine(chunksAdded.Count + " top chunk added");
                }
            }

            if (maxNbAdd > 0)
            {
                for (i = 0; i < maxNbAdd; i++)
                {
                    List<LandChunkContainer> chunksAdded = new List<LandChunkContainer>();

                    for (int j = 0; j < newChunksArea.Width; j++)
                    {
                        IntRect area = new IntRect((newChunksArea.Left + j) * CHUNK_SIZE, (newChunksArea.Top + (newChunksArea.Height - maxNbAdd) + i) * CHUNK_SIZE, CHUNK_SIZE, CHUNK_SIZE);

                        LandChunkContainer container = new LandChunkContainer(area);

                        chunksAdded.Add(container);
                    }
                    addedChunk.AddRange(chunksAdded);

                    this.landChunkArea.Insert(this.landChunkArea.Count, chunksAdded);

                    Console.WriteLine(chunksAdded.Count + " down chunk added");
                }
            }

            this.PrepareChunksUpdated(removedChunk, addedChunk);

            this.currentChunksArea = newChunksArea;
        }

        private void PrepareChunksUpdated(List<LandChunkContainer> chunksRemoved, List<LandChunkContainer> chunksAdded)
        {
            List<LandChunkContainer> subChunksAddedList = new List<LandChunkContainer>();
            foreach (LandChunkContainer container in chunksAdded)
            {
                this.mainMutex.WaitOne();

                // Order is important
                if (this.landChunkLoader.IsLandChunkLoading(container.Area) == false
                    && this.pendingLandChunksImported.ContainsKey(container.Area) == false)
                {
                    this.mainMutex.ReleaseMutex();

                    if (this.currentLoadedLandChunks.ContainsKey(container.Area))
                    {
                        LandChunkContainer landChunkContainer = this.currentLoadedLandChunks[container.Area];
                        LandChunkContainer landChunkCache = this.landChunksCache.FirstOrDefault(pElem => pElem.Area == container.Area);
                        if (landChunkCache != null)
                        {
                            this.landChunksCache.Remove(landChunkCache);

                            container.LandChunk = landChunkContainer.LandChunk;

                            this.currentLoadedLandChunks[container.Area] = container;

                            /*this.pendingLandChunksImported.Add(container.Area, new Tuple<LandChunkContainer, ILandChunk>(container, landChunkContainer.LandChunk));

                            this.currentLoadedLandChunks.Remove(container.Area);*/

                            landChunkContainer.LandChunk = null;
                        }
                    }
                    else
                    {
                        subChunksAddedList.Add(container);
                    }
                }
                else
                {
                    this.mainMutex.ReleaseMutex();

                    //chunksRemoved.RemoveAll(pElem => pElem.Area.Equals(container.Area));
                }

            }

            this.landChunkLoader.RequestChunk(subChunksAddedList);

            foreach (LandChunkContainer container in chunksRemoved)
            {
                if (container.LandChunk == null
                    && (this.landChunkLoader.IsLandChunkLoading(container.Area) || this.pendingLandChunksImported.ContainsKey(container.Area) == false))
                {
                    this.landChunksToRemove.Add(container);
                }
                else if(container.LandChunk != null
                    && this.currentLoadedLandChunks.ContainsKey(container.Area)
                    && this.currentLoadedLandChunks[container.Area] == container
                    && this.landChunksCache.Contains(container) == false)
                {
                    this.landChunksToRemove.Add(container);
                }
            }
        }

        private void OnLandChunkImported(List<Tuple<LandChunkContainer, ILandChunk>> containersImported)
        {
            this.mainMutex.WaitOne();

            foreach (Tuple<LandChunkContainer, ILandChunk> tuple in containersImported)
            {
                this.pendingLandChunksImported.Add(tuple.Item1.Area, tuple);
            }

            this.mainMutex.ReleaseMutex();
        }

        public void Update(Time deltaTime)
        {
            this.UpdateLandChunks();


        }

        private void UpdateLandChunks()
        {
            this.mainMutex.WaitOne();

            IEnumerable<Tuple<LandChunkContainer, ILandChunk>> tuplesImported = this.pendingLandChunksImported.Values.ToList();

            this.mainMutex.ReleaseMutex();

            List<ILandChunk> realLandChunksToRemove = new List<ILandChunk>();
            List<LandChunkContainer> firstLandChunksToRemove = this.landChunksToRemove.ToList();
            HashSet<ILandChunk> realLandChunksToImport = new HashSet<ILandChunk>();

            foreach (Tuple<LandChunkContainer, ILandChunk> tupleImported in tuplesImported)
            {
                tupleImported.Item1.LandChunk = tupleImported.Item2;

                this.currentLoadedLandChunks.Add(tupleImported.Item1.Area, tupleImported.Item1);

                realLandChunksToImport.Add(tupleImported.Item2);

                this.mainMutex.WaitOne();

                this.pendingLandChunksImported.Remove(tupleImported.Item1.Area);

                this.mainMutex.ReleaseMutex();
            }


            foreach (LandChunkContainer containerToRemove in firstLandChunksToRemove)
            {
                if (this.currentLoadedLandChunks.ContainsKey(containerToRemove.Area))
                {
                    this.landChunksCache.Add(this.currentLoadedLandChunks[containerToRemove.Area]);
                    if (this.landChunksCache.Count > NB_MAX_CACHE_CHUNK)
                    {
                        ILandChunk landChunkFront = this.landChunksCache.ElementAt(0).LandChunk;
                        this.landChunksCache.RemoveAt(0);

                        this.currentLoadedLandChunks.Remove(landChunkFront.Area);

                        if (realLandChunksToImport.Contains(landChunkFront))
                        {
                            realLandChunksToImport.Remove(landChunkFront);
                        }
                        else
                        {
                            realLandChunksToRemove.Add(landChunkFront);
                        }
                    }

                    this.landChunksToRemove.Remove(containerToRemove);
                }
            }

            foreach (ILandChunk landChunkReleased in realLandChunksToRemove)
            {
                this.NotifyChunkRemoved(landChunkReleased);
            }

            foreach (ILandChunk landChunkImported in realLandChunksToImport)
            {
                this.NotifyChunkAdded(landChunkImported);
            }
        }

        protected List<ILandChunk> remainingChunks = new List<ILandChunk>();

        private void NotifyChunkAdded(ILandChunk landChunkAdded)
        {
            if(this.ChunkAdded != null)
            {
                Console.WriteLine("chunk added " + landChunkAdded.Area.Left + " : " + landChunkAdded.Area.Top + " - " + remainingChunks.Count);
                remainingChunks.Add(landChunkAdded);
                this.ChunkAdded(landChunkAdded);
            }
        }

        private void NotifyChunkRemoved(ILandChunk landChunkRemoved)
        {
            if (this.ChunkRemoved != null)
            {
                Console.WriteLine("chunk removed" + landChunkRemoved.Area.Left + " : " + landChunkRemoved.Area.Top + " - " + remainingChunks.Count);
                remainingChunks.Remove(landChunkRemoved);
                this.ChunkRemoved(landChunkRemoved);
            }
        }

        private void NotifyAltitudeChanged(int newAltitude)
        {
            if (this.AltitudeChanged != null)
            {
                this.AltitudeChanged(newAltitude);
            }
        }

        public void Dispose()
        {
            this.landChunkLoader.StopThread();

            this.landChunkLoader.LandChunksImported -= this.OnLandChunkImported;
        }
    }
}
