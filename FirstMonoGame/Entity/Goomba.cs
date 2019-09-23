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

namespace FirstMonoGame
{
    class Goomba : IEntity
    {
        public Vector2 OriginalLocation { get; set; }
        public IEnemyState<Goomba> EnemyState { get; set; }
        public Vector2 Location { get { return Sprite.CurrentLocation; } set { Sprite.CurrentLocation = value; } }
        public bool VisBounding { get { return Sprite.VisibleBounding; } set { Sprite.VisibleBounding = value; } }
        public bool Visible { get { return Sprite.Display; } set { Sprite.Display = value; } }
        public bool Checkable { get; set; }
        public ISprite Sprite { get; set; }
        public ContentManager Content;
        public GraphicsDevice GraphicsDevice;
        public bool OnScreen { get; set; }
        private bool isConsumed;
        private bool activated;


        public Goomba(ContentManager content, GraphicsDevice graphicsDevice)
        {
            this.EnemyState = new StandardGoombaState();
            Content = content;
            GraphicsDevice = graphicsDevice;
            Sprite = GoombaSpriteFactory.Instance.FactoryMethod(content, this, graphicsDevice);
            isConsumed = false;
            activated = true;
        }

        public void Update(GameTime gameTime)
        {
            Sprite.Update(gameTime);
            if (OnScreen)
            {
                activated = false;
                Sprite.Acceleration = new Vector2(Sprite.Acceleration.X, (float)1300.0);
            } 

            OnScreen = Camera.Instance.IsOnScreen(new Rectangle { X = (int)BoundingBox().Left, Y = (int)BoundingBox().Top, Width = (int)BoundingBox().Width, Height = (int)BoundingBox().Height });

            if (!activated)
            {

            }
            else if (OnScreen is true)
            {
                Sprite.Velocity = new Vector2(-100,Constants.ZERO);
            }

            if (isConsumed)
            {
                Collections.Instance.GetCollisionRef().Deregister(this);
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
            if (entity is Peach peach)
            {
                AABB goombaBox = Sprite.BoundingBox;
                AABB peachBox = peach.Sprite.BoundingBox;

                if ((peachBox.Bottom - goombaBox.Top <= 10 && EnemyState is StandardGoombaState) || peach.PowerUpState is StarState) // peach is falling onto a live goomba
                {
                    TakeDamageTransition();
                    isConsumed = true;
                }
            }
            else if ((entity is Block block && block.Visible) || entity is Pipe || entity is WarpPipe)
            {
                AABB goombaBounds = Sprite.BoundingBox;
                AABB blockBounds = entity.BoundingBox();
                if (!blockBounds.MinkowskiDifferenceContainsOrigin(goombaBounds)) return;
                if (goombaBounds.Bottom - blockBounds.Top <= 10)
                {
                    if (entity is Block b && b.Visible && b.Sprite.Velocity.Y <Constants.ZERO)
                    {
                        if (b is BrickBlock br)
                        {
                            br.BumpedBy.PlayerStats.Score += 100;
                            EnemyState = new DeadGoombaState();
                            Sprite.VReflect = true;
                        }
                        else if (b is HiddenBlock h)
                        {
                            h.BumpedBy.PlayerStats.Score += 100;
                            EnemyState = new DeadGoombaState();
                            Sprite.VReflect = true;
                        }
                        else if (b is QuestionBlock q)
                        {
                            q.BumpedBy.PlayerStats.Score += 100;
                            EnemyState = new DeadGoombaState();
                            Sprite.VReflect = true;
                        }
                    }
                    Sprite.Velocity = new Vector2(Sprite.Velocity.X, entity.Sprite.Velocity.Y);
                    if (Sprite.Velocity.Y >Constants.ZERO) Sprite.Velocity *= new Vector2(1,Constants.ZERO);
                    Sprite.CurrentLocation -= new Vector2(Constants.ZERO, goombaBounds.Bottom - blockBounds.Top + 1);
                }
                else if (blockBounds.Bottom - goombaBounds.Top <= 10)
                {
                    Sprite.Velocity *= new Vector2(1, -1);
                    Sprite.CurrentLocation += new Vector2(Constants.ZERO, blockBounds.Bottom - goombaBounds.Top + 1);
                }
                else if (goombaBounds.Right - blockBounds.Left <= 10)
                {
                    Sprite.Velocity *= new Vector2(-1, 1);
                    Sprite.CurrentLocation -= new Vector2(goombaBounds.Right - blockBounds.Left + 1,Constants.ZERO);
                    Sprite.HReflect = !Sprite.HReflect;
                }
                else
                {
                    Sprite.Velocity *= new Vector2(-1, 1);
                    Sprite.CurrentLocation += new Vector2(blockBounds.Right - goombaBounds.Left + 1,Constants.ZERO);
                    Sprite.HReflect = !Sprite.HReflect;
                }
            }
            else if (entity is FireBall fb)
            {
                fb.Shooter.PlayerStats.Score += 100;
                EnemyState = new DeadGoombaState();
                Sprite.VReflect = true;
            }
        }

        public bool CanCollide(IEntity other)
        {
            return !(EnemyState is DeadGoombaState) && (other is Peach || other is FireBall || other is Block || other is Pipe || other is WarpPipe);
        }
    }
}