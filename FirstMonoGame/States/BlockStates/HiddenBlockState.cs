using FirstMonoGame.Entity;
using FirstMonoGame.Interfaces;
using FirstMonoGame.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMonoGame.States.BlockStates
{
    class HiddenBlockState:IBlockState<HiddenBlock>
    {
        // Hidden blocks behave exactly as ? blocks
        IBlockState<HiddenBlock> state;
        public GraphicsDevice GraphicsDevice;

        public HiddenBlockState(GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
        }

        public void NewTransition(HiddenBlock block)
        {
            state = new HiddenBlockState(GraphicsDevice);
            block.BlockState = state;
        }

        public void BumpTransition(HiddenBlock block)
        {
            if (block.OriginalLocation.Y == block.Sprite.CurrentLocation.Y) block.Sprite.Velocity = new Vector2(Constants.ZERO, -150);
        }

        public void UsedTransition(HiddenBlock block)
        {
            return;
        }

        public void ExplodeTransition(HiddenBlock block)
        {
            throw new NotImplementedException();
        }
    }
}
