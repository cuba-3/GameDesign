using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstMonoGame.Collision;
using FirstMonoGame.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FirstMonoGame.Entity
{
    class HealthBar : IEntity
    {
        public Vector2 Location { get { return Sprite.CurrentLocation; } set { Sprite.CurrentLocation = value; } }
        public bool Visible { get { return Sprite.Display; } set { Sprite.Display = value; } }
        public bool Checkable { get; set; }
        public bool VisBounding { get { return Sprite.VisibleBounding; } set { Sprite.VisibleBounding = value; } }
        public Vector2 OriginalLocation { get; set; }
        public bool OnScreen { get; set; }
        public ISprite Sprite { get; set; }
        private Texture2D SpriteSheet;
        public int Life;
        public ContentManager Content;
        private Bowser Bowser;
        private bool isConsumed;

        public HealthBar(ContentManager content, GraphicsDevice graphicsDevice, Bowser bowser)
        {
            Bowser = bowser;
            Life = 9;
            SpriteSheet = content.Load<Texture2D>("healthBars");
            Content = content;
            Sprite = new EnemySprite(graphicsDevice, SpriteSheet, 10, 1, new Vector2(bowser.Location.X + 5, bowser.Location.Y - 20), 9, 1);
            Sprite.InitializeSprite(SpriteSheet, graphicsDevice, 10, 1);
            Sprite = HealthBarSpriteFactory.Instance.FactoryMethod(Content, this, graphicsDevice);
            isConsumed = false;
        }

        public AABB BoundingBox()
        {
            return Sprite.BoundingBox;
        }

        public bool CanCollide(IEntity other)
        {
            return false;
        }

        public void DoCollision(IEntity collider)
        {
            return;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            Sprite.Update(gameTime);
            this.Location = new Vector2(Bowser.Location.X + 10, Bowser.Location.Y - 25);
            Life = Math.Abs(9 - Bowser.Damage);
            Sprite.SwitchAnimation(Life, 1);
            if(Life == 9)
            {
                isConsumed = true;
            }
            if (isConsumed)
            {
                Collections.Instance.GetCollisionRef().Deregister(this);
                Collections.Instance.GetLevelRef().Entities.Remove(this);
            }
        }
    }
}
