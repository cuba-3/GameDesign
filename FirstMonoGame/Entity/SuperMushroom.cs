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

namespace FirstMonoGame.Entity
{
    class SuperMushroom : IEntity
    {
        public Vector2 OriginalLocation { get; set; }
        public ISprite Sprite { get; set; }
        private bool Consumed;
        public Vector2 Location { get { return Sprite.CurrentLocation; } set { Sprite.CurrentLocation = value; } }
        public bool Visible { get { return Sprite.Display; } set { Sprite.Display = value; } }
        public bool Checkable { get; set; }
        public bool VisBounding { get { return Sprite.VisibleBounding; } set { Sprite.VisibleBounding = value; } }
        public bool OnScreen { get; set; }

        public SuperMushroom(ContentManager content, GraphicsDevice graphicsDevice)
        {
            Sprite = SuperMushroomSpriteFactory.Instance.FactoryMethod(content, graphicsDevice, this);
            Sprite.Acceleration = new Vector2(Constants.ZERO, 1300f);
        }

        public void Update(GameTime gameTime)
        {
            Sprite.Update(gameTime);
            if (!Checkable) Sprite.Velocity *= new Vector2(Constants.ZERO,Constants.ZERO);
            if (Sprite.Velocity.Y > 600) Sprite.Velocity = new Vector2(Sprite.Velocity.X, 600);
            if (Consumed)
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
                Consumed = true;
            }
           else if ((entity is Block block && block.Visible) || entity is Pipe || entity is WarpPipe)
            {
                AABB shroomBounds = BoundingBox();
                AABB blockBounds = entity.BoundingBox();
                if (!blockBounds.MinkowskiDifferenceContainsOrigin(shroomBounds)) return;
                if (shroomBounds.Bottom - blockBounds.Top <= 10)
                {
                    Sprite.Velocity = new Vector2(Sprite.Velocity.X, entity.Sprite.Velocity.Y);
                    if (entity.Sprite.Velocity.Y >Constants.ZERO) Sprite.Velocity *= new Vector2(1,Constants.ZERO);
                    Sprite.CurrentLocation -= new Vector2(Constants.ZERO, shroomBounds.Bottom - blockBounds.Top + 1);
                }
                else if (blockBounds.Bottom - shroomBounds.Top <= 10)
                {
                    if (Sprite.Velocity.Y <Constants.ZERO) Sprite.Velocity *= new Vector2(1, -1);
                    Sprite.CurrentLocation += new Vector2(Constants.ZERO, blockBounds.Bottom - shroomBounds.Top + 1);
                }
                else if (shroomBounds.Right - blockBounds.Left <= 10)
                {
                    Sprite.Velocity *= new Vector2(-1, 1);
                    Sprite.CurrentLocation -= new Vector2(shroomBounds.Right - blockBounds.Left + 1,Constants.ZERO);
                }
                else
                {
                    Sprite.Velocity *= new Vector2(-1, 1);
                    Sprite.CurrentLocation += new Vector2(blockBounds.Right - shroomBounds.Left + 1,Constants.ZERO);
                }
            }
        }
        public bool CanCollide(IEntity other)
        {
            return !Consumed && !(other is SuperMushroom) && !(other is FireFlower);
        }
    }
}
