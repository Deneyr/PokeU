using PokeU.LandGenerator.EpicenterData;
using PokeU.Model.GroundObject;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.Model
{
    public class LandWorld
    {
        private static readonly int CHUNK_SIZE = 64;

        private static readonly int ALTITUDE_RANGE = 0;

        private static readonly int LOADED_ALTITUDE_RANGE = 0;

        WorldGenerator worldGenerator;

        private List<List<ILandChunk>> landChunkArea;

        private IntRect currentChunksArea;

        public event Action<ILandChunk> ChunkAdded;

        public event Action<ILandChunk> ChunkRemoved;

        public LandWorld()
        {
            this.worldGenerator = new WorldGenerator(789, new Vector2f(0, 1f / 128), new SFML.System.Vector2f(0, 0));

            this.worldGenerator.AddGenerator("ground", 0, new GroundLayerGenerator());

            this.landChunkArea = new List<List<ILandChunk>>();

            this.currentChunksArea = new IntRect(0, 0, 0, 0);
        }

        public void OnFocusAreaChanged(Vector2f areaPosition, Vector2f areaSize, int altitude)
        {
            List<ILandChunk> removedChunk = new List<ILandChunk>();
            List<ILandChunk> addedChunk = new List<ILandChunk>();

            IntRect newArea = new IntRect(
                (int) (areaPosition.X - areaSize.X / 2 + 1), 
                (int) (areaPosition.Y - areaSize.Y / 2 + 1), 
                (int) (areaSize.X + 1), 
                (int) (areaSize.Y + 1));

            IntRect newChunksArea = new IntRect(
                (int) Math.Floor(((double) newArea.Left) / CHUNK_SIZE) - 1,
                (int) Math.Floor(((double) newArea.Top) / CHUNK_SIZE) - 1,
                newArea.Width / CHUNK_SIZE + 4,
                newArea.Height / CHUNK_SIZE + 4);

            // Remove out bounds chunks
            int minNbRemove = newChunksArea.Top - this.currentChunksArea.Top;
            int maxNbRemove = (this.currentChunksArea.Top + this.currentChunksArea.Height) - (newChunksArea.Top + newChunksArea.Height);

            if (minNbRemove > 0)
            {
                List<List<ILandChunk>> subLandChunkList = this.landChunkArea.GetRange(0, minNbRemove);
                foreach (List<ILandChunk> row in subLandChunkList)
                {
                    removedChunk.AddRange(row);

                    Console.WriteLine(row.Count + " up chunk removed");
                }
                landChunkArea.RemoveRange(0, minNbRemove);
            }

            if (maxNbRemove > 0)
            {
                List<List<ILandChunk>> subLandChunkList = this.landChunkArea.GetRange(landChunkArea.Count - maxNbRemove, maxNbRemove);
                foreach (List<ILandChunk> row in subLandChunkList)
                {
                    removedChunk.AddRange(row);

                    Console.WriteLine(row.Count + " down chunk removed");
                }
                landChunkArea.RemoveRange(landChunkArea.Count - maxNbRemove, maxNbRemove);
            }

            minNbRemove = newChunksArea.Left - this.currentChunksArea.Left;
            maxNbRemove = (this.currentChunksArea.Left + this.currentChunksArea.Width) - (newChunksArea.Left + newChunksArea.Width);

            foreach(List<ILandChunk> row in this.landChunkArea)
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
                foreach (List<ILandChunk> row in this.landChunkArea)
                {
                    List<ILandChunk> chunksAdded = new List<ILandChunk>();
                    for (int j = 0; j < minNbAdd; j++)
                    {
                        IntRect area = new IntRect((newChunksArea.Left + j) * CHUNK_SIZE, (newChunksArea.Top + i) * CHUNK_SIZE, CHUNK_SIZE, CHUNK_SIZE);

                        LandChunk landChunk = new LandChunk(area);

                        this.worldGenerator.GenerateEpicenterChunk(area);

                        List<ILandLayer> layersList = this.worldGenerator.GenerateLandChunk(area, -ALTITUDE_RANGE, ALTITUDE_RANGE);

                        foreach (ILandLayer layer in layersList)
                        {
                            landChunk.AddLandLayer(layer);
                        }

                        chunksAdded.Add(landChunk);
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
                foreach (List<ILandChunk> row in this.landChunkArea)
                {
                    List<ILandChunk> chunksAdded = new List<ILandChunk>();
                    for (int j = 0; j < maxNbAdd; j++)
                    {
                        IntRect area = new IntRect((newChunksArea.Left + row.Count + j) * CHUNK_SIZE, (newChunksArea.Top + i) * CHUNK_SIZE, CHUNK_SIZE, CHUNK_SIZE);

                        LandChunk landChunk = new LandChunk(area);

                        this.worldGenerator.GenerateEpicenterChunk(area);

                        List<ILandLayer> layersList = this.worldGenerator.GenerateLandChunk(area, -ALTITUDE_RANGE, ALTITUDE_RANGE);

                        foreach (ILandLayer layer in layersList)
                        {
                            landChunk.AddLandLayer(layer);
                        }

                        chunksAdded.Add(landChunk);
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
                    List<ILandChunk> chunksAdded = new List<ILandChunk>();

                    for (int j = 0; j < newChunksArea.Width; j++)
                    {

                        IntRect area = new IntRect((newChunksArea.Left + j) * CHUNK_SIZE, (newChunksArea.Top + i) * CHUNK_SIZE, CHUNK_SIZE, CHUNK_SIZE);

                        LandChunk landChunk = new LandChunk(area);

                        this.worldGenerator.GenerateEpicenterChunk(area);

                        List<ILandLayer> layersList = this.worldGenerator.GenerateLandChunk(area, -ALTITUDE_RANGE, ALTITUDE_RANGE);

                        foreach (ILandLayer layer in layersList)
                        {
                            landChunk.AddLandLayer(layer);
                        }

                        chunksAdded.Add(landChunk);
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
                    List<ILandChunk> chunksAdded = new List<ILandChunk>();

                    for (int j = 0; j < newChunksArea.Width; j++)
                    {

                        IntRect area = new IntRect((newChunksArea.Left + j) * CHUNK_SIZE, (newChunksArea.Top + (newChunksArea.Height - maxNbAdd) + i) * CHUNK_SIZE, CHUNK_SIZE, CHUNK_SIZE);

                        LandChunk landChunk = new LandChunk(area);

                        this.worldGenerator.GenerateEpicenterChunk(area);

                        List<ILandLayer> layersList = this.worldGenerator.GenerateLandChunk(area, -ALTITUDE_RANGE, ALTITUDE_RANGE);

                        foreach (ILandLayer layer in layersList)
                        {
                            landChunk.AddLandLayer(layer);
                        }

                        chunksAdded.Add(landChunk);
                    }
                    addedChunk.AddRange(chunksAdded);

                    this.landChunkArea.Insert(this.landChunkArea.Count, chunksAdded);

                    Console.WriteLine(chunksAdded.Count + " down chunk added");
                }
            }

            this.NotifyChunksUpdated(removedChunk, addedChunk);

            this.currentChunksArea = newChunksArea;
        }

        private void NotifyChunksUpdated(List<ILandChunk> chunksRemoved, List<ILandChunk> chunksAdded)
        {
            foreach(ILandChunk landChunk in chunksRemoved)
            {
                this.NotifyChunkRemoved(landChunk);
            }

            foreach (ILandChunk landChunk in chunksAdded)
            {
                this.NotifyChunkAdded(landChunk);
            }
        }

        private void NotifyChunkAdded(ILandChunk landChunkAdded)
        {
            if(this.ChunkAdded != null)
            {
                this.ChunkAdded(landChunkAdded);
            }
        }

        private void NotifyChunkRemoved(ILandChunk landChunkRemoved)
        {
            if (this.ChunkRemoved != null)
            {
                this.ChunkRemoved(landChunkRemoved);
            }
        }

    }
}
