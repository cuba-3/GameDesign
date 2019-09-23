using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstMonoGame.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using FirstMonoGame.Entity;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;


namespace FirstMonoGame.States.BlockStates
{
    class NewBrickBlockState : IBlockState<BrickBlock>
    {
        public GraphicsDevice GraphicsDevice;
        private SoundEffect sound;

        public NewBrickBlockState(GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
        }

        public void NewTransition(BrickBlock block)
        {
            return;       
        }

        public void BumpTransition(BrickBlock block)
        {
            sound = block.Content.Load<SoundEffect>("Sound Effects/Bump Block");
            sound.Play();
            var state = new BumpBrickBlockState(GraphicsDevice, block);
            block.BlockState = state;
            block.Sprite = BrickBlockSpriteFactory.Instance.FactoryMethod(block.Content, block, GraphicsDevice, false);
            block.Bump = false;
        }

        public void UsedTransition(BrickBlock block)
        {
            throw new NotImplementedException();
        }

        public void ExplodeTransition(BrickBlock block)
        {
            throw new NotImplementedException();
        }
    }
}
