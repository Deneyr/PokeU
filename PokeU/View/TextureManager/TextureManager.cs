using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.View.TextureManager
{
    public class TextureManager
    {
        private Dictionary<string, Texture> texturesDictionary;

        public TextureManager()
        {
            this.texturesDictionary = new Dictionary<string, Texture>();
        }

        public Texture GetTexture(string path)
        {
            return this.texturesDictionary[path];
        }

        public void LoadTextures(HashSet<string> texturesToLoad)
        {
            HashSet<string> textureNotToLoad = new HashSet<string>();
            List<string> textureToRemove = new List<string>();

            foreach(KeyValuePair<string, Texture> keyValuePair in this.texturesDictionary)
            {
                if (texturesToLoad.Contains(keyValuePair.Key) == false)
                {
                    keyValuePair.Value.Dispose();

                    textureToRemove.Add(keyValuePair.Key);
                }
                else
                {
                    textureNotToLoad.Add(keyValuePair.Key);
                }
            }

            foreach(string keyToRemove in textureToRemove)
            {
                this.texturesDictionary.Remove(keyToRemove);
            }

            foreach(string pathToLoad in texturesToLoad)
            {
                if (textureNotToLoad.Contains(pathToLoad) == false)
                {
                    this.texturesDictionary.Add(pathToLoad, new Texture(pathToLoad));
                }
            }
        }

    }
}
