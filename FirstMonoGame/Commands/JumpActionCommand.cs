namespace FirstMonoGame.Commands
{
    class JumpActionCommand : ICommand
    {
        private Peach peach;
        public JumpActionCommand(Peach peachInstance)
        {
            peach = peachInstance;
        }
        public void Execute()
        {
            peach.JumpTransition();
        }
    }
}
