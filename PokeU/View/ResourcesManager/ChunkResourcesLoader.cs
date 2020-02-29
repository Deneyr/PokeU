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
        private static readonly int NB_MAX_CACHE_ALTITUDE = 8;

        private HashSet<IntRect> loadedAltitudes;

        private List<IntRect> altitudesInCache;

        private Dictionary<string, HashSet<IntRect>> pathToAltitudesDictionary;

        private Dictionary<IntRect, HashSet<string>> altitudesToPathsDictionary;


        public ChunkResourcesLoader()
        {
            this.pathToAltitudesDictionary = new Dictionary<string, HashSet<IntRect>>();

            this.altitudesToPathsDictionary = new Dictionary<IntRect, HashSet<string>>();

            this.loadedAltitudes = new HashSet<IntRect>();

            this.altitudesInCache = new List<IntRect>();


        }

        public void LoadChunkResources(LandWorld2D world, ILandChunk landChunk)
        {
            for (int i = landChunk.AltitudeMin; i <= landChunk.AltitudeMax; i++)
            {
                this.LoadAltitudeResources(world, landChunk, i);
            }
        }

        public void UnloadChunkResources(LandWorld2D world, ILandChunk landChunk)
        {
            for (int i = landChunk.AltitudeMin; i <= landChunk.AltitudeMax; i++)
            {
                this.UnloadAltitudeResources(world, landChunk, i);
            }
        }

        public void LoadAltitudeResources(LandWorld2D world, ILandChunk landChunk, int altitude)
        {
            IntRect altitudeRect = new IntRect(landChunk.Area.Left, landChunk.Area.Top, altitude, altitude);

            if (this.loadedAltitudes.Contains(altitudeRect))
            {
                throw new Exception("Try to load an already loaded altitude");
            }

            if (altitudesInCache.Contains(altitudeRect))
            {
                altitudesInCache.Remove(altitudeRect);
            }
            else
            {
                HashSet<string> resourcesPath = new HashSet<string>();
                HashSet<ILandObject> objects = new HashSet<ILandObject>();

                List<ILandObject> landObjects = landChunk.GetLandObjectsAtAltitude(altitudeRect.Width);
                foreach (ILandObject landObject in landObjects)
                {
                    if (objects.Contains(landObject) == false)
                    {
                        IEnumerable<string> resources = LandWorld2D.MappingObjectModelView[landObject.GetType()].Resources.Keys;

                        foreach (string path in resources)
                        {
                            resourcesPath.Add(path);
                        }

                        objects.Add(landObject);
                    }
                }

                HashSet<string> realResourcesToLoad = new HashSet<string>();
                foreach (string path in resourcesPath)
                {
                    if (this.pathToAltitudesDictionary.ContainsKey(path) == false)
                    {
                        this.pathToAltitudesDictionary.Add(path, new HashSet<IntRect>());

                        realResourcesToLoad.Add(path);
                    }

                    this.pathToAltitudesDictionary[path].Add(altitudeRect);
                }

                this.altitudesToPathsDictionary.Add(altitudeRect, resourcesPath);

                if (realResourcesToLoad.Any())
                {
                    LandWorld2D.TextureManager.LoadTextures(realResourcesToLoad);
                }
            }
            loadedAltitudes.Add(altitudeRect);
        }

        public void UnloadAltitudeResources(LandWorld2D world, ILandChunk landChunk, int altitude)
        {
            IntRect altitudeRect = new IntRect(landChunk.Area.Left, landChunk.Area.Top, altitude, altitude);

            if (this.loadedAltitudes.Contains(altitudeRect) == false)
            {
                throw new Exception("Try to unload a not loaded altitude");
            }

            this.altitudesInCache.Add(altitudeRect);

            if(this.altitudesInCache.Count > NB_MAX_CACHE_ALTITUDE)
            {
                IntRect altitudeToRemove = this.altitudesInCache.First();
                this.altitudesInCache.RemoveAt(0);

                HashSet<string> pathsAltitudeToRemove = this.altitudesToPathsDictionary[altitudeToRemove];
                HashSet<string> pathsToRemove = new HashSet<string>();

                foreach(string path in pathsAltitudeToRemove)
                {
                    HashSet<IntRect> altitudes = this.pathToAltitudesDictionary[path];
                    altitudes.Remove(altitudeRect);

                    if (altitudes.Any() == false)
                    {
                        pathsToRemove.Add(path);
                        this.pathToAltitudesDictionary.Remove(path);
                    }
                }

                this.altitudesToPathsDictionary.Remove(altitudeToRemove);

                if (pathsToRemove.Any())
                {
                    LandWorld2D.TextureManager.UnloadTextures(pathsToRemove);
                }
            }

            this.loadedAltitudes.Remove(altitudeRect);
        }
    }
}
