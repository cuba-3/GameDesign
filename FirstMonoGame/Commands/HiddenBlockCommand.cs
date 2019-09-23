using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstMonoGame.Sprites;
using System.Diagnostics;
using FirstMonoGame.Entity;

namespace FirstMonoGame.Commands
{
    class HiddenBlockCommand : ICommand
    {
        private HiddenBlock Block;
         
        public HiddenBlockCommand(HiddenBlock block)
        {
            Block = block;
        }

        public void Execute()
        {
            Block.BumpHiddenBlockTransition();
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
