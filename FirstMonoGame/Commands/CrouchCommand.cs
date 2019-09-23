namespace FirstMonoGame.Commands
{
    class CrouchCommand : ICommand
    {
        private Peach Peach;

        public CrouchCommand(Peach peach)
        {
            Peach = peach;
        }

        public void Execute()
        {
            Peach.CrouchTransition();
        }

        public void HeldKey()
        {
            return;
        }

        public void Release()
        {
            return;
        }
    }
}
