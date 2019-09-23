using FirstMonoGame.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstMonoGame.Entity;
using Microsoft.Xna.Framework.Audio;

namespace FirstMonoGame.States.BlockStates
{
    class ExplodeBrickBlockState : IBlockState<ExplodingBlock>
    {
        public GraphicsDevice GraphicsDevice;
        private SoundEffect sound;

        public ExplodeBrickBlockState(GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
        }

        public void BumpTransition(ExplodingBlock block)
        {
            throw new NotImplementedException();
        }

        public void ExplodeTransition(ExplodingBlock block)
        {
            sound = block.Content.Load<SoundEffect>("Sound Effects/Brick Explodes");
            sound.Play();
            ExplodeBrickBlockState state = new ExplodeBrickBlockState(GraphicsDevice);
            block.BlockState = state;
        }


        public void NewTransition(ExplodingBlock block)
        {
            throw new NotImplementedException();
        }

        public void UsedTransition(ExplodingBlock block)
        {
            throw new NotImplementedException();
        }
    }
}
