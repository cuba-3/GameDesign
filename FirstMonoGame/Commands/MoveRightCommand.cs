using FirstMonoGame.States.PeachStates;
using FirstMonoGame.States2.PeachStates;
using System.Diagnostics;

namespace FirstMonoGame.Commands
{
    class MoveRightCommand : ICommand
    {
        private Peach Peach;

        public MoveRightCommand(Peach peach)
        {
            Peach = peach;
        }

        public void Execute()
        {
            Peach.FaceRightTransition();
        }

        public void HeldKey()
        {
            Peach.FaceRightTransition();
        }

        public void Release()
        {
            if (!(Peach.ActionState is FallingState || Peach.ActionState is JumpingState)) Peach.IdleTransition();
        }
    }
}
