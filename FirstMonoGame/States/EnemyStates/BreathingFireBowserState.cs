using FirstMonoGame.Collision;
using FirstMonoGame.Entity;
using FirstMonoGame.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMonoGame.States.EnemyStates
{
    class BreathingFireBowserState : IEnemyState<Bowser>
    {
        public BreathingFireBowserState(Bowser b)
        {
            FireBar fireBar = new FireBar(b.GraphicsDevice, b.Content);
            fireBar.Sprite.CurrentLocation = b.Sprite.CurrentLocation + new Vector2(Constants.ZERO, 30);
            fireBar.Sprite.Velocity = new Vector2(-200,Constants.ZERO);
            if (!b.Sprite.HReflect)
            {
                fireBar.Sprite.HReflect = true;
                fireBar.Sprite.Velocity *= new Vector2(-1, 1);
                fireBar.Sprite.CurrentLocation += new Vector2(128,Constants.ZERO);
            }
            fireBar.Checkable = true;
            Collections.Instance.GetCollisionRef().ReRegister(fireBar);
            Collections.Instance.GetLevelRef().Entities.Add(fireBar);
        }

        public void BreatheFireTransition(Bowser enemy)
        {
            return;
        }

        public void PunchingTransition(Bowser bowser)
        {
            var state = new PunchingBowserState();
            bowser.EnemyState = state;
            bowser.Sprite = BowserSpriteFactory.Instance.FactoryMethod(bowser.Content, bowser, bowser.GraphicsDevice);
        }

        public void StandardTransition(Bowser bowser)
        {
            var state = new StandardBowserState();
            bowser.EnemyState = state;
            bowser.Sprite = BowserSpriteFactory.Instance.FactoryMethod(bowser.Content, bowser, bowser.GraphicsDevice);
        }

        public void TakeDamageTransition(Bowser bowser)
        {
            var state = bowser.EnemyState;
            if (bowser.Damage ==Constants.ZERO) state = new DeadBowserState();
            else state = new TakingDamageBowserState();
            bowser.EnemyState = state;
            bowser.Sprite = BowserSpriteFactory.Instance.FactoryMethod(bowser.Content, bowser, bowser.GraphicsDevice);
        }

        public void WalkingTransition(Bowser bowser)
        {
            var state = new WalkingBowserState(bowser);
            bowser.EnemyState = state;
            bowser.Sprite = BowserSpriteFactory.Instance.FactoryMethod(bowser.Content, bowser, bowser.GraphicsDevice);
        }
    }
}
