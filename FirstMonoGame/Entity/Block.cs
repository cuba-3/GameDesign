using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstMonoGame.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FirstMonoGame.Entity
{
    abstract class Block : IEntity
    {
        public Vector2 Location { get => Sprite.CurrentLocation; set => Sprite.CurrentLocation = value; }
        public bool Visible { get => Sprite.Display; set => Sprite.Display = value; }
        public bool Checkable { get => Sprite.Checkable; set => Sprite.Checkable = value; }
        public bool VisBounding { get; set; }
        public Vector2 OriginalLocation { get; set; }
        public ISprite Sprite { get; set; }
        public bool OnScreen { get; set; }

        public AABB BoundingBox()
        {
            return Sprite.BoundingBox;
        }
        public virtual void DoCollision(IEntity collider)
        {
            return;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch);
        }
        public virtual void Update(GameTime gameTime)
        {
            Sprite.Update(gameTime);
        }

        public virtual bool CanCollide(IEntity other)
        {
            return !(other is Block);
        }
    }
}
