using PokeU.Model.Entity;
using PokeU.Model.Entity.Ability;
using PokeU.View.Animations;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeU.View.Entity2D
{
    public class PlayerEntity2D: ACharacterEntity2D
    {
        public PlayerEntity2D(IObject2DFactory factory, PlayerEntity playerEntity) :
            base(factory, playerEntity)
        {
            Texture texture = factory.GetTextureByIndex(0);

            IntRect[] walkTop = new IntRect[]
            {
                new IntRect(2 * 32, 0 * 32, 32, 32),
                new IntRect(11 * 32, 5 * 32, 32, 32)
            };

            Animation animationWalkTop = new Animation(walkTop, Time.FromMilliseconds(250), AnimationType.LOOP);

            IntRect[] walkBot = new IntRect[]
            {
                new IntRect(2 * 32, 2 * 32, 32, 32),
                new IntRect(2 * 32, 3 * 32, 32, 32)
            };

            Animation animationWalkBot = new Animation(walkBot, Time.FromMilliseconds(250), AnimationType.LOOP);

            IntRect[] walkRight = new IntRect[]
            {
                new IntRect(1 * 32, 2 * 32, 32, 32),
                new IntRect(1 * 32, 3 * 32, 32, 32)
            };

            Animation animationWalkRight = new Animation(walkRight, Time.FromMilliseconds(250), AnimationType.LOOP);

            IntRect[] walkLeft = new IntRect[]
            {
                new IntRect(0 * 32, 3 * 32, 32, 32),
                new IntRect(1 * 32, 0 * 32, 32, 32)
            };

            Animation animationWalkLeft = new Animation(walkLeft, Time.FromMilliseconds(250), AnimationType.LOOP);


            IntRect[] idleTop = new IntRect[]
           {
                new IntRect(9 * 32, 4 * 32, 32, 32)
           };

            Animation animationIdleTop = new Animation(idleTop, Time.FromMilliseconds(250), AnimationType.LOOP);

            IntRect[] idleBot = new IntRect[]
            {
                new IntRect(2 * 32, 1 * 32, 32, 32)
            };

            Animation animationIdleBot = new Animation(idleBot, Time.FromMilliseconds(250), AnimationType.LOOP);

            IntRect[] idleRight = new IntRect[]
            {
                new IntRect(10 * 32, 4 * 32, 32, 32)
            };

            Animation animationIdleRight = new Animation(idleRight, Time.FromMilliseconds(250), AnimationType.LOOP);

            IntRect[] idleLeft = new IntRect[]
            {
                new IntRect(9 * 32, 6 * 32, 32, 32)
            };

            Animation animationIdleLeft = new Animation(idleLeft, Time.FromMilliseconds(250), AnimationType.LOOP);

            this.animationsList.Add(animationIdleTop);
            this.animationsList.Add(animationIdleBot);
            this.animationsList.Add(animationIdleRight);
            this.animationsList.Add(animationIdleLeft);

            this.animationsList.Add(animationWalkTop);
            this.animationsList.Add(animationWalkBot);
            this.animationsList.Add(animationWalkRight);
            this.animationsList.Add(animationWalkLeft);

            this.ObjectSprite = new Sprite(texture, new IntRect(2 * 32, 1 * 32, 32, 32));

            //this.ObjectSprite.Scale = new Vector2f(0.5f, 0.5f);

            this.Position = new Vector2f(playerEntity.Position.X, playerEntity.Position.Y);

            WalkKinematicAbility walkKinematicAbility = this.characterEntity.GetAbilityById("kinematic.walk") as WalkKinematicAbility;
            walkKinematicAbility.MovingStateUpdated += OnMovingStateUpdated;
        }

        private void OnMovingStateUpdated(AKinematicAbility obj)
        {
            this.PlayAnimation((int)obj.MovingState);
        }

        public override void Dispose()
        {
            WalkKinematicAbility walkKinematicAbility = this.characterEntity.GetAbilityById("kinematic.walk") as WalkKinematicAbility;
            walkKinematicAbility.MovingStateUpdated -= OnMovingStateUpdated;
        }
    }
}
