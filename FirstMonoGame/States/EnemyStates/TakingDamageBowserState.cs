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
    class TakingDamageBowserState : IEnemyState<Bowser>
    {
        public TakingDamageBowserState()
        {
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
            return;
        }

        public void WalkingTransition(Bowser bowser)
        {
            var state = new WalkingBowserState(bowser);
            bowser.EnemyState = state;
            bowser.Sprite = BowserSpriteFactory.Instance.FactoryMethod(bowser.Content, bowser, bowser.GraphicsDevice);
        }
    }
}
