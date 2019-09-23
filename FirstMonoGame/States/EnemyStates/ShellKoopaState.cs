using FirstMonoGame.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMonoGame.States.EnemyStates
{
    class ShellKoopaState : IEnemyState<KoopaTroopa>
    {
        public void BreatheFireTransition(KoopaTroopa enemy)
        {
            throw new NotImplementedException();
        }

        public void PunchingTransition(KoopaTroopa enemy)
        {
            throw new NotImplementedException();
        }

        public void StandardTransition(KoopaTroopa koopaTroopa)
        {
            var state = new StandardKoopaTroopaState();
            koopaTroopa.EnemyState = state;
            koopaTroopa.Sprite = KoopaTroopaSpriteFactory.Instance.FactoryMethod(koopaTroopa.Content, koopaTroopa, koopaTroopa.GraphicsDevice);
        }

        public void TakeDamageTransition(KoopaTroopa koopaTroopa)
        {
            var state = new DeadKoopaTroopaState();
            koopaTroopa.EnemyState = state;
        }

        public void WalkingTransition(KoopaTroopa enemy)
        {
            throw new NotImplementedException();
        }
    }
}
