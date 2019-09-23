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
    class PunchingBowserState : IEnemyState<Bowser>
    {

        public PunchingBowserState()
        {
        }

        public void StandardTransition(Bowser bowser)
        {
            var state = new StandardBowserState();
            bowser.Sprite.Velocity = new Vector2(Constants.ZERO,Constants.ZERO);
            bowser.EnemyState = state;
            bowser.Sprite = BowserSpriteFactory.Instance.FactoryMethod(bowser.Content, bowser, bowser.GraphicsDevice);
        }

        public void PunchingTransition(Bowser enemy)
        {
            return;
        }

        //public static void FireTransition(Bowser bowser)
        //{
        //    var state = new BreathingFireBowserState(bowser);
        //    bowser.EnemyState = state;
        //    bowser.Sprite = BowserSpriteFactory.Instance.FactoryMethod(bowser.Content, bowser, bowser.GraphicsDevice);
        //}

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

        public void BreatheFireTransition(Bowser bowser)
        {
            var state = new BreathingFireBowserState(bowser);
            bowser.EnemyState = state;
            bowser.Sprite = BowserSpriteFactory.Instance.FactoryMethod(bowser.Content, bowser, bowser.GraphicsDevice);
        }
    }
}
