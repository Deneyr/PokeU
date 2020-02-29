﻿using PokeU.Model;
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
    }
}
