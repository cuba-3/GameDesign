using FirstMonoGame.Collision;
using FirstMonoGame.Entity;
using FirstMonoGame.States.PeachStates;
using FirstMonoGame.States2.PeachStates;
using FirstMonoGame.States2.PowerUpStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using FirstMonoGame.WorldScrolling;
using FirstMonoGame.States.ItemStates;
using FirstMonoGame.States.BlockStates;
using Microsoft.Xna.Framework.Input;
using FirstMonoGame.Commands;
using FirstMonoGame.States.PowerUpStates;
using FirstMonoGame.States.EnemyStates;
using System;

namespace FirstMonoGame
{
    public class Peach : IEntity
    {
        public Vector2 OriginalLocation { get; set; }
        public IActionState<Peach> ActionState { get; set; }
        public IPowerUpState<Peach> PowerUpState { get; set; }
        public Vector2 Location { get { return Sprite.CurrentLocation; } set { Sprite.CurrentLocation = value; } }
        public Vector2 Acceleration { get { return Sprite.Acceleration; } set { Sprite.Acceleration = value; } }
        public static float G { get { return (float)1300.0; } }
        public bool Visible { get { return Sprite.Display; } set { Sprite.Display = value; } }
        public bool Checkable { get; set; }
        public bool VisBounding { get { return Sprite.VisibleBounding; } set { Sprite.VisibleBounding = value; } }
        public ContentManager Content { get; set; }
        private GraphicsDevice graphics;
        public ISprite Sprite { get; set; }
        private static Texture2D StandardSpriteSheet;
        private static Texture2D FireSpriteSheet;
        private static Texture2D SuperSpriteSheet;
        private static Texture2D StarSpriteSheet;
        private static Texture2D fireBallPink;
        private static Texture2D fireBallStar;
        private static Texture2D fireBall;
        private static SoundEffect KillGoomba;
        public PlayerStats PlayerStats { get; set; }
        public bool OnScreen { get; set; }
        public bool Descending { get; set; }
        public bool Ascending { get; set; }
        public Vector2 LocationAbove { get; set; }
        public Vector2 LocationBelow { get; set; }
        public WarpPipe ExitPipe { get; set; }
        public WarpPipe EntrancePipe { get; set; }
        public IPowerUpState<Peach> StateBeforePipe { get; set; }
        private bool OnGround { get; set; }
        private bool Invincible { get; set; }
        struct frameCounter
        {
            public int frames;
            public int currentFrame;
            public bool enabled;
        }
        frameCounter frameCountA;
        public Peach(ContentManager content, GraphicsDevice graphicsDevice)
        { 
            Content = content;
            graphics = graphicsDevice;
            StandardSpriteSheet = content.Load<Texture2D>("Peach/StandardPeachSpriteSheet");
            FireSpriteSheet = content.Load<Texture2D>("Peach/FirePeachSpriteSheet");
            SuperSpriteSheet = content.Load<Texture2D>("Peach/SuperPeachSpriteSheet");
            StarSpriteSheet = content.Load<Texture2D>("Peach/StarPeachSpriteSheet");
            fireBallPink = content.Load<Texture2D>("FireBallPink");
            fireBallStar = content.Load<Texture2D>("FireBallStar");
            fireBall = content.Load<Texture2D>("FireBall");
            KillGoomba = content.Load<SoundEffect>("Sound Effects/Stomp");
            Sprite = new AvatarSprite(graphicsDevice);
            Sprite.InitializeSprite(StandardSpriteSheet, graphicsDevice, 6, 6);
            Sprite.SwitchAnimation(Constants.ZERO, 6);
            PowerUpState = new StandardState(new StandardState(null));
            Checkable = true;
            ActionState = new IdleState(this);
            Sprite.Acceleration = new Vector2(Constants.ZERO, G);
            OnGround = true;
            frameCountA.frames = 100; // invincibility ~ 1.5 s @ 60 fps
            frameCountA.currentFrame =Constants.ZERO;
            frameCountA.enabled = false;
            Invincible = false;
        }

