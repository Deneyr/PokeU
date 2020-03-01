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
    public abstract class ALandLayerGenerator
    {
        private List<EpicenterLayer> epicenterLayersList;

        public ALandLayerGenerator()
        {
            this.epicenterLayersList = new List<EpicenterLayer>();
        }

        protected abstract void InitializeGenerator();

        protected void AddEpicenterLayer(int influenceRadius, DigressionMethod digressionMethod, int nbMaxPoints, float pointPower)
        {
            this.epicenterLayersList.Add(new EpicenterLayer(influenceRadius, digressionMethod, nbMaxPoints, pointPower));
        }

        public virtual void GenerateEpicenterLayer(int seed, IntRect area)
        {
            foreach (EpicenterLayer layer in this.epicenterLayersList)
            {
                layer.GenerateEpicenterPoints(seed, area);
            }
        }

        public abstract ILandLayer GenerateLandLayer(WorldGenerator worldGenerator, IntRect area, int minAltitude, int maxAltitude);

        protected float GetPowerAt(Vector2f position)
        {
            float powerResult = 0;

            foreach(EpicenterLayer layer in this.epicenterLayersList)
            {
                powerResult += layer.GetPowerAt(position);
            }

            return powerResult; 
        }

        protected static LandTransition GetLandTransitionFrom(ref bool[,] array)
        {
            // Sides check
            if (array[1,0])
            {
                if (array[0,1])
                {
                    return LandTransition.BOT_INT_RIGHT;
                }
                else if (array[2,1])
                {
                    return LandTransition.TOP_INT_RIGHT;
                }
                else
                {
                    return LandTransition.RIGHT;
                }
            }

            if (array[0,1])
            {
                if (array[1,0])
                {
                    return LandTransition.BOT_INT_RIGHT;
                }
                else if (array[1,2])
                {
                    return LandTransition.BOT_INT_LEFT;
                }
                else
                {
                    return LandTransition.BOT;
                }
            }

            if (array[1,2])
            {
                if (array[0,1])
                {
                    return LandTransition.BOT_INT_LEFT;
                }
                else if (array[2,1])
                {
                    return LandTransition.TOP_INT_LEFT;
                }
                else
                {
                    return LandTransition.LEFT;
                }
            }

            if (array[2,1])
            {
                if (array[1,0])
                {
                    return LandTransition.TOP_INT_RIGHT;
                }
                else if (array[1,2])
                {
                    return LandTransition.TOP_INT_LEFT;
                }
                else
                {
                    return LandTransition.TOP;
                }
            }

            //Corners check
            if (array[0,0])
            {
                return LandTransition.TOP_LEFT;
            }

            if (array[0,2])
            {
                return LandTransition.TOP_RIGHT;
            }

            if (array[2,2])
            {
                return LandTransition.BOT_RIGHT;
            }

            if (array[2,0])
            {
                return LandTransition.BOT_LEFT;
            }

            return LandTransition.NONE;
        }
    }
}
