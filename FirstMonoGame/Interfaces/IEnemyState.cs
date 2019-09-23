using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMonoGame.Interfaces
{
    public interface IEnemyState<T>
    {
        void StandardTransition(T enemy);
        void TakeDamageTransition(T enemy);
        void WalkingTransition(T enemy);
        void PunchingTransition(T enemy);
        void BreatheFireTransition(T enemy);
    }
}
