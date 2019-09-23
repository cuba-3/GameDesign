using FirstMonoGame.Entity;
using FirstMonoGame.Interfaces;
using FirstMonoGame.Sprites;
using FirstMonoGame.States.BlockStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstMonoGame.Collision;


namespace FirstMonoGame.Entity
{
    class ExplodingBlock : IEntity
    {
        public Vector2 OriginalLocation { get; set; }
        public IBlockState<ExplodingBlock> BlockState { get; set; }
        public Vector2 Location { get { return Sprite.CurrentLocation; } set { Sprite.CurrentLocation = value; } }
        public bool VisBounding { get { return Sprite.VisibleBounding; } set { Sprite.VisibleBounding = value; } }
        public bool Visible { get { return Sprite.Display; } set { Sprite.Display = value; } }
        public bool Checkable { get { return Sprite.Checkable; } set { Sprite.Checkable = value; } }
        public ISprite Sprite { get; set; }
        public ContentManager Content { get; set; }
        public GraphicsDevice GraphicsDevice;
        public bool OnScreen { get; set; }

        public ExplodingBlock(ContentManager content, GraphicsDevice graphicsDevice, bool Glass)
        {
            GraphicsDevice = graphicsDevice;
            Content = content;
            BlockState = new ExplodeBrickBlockState(graphicsDevice);
            Sprite = ExplodingBlockSpriteFactory.Instance.FactoryMethod(this.Content, this, GraphicsDevice, Glass);
        }

        public void ExplodingBlockTransition()
        {
            BlockState.ExplodeTransition(this);
            Sprite.Display = true;
            Sprite.Acceleration = new Vector2(Sprite.Acceleration.X, (float)1300.0);
        }

        public void Update(GameTime gameTime)
        {
            Sprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch);
        }
        public AABB BoundingBox()
        {
            return Sprite.BoundingBox;
        }
        public void DoCollision(IEntity entity)
        {
            throw new NotImplementedException();
        }
        public bool CanCollide(IEntity other)
        {
            return false;
        }
    }
}
