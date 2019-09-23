using FirstMonoGame.Entity;
using FirstMonoGame.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace FirstMonoGame.States.EnemyStates
{
    class StandardBowserState : IEnemyState<Bowser>
    {
        public void StandardTransition(Bowser bowser)
        {
            bowser.Sprite.Velocity = new Vector2(Constants.ZERO,Constants.ZERO);
        }

        public void PunchingTransition(Bowser bowser)
        {
            var state = new PunchingBowserState();
            bowser.EnemyState = state;
            bowser.Sprite = BowserSpriteFactory.Instance.FactoryMethod(bowser.Content, bowser, bowser.GraphicsDevice);
        }

        public void WalkingTransition(Bowser bowser)
        {
            var state = new WalkingBowserState(bowser);
            bowser.EnemyState = state;
            bowser.Sprite = BowserSpriteFactory.Instance.FactoryMethod(bowser.Content, bowser, bowser.GraphicsDevice);
        }

        public void TakeDamageTransition(Bowser bowser)
        {
            Debug.WriteLine(" (Standard) Take damage");
            var state = bowser.EnemyState;
            if (bowser.Damage ==Constants.ZERO) state = new DeadBowserState();
            else state = new TakingDamageBowserState();
            bowser.EnemyState = state;
            bowser.Sprite = BowserSpriteFactory.Instance.FactoryMethod(bowser.Content, bowser, bowser.GraphicsDevice);
        }

        public void BreatheFireTransition(Bowser bowser)
        {
            var state = new BreathingFireBowserState(bowser);
            bowser.EnemyState = state;
            bowser.Sprite = BowserSpriteFactory.Instance.FactoryMethod(bowser.Content, bowser, bowser.GraphicsDevice);
        }
    }
}
