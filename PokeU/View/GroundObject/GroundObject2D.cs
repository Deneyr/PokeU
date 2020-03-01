using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokeU.Model;
using PokeU.Model.GroundObject;
using SFML.Graphics;
using SFML.System;

namespace PokeU.View.GroundObject
{
    public class GroundObject2D : AObject2D, ILandObject2D
    {
        public Sprite SecondSprite
        {
            get;
            protected set;
        }

        public GroundObject2D(IObject2DFactory factory, GroundLandObject landObject)
        {
            Texture texture = factory.GetTextureByIndex((int)landObject.Type);


            /*switch(Math.Abs((int) (Math.PI * (landObject.Position.X + landObject.Position.Y))) % 4)
            {*/
            Random random = new Random(landObject.Position.X - landObject.Position.Y * landObject.Position.Y);
            switch (random.Next(0, 4))
            {
                case 0:
                    this.ObjectSprite = new Sprite(texture, new IntRect(2 * MainWindow.MODEL_TO_VIEW, 4 * MainWindow.MODEL_TO_VIEW, 2 * MainWindow.MODEL_TO_VIEW, 2 * MainWindow.MODEL_TO_VIEW));
                    break;
                case 1:
                    this.ObjectSprite = new Sprite(texture, new IntRect(0 * MainWindow.MODEL_TO_VIEW, 0 * MainWindow.MODEL_TO_VIEW, 2 * MainWindow.MODEL_TO_VIEW, 2 * MainWindow.MODEL_TO_VIEW));
                    break;
                case 2:
                    this.ObjectSprite = new Sprite(texture, new IntRect(2 * MainWindow.MODEL_TO_VIEW, 0 * MainWindow.MODEL_TO_VIEW, 2 * MainWindow.MODEL_TO_VIEW, 2 * MainWindow.MODEL_TO_VIEW));
                    break;
                case 3:
                    this.ObjectSprite = new Sprite(texture, new IntRect(4 * MainWindow.MODEL_TO_VIEW, 0 * MainWindow.MODEL_TO_VIEW, 2 * MainWindow.MODEL_TO_VIEW, 2 * MainWindow.MODEL_TO_VIEW));
                    break;
            }

            this.ObjectSprite.Scale = new Vector2f(0.5f, 0.5f);

            this.Position = new Vector2f(landObject.Position.X, landObject.Position.Y);

            this.SecondSprite = null;
            if (landObject.Type != landObject.SecondType && (int)landObject.Type < 3)
            {
                texture = factory.GetTextureByIndex((int)landObject.SecondType);
                this.SecondSprite = new Sprite(texture, this.GetTransitionTextureCoord(landObject.LandTransition));

                this.SecondSprite.Scale = this.ObjectSprite.Scale;

                this.SecondSprite.Position = this.ObjectSprite.Position;
            }
        }

        private IntRect GetTransitionTextureCoord(LandTransition landTransition)
        {
            IntRect result = new IntRect(0, 0, 1, 1);

            switch (landTransition)
            {
                case LandTransition.TOP:
                    result.Left = 1;
                    result.Top = 1;
                    break;
                case LandTransition.RIGHT:
                    result.Left = 2;
                    result.Top = 2;
                    break;
                case LandTransition.BOT:
                    result.Left = 1;
                    result.Top = 3;
                    break;
                case LandTransition.LEFT:
                    result.Left = 0;
                    result.Top = 2;
                    break;
                case LandTransition.TOP_LEFT:
                    result.Left = 2;
                    result.Top = 3;
                    break;
                case LandTransition.TOP_RIGHT:
                    result.Left = 0;
                    result.Top = 3;
                    break;
                case LandTransition.BOT_LEFT:
                    result.Left = 2;
                    result.Top = 1;
                    break;
                case LandTransition.BOT_RIGHT:
                    result.Left = 0;
                    result.Top = 1;
                    break;
                case LandTransition.TOP_INT_LEFT:
                    result.Left = 3;
                    result.Top = 0;
                    break;
                case LandTransition.TOP_INT_RIGHT:
                    result.Left = 3;
                    result.Top = 1;
                    break;
                case LandTransition.BOT_INT_LEFT:
                    result.Left = 3;
                    result.Top = 2;
                    break;
                case LandTransition.BOT_INT_RIGHT:
                    result.Left = 3;
                    result.Top = 3;
                    break;
            }

            result.Left *= 2 * MainWindow.MODEL_TO_VIEW;
            result.Top *= 2 * MainWindow.MODEL_TO_VIEW;
            result.Width *= 2 * MainWindow.MODEL_TO_VIEW;
            result.Height *= 2 * MainWindow.MODEL_TO_VIEW;

            return result;
        }

        public override void DrawIn(RenderWindow window, ref FloatRect boundsView)
        {
            base.DrawIn(window, ref boundsView);

            if(this.SecondSprite != null)
            {
                window.Draw(this.SecondSprite);
            }
        }
    }
}
