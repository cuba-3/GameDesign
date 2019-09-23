using FirstMonoGame.Sprites;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using FirstMonoGame.Collision;
using FirstMonoGame.Levels;

namespace FirstMonoGame.Entity
{
    class FireFlower : IEntity
    {
        public Vector2 OriginalLocation { get; set; }
        public ISprite Sprite { get; set; }
        public Vector2 Location { get { return Sprite.CurrentLocation; } set { Sprite.CurrentLocation = value; } }
        public bool Visible { get { return Sprite.Display; } set { Sprite.Display = value; } }
        public bool VisBounding { get { return Sprite.VisibleBounding; } set { Sprite.VisibleBounding = value; } }
        public bool Checkable { get; set; }
        private bool isConsumed;
        public bool OnScreen { get; set; }

        public FireFlower(ContentManager content, GraphicsDevice graphicsDevice)
        {
            Sprite = FireFlowerSpriteFactory.Instance.FactoryMethod(content, graphicsDevice, this);
            isConsumed = false;
            Sprite.Acceleration = new Vector2(Constants.ZERO, 1300);
        }

        public void Update(GameTime gameTime)
        {
            Sprite.Update(gameTime);
            if (!Checkable) Sprite.Velocity *= new Vector2(Constants.ZERO,Constants.ZERO);
            if (Sprite.Velocity.Y > 180) Sprite.Velocity = new Vector2(Sprite.Velocity.X, 180);
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
                AABB itemBounds = BoundingBox();
                AABB blockBounds = entity.BoundingBox();
                if (!blockBounds.MinkowskiDifferenceContainsOrigin(itemBounds)) return;
                if (itemBounds.Bottom - blockBounds.Top <= 5)
                {
                    Sprite.Velocity = new Vector2(Sprite.Velocity.X, entity.Sprite.Velocity.Y);
                    if (entity.Sprite.Velocity.Y >Constants.ZERO) Sprite.Velocity *= new Vector2(1,Constants.ZERO);
                    Sprite.CurrentLocation -= new Vector2(Constants.ZERO, itemBounds.Bottom - blockBounds.Top + 1);
                }
                else if (blockBounds.Bottom - itemBounds.Top <= 10)
                {
                    if (Sprite.Velocity.Y <Constants.ZERO) Sprite.Velocity *= new Vector2(1, -1);
                    Sprite.CurrentLocation += new Vector2(Constants.ZERO, blockBounds.Bottom - itemBounds.Top + 1);
                }
                else if (itemBounds.Right - blockBounds.Left <= 10)
                {
                    Sprite.CurrentLocation -= new Vector2(itemBounds.Right - blockBounds.Left + 1,Constants.ZERO);
                }
                else
                {
                    Sprite.CurrentLocation += new Vector2(blockBounds.Right - itemBounds.Left + 1,Constants.ZERO);
                }
            }
        }

        public bool CanCollide(IEntity other)
        {
            return !isConsumed && !(other is FireFlower);
        }
    }
}
