using FirstMonoGame.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMonoGame.Interfaces
{
     interface IItemState<T>
    {
        void DoAction(T entity);
    }
}
