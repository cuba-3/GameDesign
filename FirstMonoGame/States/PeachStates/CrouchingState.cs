using FirstMonoGame.States2.PeachStates;
using FirstMonoGame.States2.PowerUpStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace FirstMonoGame.States.PeachStates
{
    class CrouchingState : IActionState<Peach>
    {
        //private IActionState<Peach> PreviousState;
        private Peach peach;

        public CrouchingState(Peach p)
        {
            //PreviousState = previousState;
            peach = p;
        }
        public void CrouchingStateTransition()
        {
            return;
        }

        public void FallingStateTransition()
        {
            return;
        }

        public void IdleStateTransition()
        {
            var state = new IdleState(peach);
            peach.ActionState = state;
            peach.Sprite = PeachSpriteFactory.Instance.FactoryMethod(peach);
        }

        public void JumpingStateTransition()
        {
            IdleStateTransition();
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