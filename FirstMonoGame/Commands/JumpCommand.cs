namespace FirstMonoGame.Commands
{
    class JumpCommand : ICommand
    {
        private Peach Peach;

        public JumpCommand(Peach peach)
        {
            Peach = peach;
        }

        public void Execute()
        {
            Peach.JumpTransition();
        }
        public void HeldKey()
        {
            return;
        }

        public void Release()
        {
            // TODO
        }
    }
}
