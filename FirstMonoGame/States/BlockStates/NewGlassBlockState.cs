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
    class NewGlassBlockState : IBlockState<GlassBlock>
    {
        public GraphicsDevice GraphicsDevice;
        private SoundEffect sound;

        public NewGlassBlockState(GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
        }

        public void NewTransition(GlassBlock block)
        {
            return;       
        }

        public void BumpTransition(GlassBlock block)
        {
            Debug.WriteLine("Cracking transition");
            sound = block.Content.Load<SoundEffect>("Sound Effects/Bump Block");
            sound.Play();
            var state = new CrackedGlassBlockState();
            block.BlockState = state;
            block.Sprite = GlassBlockSpriteFactory.Instance.FactoryMethod(block.Content, block, GraphicsDevice, false);
            block.Bump = true;
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
