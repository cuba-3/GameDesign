using FirstMonoGame.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;
using FirstMonoGame.States.GameState;
using static FirstMonoGame.Constants;

namespace FirstMonoGame
{
    public class PlayerStats
    { 
        private SpriteFont TextFont;
        public SpriteBatch SpriteBatch { get; set; }
        public int Score { get; set; }
        public int Lives { get; set; }
        public int Coins { get; set; }
        public int TimeRemaining { get; set; }
        private double passedSeconds;
        private double passedSecondsGradeRoom;
        private double passedSecondsCoinRoom;
        private TimeSpan timer;
        public string PlayerName { get; set; }
        private Vector2 TopLeft;
        private Vector2 Top2ndLeft;
        private Vector2 TopCenter;
        private Vector2 Top2ndRight;
        private Vector2 TopRight;
        public Vector2 StartingPoint { get; set; }
        private GameMain Game;
        private ISprite MiniCoin;
        private ISprite MiniAvatar;
        private bool counting;
        private SoundEffect PeachDiesSound;
        private SoundEffect TimeWarningSound;
        private SoundEffect LevelClearSound;
        public bool Victory { get; set; }
        public int FlagPoints { get; set; }
        private bool FlagPointsAdded;
        public bool GradeRoom { get; set; }
        public bool CoinRoom { get; set; }
        public double GPA { get; set; }
        public int Grades { get; set; }
        public bool FireBallPowerUp { get; set; }
        public int TimeRemainingGradeRoom { get; set; }
        public int TimeRemainingCoinRoom { get; set; }
        public int CoinRoomCoin { get; set; }
        public bool CoinMultiplier { get; set; }
        public PlayerStats(ContentManager content, GraphicsDevice graphics, String playerName, GameMain game)
        {
            TextFont = content.Load<SpriteFont>("Fonts/File");
            TimeWarningSound = content.Load<SoundEffect>("Sound Effects/Time Warning");
            PeachDiesSound = content.Load<SoundEffect>("Sound Effects/Peach Dies");
            LevelClearSound = content.Load<SoundEffect>("Sound Effects/LevelClear");
            SpriteBatch = new SpriteBatch(graphics);
            PlayerName = playerName;
            Score = Constants.SCORE;
            Lives = Constants.LIVES;
            TimeRemaining = Constants.TIMER_MAIN_GAME;
            passedSeconds =Constants.ZERO;
            passedSecondsGradeRoom =Constants.ZERO;
            passedSecondsCoinRoom =Constants.ZERO;
            counting = false;
            TopLeft = new Vector2(Constants.ZERO, -5);
            TopCenter = new Vector2((graphics.Viewport.Width / 2) - 45, -5);
            Top2ndLeft = new Vector2(TopCenter.X / 2, -5);
            Top2ndRight = new Vector2(Top2ndLeft.X * 3, -5);
            TopRight = new Vector2(graphics.Viewport.Width - 90, -5);
            StartingPoint = Vector2.Zero;
            Game = game;
            MiniCoin = MakeCoinSprite(content, graphics);
            MiniAvatar = MakeAvatarSprite(content, graphics);
            Victory = false;
            FlagPoints = Constants.FLAG_POINTS;
            FlagPointsAdded = true;
            GradeRoom = false;
            CoinRoom = false;
            GPA = Constants.GPA;
            Grades = Constants.GRADES;
            TimeRemainingGradeRoom = Constants.TIMER_GRADE_ROOM;
            TimeRemainingCoinRoom = Constants.TIMER_GRADE_ROOM;
            FireBallPowerUp = false;
            CoinRoomCoin =Constants.ZERO;
            CoinMultiplier = false;
        }

        public void Update(GameTime gameTime)
        {
            MiniCoin.Update(gameTime);
            MiniAvatar.Update(gameTime);
            passedSeconds += gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0;
            if (counting) timer += gameTime.ElapsedGameTime;
            if (!counting && timer.TotalMilliseconds >Constants.ZERO) timer = new TimeSpan();
            if (passedSeconds >= 1)
            {
                if (Lives >Constants.ZERO && !(Game.currentGameState is WinnerState)) TimeRemaining--;
                if(TimeRemaining == 100)
                {
                    TimeWarningSound.Play();
                }
                passedSeconds--;
            }
            if (Coins >= 100)
            {
                Lives++;
                Coins -= 100;
            }
            if (TimeRemaining==0 && Lives>=1)
            {
                Lives--;
                if (Lives >Constants.ZERO) TimeRemaining = Constants.TIMER_MAIN_GAME;
                Game.songA.Stop();
                Game.peach.DeadPowerUpTransition();
                Game.LoadLevel(Game.CurrentLevelXML);
            }
            if (Lives ==Constants.ZERO)
            {
                if (!counting) counting = true; // if not already delaying frame of game over screen, start
                if (timer.TotalSeconds >= 3)
                {
                    Debug.WriteLine("total seconds >= 3");
                    counting = false;
                    Game.currentGameState.GameOverTransition();
                }
            }   
            if (Victory)
            {
                Victory = false;

                Game.songA.Stop();
                
                if (FlagPointsAdded)
                {
                    Score += FlagPoints;
                    LevelClearSound.Play();
                    FlagPointsAdded = false;
                    Score += (TimeRemaining * 2); //time bonus for winning
                }
                if (Game.CurrentLevelXML == "level.xml")
                {
                    Lives++;
                    Game.currentGameState.GameTransitionStateTransistion();
                }
                else
                {
                    Game.currentGameState.WinnerTransition();
                }
            }
            if (GradeRoom)
            {
                if (TimeRemainingGradeRoom >Constants.ZERO)
                {
                    passedSecondsGradeRoom += gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0;
                    if (passedSecondsGradeRoom >= 1)
                    {
                        TimeRemainingGradeRoom--;
                        if (TimeRemainingGradeRoom == 10)
                        {
                            TimeWarningSound.Play();
                        }
                        passedSecondsGradeRoom--;
                    }
                }
                else if (TimeRemainingGradeRoom ==Constants.ZERO)
                {
                    string gpa = CalculateGPA();
                    if (Double.Parse(gpa) >= 3.2)
                    {
                        FireBallPowerUp = true;
                    }
                }
                Game.currentGameState.GradeRoomTransition();              
            }
            else if (CoinRoom)
            {
                if (TimeRemainingCoinRoom >Constants.ZERO)
                {
                    passedSecondsCoinRoom += gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0;
                    if (passedSecondsCoinRoom >= 1)
                    {
                        TimeRemainingCoinRoom--;
                        if (TimeRemainingCoinRoom == 10)
                        {
                            TimeWarningSound.Play();
                        }
                        passedSecondsCoinRoom--;
                    }
                } else if (TimeRemainingCoinRoom ==Constants.ZERO)
                {
                    //do something
                }
                Game.currentGameState.CoinRoomTransition();
            }
            else
            {
                if (!(Game.currentGameState is GameOverState) && !(Game.currentGameState is WinnerState) && !(Game.currentGameState is GameTransitionState))
                {
                    Game.currentGameState.InitializeTransition();
                }
            }   
           Game.peach.PlayerStats = this;
        }

