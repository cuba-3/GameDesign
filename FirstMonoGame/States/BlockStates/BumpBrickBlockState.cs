using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstMonoGame.Entity;
using FirstMonoGame.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace FirstMonoGame.States.BlockStates
{
    class BumpBrickBlockState : IBlockState<BrickBlock>
    {
        public GraphicsDevice GraphicsDevice;
        private SoundEffect deadgoomba;
        private Block b;

        public BumpBrickBlockState(GraphicsDevice graphicsDevice, Block block)
        {
            GraphicsDevice = graphicsDevice;
            b = block;
            b.Sprite.Velocity = new Vector2(Constants.ZERO, -150);
        }

        public void NewTransition(BrickBlock block)
        {
            block.Sprite = BrickBlockSpriteFactory.Instance.FactoryMethod(block.Content, block, GraphicsDevice, false);
            var state = new NewBrickBlockState(GraphicsDevice);
            block.BlockState = state;
        }

        public void BumpTransition(BrickBlock block)
        {
            deadgoomba = block.Content.Load<SoundEffect>("Sound Effect/Bump Block");
            deadgoomba.Play();
        }

        public void UsedTransition(BrickBlock block)
        {
            return;
        }

        public void ExplodeTransition(BrickBlock block)
        {
            throw new NotImplementedException();

        }
    }
}
