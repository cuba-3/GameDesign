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


namespace FirstMonoGame.Entity
{
    class Bush : IEntity
    {
        public Vector2 OriginalLocation { get; set; }
        public ISprite Sprite { get; set; }
        public Vector2 Location { get { return Sprite.CurrentLocation; } set { Sprite.CurrentLocation = value; } }
        public bool Visible { get { return Sprite.Display; } set { Sprite.Display = value; } }
        public bool VisBounding { get { return Sprite.VisibleBounding; } set { Sprite.VisibleBounding = value; } }
        public bool Checkable { get; set; }
        public bool OnScreen { get; set; }

        public Bush(ContentManager content, GraphicsDevice graphics)
        {
            Sprite = BushSpriteFactory.Instance.FactoryMethod(content, graphics, this);
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

        }

        public bool CanCollide(IEntity other)
        {
            return false;
        }
    }
}
