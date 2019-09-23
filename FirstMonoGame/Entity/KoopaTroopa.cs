using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstMonoGame.States.EnemyStates;
using FirstMonoGame.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using FirstMonoGame.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using FirstMonoGame.Entity;
using FirstMonoGame.Collision;
using FirstMonoGame.WorldScrolling;
using FirstMonoGame.States2.PowerUpStates;

namespace FirstMonoGame
{
    class KoopaTroopa : IEntity
    {
        public Vector2 OriginalLocation { get; set; }
        public IEnemyState<KoopaTroopa> EnemyState { get; set; }
        public ISprite Sprite { get; set; }
        public Vector2 Location { get { return Sprite.CurrentLocation; } set { Sprite.CurrentLocation = value; } }
        public bool Visible { get { return Sprite.Display; } set { Sprite.Display = value; } }
        public bool Checkable { get; set; }
        public bool VisBounding { get { return Sprite.VisibleBounding; } set { Sprite.VisibleBounding = value; } }
        public bool OnScreen { get; set; }
        private bool isConsumed;
        private bool activated;
        public GraphicsDevice GraphicsDevice;
        public ContentManager Content;
        private static Texture2D GreenKoopaSheet;
        private static Texture2D RedKoopaSheet;

        public KoopaTroopa(ContentManager content, GraphicsDevice graphicsDevice, bool red)
        {
            this.EnemyState = new StandardKoopaTroopaState();
            GreenKoopaSheet = content.Load<Texture2D>("GreenKoopaSheet");
            RedKoopaSheet = content.Load<Texture2D>("RedKoopaSheet");
            if (red)
            {
                Sprite = new EnemySprite(graphicsDevice, RedKoopaSheet, 2, 2, new Vector2(1200, 738),Constants.ZERO, 2);
            }
            else
            {
                Sprite = new EnemySprite(graphicsDevice, GreenKoopaSheet, 2, 2, new Vector2(1200, 738),Constants.ZERO, 2);
            }
            Content = content;
            GraphicsDevice = graphicsDevice;
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
                Collections.Instance.GetLevelRef().Entities.Remove(this);
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
            this.Sprite.RectangleTexture.SetData<Color>(new Color[] { Color.DarkRed });

            if (entity is Peach peach)
            {
                AABB troopaBounds = BoundingBox();
                AABB peachBounds = entity.BoundingBox();
                if (peachBounds.Bottom - troopaBounds.Top <= 10 || peach.PowerUpState is StarState)
                {
                    if (EnemyState is ShellKoopaState || peach.PowerUpState is StarState) isConsumed = true;
                    TakeDamageTransition();
                }
            }
            else if ((entity is Block block && block.Visible) || entity is Pipe || entity is WarpPipe)
            {
                AABB troopaBounds = BoundingBox();
                AABB blockBounds = entity.BoundingBox();
                if (!blockBounds.MinkowskiDifferenceContainsOrigin(troopaBounds)) return;
                if (troopaBounds.Bottom - blockBounds.Top <= 10)
                {
                    if (entity is Block b && b.Visible && b.Sprite.Velocity.Y <Constants.ZERO)
                    {
                        if (b is BrickBlock br)
                        {
                            br.BumpedBy.PlayerStats.Score += 100;
                            EnemyState = new DeadKoopaTroopaState();
                            Sprite.VReflect = true;
                        }
                        else if (b is HiddenBlock h)
                        {
                            h.BumpedBy.PlayerStats.Score += 100;
                            EnemyState = new DeadKoopaTroopaState();
                            Sprite.VReflect = true;
                        }
                        else if (b is QuestionBlock q)
                        {
                            q.BumpedBy.PlayerStats.Score += 100;
                            EnemyState = new DeadKoopaTroopaState();
                            Sprite.VReflect = true;
                        }
                    }
                    Sprite.Velocity = new Vector2(Sprite.Velocity.X, entity.Sprite.Velocity.Y);
                    if (Sprite.Velocity.Y >Constants.ZERO) Sprite.Velocity *= new Vector2(1,Constants.ZERO);
                    Sprite.CurrentLocation -= new Vector2(Constants.ZERO, troopaBounds.Bottom - blockBounds.Top + 1);
                }
                else if (blockBounds.Bottom - troopaBounds.Top <= 10)
                {
                    Sprite.Velocity *= new Vector2(1, -1);
                    Sprite.CurrentLocation += new Vector2(Constants.ZERO, blockBounds.Bottom - troopaBounds.Top + 1);
                }
                else if (troopaBounds.Right - blockBounds.Left <= 10)
                {
                    Sprite.Velocity *= new Vector2(-1, 1);
                    Sprite.CurrentLocation -= new Vector2(troopaBounds.Right - blockBounds.Left + 1,Constants.ZERO);
                    Sprite.HReflect = !Sprite.HReflect;
                }
                else
                {
                    Sprite.Velocity *= new Vector2(-1, 1);
                    Sprite.CurrentLocation += new Vector2(blockBounds.Right - troopaBounds.Left + 1,Constants.ZERO);
                    Sprite.HReflect = !Sprite.HReflect;
                }
            }
            else if (entity is FireBall fb)
            {
                fb.Shooter.PlayerStats.Score += 100;
                EnemyState = new DeadKoopaTroopaState();
                Sprite.VReflect = true;
            }
        }

        public bool CanCollide(IEntity other)
        {
            return !isConsumed && !(EnemyState is DeadKoopaTroopaState) && (other is Peach || other is FireBall || other is Block || other is Pipe || other is WarpPipe);
        }
    }
}
