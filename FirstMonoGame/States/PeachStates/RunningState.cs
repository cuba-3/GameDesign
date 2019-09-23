using FirstMonoGame.States2.PeachStates;
using FirstMonoGame.States2.PowerUpStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace FirstMonoGame.States.PeachStates
{
    class RunningState : IActionState<Peach>
    {
        private SoundEffect sound;
        private Peach peach;
        public RunningState(Peach p)
        {
            peach = p;
            peach.Sprite.Velocity = new Vector2(360, peach.Sprite.Velocity.Y);
            if (peach.Sprite.HReflect) peach.Sprite.Velocity *= new Vector2(-1, 1);
        }
        //Run -> walk, die, jump, idle, run
        public void IdleStateTransition()
        {
            var state = new IdleState(peach);
            peach.ActionState = state;
            peach.Sprite = PeachSpriteFactory.Instance.FactoryMethod(peach);
        }

        public void CrouchingStateTransition()
        {
            return;
        }

        public void FallingStateTransition()
        {
            var state = new FallingState(peach);
            peach.ActionState = state;
            peach.Sprite = PeachSpriteFactory.Instance.FactoryMethod(peach);
        }

        public void JumpingStateTransition()
        {
            if (peach.PowerUpState is StandardState)
            {
                sound = peach.Content.Load<SoundEffect>("Sound Effects/Peach Jumps");
            }
            else
            {
                sound = peach.Content.Load<SoundEffect>("Sound Effects/Peach Jumps Super");
            }
            sound.Play();
            var state = new JumpingState(this, peach);
            peach.ActionState = state;
            peach.Sprite = PeachSpriteFactory.Instance.FactoryMethod(peach);
        }

        public void RunningStateTransition()
        {
            return;
        }

        public void WalkingStateTransition()
        {
            var state = new WalkingState(peach);
            peach.ActionState = state;
            peach.Sprite = PeachSpriteFactory.Instance.FactoryMethod(peach);
        }

        public void FaceLeftTransition()
        {
            if (!(peach.PowerUpState is DeadState))
            {
                if (!peach.Sprite.HReflect)
                {
                    IdleStateTransition();
                }
            }
        }

        public void FaceRightTransition()
        {
            if (!(peach.PowerUpState is DeadState))
            {
                if (peach.Sprite.HReflect)
                {
                    IdleStateTransition();
                }
            }
        }
    }
}
