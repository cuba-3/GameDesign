using FirstMonoGame.States.PeachStates;
using FirstMonoGame.States2.PowerUpStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace FirstMonoGame.States2.PeachStates
{
    class IdleState : IActionState<Peach>
    {
        //private IActionState<Peach> PreviousState;
        private SoundEffect sound;
        private Peach peach;

        public IdleState(Peach p)
        {
            peach = p;
            peach.Sprite.Velocity = new Vector2(Constants.ZERO,Constants.ZERO);
        }
        //Idle -> idle, walk, dead, jump
        public void IdleStateTransition()
        {
            return;
        }

        public void CrouchingStateTransition()
        {
            if (!(peach.PowerUpState is DeadState))
            {
                var state = new CrouchingState(peach);
                peach.ActionState = state;
                peach.Sprite = PeachSpriteFactory.Instance.FactoryMethod(peach);
            }
        }

        public void FallingStateTransition()
        {
            if (!(peach.PowerUpState is DeadState))
            {
                var state = new FallingState(peach);
                peach.ActionState = state;
                peach.Sprite = PeachSpriteFactory.Instance.FactoryMethod(peach);
            }
        }

        public void JumpingStateTransition()
        {
            if (!(peach.PowerUpState is DeadState))
            {
                if(peach.PowerUpState is StandardState)
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
                    peach.Sprite.HReflect = true;
                }
                else
                {
                    WalkingStateTransition();
                }
            }
        }

        public void FaceRightTransition()
        {
            if (!(peach.PowerUpState is DeadState))
            {
                if (peach.Sprite.HReflect)
                {
                    peach.Sprite.HReflect = false;
                }
                else
                {
                    WalkingStateTransition();
                }
            }
        }
    }
}
