using FirstMonoGame.Collision;
using FirstMonoGame.Sprites;
using FirstMonoGame.WorldScrolling;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMonoGame.Entity
{
    class FireBar : IEntity
    {
        public Vector2 Location { get { return Sprite.CurrentLocation; } set { Sprite.CurrentLocation = value; } }
        public bool Visible { get { return Sprite.Display; } set { Sprite.Display = value; } }
        public bool Checkable { get; set; }
        public bool VisBounding { get { return Sprite.VisibleBounding; } set { Sprite.VisibleBounding = value; } }
        public Vector2 OriginalLocation { get; set; }
        public bool OnScreen { get; set; }
        public ISprite Sprite { get; set; }
        private bool Consumed;
        private Texture2D fireBar;

        public FireBar(GraphicsDevice graphics, ContentManager content)
        {
            fireBar = content.Load<Texture2D>("FireBar");
            Sprite = new ItemSprite(graphics, fireBar, 1, 2,Constants.ZERO, 2);
            Consumed = false;
        }

        public void Update(GameTime gameTime)
        {
            if (Sprite.CurrentLocation.X < Camera.Instance.CameraBounds.Left - 300 || Sprite.CurrentLocation.X > Camera.Instance.CameraBounds.Right + 300) Consumed = true;
            Sprite.Update(gameTime);
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

        public void DoCollision(IEntity other)
        {
            AABB fireBounds = BoundingBox();
            AABB otherBounds = other.BoundingBox();
            if (!otherBounds.MinkowskiDifferenceContainsOrigin(fireBounds)) return;
            Consumed = true;
        }

        public bool CanCollide(IEntity other)
        {
            return !Consumed && other is Peach;
        }
    }
}
