using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstMonoGame.Collision;
using FirstMonoGame.Sprites;
using FirstMonoGame.WorldScrolling;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FirstMonoGame.Entity
{
    class CGrade : IEntity
    {
        public Vector2 OriginalLocation { get; set; }
        public ISprite Sprite { get; set; }
        public Vector2 Location { get { return Sprite.CurrentLocation; } set { Sprite.CurrentLocation = value; } }
        public bool Visible { get { return Sprite.Display; } set { Sprite.Display = value; } }
        public bool VisBounding { get { return Sprite.VisibleBounding; } set { Sprite.VisibleBounding = value; } }
        public bool Checkable { get; set; }
        public bool OnScreen { get; set; }
        bool isConsumed;

        public CGrade(ContentManager content, GraphicsDevice graphicsDevice)
        {
            Sprite = (ItemSprite)CGradeSpriteFactory.Instance.FactoryMethod(content, graphicsDevice);
            isConsumed = false;
        }
        public AABB BoundingBox()
        {
            return Sprite.BoundingBox;
        }

        public void DoCollision(IEntity collider)
        {
            if (collider is Peach)
            {
                //sound = peach.Content.Load<SoundEffect>("Sound Effects/Collect Coin");
                //sound.Play();
                isConsumed = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            Sprite.Update(gameTime);
            if (!Camera.Instance.IsOnScreen(new Rectangle { X = (int)BoundingBox().Left, Y = (int)BoundingBox().Top, Width = (int)BoundingBox().Width, Height = (int)BoundingBox().Height }))
            {
                isConsumed = true;
            }
            if (isConsumed)
            {
                Collections.Instance.GetCollisionRef().Deregister(this);
                Collections.Instance.GetLevelRef().Entities.Remove(this);
            }
        }

        public bool CanCollide(IEntity other)
        {
            return other is Peach && !isConsumed;
        }
    }
}
