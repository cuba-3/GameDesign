using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMonoGame
{
    public interface IPowerUpState<T>
    {
        void StandardTransition(T avatar);
        void FireTransition(T avatar);
        void StarTransition(T avatar);
        void TakeDamageTransition(T avatar);
        void SuperTransition(T avatar);
        void DeadTransition(T avatar);
        void WarpTransition(T avatar);
    }
}