        public void Update(GameTime gameTime)
        {
            if (frameCountA.enabled) frameCountA.currentFrame++;
            if (frameCountA.currentFrame > frameCountA.frames)
            {
                frameCountA.enabled = false;
                frameCountA.currentFrame =Constants.ZERO;
                Invincible = false;
            }
            float allowedV = float.PositiveInfinity;
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (dt !=Constants.ZERO) allowedV = 1 / dt; // can she move 1 px in yhat by self?
            if (Sprite.Velocity.Y >= allowedV || ActionState is JumpingState) OnGround = false;
            CheckForFallingDeath();
            if (Invincible && !(PowerUpState is WarpingState))
            {
                if (frameCountA.currentFrame % 10 > 4)
                {
                    Sprite.Display = true;
                }
                else
                {
                    Sprite.Display = false;
                }
            }
            else if (!Sprite.Display)
            {
                Sprite.Display = true;
            }
            Sprite.Update(gameTime);
            if (!(Sprite.Velocity.Y <Constants.ZERO && ActionState is JumpingState) && !OnGround)
            {
                FallTransition();
            } 
            if (ActionState is JumpingState || ActionState is FallingState) //aerial movement
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    Sprite.Velocity += new Vector2(7f,Constants.ZERO);
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    Sprite.Velocity += new Vector2(-7f,Constants.ZERO);
                }
                if (Sprite.Velocity.X > 300) Sprite.Velocity = new Vector2(300, Sprite.Velocity.Y);
                if (Sprite.Velocity.X < -300) Sprite.Velocity = new Vector2(-300, Sprite.Velocity.Y);
                if (Sprite.Velocity.Y > 900) Sprite.Velocity = new Vector2(Sprite.Velocity.X, 900); // terminal fall velocity Y
            }

            if (Descending && ActionState is CrouchingState)
            {
                StateBeforePipe = PowerUpState;
                OnGround = true;
                if (ExitPipe.MiniGame == 1)
                {
                    PlayerStats.GradeRoom = true;
                }
                if (ExitPipe.MiniGame == 2)
                {
                    PlayerStats.CoinRoom = true;
                }
                if(Location.Y < LocationAbove.Y + 100)
                {
                    Sprite.Velocity = new Vector2(Constants.ZERO, 25);
                }
                if (!ExitPipe.Underworld)
                {
                    PlayerStats.GradeRoom = false;
                    PlayerStats.CoinRoom = false;
                }
                Descending = false;
                Ascending = true;
                AscendCommand command = new AscendCommand(this, ExitPipe);
                command.Execute();                
            }

