using PokeU.Model;
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
        private Dictionary<ILandChunk, LandChunk2D> landChunksDictionary;

        public LandWorld2D(LandWorld landWorld)
        {
            this.landChunksDictionary = new Dictionary<ILandChunk, LandChunk2D>();

            landWorld.ChunkAdded += OnChunkAdded;
            landWorld.ChunkRemoved += OnChunkRemoved;
        }

        public void DrawIn(RenderWindow window)
        {
            foreach (LandChunk2D landChunk2D in this.landChunksDictionary.Values)
            {
                window.Draw(landChunk2D.ObjectSprite);
            }
        }

        private void OnChunkAdded(ILandChunk obj)
        {
            this.landChunksDictionary.Add(obj, new LandChunk2D(obj, 0, 0));
        }


        private void OnChunkRemoved(ILandChunk obj)
        {
            this.landChunksDictionary.Remove(obj);
        }

    }
}
