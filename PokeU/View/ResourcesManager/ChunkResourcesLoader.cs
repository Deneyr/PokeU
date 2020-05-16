using PokeU.Model;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.View.ResourcesManager
{
    public class ChunkResourcesLoader
    {
        private static readonly int NB_MAX_CACHE_CHUNKS = 0;

        private HashSet<IntRect> loadedChunks;

        private List<IntRect> chunksInCache;

        private Dictionary<string, HashSet<IntRect>> pathToChunksDictionary;

        private Dictionary<IntRect, HashSet<string>> chunksToPathsDictionary;


        public ChunkResourcesLoader()
        {
            this.pathToChunksDictionary = new Dictionary<string, HashSet<IntRect>>();

            this.chunksToPathsDictionary = new Dictionary<IntRect, HashSet<string>>();

            this.loadedChunks = new HashSet<IntRect>();

            this.chunksInCache = new List<IntRect>();


        }

        public void LoadChunkResources(LandWorld2D world, ILandChunk landChunk)
        {
            /*for (int i = landChunk.AltitudeMin; i <= landChunk.AltitudeMax; i++)
            {
                this.LoadAltitudeResources(world, landChunk, i);
            }*/

            IntRect altitudeRect = new IntRect(landChunk.Area.Left, landChunk.Area.Top, 0, 0);

            if (this.loadedChunks.Contains(altitudeRect))
            {
                throw new Exception("Try to load an already loaded chunk");
            }

            if (chunksInCache.Contains(altitudeRect))
            {
                chunksInCache.Remove(altitudeRect);
            }
            else
            {
                HashSet<string> resourcesPath = new HashSet<string>();

                LandCase[,] landCases = landChunk.GetLandObjectsAtAltitude(altitudeRect.Width);

                HashSet<Type> landObjectTypes = landChunk.TypesInChunk;

                foreach (Type type in landObjectTypes)
                {
                    IEnumerable<string> resources = LandWorld2D.MappingObjectModelView[type].Resources.Keys;

                    foreach (string path in resources)
                    {
                        resourcesPath.Add(path);
                    }
                }

                HashSet<string> realResourcesToLoad = new HashSet<string>();
                foreach (string path in resourcesPath)
                {
                    if (this.pathToChunksDictionary.ContainsKey(path) == false)
                    {
                        this.pathToChunksDictionary.Add(path, new HashSet<IntRect>());

                        realResourcesToLoad.Add(path);
                    }

                    this.pathToChunksDictionary[path].Add(altitudeRect);
                }

                this.chunksToPathsDictionary.Add(altitudeRect, resourcesPath);

                if (realResourcesToLoad.Any())
                {
                    LandWorld2D.TextureManager.LoadTextures(realResourcesToLoad);
                }
            }
            loadedChunks.Add(altitudeRect);
        }

        public void UnloadChunkResources(LandWorld2D world, ILandChunk landChunk)
        {
            /*for (int i = landChunk.AltitudeMin; i <= landChunk.AltitudeMax; i++)
            {
                this.UnloadAltitudeResources(world, landChunk, i);
            }*/

            IntRect altitudeRect = new IntRect(landChunk.Area.Left, landChunk.Area.Top, 0, 0);

            if (this.loadedChunks.Contains(altitudeRect) == false)
            {
                throw new Exception("Try to unload a not loaded chunk");
            }

            this.chunksInCache.Add(altitudeRect);

            if (this.chunksInCache.Count > NB_MAX_CACHE_CHUNKS)
            {
                IntRect altitudeToRemove = this.chunksInCache.First();
                this.chunksInCache.RemoveAt(0);

                HashSet<string> pathsAltitudeToRemove = this.chunksToPathsDictionary[altitudeToRemove];
                HashSet<string> pathsToRemove = new HashSet<string>();

                foreach (string path in pathsAltitudeToRemove)
                {
                    HashSet<IntRect> altitudes = this.pathToChunksDictionary[path];
                    altitudes.Remove(altitudeRect);

                    if (altitudes.Any() == false)
                    {
                        pathsToRemove.Add(path);
                        this.pathToChunksDictionary.Remove(path);
                    }
                }

                this.chunksToPathsDictionary.Remove(altitudeToRemove);

                if (pathsToRemove.Any())
                {
                    LandWorld2D.TextureManager.UnloadTextures(pathsToRemove);
                }
            }

            this.loadedChunks.Remove(altitudeRect);
        }
    }
}
