using FirstMonoGame.Sprites;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using FirstMonoGame.Collision;
using FirstMonoGame.Levels;
using System.Diagnostics;

namespace FirstMonoGame.Entity
{
    class Star : IEntity
    {
        public Vector2 OriginalLocation { get; set; }
        public Vector2 Location { get { return Sprite.CurrentLocation; } set { Sprite.CurrentLocation = value; } }
        public bool Visible { get { return Sprite.Display; } set { Sprite.Display = value; } }
        public bool Checkable { get; set; }
        public ISprite Sprite { get; set; }
        private bool isConsumed;
        public int delay;
        public bool VisBounding { get { return Sprite.VisibleBounding; } set { Sprite.VisibleBounding = value; } }
        public bool OnScreen { get; set; }

        public Star(ContentManager content, GraphicsDevice graphicsDevice)
        {
            Sprite = StarSpriteFactory.Instance.FactoryMethod(content, graphicsDevice, this);
            isConsumed = false;
            delay =Constants.ZERO;
            Sprite.Acceleration = new Vector2(Constants.ZERO, 1300f);
        }

        public void Update(GameTime gameTime)
        {
            Sprite.Update(gameTime);
            delay++;
            if (!Checkable) Sprite.Velocity *= new Vector2(Constants.ZERO,Constants.ZERO);
            if (Sprite.Velocity.Y > 700) Sprite.Velocity = new Vector2(Sprite.Velocity.X, 700);
            if (isConsumed)
            {
                Collections.Instance.GetCollisionRef().Deregister(this);
                Collections.Instance.GetLevelRef().Entities.Remove(this);
            }
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
            if (entity is Peach)
            {
                isConsumed = true;
            }
            else if ((entity is Block block && block.Visible) || entity is Pipe || entity is WarpPipe)
            {
                AABB starBounds = BoundingBox();
                AABB blockBounds = entity.BoundingBox();
                if (!blockBounds.MinkowskiDifferenceContainsOrigin(starBounds)) return;
                if (starBounds.Bottom - blockBounds.Top <= 10)
                {
                    if (Sprite.Velocity.Y >Constants.ZERO) Sprite.Velocity = new Vector2(Sprite.Velocity.X, -700);
                    Sprite.CurrentLocation -= new Vector2(Constants.ZERO, starBounds.Bottom - blockBounds.Top + 1);
                }
                else if (blockBounds.Bottom - starBounds.Top <= 10)
                {
                    if (Sprite.Velocity.Y <Constants.ZERO) Sprite.Velocity *= new Vector2(1, -1);
                    Sprite.CurrentLocation += new Vector2(Constants.ZERO, blockBounds.Bottom - starBounds.Top + 1);
                }
                else if (starBounds.Right - blockBounds.Left <= 10)
                {
                    Sprite.Velocity *= new Vector2(-1, 1);
                    Sprite.CurrentLocation -= new Vector2(starBounds.Right - blockBounds.Left + 1);
                }
                else
                {
                    Sprite.Velocity *= new Vector2(-1, 1);
                    Sprite.CurrentLocation += new Vector2(blockBounds.Right - starBounds.Left + 1);
                }
            }
        }

        public bool CanCollide(IEntity other)
        {
            return !isConsumed;
        }
    }
}
