using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstMonoGame.States.EnemyStates;
using FirstMonoGame.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using FirstMonoGame.Sprites;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using FirstMonoGame.Entity;
using FirstMonoGame.Collision;
using FirstMonoGame.WorldScrolling;
using FirstMonoGame.States2.PowerUpStates;
using System.Diagnostics;

namespace FirstMonoGame
{
    public class Mario : IEntity
    {
        public Vector2 OriginalLocation { get; set; }
        public IEnemyState<Mario> EnemyState { get; set; }
        public Vector2 Location { get { return Sprite.CurrentLocation; } set { Sprite.CurrentLocation = value; } }
        public bool VisBounding { get { return Sprite.VisibleBounding; } set { Sprite.VisibleBounding = value; } }
        public bool Visible { get { return Sprite.Display; } set { Sprite.Display = value; } }
        public bool Checkable { get; set; }
        public ISprite Sprite { get; set; }
        public bool OnScreen { get; set; }
        private int Delay;
        public PlayerStats PlayerStats { get; set; }
        public Mario(ContentManager content, GraphicsDevice graphicsDevice)
        {
            Sprite = (EnemySprite)MarioSpriteFactory.Instance.FactoryMethod(content, this, graphicsDevice);
            Delay =Constants.ZERO;
            Checkable = true;
        }

        public void Update(GameTime gameTime)
        {
            Delay++;
            Sprite.Update(gameTime);
            if (OnScreen)
            {
                Sprite.Acceleration = new Vector2(Sprite.Acceleration.X, (float)1300.0);
            }
            OnScreen = Camera.Instance.IsOnScreen(new Rectangle { X = (int)BoundingBox().Left, Y = (int)BoundingBox().Top, Width = (int)BoundingBox().Width, Height = (int)BoundingBox().Height });
            if (Delay >Constants.ZERO && Delay < 100)
            {
                Sprite.Velocity = new Vector2(-200,Constants.ZERO);
            }
            else if (Delay > 101 && Delay < 200)
            {
                Sprite.Velocity = new Vector2(200,Constants.ZERO);
            }
            else if (Delay > 201 && Delay < 250)
            {
                Sprite.Velocity = new Vector2(Constants.ZERO, -400);
            }
            else if (Delay > 251 && Delay < 300)
            {
                Sprite.Velocity = new Vector2(Constants.ZERO, 400);
            }
            else if(Delay > 301 && Delay < 500)
            {
                Sprite.Velocity = new Vector2(-200,Constants.ZERO);
            }
            else if(Delay > 501 && Delay < 700)
            {
                Sprite.Velocity = new Vector2(200,Constants.ZERO);
            }
            else if(Delay > 701 && Delay < 800)
            {
                Sprite.Velocity = new Vector2(Constants.ZERO, -400);
            }
            else if(Delay > 801 && Delay < 900)
            {
                Sprite.Velocity = new Vector2(Constants.ZERO, 400);
            }
            else if (Delay > 901 && Delay < 1000)
            {
                Sprite.Velocity = new Vector2(-200,Constants.ZERO);
            }
            else if (Delay > 1001 && Delay < 1100)
            {
                Sprite.Velocity = new Vector2(200,Constants.ZERO);
            }
            else if (Delay > 1101 && Delay < 1200)
            {
                Sprite.Velocity = new Vector2(Constants.ZERO, -400);
            }
            else if (Delay > 1201 && Delay < 1300)
            {
                Sprite.Velocity = new Vector2(Constants.ZERO, 400);
            }
            else if (Delay > 1301 && Delay < 1400)
            {
                Sprite.Velocity = new Vector2(-200,Constants.ZERO);
            }
            else if (Delay > 1401 && Delay < 1500)
            {
                Sprite.Velocity = new Vector2(200,Constants.ZERO);
            }
            else if (Delay > 1501 && Delay < 1550)
            {
                Sprite.Velocity = new Vector2(Constants.ZERO, -400);
            }
            else if (Delay > 1551 && Delay < 1600)
            {
                Sprite.Velocity = new Vector2(Constants.ZERO, 400);
            }
            else if (Delay > 1601 && Delay < 1650)
            {
                Sprite.Velocity = new Vector2(-200,Constants.ZERO);
            }
            else if (Delay > 1651 && Delay < 1700)
            {
                Sprite.Velocity = new Vector2(200,Constants.ZERO);
            }
            else
            {
                Sprite.Velocity = new Vector2(Constants.ZERO,Constants.ZERO);
            }
            

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch);
        }

        public void TakeDamageTransition()
        {
            EnemyState.TakeDamageTransition(this);
        }

        public AABB BoundingBox()
        {
            return Sprite.BoundingBox;
        }

        public void DoCollision(IEntity entity)
        {
            if (entity is Coin)
            {
                if (PlayerStats.CoinRoom)
                {
                    PlayerStats.CollectCoin();
                    if (PlayerStats.CoinRoom)
                    {
                        PlayerStats.CoinRoomCoin++;
                    }
                }
            }
            
        }

        public bool CanCollide(IEntity other)
        {
            return (other is Coin);
        }
    }
}
