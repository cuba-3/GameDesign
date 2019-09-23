using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace FirstMonoGame.Commands
{
    class StarPowerUpCommand : ICommand
    {
        private Peach Peach;

        public StarPowerUpCommand(Peach peach)
        {
            Peach = peach;
        }

        public void Execute()
        {
            Peach.StarPowerUpTransition();
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
