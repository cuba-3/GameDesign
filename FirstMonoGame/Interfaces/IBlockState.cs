using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMonoGame.Interfaces
{
    public interface IBlockState<T>
    {
        void NewTransition(T block);
        void BumpTransition(T block);
        void UsedTransition(T block);
        void ExplodeTransition(T block);
    }
}
