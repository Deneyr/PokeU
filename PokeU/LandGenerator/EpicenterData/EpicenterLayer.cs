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
        private int influenceRadius;

        private int probability;

        private float pointPower;

        private IntRect currentGeneratedArea;

        private DigressionMethod digressionMethod;

        private List<Vector2f> epicenterPoints;


        public EpicenterLayer(int influenceRadius, DigressionMethod digressionMethod, int probability, float pointPower)
        {
            this.influenceRadius = influenceRadius;

            this.digressionMethod = digressionMethod;

            this.probability = probability;

            this.pointPower = pointPower;

            this.InitializeGeneration();
        }

        public void InitializeGeneration()
        {
            this.currentGeneratedArea = new IntRect(0, 0, 0, 0);

            this.epicenterPoints = new List<Vector2f>();
        }

        public void GenerateEpicenterPoints(int seed, IntRect area)
        {
            this.InitializeGeneration();

            this.currentGeneratedArea = area;

            int nbAreasHeight = 2 * ((int) Math.Ceiling( (decimal)(this.influenceRadius / area.Width) )) + 1;
            int nbAreasWidth = 2 * ((int)Math.Ceiling( (decimal)(this.influenceRadius / area.Height) )) + 1;

            /*string folderName = "log_left_" + area.Left + "_top_" + area.Top;
            if (System.IO.Directory.Exists(folderName) == false)
            {
                System.IO.Directory.CreateDirectory(folderName);
            }*/

            for (int i = 0; i < nbAreasHeight; i++)
            {
                for (int j = 0; j < nbAreasWidth; j++)
                {
                    IntRect currentArea = new IntRect(area.Left - area.Width * (nbAreasWidth / 2 - j), area.Top - area.Height * (nbAreasHeight / 2 - i), area.Width, area.Height);

                    int areaSeed = seed + currentArea.Left + currentArea.Top * 1000 + this.influenceRadius * 5000;
                    // int areaSeed = seed * currentArea.Left * currentArea.Top;

                    Random random = new Random(areaSeed);

                    int chanceToAddPoint = random.Next(0, 100);

                    //List<Vector2f> epicenterPointsArea = new List<Vector2f>();
                    while(chanceToAddPoint < this.probability)
                    {
                        //epicenterPointsArea.Add(new Vector2f(currentArea.Left + random.Next(0, currentArea.Width), currentArea.Top + random.Next(0, currentArea.Height)));

                        this.epicenterPoints.Add(new Vector2f(currentArea.Left + random.Next(0, currentArea.Width), currentArea.Top + random.Next(0, currentArea.Height)));

                        chanceToAddPoint = random.Next(0, 100);
                    }

                    /*this.epicenterPoints.AddRange(epicenterPointsArea);

                    string fileName = "log_left_" + currentArea.Left + "_top_" + currentArea.Top + ".txt";
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(folderName + @"\" + fileName, true))
                    {
                        foreach (Vector2f vector in epicenterPointsArea)
                        {
                            file.WriteLine(vector.X + " : " + vector.Y + " : " + this.influenceRadius);
                        }
                    }*/

                    /*for(int counter = 0; counter < nbPoints; counter++)
                    {
                        this.epicenterPoints.Add(new Vector2f(currentArea.Left + random.Next(0, currentArea.Width), currentArea.Top + random.Next(0, currentArea.Height)));
                    }*/
                }
            }

        }

        public float GetPowerAt(Vector2f position)
        {
            float powerResult = 0;

            foreach(Vector2f point in this.epicenterPoints)
            {
                float norm = (float) Math.Sqrt((position.X - point.X) * (position.X - point.X) + (position.Y - point.Y) * (position.Y - point.Y));

                if (norm < this.influenceRadius)
                {
                    powerResult += this.GetPowerFromDistance(norm / this.influenceRadius);
                }
            }

            return powerResult;
        }

        private float GetPowerFromDistance(float distanceRatio)
        {
            switch (this.digressionMethod)
            {
                case DigressionMethod.LINEAR:
                    return (1 - distanceRatio) * this.pointPower;
                case DigressionMethod.SQUARE_ACC:
                    return (1 - distanceRatio) * (1 - distanceRatio) * this.pointPower;
                case DigressionMethod.SQUARE_DEC:
                    return (1 - distanceRatio * distanceRatio) * this.pointPower;
                case DigressionMethod.SMOOTH:
                    return (((1 - distanceRatio * distanceRatio) * this.pointPower) + ((1 - distanceRatio) * (1 - distanceRatio) * this.pointPower)) / 2;
            }

            return 0f;
        }

    }

    public enum DigressionMethod
    {
        LINEAR,
        SQUARE_ACC,
        SQUARE_DEC,
        SMOOTH
    }
}
