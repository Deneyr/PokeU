using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokeU.View.Animations;
using SFML.Graphics;
using SFML.System;

namespace PokeU.View
{
    public abstract class AObject2D : IObject2D
    {
        protected static AnimationManager animationManager;

        protected static ZoomAnimationManager zoomAnimationManager;

        private Sprite sprite;

        private List<IAnimation> animationsList;

        public Sprite ObjectSprite
        {
            get
            {
                return this.sprite;
            }

            protected set
            {
                this.sprite = value;
            }
        }

        public Vector2f Position
        {
            get
            {
                return this.ObjectSprite.Position;
            }

            protected set
            {
                this.ObjectSprite.Position = value * MainWindow.MODEL_TO_VIEW;
            }
        }

        static AObject2D()
        {
            AObject2D.animationManager = new AnimationManager();

            AObject2D.zoomAnimationManager = new ZoomAnimationManager();
        }

        public AObject2D()
        {
            this.sprite = new Sprite();

            this.animationsList = new List<IAnimation>();
        }

        public virtual void Dispose()
        {

        }

        public virtual void DrawIn(RenderWindow window, ref FloatRect boundsView)
        {
            window.Draw(this.ObjectSprite);
        }

        // Part animations.
        public static IntRect[] CreateAnimation(int leftStart, int topStart, int width, int height, int nbFrame)
        {
            IntRect[] result = new IntRect[nbFrame];

            for (int i = 0; i < nbFrame; i++)
            {
                result[i] = new IntRect(leftStart + i * width, topStart, width, height);
            }

            return result;
        }

        public void PlayAnimation(int index)
        {
            IAnimation animation = this.animationsList[index];

            if (animation is ZoomAnimation)
            {
                AObject2D.zoomAnimationManager.PlayAnimation(this, animation as ZoomAnimation);
            }
            else
            {
                AObject2D.animationManager.PlayAnimation(this, animation);
            }
        }

        public static void StopAnimationManager()
        {
            AObject2D.animationManager.Play = false;
        }

        public static void UpdateZoomAnimationManager(Time deltaTime)
        {
            AObject2D.zoomAnimationManager.Run(deltaTime);
        }

        public void SetCanevas(IntRect newCanevas)
        {
            this.sprite.TextureRect = newCanevas;
        }

        public void SetZoom(float newZoom)
        {
            this.sprite.Scale = new Vector2f(newZoom, newZoom);
        }
    }
}
