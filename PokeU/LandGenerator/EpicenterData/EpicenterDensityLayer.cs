using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.LandGenerator.EpicenterData
{
    public class EpicenterDensityLayer: EpicenterLayer
    {

        public EpicenterDensityLayer(int influenceRadius, DigressionMethod digressionMethod, int probability, float pointPowerMin, float pointPowerMax) :
            base(influenceRadius, digressionMethod, probability, pointPowerMin, pointPowerMax)
        {

        }


        public override void GenerateEpicenterPoints(int seed, IntRect area)
        {
            this.InitializeGeneration();

            this.currentGeneratedArea = area;

            int nbAreasHeight = 2 * ((int)Math.Ceiling((decimal)(this.influenceRadius / area.Width))) + 3;
            int nbAreasWidth = 2 * ((int)Math.Ceiling((decimal)(this.influenceRadius / area.Height))) + 3;

            for (int i = 0; i < nbAreasHeight; i++)
            {
                for (int j = 0; j < nbAreasWidth; j++)
                {
                    IntRect currentArea = new IntRect(area.Left - area.Width * (nbAreasWidth / 2 - j), area.Top - area.Height * (nbAreasHeight / 2 - i), area.Width, area.Height);

                    int areaSeed = seed + currentArea.Left + currentArea.Top * 1000 + this.influenceRadius * 5000;

                    Random random = new Random(areaSeed);

                    int chanceToAddPoint = random.Next(0, 100);

                    for(int index = 0; index < this.probability; index++)
                    {
                        this.epicenterPoints.Add(new Tuple<Vector2f, float>(new Vector2f(currentArea.Left + random.Next(0, currentArea.Width), currentArea.Top + random.Next(0, currentArea.Height)),
                            (float)(random.NextDouble() * (this.pointPowerMax - this.pointPowerMin) + this.pointPowerMin)));
                    }
                }
            }

        }
    }
}