        public void Draw()
        {
            SpriteBatch.Begin();            
            MiniCoin.Draw(SpriteBatch);
            MiniAvatar.Draw(SpriteBatch);
            SpriteBatch.DrawString(TextFont, "PLAYER", TopLeft, Color.Lerp(Color.White, Color.Transparent, .05f));
            SpriteBatch.DrawString(TextFont, PlayerName, TopLeft + new Vector2(Constants.ZERO, 25), Color.Lerp(Color.White, Color.Transparent, .05f));
            SpriteBatch.DrawString(TextFont, "SCORE", Top2ndRight, Color.Lerp(Color.White, Color.Transparent, .05f));
            SpriteBatch.DrawString(TextFont, Score.ToString(), Top2ndRight + new Vector2(Constants.ZERO, 25), Color.Lerp(Color.White, Color.Transparent, .05f));
            SpriteBatch.DrawString(TextFont, "LIVES", TopCenter, Color.Lerp(Color.White, Color.Transparent, .05f));
            SpriteBatch.DrawString(TextFont, Lives.ToString(), TopCenter + new Vector2(Constants.ZERO, 25), Color.Lerp(Color.White, Color.Transparent, .05f));
            SpriteBatch.DrawString(TextFont, "COINS", Top2ndLeft, Color.Lerp(Color.White, Color.Transparent, .05f));
            SpriteBatch.DrawString(TextFont, Coins.ToString(), Top2ndLeft + new Vector2(Constants.ZERO, 25), Color.Lerp(Color.White, Color.Transparent, .05f));
            SpriteBatch.DrawString(TextFont, "TIME", TopRight, Color.Lerp(Color.White, Color.Transparent, .05f));
            SpriteBatch.DrawString(TextFont, TimeRemaining.ToString(), TopRight + new Vector2(Constants.ZERO, 25), Color.Lerp(Color.White, Color.Transparent, .05f));
            SpriteBatch.End();      
        }

        public void LoseLife()
        {
            if (Lives !=Constants.ZERO)
            {
                Lives--;
            }
            if (Lives >Constants.ZERO)
            {
                Game.LoadLevel(Game.CurrentLevelXML);
            }
            else
            {
                Game.songA.Pause();
                PeachDiesSound.Play();
            }
        }

        public void GainLife()
        {
            Lives++;
        }

        public void CollectCoin()
        {
            if (CoinMultiplier)
            {
                Coins+=3;
                Score += 600;
            }
            else
            {
                Coins++;
                Score += 200;
            }
        }

        public string CalculateGPA()
        {
            if (GPA ==Constants.ZERO) return "0.0";
            else return string.Format("{0:0.0}", GPA / (double)Grades);
        }
        public void PassCheckpoint(Vector2 flagLocation)
        {
            StartingPoint = flagLocation;
        }

        private ISprite MakeCoinSprite(ContentManager content, GraphicsDevice graphics)
        {
            ISprite sprite;
            Texture2D coin = content.Load<Texture2D>("ItemSpriteSheet");
            sprite = new ItemSprite(graphics, coin, 5, 10, 2, 10);
            sprite.CurrentLocation = new Vector2(Top2ndLeft.X + 112, 1);
            sprite.ScaleSprite(1.5f);

            return sprite;
        }

        private ISprite MakeAvatarSprite(ContentManager content, GraphicsDevice graphics)
        {
            ISprite sprite;
            Texture2D avatar = content.Load<Texture2D>("Peach/StandardPeachSpriteSheet");
            sprite = new AvatarSprite(graphics);
            sprite.InitializeSprite(avatar, graphics, 6, 6);
            sprite.SwitchAnimation(Constants.ZERO, 6);
            sprite.ScaleSprite(0.39f);
            sprite.CurrentLocation = new Vector2(TopLeft.X + 140, -5);

            return sprite;
        }
    }
}
