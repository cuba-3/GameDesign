using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMonoGame.States.PowerUpStates
{
    class WarpingState : IPowerUpState<Peach>
    {
        public WarpingState()
        {
        }

        void IPowerUpState<Peach>.DeadTransition(Peach avatar)
        {
            throw new NotImplementedException();
        }

        void IPowerUpState<Peach>.FireTransition(Peach avatar)
        {
            throw new NotImplementedException();
        }

        void IPowerUpState<Peach>.StandardTransition(Peach avatar)
        {
            throw new NotImplementedException();
        }

        void IPowerUpState<Peach>.StarTransition(Peach avatar)
        {
            throw new NotImplementedException();
        }

        void IPowerUpState<Peach>.SuperTransition(Peach avatar)
        {
            throw new NotImplementedException();
        }

        void IPowerUpState<Peach>.TakeDamageTransition(Peach avatar)
        {
            return;
        }

        void IPowerUpState<Peach>.WarpTransition(Peach avatar)
        {
            return;
        }
    }
}
