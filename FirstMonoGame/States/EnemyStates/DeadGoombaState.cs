using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstMonoGame.Interfaces;
using Microsoft.Xna.Framework.Audio;

namespace FirstMonoGame.States.EnemyStates
{
    class DeadGoombaState : IEnemyState<Goomba>
    {
        public DeadGoombaState() 
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
            var state = new DeadGoombaState();
            goomba.Sprite = GoombaSpriteFactory.Instance.FactoryMethod(goomba.Content, goomba, goomba.GraphicsDevice);
            goomba.EnemyState = state;
        }

        public void WalkingTransition(Goomba enemy)
        {
            throw new NotImplementedException();
        }
    }
}
