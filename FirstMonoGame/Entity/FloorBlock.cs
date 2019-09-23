using FirstMonoGame.Collision;
using FirstMonoGame.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace FirstMonoGame.Entity
{
    class FloorBlock : Block {
        
        public bool Underworld { get; set; }
        public FloorBlock(ContentManager Content, GraphicsDevice graphicsDevice, bool underworld) 
        {
            Underworld = underworld;
            Sprite = new BlockSprite(graphicsDevice);
            Sprite = FloorBlockSpriteFactory.Instance.FactoryMethod(Content, this, graphicsDevice, Underworld);
        }
    }
}
