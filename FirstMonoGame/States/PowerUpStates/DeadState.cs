using Microsoft.Xna.Framework.Audio;

namespace FirstMonoGame.States2.PowerUpStates
{
    public class DeadState : IPowerUpState<Peach>
    {
        private SoundEffect deathNormal;
        
        public DeadState(Peach p)
        {
            deathNormal = p.Content.Load<SoundEffect>("Sound Effects/Peach Dies");
            p.SwitchToStandardSprite();
            p.Sprite.SwitchAnimation(5, 4);
            deathNormal.Play();
        }

        public void StandardTransition(Peach peach)
        {
            var state = new StandardState(this);
            peach.PowerUpState = state; // Standard
            peach.Sprite.SwitchAnimation(Constants.ZERO, 6); // Idle anim
            peach.Sprite.SetFrame(Constants.ZERO); // from start of anim
            peach.Sprite.LoopFrame = true; // loop anim
        }

        public void SuperTransition(Peach peach)
        {
            return;
        }

        public void StarTransition(Peach peach)
        {
            return;
        }

        public void FireTransition(Peach peach)
        {
            return;
        }

        public void TakeDamageTransition(Peach peach)
        {
            return;
        }

        public void DeadTransition(Peach peach)
        {
            return;
        }

        public void WarpTransition(Peach peach)
        {
            return;
        }
    }
}
