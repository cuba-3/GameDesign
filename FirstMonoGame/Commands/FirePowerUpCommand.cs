using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace FirstMonoGame.Commands
{
    class FirePowerUpCommand : ICommand
    {
        private Peach Peach;

        public FirePowerUpCommand(Peach peach)
        {
            Peach = peach;
        }

        public void Execute()
        {
            Peach.FirePowerUpTransition();
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
