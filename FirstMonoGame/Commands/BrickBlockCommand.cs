using FirstMonoGame.Entity;
using FirstMonoGame.States.PowerUpStates;
using FirstMonoGame.States2.PowerUpStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMonoGame.Commands
{
    class BrickBlockCommand : ICommand
    {
        private BrickBlock Block;
        private Peach Peach;
        public BrickBlockCommand(BrickBlock block, Peach peach)
        {
            Block = block;
            Peach = peach;
        }

        public void Execute()
        {
            if (Peach.PowerUpState is SuperState)
            {
                Block.ExplodeBrickBlockTransition();
            } else
            {
                Block.BumpBrickBlockTransition();
            }
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
