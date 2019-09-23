using FirstMonoGame.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMonoGame.Commands
{
    class DollarBillCommand : ICommand
    {
        public Coin Coin;
        public DollarBillCommand(Coin coin)
        {
            Coin = coin;
        }
        public void Execute()
        {
            Coin.DollarTransition();
        }

        public void HeldKey()
        {
            throw new NotImplementedException();
        }

        public void Release()
        {
            throw new NotImplementedException();
        }
    }
}
