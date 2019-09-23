using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstMonoGame.Collision;
using FirstMonoGame.Interfaces;
using FirstMonoGame.Sprites;
using FirstMonoGame.States.EnemyStates;
using FirstMonoGame.States.PeachStates;
using FirstMonoGame.States2.PeachStates;
using FirstMonoGame.States2.PowerUpStates;
using FirstMonoGame.WorldScrolling;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FirstMonoGame.Entity
{
    class Bowser : IEntity
    {
        public Vector2 OriginalLocation { get; set; }
        public IEnemyState<Bowser> EnemyState { get; set; }
        public Vector2 Location { get { return Sprite.CurrentLocation; } set { Sprite.CurrentLocation = value; } }
        public bool VisBounding { get { return Sprite.VisibleBounding; } set { Sprite.VisibleBounding = value; } }
        public bool Visible { get { return Sprite.Display; } set { Sprite.Display = value; } }
        public bool Checkable { get; set; }
        public ISprite Sprite { get; set; }
        public ContentManager Content;
        public GraphicsDevice GraphicsDevice;
        public bool OnScreen { get; set; }
        private int Delay;
        public int Damage;
        private static Texture2D BowserSpriteSheet;
        static Random Random;
        public bool isConsumed;
        public HealthBar Health;
        
        public Bowser(ContentManager content, GraphicsDevice graphicsDevice)
        {
            Content = content;
            BowserSpriteSheet = Content.Load<Texture2D>("Bowser");
            Sprite = new EnemySprite(graphicsDevice, BowserSpriteSheet, 6, 17, OriginalLocation, 2, 1);
            Sprite.InitializeSprite(BowserSpriteSheet, graphicsDevice, 6, 17);
            Sprite.SwitchAnimation(2, 1);
            this.EnemyState = new StandardBowserState();
            GraphicsDevice = graphicsDevice;
            Sprite = BowserSpriteFactory.Instance.FactoryMethod(content, this, graphicsDevice);
            EnemyState = new StandardBowserState();
            Random = new Random();
            Delay =Constants.ZERO;
            Health = new HealthBar(content, graphicsDevice, this);
            Damage = 9;
            isConsumed = false;
        }

        public AABB BoundingBox()
        {
            return Sprite.BoundingBox;
        }

        public void PunchingTransition()
        {
            EnemyState.PunchingTransition(this);
        }

        public void TakeDamageTransition()
        {
            EnemyState.TakeDamageTransition(this);
        }

        public void WalkingTransition()
        {
            EnemyState.WalkingTransition(this);
        }

        public void BreatheFireTransition()
        {
            EnemyState.BreatheFireTransition(this);
        }

        public void StandardTransition()
        {
            EnemyState.StandardTransition(this);
        }

        public void DoCollision(IEntity entity)
        {
            if(EnemyState is DeadBowserState && entity is Peach peach2)
            {
                AABB goombaBox = Sprite.BoundingBox;
                AABB peachBox = peach2.Sprite.BoundingBox;

                if(peach2.PowerUpState is StarState) // Star peach kills shell
                {
                    this.isConsumed = true;
                }
                else if ((peachBox.Bottom - goombaBox.Top <= 5) && (peach2.ActionState is FallingState || peach2.ActionState is JumpingState)) // Any other peach lands on top of spikey shell -> dies
                {
                    peach2.TakeDamageTransition();
                }
                else // else peach bumps from side -> shell moves peach idles
                {
                    //this.Sprite.Velocity = new Vector2(peach2.Sprite.Velocity.X, this.Sprite.Velocity.Y);
                    peach2.IdleTransition();
                }
            }
            else if (entity is Peach peach && !(EnemyState is TakingDamageBowserState))
            {
                AABB goombaBox = Sprite.BoundingBox;
                AABB peachBox = peach.Sprite.BoundingBox;

                if ((peachBox.Bottom - goombaBox.Top <= 5) || peach.PowerUpState is StarState) // peach is falling onto Bowser
                {
                    Damage--;
                    TakeDamageTransition();
                    Sprite.Velocity = new Vector2(Constants.ZERO,Constants.ZERO);
                }
                else
                {
                    return;
                }
            }
            if ((entity is Block block && block.Visible) || entity is Pipe || entity is WarpPipe)
            {
                AABB bowserBounds = Sprite.BoundingBox;
                AABB blockBounds = entity.BoundingBox();
                if (bowserBounds.Bottom - blockBounds.Top <= 10) // colliding from above
                {
                    Sprite.Velocity *= new Vector2(1,Constants.ZERO);
                    Sprite.CurrentLocation -= new Vector2(Constants.ZERO, bowserBounds.Bottom - blockBounds.Top + 1);
                }
                else if (blockBounds.Bottom - bowserBounds.Top <= 10) // colliding from below
                {
                    Sprite.Velocity *= new Vector2(1, -1);
                    Sprite.CurrentLocation += new Vector2(Constants.ZERO, blockBounds.Bottom - bowserBounds.Top + 1);
                }
                else if (bowserBounds.Right - blockBounds.Left <= 10)
                {
                    Sprite.Velocity *= new Vector2(-1, 1);
                    Sprite.CurrentLocation -= new Vector2(bowserBounds.Right - blockBounds.Left + 1,Constants.ZERO);
                    Sprite.HReflect = !Sprite.HReflect;
                }
                else
                {
                    Sprite.Velocity *= new Vector2(-1, 1);
                    Sprite.CurrentLocation += new Vector2(blockBounds.Right - bowserBounds.Left + 1,Constants.ZERO);
                    Sprite.HReflect = !Sprite.HReflect;
                }
            }
            if(entity is FireBall)
            {
                Damage--;
                TakeDamageTransition();
                Sprite.Velocity *= new Vector2(Constants.ZERO,Constants.ZERO);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            int rand = Random.Next(Constants.ZERO, 3);
            Sprite.Update(gameTime);
            Delay++;

            OnScreen = Camera.Instance.IsOnScreen(new Rectangle { X = (int)BoundingBox().Left, Y = (int)BoundingBox().Top, Width = (int)BoundingBox().Width, Height = (int)BoundingBox().Height });
            if (OnScreen is true && Delay == 100 && !(EnemyState is DeadBowserState))
            {
                if(rand ==Constants.ZERO)
                {
                    WalkingTransition();
                    Sprite.Velocity = new Vector2(200,Constants.ZERO);
                    if (Sprite.HReflect) Sprite.Velocity *= new Vector2(-1, 1);
                }else if(rand == 1)
                {
                    PunchingTransition();
                    Sprite.Velocity = new Vector2(Constants.ZERO,Constants.ZERO);
                }
                else if(rand == 2)
                {
                    BreatheFireTransition();
                    Sprite.Velocity = new Vector2(Constants.ZERO,Constants.ZERO);
                }
                else
                {
                    StandardTransition();
                    Sprite.Velocity = new Vector2(Constants.ZERO,Constants.ZERO);
                }
                Delay =Constants.ZERO;
            }
            if (isConsumed)
            {
                Collections.Instance.GetCollisionRef().Deregister(this);
                Collections.Instance.GetLevelRef().Entities.Remove(this);
            }
        }

        public bool CanCollide(IEntity other)
        {
            return !(EnemyState is TakingDamageBowserState) && (other is Block || other is WarpPipe || other is Pipe || other is Peach || other is FireBall); 
        }
    }
}
