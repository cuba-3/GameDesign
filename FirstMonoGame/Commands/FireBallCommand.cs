using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMonoGame.Commands
{
    class FireBallCommand : ICommand
    {
        private Peach peach;

        public FireBallCommand(Peach p)
        {
            peach = p;
        }

        public void Execute()
        {
            peach.ShootFireball();
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
