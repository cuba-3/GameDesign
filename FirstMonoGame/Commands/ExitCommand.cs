namespace FirstMonoGame.Commands
{
    class ExitCommand : ICommand
    {
        GameMain Sprint0;

        public ExitCommand(GameMain sprint0)
        {
            this.Sprint0 = sprint0;
        }

        public void Execute()
        {
            Sprint0.Exit();
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
