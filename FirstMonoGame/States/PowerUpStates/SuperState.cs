using FirstMonoGame.States.PeachStates;
using FirstMonoGame.States2.PeachStates;
using FirstMonoGame.States2.PowerUpStates;
using Microsoft.Xna.Framework.Audio;

namespace FirstMonoGame.States.PowerUpStates
{
    class SuperState : IPowerUpState<Peach>
    {
        private SoundEffect sound;
        public SuperState()
        {
        }

        public void FireTransition(Peach peach)
        {
            sound = peach.Content.Load<SoundEffect>("Sound Effects/Power Up");
            sound.Play();
            var state = new FireState(this);
            peach.PowerUpState = state;
            peach.SwitchToFireSprite();

        }

        public void StandardTransition(Peach peach)
        {
            var state = new StandardState(this);
            peach.PowerUpState = state;
            peach.SwitchToStandardSprite();
            if (peach.ActionState is CrouchingState)
            {
                peach.ActionState = new IdleState(peach);
                peach.Sprite = PeachSpriteFactory.Instance.FactoryMethod(peach);
            }
        }

        public void StarTransition(Peach peach)
        {
            sound = peach.Content.Load<SoundEffect>("Sound Effects/Power Up");
            sound.Play();
            var state = new StarState(this);
            peach.PowerUpState = state;
            peach.SwitchToStarSprite();
        }

        public void SuperTransition(Peach peach)
        {
            return;
        }

        public void TakeDamageTransition(Peach peach)
        {
            StandardTransition(peach);
        }

        public void DeadTransition(Peach peach)
        {
            var state = new DeadState(peach);
            peach.PowerUpState = state;
        }

        public void WarpTransition(Peach peach)
        {
            var state = new WarpingState();
            peach.PowerUpState = state;
        }
    }
}
