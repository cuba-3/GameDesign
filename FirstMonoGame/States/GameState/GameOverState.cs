using FirstMonoGame.Entity;
using FirstMonoGame.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMonoGame.States.GameState
{
    class GameOverState : IGameState
    {
        private GameMain Game;
        private Vector2 MiddleFirst;
        private Vector2 MiddleSecond;
        private Vector2 MiddleThird;
        private Vector2 MiddleFourth;
        private Vector2 MiddleFifth;
        private Vector2 MiddleSixth;
        private Vector2 MiddleSeventh;
        private SpriteFont TextFont;
        private SpriteFont TextFontStats;
        private SoundEffect GameOver;
        private Texture2D GradDitherBG;

        public GameOverState(GameMain game)
        {
            Game = game;
            GameOver = game.Content.Load<SoundEffect>("Sound Effects/game over");
            GradDitherBG = game.Content.Load<Texture2D>("Backgrounds/GradDitherBG");
            Game.songA.Stop();
            GameOver.Play();
        }

        public void GameOverTransition()
        {
            return; 
        }
        public void GameTransitionStateTransistion()
        {
            throw new NotImplementedException();
        }

        public void InitializeTransition()
        {
            throw new NotImplementedException();
        }

        public void WinnerTransition()
        {
            throw new NotImplementedException();
        }

        public void CoinRoomTransition()
        {
            throw new NotImplementedException();
        }

        public void GradeRoomTransition()
        {
            throw new NotImplementedException();
        }

        public void BeginningTransition()
        {
            throw new NotImplementedException();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            PlayerStats stats = Game.peach.PlayerStats;
            SplashScreenDetails(Game);
            Game.level.Continue = false;
            Game.level.Entities.Clear();

            spriteBatch.End();
            Game.GraphicsDevice.Clear(Color.Black);
            SamplerState pointFilter = new SamplerState();
            pointFilter.Filter = TextureFilter.Point;
            spriteBatch.Begin(SpriteSortMode.Deferred, null, pointFilter);
            spriteBatch.Draw(GradDitherBG, new Rectangle { X =Constants.ZERO, Y =Constants.ZERO, Width = spriteBatch.GraphicsDevice.Viewport.Width, Height = spriteBatch.GraphicsDevice.Viewport.Height }, Color.White);
            spriteBatch.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            spriteBatch.DrawString(TextFont, "Game Over", MiddleFirst, Color.White);
            spriteBatch.DrawString(TextFontStats, "PLAYER", MiddleSecond, Color.Lerp(Color.White, Color.Transparent, .05f));
            spriteBatch.DrawString(TextFontStats, stats.PlayerName, MiddleSecond + new Vector2(215,Constants.ZERO), Color.Lerp(Color.White, Color.Transparent, .05f));
            spriteBatch.DrawString(TextFontStats, "SCORE", MiddleThird, Color.Lerp(Color.White, Color.Transparent, .05f));
            spriteBatch.DrawString(TextFontStats, stats.Score.ToString(), MiddleThird + new Vector2(215,Constants.ZERO), Color.Lerp(Color.White, Color.Transparent, .05f));
            spriteBatch.DrawString(TextFontStats, "LIVES", MiddleFourth, Color.Lerp(Color.White, Color.Transparent, .05f));
            spriteBatch.DrawString(TextFontStats, stats.Lives.ToString(), MiddleFourth + new Vector2(215,Constants.ZERO), Color.Lerp(Color.White, Color.Transparent, .05f));
            spriteBatch.DrawString(TextFontStats, "COINS", MiddleFifth, Color.Lerp(Color.White, Color.Transparent, .05f));
            spriteBatch.DrawString(TextFontStats, stats.Coins.ToString(), MiddleFifth + new Vector2(215,Constants.ZERO), Color.Lerp(Color.White, Color.Transparent, .05f));
            spriteBatch.DrawString(TextFontStats, "TIME", MiddleSixth, Color.Lerp(Color.White, Color.Transparent, .05f));
            spriteBatch.DrawString(TextFontStats, stats.TimeRemaining.ToString(), MiddleSixth + new Vector2(215,Constants.ZERO), Color.Lerp(Color.White, Color.Transparent, .05f));
            spriteBatch.DrawString(TextFontStats, "Press R to replay or Q to quit", MiddleSeventh, Color.Lerp(Color.White, Color.Transparent, .05f));
        }
        

        public void Update(GameTime gameTime)
        {

        }

        private void SplashScreenDetails(GameMain game)
        {

            TextFont = game.Content.Load<SpriteFont>("Fonts/Header");
            TextFontStats = game.Content.Load<SpriteFont>("Fonts/File");
            MiddleFirst = new Vector2((game.GraphicsDevice.Viewport.Width / 2) - 450, (game.GraphicsDevice.Viewport.Height / 2) - 420);
            MiddleSecond = new Vector2((game.GraphicsDevice.Viewport.Width / 2) - 425, (game.GraphicsDevice.Viewport.Height / 2) - 200);
            MiddleThird = new Vector2((game.GraphicsDevice.Viewport.Width / 2) - 425, (game.GraphicsDevice.Viewport.Height / 2) - 100);
            MiddleFourth = new Vector2((game.GraphicsDevice.Viewport.Width / 2) - 425, (game.GraphicsDevice.Viewport.Height / 2));
            MiddleFifth = new Vector2((game.GraphicsDevice.Viewport.Width / 2) - 425, (game.GraphicsDevice.Viewport.Height / 2) + 100);
            MiddleSixth = new Vector2((game.GraphicsDevice.Viewport.Width / 2) - 425, (game.GraphicsDevice.Viewport.Height / 2) + 200);
            MiddleSeventh = new Vector2((game.GraphicsDevice.Viewport.Width / 2) - 425, (game.GraphicsDevice.Viewport.Height / 2) + 300);
        }
    }
}
