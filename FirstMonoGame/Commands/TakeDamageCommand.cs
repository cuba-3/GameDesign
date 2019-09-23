using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMonoGame.Commands
{
    class TakeDamageCommand : ICommand
    {
        private Peach Peach;

        public TakeDamageCommand(Peach peach)
        {
            Peach = peach;
        }

        public void Execute()
        {
            Peach.TakeDamageTransition();
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
