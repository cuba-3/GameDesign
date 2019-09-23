using FirstMonoGame.Entity;
using FirstMonoGame.Interfaces;
using FirstMonoGame.Sprites;
using FirstMonoGame.States.BlockStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstMonoGame.Collision;
using FirstMonoGame.Levels;
using System.Diagnostics;

namespace FirstMonoGame
{
    class UsedBlock : Block
    {
        public ContentManager Content { get; set; }

        public UsedBlock(ContentManager content, GraphicsDevice graphicsDevice)
        {
            Content = content;
            Sprite = new BlockSprite(graphicsDevice);
            Sprite = UsedBlockSpriteFactory.Instance.FactoryMethod(this.Content, this, graphicsDevice, false);
        }
    }
}
