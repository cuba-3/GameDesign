using FirstMonoGame.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstMonoGame.Collision;
using System.Diagnostics;

namespace FirstMonoGame.Entity
{
    class StairBlock : Block
    {
        public ContentManager Content { get; set; }

        public StairBlock(ContentManager content, GraphicsDevice graphicsDevice)
        {
            Content = content;
            Sprite = new BlockSprite(graphicsDevice);
            Sprite = StairBlockSpriteFactory.Instance.FactoryMethod(this.Content, this, graphicsDevice, false);
        }
    }
}
