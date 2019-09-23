using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstMonoGame.Interfaces;
using Microsoft.Xna.Framework.Graphics;

namespace FirstMonoGame.States.BlockStates
{
    class UsedQuestionBlockState : IBlockState<QuestionBlock>
    {
        //GraphicsDevice GraphicsDevice;

        public UsedQuestionBlockState()
        {
           // GraphicsDevice = graphicsDevice;
        }
        public void NewTransition(QuestionBlock block)
        {
            return;
        }
        public void BumpTransition(QuestionBlock block)
        {
            return;
        }
        public void UsedTransition(QuestionBlock block)
        {
            return;
        }

        public void ExplodeTransition(QuestionBlock block)
        {
            throw new NotImplementedException();
        }
    }
}
