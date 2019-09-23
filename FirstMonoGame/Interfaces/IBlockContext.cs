using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMonoGame.Interfaces
{
    public interface IBlockContext<T>
    {
        void SetBlockState(IBlockState<T> blockState);
    }
}
