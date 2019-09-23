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
    class FireBall : IEntity
    {
        public Vector2 Location { get { return Sprite.CurrentLocation; } set { Sprite.CurrentLocation = value; } }
        public bool Visible { get { return Sprite.Display; } set { Sprite.Display = value; } }
        public bool Checkable { get; set; }
        public bool VisBounding { get { return Sprite.VisibleBounding; } set { Sprite.VisibleBounding = value; } }
        public Vector2 OriginalLocation { get; set; }
        public bool OnScreen { get; set; }
        public ISprite Sprite { get; set; }
        private bool Consumed { get; set; }
        private Texture2D fireBall;
        public Peach Shooter { get; set; }

        public FireBall(GraphicsDevice graphicsDevice, Texture2D tex, Peach p)
        {
            fireBall = tex;
            Sprite = new ItemSprite(graphicsDevice, fireBall, 1, 3,Constants.ZERO, 3);
            Sprite.Acceleration = new Vector2(Constants.ZERO, 1300);
            Consumed = false;
            Checkable = true;
            Shooter = p;
        }

        public void Update(GameTime gameTime)
        {
            Sprite.Update(gameTime);
            if (Sprite.Velocity.Y > 380) Sprite.Velocity = new Vector2(Sprite.Velocity.X, 380);
            if (Sprite.CurrentLocation.X < Camera.Instance.LevelBounds.Left - 200 || Sprite.CurrentLocation.X > Camera.Instance.LevelBounds.Right + 200) Consumed = true;
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
            AABB fireBallBounds = BoundingBox();
            AABB otherBounds = other.BoundingBox();
            if (!otherBounds.MinkowskiDifferenceContainsOrigin(fireBallBounds)) return;
            if (other is Block || other is Pipe || other is WarpPipe)
            {
                if (fireBallBounds.Bottom - otherBounds.Top <= 10)
                {
                    Sprite.Velocity = new Vector2(Sprite.Velocity.X, other.Sprite.Velocity.Y - 380);
                    if (other.Sprite.Velocity.Y >Constants.ZERO) Sprite.Velocity = new Vector2(Sprite.Velocity.X, -380);
                    Sprite.CurrentLocation -= new Vector2(Constants.ZERO, fireBallBounds.Bottom - otherBounds.Top + 1);
                }
                else if (otherBounds.Bottom - fireBallBounds.Top <= 10)
                {
                    if (Sprite.Velocity.Y <Constants.ZERO) Sprite.Velocity *= new Vector2(1, -1);
                    Sprite.CurrentLocation += new Vector2(Constants.ZERO, otherBounds.Bottom - fireBallBounds.Top + 1);
                }
                else if (otherBounds.Right - fireBallBounds.Left <= 10)
                {
                    Sprite.CurrentLocation += new Vector2(otherBounds.Right - fireBallBounds.Left + 1,Constants.ZERO);
                    Sprite.Velocity *= new Vector2(Constants.ZERO,Constants.ZERO);
                    Consumed = true;
                }
                else
                {
                    Sprite.CurrentLocation -= new Vector2(fireBallBounds.Right - otherBounds.Left + 1,Constants.ZERO);
                    Sprite.Velocity *= new Vector2(Constants.ZERO,Constants.ZERO);
                    Consumed = true;
                }
            }
            else
            {
                Consumed = true;
            }
        }

        public bool CanCollide(IEntity other)
        {
            return !Consumed && (other is Block || other is WarpPipe || other is Pipe || other is Bowser || other is KoopaTroopa || other is Goomba || other is Piranha);
        }
    }
}
