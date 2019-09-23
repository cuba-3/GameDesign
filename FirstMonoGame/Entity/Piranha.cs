using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FirstMonoGame.Entity;
using FirstMonoGame.Collision;
using FirstMonoGame.WorldScrolling;
using FirstMonoGame.Interfaces;
using Microsoft.Xna.Framework.Content;
using FirstMonoGame.Sprites;
using System.Diagnostics;
using FirstMonoGame.States2.PowerUpStates;

namespace FirstMonoGame.Entity
{
    public class Piranha : IEntity
    {
        public Vector2 OriginalLocation { get; set; }
        public Vector2 Location { get { return Sprite.CurrentLocation; } set { Sprite.CurrentLocation = value; } }
        public bool VisBounding { get { return Sprite.VisibleBounding; } set { Sprite.VisibleBounding = value; } }
        public bool Visible { get { return Sprite.Display; } set { Sprite.Display = value; } }
        public bool Checkable { get; set; }
        public ISprite Sprite { get; set; }
        public ContentManager Content { get; set; }
        public GraphicsDevice GraphicsDevice { get; set; }
        public bool OnScreen { get; set; }
        private bool isConsumed;
        private Peach Peach;
        private bool activated; // TREAT ACTIVATED AS A WAY TO TELL IF PEACH IS NEXT TO OR ON TOP OF WARP PIPE
        public Piranha(ContentManager content, GraphicsDevice graphics, Peach peach)
        {
            Content = content;
            GraphicsDevice = graphics;
            Sprite = PiranhaSpriteFactory.Instance.FactoryMethod(content, this, graphics);
            isConsumed = false;
            activated = true;
            Peach = peach;
        }

        public AABB BoundingBox()
        {
            return new AABB(Location.X + 15, Location.Y, 25, 72);
        }

        public void DoCollision(IEntity collider)
        {
            if (collider is Peach p && p.PowerUpState is StarState)
            {
                isConsumed = true;
                p.PlayerStats.Score += 200;
            }
            if (collider is FireBall fb)
            {
                isConsumed = true;
                fb.Shooter.PlayerStats.Score += 200;
            }
        }

        public bool CanCollide(IEntity other)
        {
            return (other is Peach || other is FireBall) && !isConsumed;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            // max upwards is y = 627
            Sprite.Update(gameTime);
            OnScreen = Camera.Instance.IsOnScreen(new Rectangle { X = (int)BoundingBox().X, Y = (int)BoundingBox().Y, Width = (int)BoundingBox().Width, Height = (int)BoundingBox().Height });
            if(Math.Abs(Peach.Location.X - this.Location.X) >= 300)
            {
                SetActive(true);
            }
            else if (Math.Abs(Peach.Location.X-this.Location.X) < 300)
            {
                SetActive(false);
                //Debug.WriteLine("Set active to false");
            }

            if (activated && OnScreen)
            {
                if (Location.Y <= OriginalLocation.Y - 72)
                {
                    Sprite.Velocity = new Vector2(Sprite.Velocity.X, 25);
                }
                else if (Location.Y >= OriginalLocation.Y)
                {
                    Sprite.Velocity = new Vector2(Sprite.Velocity.X, -25);
                }
            }
            else
            {
                Sprite.Velocity = new Vector2(Sprite.Velocity.X, 25);
                if(Location.Y >= OriginalLocation.Y + 20)
                {
                    Sprite.Velocity = new Vector2(Constants.ZERO,Constants.ZERO);
                }
            }

            if (isConsumed)
            {
                Collections.Instance.GetCollisionRef().Deregister(this);
                Collections.Instance.GetLevelRef().Entities.Remove(this);
            }
        }
        
        public void SetConsumed(bool consumed)
        {
            isConsumed = consumed;
        }

        public void SetActive(bool active)
        {
            activated = active;
        }
    }
}
