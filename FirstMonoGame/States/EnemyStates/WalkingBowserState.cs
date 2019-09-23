using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstMonoGame.Entity;
using FirstMonoGame.Interfaces;
using Microsoft.Xna.Framework;

namespace FirstMonoGame.States.EnemyStates
{
    class WalkingBowserState : IEnemyState<Bowser>
    {

        public WalkingBowserState(Bowser bowser)
        {
            bowser.Sprite.Velocity = new Vector2(200,Constants.ZERO);
            if (bowser.Sprite.HReflect) bowser.Sprite.Velocity *= new Vector2(-1, 1);
        }

        public void BreatheFireTransition(Bowser bowser)
        {
            var state = new BreathingFireBowserState(bowser);
            bowser.EnemyState = state;
            bowser.Sprite = BowserSpriteFactory.Instance.FactoryMethod(bowser.Content, bowser, bowser.GraphicsDevice);
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
            return;
        }
    }
}
