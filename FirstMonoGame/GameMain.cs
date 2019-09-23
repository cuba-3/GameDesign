using FirstMonoGame.Collision;
using FirstMonoGame.Commands;
using FirstMonoGame.Controllers;
using FirstMonoGame.Entity;
using FirstMonoGame.Interfaces;
using FirstMonoGame.Levels;
using FirstMonoGame.States.GameState;
using FirstMonoGame.States.PowerUpStates;
using FirstMonoGame.WorldScrolling;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace FirstMonoGame
{
    public class GameMain : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public SoundEffectInstance songA { get; set; }

        // Main Game Camera View
        Camera Camera;

        // Scrolling background control
        BackgroundControl BackgroundControl;

        //  Collision Detection
        private CollisionDetection collisions;

        // Instances of Player Stats
        public PlayerStats peachStatsInstance { get; set; }
        public PlayerStats marioStatsInstance { get; set; }


        // Controller
        private IController Controller;
        public IGameState currentGameState { get; set; }
        public bool FreezeControls { get; set; }
        #region Entities

        public Peach peach { get; set; }
        public Mario mario { get; set; }
        #endregion 

        public Level level;
        LevelLoader levelLoader = new LevelLoader();
        public string CurrentLevelXML { get; set; }
        public int Delay { get; set; }

        public GameMain()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1920,
                PreferredBackBufferHeight = 1080
            };
            graphics.ApplyChanges();
            songA = null;

            Content.RootDirectory = "Content";
            FreezeControls = false;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            CurrentLevelXML = "level.xml";
            LoadLevel(CurrentLevelXML);

            // Create a new SpriteBatch
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
            // Unload all content
            Content.Unload();
        }
        public bool gamePaused { get; set; }
        KeyboardState currentKB, previousKB;
        protected override void Update(GameTime gameTime)
        {
            if (!(currentGameState is BeginningState))
            {
                previousKB = currentKB;
                currentKB = Keyboard.GetState();

                if (currentKB.IsKeyUp(Keys.P) && previousKB.IsKeyDown(Keys.P))
                {
                    gamePaused = !gamePaused;
                }
                if (gamePaused)
                {
                    songA.Pause();
                    return;
                }
                else
                {
                    if (songA.State is SoundState.Paused)
                    {
                        songA.Resume();
                    }
                }
                if (!FreezeControls)
                {
                    Controller.Update();
                }
                // currentGameState.Update(gameTime);
                level.Update(gameTime, this);
                collisions.FindCollisions(gameTime, level.Continue);
                Camera.Update();
                BackgroundControl.Update();
                currentGameState.Update(gameTime);
                peach.PlayerStats.Update(gameTime);
                base.Update(gameTime);
            } else
            {
                currentGameState.Update(gameTime);
            }
        }
        protected override void Draw(GameTime gameTime)
        {
            if (currentGameState is BeginningState)
            {
                Delay++;
                currentGameState.Draw(spriteBatch);
            }
            else
            {
                GraphicsDevice.Clear(new Color(110, 170, 235));
                Texture2D pink = Content.Load<Texture2D>("Backgrounds/pinkBackground");
                SpriteFont TextFont = Content.Load<SpriteFont>("Fonts/File");

                // Speed of things in relation to peach
                Vector2 parallax = new Vector2(1.0f);
                Rectangle destinationRectangleGradeRoom = new Rectangle(Constants.ZERO, 1700, 2000, 2200);
                Rectangle destinationRectangleCoinRoom = new Rectangle(Constants.ZERO, 3000, 2000, 2200);

                // Backgrounds independent since they need a linear wrap effect
                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null);
                BackgroundControl.Draw(spriteBatch);
                spriteBatch.End();

                // Everything drawn with this sprite batch moves with peach
                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Camera.GetViewMatrix(parallax));
                spriteBatch.Draw(pink, destinationRectangleGradeRoom, destinationRectangleGradeRoom, Color.White);
                spriteBatch.Draw(pink, destinationRectangleCoinRoom, destinationRectangleCoinRoom, Color.White);
                spriteBatch.DrawString(TextFont, "Welcome to", new Vector2(2000, 225), Color.White);
                Texture2D OSU = Content.Load<Texture2D>("OSU");
                spriteBatch.Draw(OSU, new Vector2(2000, 300), Color.White);
                spriteBatch.DrawString(TextFont, "Go Down Here", new Vector2(2725, 580), Color.White);
                spriteBatch.DrawString(TextFont, "Hey You", new Vector2(5300, 400), Color.White);
                spriteBatch.DrawString(TextFont, "Yeah YOU", new Vector2(5900, 500), Color.White);
                spriteBatch.DrawString(TextFont, "Go Down Here!", new Vector2(6225, 580), Color.White);
                spriteBatch.DrawString(TextFont, "Hi Scott", new Vector2(7000, 300), Color.White, 0.0f, new Vector2(Constants.ZERO, Constants.ZERO), 0.75f, SpriteEffects.None, 0.0f);
                Texture2D wave = Content.Load<Texture2D>("stitch");
                spriteBatch.Draw(wave, new Vector2(7000, 330), Color.White);
                level.Draw(spriteBatch, this);
                if (currentGameState is GradeRoomState || currentGameState is GameTransitionState || currentGameState is CoinRoomState || currentGameState is GameOverState || currentGameState is WinnerState)
                {
                    currentGameState.Draw(spriteBatch);
                }
                spriteBatch.End();

                if (currentGameState is InitialState) peach.PlayerStats.Draw();

                base.Draw(gameTime);
            }
        }

        public void LoadLevel(string levelXML)
        {
            if (songA != null) songA.Stop();
            Delay =Constants.ZERO;
            CurrentLevelXML = levelXML;
            levelLoader = new LevelLoader();
            Camera = Camera.Instance;
            Camera.SetViewport(GraphicsDevice.Viewport);

            BackgroundControl = new BackgroundControl(GraphicsDevice.Viewport, this.Content);

            //Load Song
            SoundEffect song = Content.Load<SoundEffect>("OverworldNormal");
            songA = song.CreateInstance();
            songA.IsLooped = true;
            songA.Play();

            collisions = new CollisionDetection();
            level = levelLoader.LoadLevel(CurrentLevelXML, this.Content, this.GraphicsDevice, collisions, BackgroundControl);
            collisions.StartDetection(levelLoader.levelWidth, levelLoader.levelHeight);

            Collections.Instance.SetCollisionRef(ref collisions);
            Collections.Instance.SetLevelRef(ref level);
            if (levelXML == "level.xml")
            {
                currentGameState = new BeginningState(this);
                currentGameState.BeginningTransition();
            } else
            {
                currentGameState = new InitialState(this);
            }
            foreach (IEntity e in level.Entities)
            {
                if (e is Peach) peach = (Peach)e;
                else if (e is Mario) mario = (Mario)e;
            }

            if (peachStatsInstance is null)
            {
                peach.PlayerStats = new PlayerStats(this.Content, GraphicsDevice, "Peach", this);
                peachStatsInstance = peach.PlayerStats;
            }
            else
            {
                peach.PlayerStats = peachStatsInstance;
                if(peachStatsInstance.StartingPoint != Vector2.Zero)
                {
                    //Debug.WriteLine("Peach respawn X: " + playerStats.StartingPoint.X);
                    //Debug.WriteLine("Peach respawn Y: " + playerStats.StartingPoint.Y);
                    peach.Location = peachStatsInstance.StartingPoint;
                }
            }

            Controller = new Controller();

            #region Add Commands
            Controller.AddCommand(Keys.Q, new ExitCommand(this));
            Controller.AddCommand(Keys.Left, new MoveLeftCommand(peach));
            Controller.AddCommand(Keys.Right, new MoveRightCommand(peach));
            Controller.AddCommand(Keys.Up, new JumpCommand(peach));
            Controller.AddCommand(Keys.Down, new CrouchCommand(peach));
            Controller.AddCommand(Keys.A, new MoveLeftCommand(peach));
            Controller.AddCommand(Keys.D, new MoveRightCommand(peach));
            Controller.AddCommand(Keys.W, new JumpCommand(peach));
            Controller.AddCommand(Keys.S, new CrouchCommand(peach));
            Controller.AddCommand(Keys.Y, new StandardPowerUpCommand(peach));
            Controller.AddCommand(Keys.I, new FirePowerUpCommand(peach));
            Controller.AddCommand(Keys.V, new StarPowerUpCommand(peach));
            Controller.AddCommand(Keys.O, new TakeDamageCommand(peach));
            Controller.AddCommand(Keys.U, new SuperPowerUpCommand(peach));
            Controller.AddCommand(Keys.Space, new FireBallCommand(peach));
            Controller.AddCommand(Keys.M, new MuteCommand());
            Controller.AddCommand(Buttons.Back, new ExitCommand(this));
            Controller.AddCommand(Buttons.DPadLeft, new MoveLeftCommand(peach));
            Controller.AddCommand(Buttons.DPadRight, new MoveRightCommand(peach));
            Controller.AddCommand(Buttons.DPadUp, new JumpCommand(peach));
            Controller.AddCommand(Buttons.DPadDown, new CrouchCommand(peach));
            Controller.AddCommand(Buttons.B, new FireBallCommand(peach));
            Controller.AddCommand(Keys.R, new ResetLevelCommand(this));
            Controller.AddCommand(Keys.C, new ToggleBoundingBoxCommand(level.Entities));
            #endregion
            Controller = new Controller();

            #region Add Commands
            Controller.AddCommand(Keys.Q, new ExitCommand(this));
            Controller.AddCommand(Keys.Left, new MoveLeftCommand(peach));
            Controller.AddCommand(Keys.Right, new MoveRightCommand(peach));
            Controller.AddCommand(Keys.Up, new JumpCommand(peach));
            Controller.AddCommand(Keys.Down, new CrouchCommand(peach));
            Controller.AddCommand(Keys.A, new MoveLeftCommand(peach));
            Controller.AddCommand(Keys.D, new MoveRightCommand(peach));
            Controller.AddCommand(Keys.W, new JumpCommand(peach));
            Controller.AddCommand(Keys.S, new CrouchCommand(peach));
            Controller.AddCommand(Keys.Y, new StandardPowerUpCommand(peach));
            Controller.AddCommand(Keys.I, new FirePowerUpCommand(peach));
            Controller.AddCommand(Keys.V, new StarPowerUpCommand(peach));
            Controller.AddCommand(Keys.O, new TakeDamageCommand(peach));
            Controller.AddCommand(Keys.U, new SuperPowerUpCommand(peach));
            Controller.AddCommand(Keys.Space, new FireBallCommand(peach));
            Controller.AddCommand(Keys.M, new MuteCommand());
            Controller.AddCommand(Buttons.Back, new ExitCommand(this));
            Controller.AddCommand(Buttons.DPadLeft, new MoveLeftCommand(peach));
            Controller.AddCommand(Buttons.DPadRight, new MoveRightCommand(peach));
            Controller.AddCommand(Buttons.DPadUp, new JumpCommand(peach));
            Controller.AddCommand(Buttons.DPadDown, new CrouchCommand(peach));
            Controller.AddCommand(Buttons.B, new FireBallCommand(peach));
            Controller.AddCommand(Keys.R, new ResetLevelCommand(this));
            Controller.AddCommand(Keys.C, new ToggleBoundingBoxCommand(level.Entities));
            #endregion
        }
    }
}
 
