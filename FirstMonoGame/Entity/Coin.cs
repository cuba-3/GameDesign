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
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;
using FirstMonoGame.WorldScrolling;

namespace FirstMonoGame.Entity
{
    class Coin : IEntity
    {
        public Vector2 OriginalLocation { get; set; }
        public ISprite Sprite { get; set; }
        public Vector2 Location { get { return Sprite.CurrentLocation; } set { Sprite.CurrentLocation = value; } }
        public bool Visible { get { return Sprite.Display; } set { Sprite.Display = value; } }
        public bool VisBounding { get { return Sprite.VisibleBounding; } set { Sprite.VisibleBounding = value; } }
        public bool Checkable { get; set; }
        public bool isConsumed;
        public bool OnScreen { get; set; }

        public Coin(ContentManager content, GraphicsDevice graphicsDevice) 
        {
            Sprite = CoinSpriteFactory.Instance.FactoryMethod(content, graphicsDevice, this);
            isConsumed = false;
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
            if (entity is Mario mario)
            {
                mario.PlayerStats.CoinRoomCoin++;
                isConsumed = true;
            }
            else if(entity is Peach)
            {
                isConsumed = true;
            }
        }

        public bool CanCollide(IEntity other)
        {
            return !isConsumed && (other is Peach || other is Mario);
        }
    }
}
