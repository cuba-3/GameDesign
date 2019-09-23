using FirstMonoGame.States2.PeachStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace FirstMonoGame.States.PeachStates
{
    class JumpingState : IActionState<Peach>
    {
        private IActionState<Peach> PreviousState;
        private Peach peach;
        public JumpingState(IActionState<Peach> previousState, Peach p)
        {
            PreviousState = previousState;
            peach = p;
            peach.Sprite.Velocity = new Vector2(peach.Sprite.Velocity.X, -790);
        }
        //Jump -> fall, jump, run, walk
        public void IdleStateTransition()
        {
            if (PreviousState is WalkingState || PreviousState is RunningState) PreviousState = new IdleState(peach); // let go of key midair
            return;
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
