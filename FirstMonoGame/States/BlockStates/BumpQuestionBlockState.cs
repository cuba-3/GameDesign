﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstMonoGame.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace FirstMonoGame.States.BlockStates
{
    class BumpQuestionBlockState : IBlockState<QuestionBlock>
    {
        public GraphicsDevice GraphicsDevice;
        private SoundEffect sound;

        public BumpQuestionBlockState(GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
        }

        public void NewTransition(QuestionBlock block)
        {
            return;
        }

        public void BumpTransition(QuestionBlock block)
        {
            sound = block.Content.Load<SoundEffect>("Sound Effects/Bump Block");
            sound.Play();
            var state = new BumpQuestionBlockState(GraphicsDevice);
            block.BlockState = state;
            block.Sprite = QuestionBlockSpriteFactory.Instance.FactoryMethod(block.Content, block, GraphicsDevice, false);
        }

        public void UsedTransition(QuestionBlock block)
        {
            var state = new UsedQuestionBlockState();
            block.BlockState = state;
            block.Sprite = QuestionBlockSpriteFactory.Instance.FactoryMethod(block.Content, block, GraphicsDevice, false);
        }

        public void ExplodeTransition(QuestionBlock block)
        {
            throw new NotImplementedException();
        }
    }
}
