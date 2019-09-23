using FirstMonoGame.States2.PeachStates;
using FirstMonoGame.States2.PowerUpStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace FirstMonoGame.States.PeachStates
{
    class WalkingState : IActionState<Peach>
    {
        private SoundEffect sound;
        private Peach peach;
        public WalkingState(Peach p)
        {
            peach = p;
            peach.Sprite.Velocity = new Vector2(180, peach.Sprite.Velocity.Y);
            if (peach.Sprite.HReflect) peach.Sprite.Velocity *= new Vector2(-1, 1);
        }
        //Walk -> dead, jump, walk, run, idle
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
            var state = new RunningState(peach);
            peach.ActionState = state;
            peach.Sprite = PeachSpriteFactory.Instance.FactoryMethod(peach);
        }

        public void WalkingStateTransition()
        {
            return;
        }

        public void FaceLeftTransition()
        {
            if (!(peach.PowerUpState is DeadState))
            {
                if (!peach.Sprite.HReflect)
                {
                    IdleStateTransition();
                }
                else
                {
                    RunningStateTransition();
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
                else
                {
                    RunningStateTransition();
                }

            }
        }
    }
}
