using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace FirstMonoGame.Commands
{
    class SuperPowerUpCommand : ICommand
    {
        private Peach Peach;

        public SuperPowerUpCommand(Peach peach)
        {
            Peach = peach;
        }

        public void Execute()
        {
            Peach.SuperPowerUpTransition();
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
