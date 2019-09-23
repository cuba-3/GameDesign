using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstMonoGame.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using FirstMonoGame.Entity;
using Microsoft.Xna.Framework;

namespace FirstMonoGame.States.BlockStates
{
    class CrackedGlassBlockState : IBlockState<GlassBlock>
    {

        public CrackedGlassBlockState()
        {
        }

        public void NewTransition(GlassBlock block)
        {
            throw new NotImplementedException();
        }

        public void BumpTransition(GlassBlock block)
        {
            throw new NotImplementedException();
        }

        public void UsedTransition(GlassBlock block)
        {
            throw new NotImplementedException();
        }

        public void ExplodeTransition(GlassBlock block)
        {
            throw new NotImplementedException();
        }
    }
}
