using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.LandGenerator.EpicenterData
{
    public class EpicenterLayer
    {
        protected int influenceRadius;

        protected int probability;

        protected float pointPowerMin;

        protected float pointPowerMax;

        protected IntRect currentGeneratedArea;

        protected DigressionMethod digressionMethod;

        protected List<Tuple<Vector2f, float>> epicenterPoints;


        public EpicenterLayer(int influenceRadius, DigressionMethod digressionMethod, int probability, float pointPowerMin, float pointPowerMax)
        {
            this.influenceRadius = influenceRadius;

            this.digressionMethod = digressionMethod;

            this.probability = probability;

            this.pointPowerMin = pointPowerMin;

            this.pointPowerMax = pointPowerMax;

            this.InitializeGeneration();
        }

        public void InitializeGeneration()
        {
            this.currentGeneratedArea = new IntRect(0, 0, 0, 0);

            this.epicenterPoints = new List<Tuple<Vector2f, float>>();
        }

        public virtual void GenerateEpicenterPoints(int seed, IntRect area)
        {
            this.InitializeGeneration();

            this.currentGeneratedArea = area;

            int nbAreasHeight = 2 * ((int) Math.Ceiling( (decimal)(this.influenceRadius / area.Width) )) + 3;
            int nbAreasWidth = 2 * ((int)Math.Ceiling( (decimal)(this.influenceRadius / area.Height) )) + 3;

            for (int i = 0; i < nbAreasHeight; i++)
            {
                for (int j = 0; j < nbAreasWidth; j++)
                {
                    IntRect currentArea = new IntRect(area.Left - area.Width * (nbAreasWidth / 2 - j), area.Top - area.Height * (nbAreasHeight / 2 - i), area.Width, area.Height);

                    int areaSeed = seed + currentArea.Left + currentArea.Top * 1000 + this.influenceRadius * 5000;

                    Random random = new Random(areaSeed);

                    int chanceToAddPoint = random.Next(0, 100);

                    while(chanceToAddPoint < this.probability)
                    {
                        this.epicenterPoints.Add(new Tuple<Vector2f, float>(new Vector2f(currentArea.Left + random.Next(0, currentArea.Width), currentArea.Top + random.Next(0, currentArea.Height)), 
                            (float) (random.NextDouble() * (this.pointPowerMax - this.pointPowerMin) + this.pointPowerMin)));

                        chanceToAddPoint = random.Next(0, 100);
                    }
                }
            }

        }

        public virtual float GetPowerAt(Vector2f position)
        {
            float powerResult = 0;

            foreach(Tuple<Vector2f, float> tuplePoint in this.epicenterPoints)
            {
                float norm = (float) Math.Sqrt((position.X - tuplePoint.Item1.X) * (position.X - tuplePoint.Item1.X) + (position.Y - tuplePoint.Item1.Y) * (position.Y - tuplePoint.Item1.Y));

                if (norm < this.influenceRadius)
                {
                    powerResult += this.GetPowerFromDistance(norm / this.influenceRadius, tuplePoint.Item2);
                }
            }

            return powerResult;
        }

        protected float GetPowerFromDistance(float distanceRatio, float pointPower)
        {
            switch (this.digressionMethod)
            {
                case DigressionMethod.CONSTANT:
                    return pointPower;
                case DigressionMethod.LINEAR:
                    return (1 - distanceRatio) * pointPower;
                case DigressionMethod.SQUARE_ACC:
                    return (1 - distanceRatio) * (1 - distanceRatio) * pointPower;
                case DigressionMethod.SQUARE_DEC:
                    return (1 - distanceRatio * distanceRatio) * pointPower;
                case DigressionMethod.SMOOTH:
                    return (((1 - distanceRatio * distanceRatio) * pointPower) + ((1 - distanceRatio) * (1 - distanceRatio) * pointPower)) / 2;
                case DigressionMethod.CIRCLE:

                    distanceRatio = 1 - Math.Abs(distanceRatio * 2 - 1);

                    return (((1 - distanceRatio * distanceRatio) * pointPower) + ((1 - distanceRatio) * (1 - distanceRatio) * pointPower)) / 2;
            }

            return 0f;
        }

    }

    public enum DigressionMethod
    {
        CONSTANT,
        LINEAR,
        SQUARE_ACC,
        SQUARE_DEC,
        SMOOTH,
        CIRCLE
    }
}
