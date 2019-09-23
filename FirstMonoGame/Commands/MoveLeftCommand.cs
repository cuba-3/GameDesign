using FirstMonoGame.States.PeachStates;
using FirstMonoGame.States2.PeachStates;
using System.Diagnostics;

namespace FirstMonoGame.Commands
{
    class MoveLeftCommand : ICommand
    {
        private Peach Peach;

        public MoveLeftCommand(Peach peach)
        {
            Peach = peach;
        }

        public void Execute()
        {
            Peach.FaceLeftTransition();
        }
        public void HeldKey()
        {
            Peach.FaceLeftTransition();
        }

        public void Release()
        {
            if (!(Peach.ActionState is FallingState || Peach.ActionState is JumpingState)) Peach.IdleTransition();
        }
    }
}
