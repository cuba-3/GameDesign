using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstMonoGame.Interfaces;
using Microsoft.Xna.Framework.Audio;

namespace FirstMonoGame.States.EnemyStates
{
    class DeadKoopaTroopaState : IEnemyState<KoopaTroopa>
    {
        SoundEffect deadKoopaTroopa;
        public DeadKoopaTroopaState()
        {
        }

        public void BreatheFireTransition(KoopaTroopa enemy)
        {
            throw new NotImplementedException();
        }

        public void PunchingTransition(KoopaTroopa enemy)
        {
            throw new NotImplementedException();
        }

        public void StandardTransition(KoopaTroopa koopatroopa)
        {
            var state = new StandardKoopaTroopaState();
            koopatroopa.EnemyState = state;
            koopatroopa.Sprite = KoopaTroopaSpriteFactory.Instance.FactoryMethod(koopatroopa.Content, koopatroopa, koopatroopa.GraphicsDevice);
        }
        public void TakeDamageTransition(KoopaTroopa koopatroopa)
        {
            deadKoopaTroopa = koopatroopa.Content.Load<SoundEffect>("Sound Effects/Stomp");
            deadKoopaTroopa.Play();
            var state = new DeadKoopaTroopaState();
            koopatroopa.EnemyState = state;
        }

        public void WalkingTransition(KoopaTroopa enemy)
        {
            throw new NotImplementedException();
        }
    }
}
