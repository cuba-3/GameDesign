using FirstMonoGame.States.PeachStates;
using FirstMonoGame.States.PowerUpStates;
using FirstMonoGame.States2.PeachStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace FirstMonoGame.States2.PowerUpStates
{
    public class FireState : IPowerUpState<Peach>
    {
        private IPowerUpState<Peach> PreviousState;
        private SoundEffect sound;
        public FireState(IPowerUpState<Peach> previousState)
        {
            PreviousState = previousState;
        }

        public void FireTransition(Peach peach)
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
            sound = peach.Content.Load<SoundEffect>("Sound Effects/Power Up");
            sound.Play();
            var state = new SuperState();
            peach.PowerUpState = state;
            peach.SwitchToSuperSprite(PreviousState);
        }

        public void TakeDamageTransition(Peach peach)
        {
            var state = new StandardState(this);
            peach.PowerUpState = state;
            peach.SwitchToStandardSprite();
        }

        public void DeadTransition(Peach peach)
        {
            sound = peach.Content.Load<SoundEffect>("Sound Effects/Peach Dies");
            sound.Play();
            var state = new StandardState(this);
            peach.PowerUpState = state;
            peach.SwitchToStandardSprite();
            peach.Sprite.SwitchAnimation(5, 4);
        }

        public void WarpTransition(Peach peach)
        {
            var state = new WarpingState();
            peach.PowerUpState = state;
        }
    }
}
