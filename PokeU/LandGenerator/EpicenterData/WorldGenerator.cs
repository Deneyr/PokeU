using PokeU.Model;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.LandGenerator.EpicenterData
{
    public class WorldGenerator
    {
        private SortedList<int, ALandLayerGenerator> generatorsSortedList;

        private Dictionary<string, ALandLayerGenerator> generatorsDictionary;

        private Vector2f temperatureVector;

        private Vector2f positionZero;

        private int seed;

        public int Seed
        {
            get
            {
                return this.seed;
            }
        }

        public Dictionary<string, ALandLayerGenerator> Generators
        {
            get
            {
                return this.generatorsDictionary;
            }
        }
            

        public WorldGenerator(int seed, Vector2f temperatureVector, Vector2f positionZero)
        {
            this.generatorsSortedList = new SortedList<int, ALandLayerGenerator>();

            this.generatorsDictionary = new Dictionary<string, ALandLayerGenerator>();

            this.temperatureVector = temperatureVector;

            this.positionZero = positionZero;

            this.seed = seed;
        }

        public float GetGlobalTemperatureAt(Vector2f position)
        {
            Vector2f diffVector = position - this.positionZero;
            float component = diffVector.X * this.temperatureVector.X + diffVector.Y * this.temperatureVector.Y;

            return component;
        }

        public void AddGenerator(int generatorPriority, ALandLayerGenerator generator)
        {
            this.generatorsSortedList.Add(generatorPriority, generator);

            this.generatorsDictionary.Add(generator.Name, generator);
        }

        public void GenerateEpicenterChunk(IntRect area)
        {
            foreach (ALandLayerGenerator generator in this.generatorsSortedList.Values)
            {
                generator.GenerateEpicenterLayer(this.seed, area);
            }
        }

        public LandChunk GenerateLandChunk(IntRect area, int minAltitude, int maxAltitude)
        {
            LandChunk landChunk = new LandChunk(minAltitude, maxAltitude, area);

            //List<ILandLayer> landLayers = new List<ILandLayer>();
            foreach (ALandLayerGenerator generator in this.generatorsSortedList.Values)
            {
                generator.GenerateLandLayer(this, landChunk, area, minAltitude, maxAltitude);

                //if (landLayer != null)
                //{
                //    landLayers.Add(landLayer);
                //}
            }

            //landChunk.ComputeObjectsArray(landLayers);

            return landChunk;
        }

    }
}
