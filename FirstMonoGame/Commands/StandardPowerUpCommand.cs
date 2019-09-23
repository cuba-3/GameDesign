using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace FirstMonoGame.Commands
{
    class StandardPowerUpCommand : ICommand
    {
        private Peach Peach;

        public StandardPowerUpCommand(Peach peach)
        {
            Peach = peach;
        }

        public void Execute()
        {
            Peach.StandardPowerUpTransition();
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
