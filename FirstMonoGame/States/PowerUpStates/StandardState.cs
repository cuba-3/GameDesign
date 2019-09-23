using FirstMonoGame.States.PowerUpStates;
using FirstMonoGame.States2.PeachStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace FirstMonoGame.States2.PowerUpStates
{
    public class StandardState : IPowerUpState<Peach>
    {
        IPowerUpState<Peach> PreviousState;
        private SoundEffect sound;
        public StandardState(IPowerUpState<Peach> previousState)
        {
           PreviousState = previousState;
        }

        public void FireTransition(Peach peach)
        {
            sound = peach.Content.Load<SoundEffect>("Sound Effects/Power Up");
            sound.Play();
            var state = new SuperState();
            peach.PowerUpState = state;
            peach.SwitchToSuperSprite(PreviousState);
        }

        public void StandardTransition(Peach peach)
        {
            return;
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
            sound = peach.Content.Load<SoundEffect>("Sound Effects/Peach Dies");
            sound.Play();
            var state = new DeadState(peach);
            peach.PowerUpState = state;
            peach.PlayerStats.LoseLife();
            var actState = new IdleState(peach); // reduce checking for dead state by making action state idle upon death -> switch anim on respawn
            peach.ActionState = actState;
            peach.Sprite.SwitchAnimation(5, 4); //dead animation
            peach.Sprite.Velocity = new Vector2(Constants.ZERO,Constants.ZERO); // if dead, can't move!
            peach.Sprite.HReflect = false; // no reflect anim
            peach.Sprite.SetFrame(Constants.ZERO); // from start of anim
            peach.Sprite.LoopFrame = true; // no loop anim
        }
        public void WarpTransition(Peach peach)
        {
            var state = new WarpingState();
            peach.PowerUpState = state;
        }
        public void DeadTransition(Peach peach)
        {
            var state = new DeadState(peach);
            peach.PowerUpState = state;
        }
    }
}
