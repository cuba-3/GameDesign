using FirstMonoGame.States.PeachStates;
using FirstMonoGame.States.PowerUpStates;
using FirstMonoGame.States2.PeachStates;
using Microsoft.Xna.Framework.Audio;

namespace FirstMonoGame.States2.PowerUpStates
{
    public class StarState : IPowerUpState<Peach>
    {
        private IPowerUpState<Peach> PreviousState;
        private SoundEffect sound;
        public StarState(IPowerUpState<Peach> previousState)
        {
            PreviousState = previousState;
        }

        public void StarTransition(Peach peach)
        {
            return;
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

        public void SuperTransition(Peach peach)
        {
            sound = peach.Content.Load<SoundEffect>("Sound Effects/Power Up");
            sound.Play();
            var state = new SuperState();
            peach.PowerUpState = state;
            peach.SwitchToSuperSprite(PreviousState);
        }

        public void FireTransition(Peach peach)
        {
            sound = peach.Content.Load<SoundEffect>("Sound Effects/Power Up");
            sound.Play();
            var state = new FireState(this);
            peach.PowerUpState = state;
            peach.SwitchToFireSprite();
        }

        public void TakeDamageTransition(Peach peach)
        {
            return; // star peach is invincible, doesn't take damage
        }

        public void DeadTransition(Peach peach)
        {
            // TODO - death by timer?
        }

        public void WarpTransition(Peach peach)
        {
            var state = new WarpingState();
            peach.PowerUpState = state;
        }
    }

}
