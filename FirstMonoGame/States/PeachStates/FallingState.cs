using System;
using System.Diagnostics;
using FirstMonoGame.States.PeachStates;
using FirstMonoGame.States2.PowerUpStates;
using Microsoft.Xna.Framework;

namespace FirstMonoGame.States2.PeachStates
{
    class FallingState : IActionState<Peach>
    {
        private Peach peach;
        public FallingState(Peach p)
        {
            peach = p;
        }
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
            return;
        }

        public void JumpingStateTransition()
        {
            return;
        }

        public void RunningStateTransition()
        {
            return;
        }

        public void WalkingStateTransition()
        {
            return;
        }

        public void FaceLeftTransition()
        {
            return;
        }

        public void FaceRightTransition()
        {
            return;
        }
    }
}
