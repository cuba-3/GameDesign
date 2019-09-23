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
    class Pipe : IEntity
    {
        public Vector2 OriginalLocation { get; set; }
        public Vector2 Location { get { return Sprite.CurrentLocation; } set { Sprite.CurrentLocation = value; } }
        public bool Visible { get { return Sprite.Display; } set { Sprite.Display = value; } }
        public bool Checkable { get { return Sprite.Checkable; } set { Sprite.Checkable = value; } }
        public ISprite Sprite { get; set; }
        public bool VisBounding { get { return Sprite.VisibleBounding; } set { Sprite.VisibleBounding = value; } }
        public bool OnScreen { get; set; }

        public Pipe(ContentManager content, GraphicsDevice graphics)
        {
            Sprite = PipeSpriteFactory.Instance.FactoryMethod(content, this, graphics, false);
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
            return;
        }

        public bool CanCollide(IEntity other)
        {
            return true;
        }
    }
}

