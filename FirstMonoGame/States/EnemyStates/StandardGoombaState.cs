using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstMonoGame.Interfaces;
using FirstMonoGame.States.EnemyStates;
using Microsoft.Xna.Framework.Content;
using FirstMonoGame.Sprites;
using Microsoft.Xna.Framework.Audio;


namespace FirstMonoGame.States.EnemyStates
{
    class StandardGoombaState : IEnemyState<Goomba>
    {
        private SoundEffect sound;
        public StandardGoombaState()
        {
        }

        public void BreatheFireTransition(Goomba enemy)
        {
            throw new NotImplementedException();
        }

        public void PunchingTransition(Goomba enemy)
        {
            throw new NotImplementedException();
        }

        public void StandardTransition(Goomba goomba)
        {
            var state = new StandardGoombaState();
            goomba.EnemyState = state;
        }

        public void TakeDamageTransition(Goomba goomba)
        {
            sound = goomba.Content.Load<SoundEffect>("Sound Effects/Stomp");
            sound.Play();
            var state = new DeadGoombaState();
            goomba.EnemyState = state;
            goomba.Sprite = GoombaSpriteFactory.Instance.FactoryMethod(goomba.Content, goomba, goomba.GraphicsDevice);
        }

        public void WalkingTransition(Goomba enemy)
        {
            throw new NotImplementedException();
        }
    }
}
