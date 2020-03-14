using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.LandGenerator.EpicenterData
{
    public class EpicenterAreaLayer: EpicenterLayer
    {
        public EpicenterAreaLayer(int influenceRadius, DigressionMethod digressionMethod, int probability, float pointPowerMin, float pointPowerMax):
            base(influenceRadius, digressionMethod, probability, pointPowerMin, pointPowerMax)
        {

        }

        public override void GenerateEpicenterPoints(int seed, IntRect area)
        {
            this.InitializeGeneration();

            this.currentGeneratedArea = area;

            int nbAreasHeight = 2 * ((int)Math.Ceiling((decimal)(this.influenceRadius / area.Width))) + 1;
            int nbAreasWidth = 2 * ((int)Math.Ceiling((decimal)(this.influenceRadius / area.Height))) + 1;

            for (int i = 0; i < nbAreasHeight; i++)
            {
                for (int j = 0; j < nbAreasWidth; j++)
                {
                    IntRect currentArea = new IntRect(area.Left - area.Width * (nbAreasWidth / 2 - j), area.Top - area.Height * (nbAreasHeight / 2 - i), area.Width, area.Height);

                    int areaSeed = seed + currentArea.Left + currentArea.Top * 1000 + this.influenceRadius * 5000;

                    Random random = new Random(areaSeed);

                    int chanceToAddPoint = random.Next(0, 100);

                    while (chanceToAddPoint < this.probability)
                    {
                        this.epicenterPoints.Add(new Tuple<Vector2f, float>(new Vector2f(currentArea.Left + random.Next(0, currentArea.Width), currentArea.Top + random.Next(0, currentArea.Height)),
                            random.Next((int) this.pointPowerMin, ((int) this.pointPowerMax) + 1)));

                        chanceToAddPoint = random.Next(0, 100);
                    }
                }
            }
        }

        public override float GetPowerAt(Vector2f position)
        {
            int powerMin = (int)this.pointPowerMin;
            int powerMax = (int)this.pointPowerMax;
            Dictionary<int, float> dictionaryInfluence = new Dictionary<int, float>();
            for(int i = powerMin; i <= powerMax; i++)
            {
                dictionaryInfluence.Add(i, 0);
            }

            foreach (Tuple<Vector2f, float> tuplePoint in this.epicenterPoints)
            {
                float norm = (float)Math.Sqrt((position.X - tuplePoint.Item1.X) * (position.X - tuplePoint.Item1.X) + (position.Y - tuplePoint.Item1.Y) * (position.Y - tuplePoint.Item1.Y));

                if (norm < this.influenceRadius)
                {
                    dictionaryInfluence[(int)tuplePoint.Item2] += this.GetPowerFromDistance(norm, 1);
                }
            }

            int lResult = (int)this.pointPowerMin;
            float influence = dictionaryInfluence[0];
            for (int i = powerMin; i <= powerMax; i++)
            {
                if(dictionaryInfluence[i] > influence)
                {
                    lResult = i;
                    influence = dictionaryInfluence[i];
                }
            }

            return lResult;
        }

    }
}
