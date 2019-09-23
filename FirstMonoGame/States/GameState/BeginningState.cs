using FirstMonoGame.Interfaces;
using FirstMonoGame.Sprites;
using FirstMonoGame.WorldScrolling;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using static FirstMonoGame.Constants;

namespace FirstMonoGame.States.GameState
{
    class BeginningState : IGameState
    {
        private GameMain Game;
        public SpriteFont TextFont;
        private Vector2 TopCenter;
        Texture2D University;
        ItemSprite UniversitySprite;
        Texture2D Logo;
        ItemSprite LogoSprite;
        Peach Peach;
        public int TimeRemaining { get; set; }
        private double passedSeconds;

        public BeginningState(GameMain game)
        {
            Game = game;
            TextFont = Game.Content.Load<SpriteFont>("Fonts/File");
            TopCenter = new Vector2(100, 200);
            Peach = new Peach(Game.Content, Game.GraphicsDevice);
            Peach.Sprite.Acceleration = new Vector2(Constants.ZERO, Constants.ZERO);
            University = Game.Content.Load<Texture2D>("University");
            UniversitySprite = new ItemSprite(Game.GraphicsDevice, University, 1, 1,Constants.ZERO, 1);
            UniversitySprite.CurrentLocation = new Vector2(500, 500);
            Logo = Game.Content.Load<Texture2D>("IntroLogo");
            LogoSprite = new ItemSprite(Game.GraphicsDevice, Logo, 1, 18,Constants.ZERO, 18);
            LogoSprite.CurrentLocation = new Vector2(600, 400);
            LogoSprite.ScaleSprite(4f);
            LogoSprite.LoopFrame = true;
            Peach.Location = new Vector2(Constants.ZERO, 930);
            TimeRemaining = Constants.TIMER_BEGINNING;
            passedSeconds =Constants.ZERO;
        }

        public void GameOverTransition()
        {
            throw new NotImplementedException();
        }

        public void GradeRoomTransition()
        {
            throw new NotImplementedException();
        }
        public void InitializeTransition()
        {
            var state = new InitialState(Game);
            Game.currentGameState = state;
        }

        public void WinnerTransition()
        {
            throw new NotImplementedException();
        }

        public void CoinRoomTransition()
        {
            throw new NotImplementedException();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Game.Delay < 1800)
            {
                Game.GraphicsDevice.Clear(Color.Orchid);
                spriteBatch.Begin();
                spriteBatch.DrawString(TextFont, "Starting game in..." + (TimeRemaining), TopCenter + new Vector2(1400, -175), Color.White);
                spriteBatch.DrawString(TextFont, "Princess Peach has decided to give up her crown. There are too few women in the STEM field.", TopCenter + new Vector2(Constants.ZERO, 30), Color.White);
                spriteBatch.DrawString(TextFont, "After changing her major from Women Studies, Peach has decided she wants to get a degree in", TopCenter + new Vector2(Constants.ZERO, 80), Color.White);
                spriteBatch.DrawString(TextFont, "Computer Science and Engineering. BUT, little did she know, CSE classrooms are flooded with", TopCenter + new Vector2(Constants.ZERO, 130), Color.White);
                spriteBatch.DrawString(TextFont, "men. Peach is determined to compete with those in her classes (Bowser and Mario), get into", TopCenter + new Vector2(Constants.ZERO, 180), Color.White);
                spriteBatch.DrawString(TextFont, "the CSE program with a 3.2 GPA, even out the wage gap, and then...break the glass ceiling.", TopCenter + new Vector2(Constants.ZERO, 230), Color.White);
                spriteBatch.DrawString(TextFont, "Gasp! Will she do it?", TopCenter + new Vector2(Constants.ZERO, 280), Color.White);               
                UniversitySprite.Draw(spriteBatch);
                Peach.Draw(spriteBatch);
                spriteBatch.End();
            } else if(Game.Delay >= 1800 && Game.Delay < 1950)
            {
                //comment 
                Game.GraphicsDevice.Clear(Color.White);
                spriteBatch.Begin();
                LogoSprite.Draw(spriteBatch);
                spriteBatch.End();
            }
            else
            {
                Game.currentGameState.InitializeTransition();
            }
        }

        public void Update(GameTime gameTime)
        {
            passedSeconds += gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0;
            if (passedSeconds >= 1)
            {
                TimeRemaining--;
                passedSeconds--;
            }
            if (Peach.Location.X < 850)
            {
                Peach.Sprite.SwitchAnimation(1, 4);
                Peach.Location += new Vector2(5,Constants.ZERO);
            } else
            {
                Peach.Sprite.SwitchAnimation(Constants.ZERO, 6);
            }
            Peach.Update(gameTime);
            LogoSprite.Update(gameTime);
        }

        public void GameTransitionStateTransistion()
        {
            throw new NotImplementedException();
        }

        public void BeginningTransition()
        {
            var state = new BeginningState(Game);
            Game.currentGameState = state;
        }
    }
}