            if (Ascending)
            {
                Descending = false;
                OnGround = true;
                //Debug.WriteLine("Location Below Y: " + this.LocationBelow.Y);
                if(Location.Y > LocationBelow.Y - 112) //&& PowerUpState is WarpingState)
                {                
                    Sprite.Velocity = new Vector2(Constants.ZERO, -25);
                    Location += new Vector2(Constants.ZERO, -115);
                }
                    PowerUpState = this.StateBeforePipe;
                    Sprite.Velocity = new Vector2(Constants.ZERO, Constants.ZERO);
                    Collections.Instance.GetCollisionRef().Register(ExitPipe);
                    Collections.Instance.GetCollisionRef().StartDetection(10000, 5000);
                    Ascending = false; 
                    //if (!ExitPipe.Underworld)
                    //{
                        
                    //}
                
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch);
        }

        public AABB BoundingBox()
        {
            if (PowerUpState is StandardState || ActionState is CrouchingState)
            {
                AvatarSprite peachSprite = (AvatarSprite)Sprite;
                return new AABB { X = Sprite.CurrentLocation.X + 25, Y = Sprite.CurrentLocation.Y + 28, Width = peachSprite.Scale * 45, Height = peachSprite.Scale * 80 - 10};
            }
            else
            {
                AvatarSprite peachSprite = (AvatarSprite)Sprite;
                return new AABB { X = Sprite.CurrentLocation.X + 25, Y = Sprite.CurrentLocation.Y + 18, Width = peachSprite.Scale * 45, Height = peachSprite.Scale * 80 };
            }
        }

        public void DoCollision(IEntity entity)
        {
            this.Sprite.RectangleTexture.SetData<Color>(new Color[] { Color.YellowGreen });
            AABB peachBox = BoundingBox();
            AABB otherBox = entity.BoundingBox();
            if (entity is Goomba || entity is Bowser)
            {
                if (entity is Bowser bowser && bowser.EnemyState is DeadBowserState)
                {
                    if (!(this.ActionState is FallingState || this.ActionState is JumpingState))
                    {
                        bowser.Sprite.Velocity = new Vector2(this.Sprite.Velocity.X, bowser.Sprite.Velocity.Y);
                        IdleTransition();
                        Sprite.CurrentLocation -= new Vector2(peachBox.Right - otherBox.Left + 1,Constants.ZERO);
                    } else if(!(this.PowerUpState is StarState))
                    {
                        TakeDamageTransition();
                    }
                }
                else if (peachBox.Bottom - otherBox.Top <= 10 && !(PowerUpState is StarState || PowerUpState is DeadState))
                {
                    PlayerStats.Score += 100;
                    KillGoomba.Play();
                    ActionState = new JumpingState(ActionState, this);
                    Sprite = PeachSpriteFactory.Instance.FactoryMethod(this);
                    Sprite.Velocity = new Vector2(Sprite.Velocity.X, -400f); // override jump vy
                    Sprite.CurrentLocation -= new Vector2(Constants.ZERO, peachBox.Bottom - otherBox.Top + 1);
                }
                else
                {
                    if (PowerUpState is StarState) PlayerStats.Score += 100;
                    TakeDamageTransition();
                }
            }
            else if (entity is KoopaTroopa koopaTroopa)
            {
                if (peachBox.Bottom - otherBox.Top <= 10 && !(PowerUpState is StarState || PowerUpState is DeadState))
                {
                    if (!(koopaTroopa.EnemyState is ShellKoopaState)) PlayerStats.Score += 100;
                    ActionState = new JumpingState(ActionState, this);
                    Sprite = PeachSpriteFactory.Instance.FactoryMethod(this);
                    Sprite.Velocity = new Vector2(Sprite.Velocity.X, -400f); // override jump vy
                    Sprite.CurrentLocation -= new Vector2(Constants.ZERO, peachBox.Bottom - otherBox.Top + 1);
                }
                else
                {
                    if (PowerUpState is StarState) PlayerStats.Score += 100;
                    TakeDamageTransition();
                }
            }
            else if(entity is Piranha)
            {
                TakeDamageTransition();
            }
            else if ((entity is Block block && block.Visible) || entity is Pipe)
            {
                if (!otherBox.MinkowskiDifferenceContainsOrigin(peachBox)) return;
                if (peachBox.Bottom - otherBox.Top <= 6)
                {
                    if (ActionState is FallingState) IdleTransition();
                    Sprite.CurrentLocation -= new Vector2(Constants.ZERO, peachBox.Bottom - otherBox.Top + 1);
                    if (!(ActionState is JumpingState))Sprite.Velocity *= new Vector2(1,Constants.ZERO);
                    OnGround = true;
                }
                else if (otherBox.Bottom - peachBox.Top <= 10)
                {
                    Debug.WriteLine("depth from bottom " + (otherBox.Bottom - peachBox.Top));
                    Sprite.CurrentLocation += new Vector2(Constants.ZERO, otherBox.Bottom - peachBox.Top + 1);
                    if (Sprite.Velocity.Y <Constants.ZERO) Sprite.Velocity *= new Vector2(1, -1);
                }
                else if (otherBox.Right - peachBox.Left <= 10)
                {
                    Sprite.CurrentLocation += new Vector2(otherBox.Right - peachBox.Left + 1,Constants.ZERO);
                    Sprite.Velocity *= new Vector2(Constants.ZERO, 1);
                }
                else
                {
                    Sprite.CurrentLocation -= new Vector2(peachBox.Right - otherBox.Left + 1,Constants.ZERO);
                    Sprite.Velocity *= new Vector2(Constants.ZERO, 1);
                }
            }
            else if (entity is HiddenBlock)
            {
                if (!otherBox.MinkowskiDifferenceContainsOrigin(peachBox)) return;
                if (otherBox.Bottom - peachBox.Top <= 10)
                {
                    if (Sprite.Velocity.Y <Constants.ZERO)
                    {
                        Sprite.Velocity = new Vector2(Sprite.Velocity.X, Sprite.Velocity.Y * -1);
                        Sprite.CurrentLocation = new Vector2(Sprite.CurrentLocation.X, Sprite.CurrentLocation.Y + otherBox.Bottom - peachBox.Top + 1);
                    }
                }
            }
            else if (entity is Coin)
            {
                PlayerStats.CollectCoin();
                if (PlayerStats.CoinRoom) {
                    PlayerStats.CoinRoomCoin++;
                }
            }
            else if (entity is FireFlower)
            {
                FirePowerUpTransition();
            }
            else if (entity is CheckpointFlag)
            {
                PlayerStats.PassCheckpoint(entity.Sprite.CurrentLocation);
            }
            else if (entity is Flag flag)
            {
                if (flag.FlagState is FlagNewState) Sprite.Velocity *= new Vector2(Constants.ZERO, Constants.ZERO);
                if (Location.Y >=Constants.ZERO && Location.Y < 275) PlayerStats.GainLife();
                else if (Location.Y >= 275 && Location.Y < 322) PlayerStats.FlagPoints = 4000; //128-153
                else if (Location.Y >= 322 && Location.Y < 412) PlayerStats.FlagPoints = 2000; //82-127
                else if (Location.Y >= 340 && Location.Y < 458) PlayerStats.FlagPoints = 800; //58-81
                else if (Location.Y >= 458 && Location.Y < 536) PlayerStats.FlagPoints = 400; //18-57
                else PlayerStats.FlagPoints = 100; //0-17
                PlayerStats.Victory = true;
            }
            else if (entity is OneUpMushroom)
            {
                PlayerStats.GainLife();
            }
            else if(entity is WarpPipe warpPipe)
            {
                if (!otherBox.MinkowskiDifferenceContainsOrigin(peachBox)) return;
                EntrancePipe = warpPipe;
                if(warpPipe.ContainedItems != null)
                {
                    foreach(IEntity child in warpPipe.ContainedItems)
                    {
                        if(child is WarpPipe childPipe)
                        {
                            this.LocationAbove = this.Location;
                            this.Descending = true;                         
                            ExitPipe = childPipe;
                            if (childPipe.ExitPipe)
                            {
                                
                                Vector2 respawnLocation = new Vector2(childPipe.Location.X - 15, childPipe.Location.Y - 100);
                                this.PlayerStats.PassCheckpoint(respawnLocation);
                            }
                        }
                    }
                }

                if (peachBox.Bottom - otherBox.Top <= 10 && !(ActionState is JumpingState))
                {
                    if (ActionState is FallingState)
                    {
                        IdleTransition();
                    }
                    Sprite.CurrentLocation -= new Vector2(Constants.ZERO, peachBox.Bottom - otherBox.Top + 1);
                    Sprite.Velocity *= new Vector2(1,Constants.ZERO);
                    OnGround = true;
                }
                else if (otherBox.Bottom - peachBox.Top <= 15)
                {
                    if (Sprite.Velocity.Y <Constants.ZERO)
                    {
                        Sprite.Velocity = new Vector2(Sprite.Velocity.X, Sprite.Velocity.Y * -1);
                        Sprite.CurrentLocation += new Vector2(Constants.ZERO, otherBox.Bottom - peachBox.Top + 1);
                    }
                }
                else if (otherBox.Right - peachBox.Left <= 10)
                {
                    Sprite.Velocity *= new Vector2(Constants.ZERO, 1);
                    if (ActionState is WalkingState || ActionState is RunningState) IdleTransition();
                    Sprite.CurrentLocation += new Vector2(otherBox.Right - peachBox.Left + 1,Constants.ZERO);
                }
                else
                {
                    Sprite.Velocity *= new Vector2(Constants.ZERO, 1);
                    if (ActionState is WalkingState || ActionState is RunningState) IdleTransition();
                    Sprite.CurrentLocation -= new Vector2(peachBox.Right - otherBox.Left + 1,Constants.ZERO);
                }
            }
            else if (entity is Star)
            {
                StarPowerUpTransition();
            }
            else if (entity is SuperMushroom)
            {
                SuperPowerUpTransition();
            }
            else if (entity is AGrade)
            {
                PlayerStats.GPA += 4.00;
                PlayerStats.Grades++;
            }
            else if (entity is BGrade)
            {
                PlayerStats.GPA += 3.00;
                PlayerStats.Grades++;
            }
            else if (entity is CGrade)
            {
                PlayerStats.GPA += 2.00;
                PlayerStats.Grades++;
            }
            else if (entity is DGrade)
            {
                PlayerStats.GPA += 1.00;
                PlayerStats.Grades++;
            }
            else if (entity is FGrade)
            {
                PlayerStats.GPA += 0.00;
                PlayerStats.Grades++;
            }
            else if (entity is FireBar)
            {
                TakeDamageTransition();
            }
            return;
        }
        public bool CanCollide(IEntity other)
        {
            bool invincibleWithEnemy = Invincible && (other is KoopaTroopa || other is Goomba || other is Bowser || other is Piranha);
            return !(PowerUpState is DeadState || PowerUpState is WarpingState) && !invincibleWithEnemy;
        }
        public void JumpTransition()
        {
            ActionState.JumpingStateTransition();
        }
        public void CrouchTransition()
        {
            ActionState.CrouchingStateTransition();
        }
        public void WalkTransition()
        {
            ActionState.WalkingStateTransition();
        }
        public void RunTransition()
        {
            ActionState.RunningStateTransition();
        }
        public void IdleTransition()
        {
            ActionState.IdleStateTransition();
        }
        public void FallTransition()
        {
            ActionState.FallingStateTransition();
        }
        public void FaceLeftTransition()
        {
            ActionState.FaceLeftTransition();
        }
        public void FaceRightTransition()
        {
            ActionState.FaceRightTransition();
        }
        public void ShootFireball()
        {
            if (PlayerStats.FireBallPowerUp || PowerUpState is FireState)
            {
                FireBall fb;
                if (PowerUpState is FireState)
                {
                    fb = new FireBall(graphics, fireBall, this);
                }
                else if (PowerUpState is StarState)
                {
                    fb = new FireBall(graphics, fireBallStar, this);
                }
                else
                {
                    fb = new FireBall(graphics, fireBallPink, this);
                }
                fb.Sprite.Velocity = new Vector2(430,Constants.ZERO);
                fb.Sprite.CurrentLocation = Sprite.CurrentLocation + new Vector2(64, 32);
                if (Sprite.HReflect)
                {
                    fb.Sprite.Velocity *= new Vector2(-1, 1);
                    fb.Sprite.CurrentLocation -= new Vector2(64,Constants.ZERO);
                }
                Collections.Instance.GetCollisionRef().ReRegister(fb);
                Collections.Instance.GetLevelRef().Entities.Add(fb);
            }
        }
        public void StandardPowerUpTransition()
        {
            PowerUpState.StandardTransition(this);
        }

        public void FirePowerUpTransition()
        {
            PowerUpState.FireTransition(this);
        }

        public void StarPowerUpTransition()
        {
            PowerUpState.StarTransition(this);
        }

        public void DeadPowerUpTransition()
        {
            PowerUpState.DeadTransition(this);
        }

        public void TakeDamageTransition()
        {
            if (!(PowerUpState is StandardState || PowerUpState is StarState) && !Invincible)
            {
                frameCountA.enabled = true;
                PowerUpState.TakeDamageTransition(this);
                Invincible = true;
            }
            else if (PowerUpState is StandardState)
            {
                PowerUpState.TakeDamageTransition(this);
            }
         }

        public void WarpTransition()
        {
            PowerUpState.WarpTransition(this);
        }

        public void CheckForFallingDeath()
        {
            Rectangle cast = new Rectangle { X = (int)BoundingBox().Left, Y = (int)BoundingBox().Top, Width = (int)BoundingBox().Width, Height = (int)BoundingBox().Height };
            if (!Camera.Instance.IsOnScreen(cast) && !(PowerUpState is WarpingState || PowerUpState is DeadState)) //&& !(PowerUpState is StandardState))
            {
                Debug.WriteLine("!ERROR: Peach went off screen while not in warpingstate !");
                Debug.WriteLine("You have killed peach -> restarting");
                PowerUpState.DeadTransition(this);
                Sprite.Velocity *= new Vector2(Constants.ZERO,Constants.ZERO);
                Sprite.Acceleration *= new Vector2(Constants.ZERO,Constants.ZERO);
                if (PlayerStats != null) PlayerStats.LoseLife();
            }
        }

        public void SuperPowerUpTransition()
        {
            PowerUpState.SuperTransition(this);
        }

        public void SwitchToStandardSprite()
        {
            this.Sprite.ChangeTexture(StandardSpriteSheet, 6, 6);
            this.Location = new Vector2(this.Location.X,this.Location.Y + 5);
        }

        public void SwitchToFireSprite()
        {
            this.Sprite.ChangeTexture(FireSpriteSheet, 5, 6);
        }

        public void SwitchToSuperSprite(IPowerUpState<Peach> previousState)
        {
            this.Sprite.ChangeTexture(SuperSpriteSheet, 5, 6);
            if (previousState is StandardState)
            {
                this.Location = new Vector2(this.Location.X, this.Location.Y - 14);
            }
        }
        public void SwitchToStarSprite()
        {
            this.Sprite.ChangeTexture(StarSpriteSheet, 5, 6);
            this.Location = new Vector2(this.Location.X, this.Location.Y - 13);
        }
    }
}
