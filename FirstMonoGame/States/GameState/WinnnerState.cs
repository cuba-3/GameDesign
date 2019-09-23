using FirstMonoGame.Entity;
using FirstMonoGame.Interfaces;
using FirstMonoGame.WorldScrolling;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMonoGame.States.GameState
{
    class WinnerState : IGameState
    {
        private GameMain Game;
        private Vector2 MiddleSecond;
        private Vector2 MiddleThird;
        private Vector2 MiddleFourth;
        private Vector2 MiddleFifth;
        private Vector2 MiddleSixth;
        private Vector2 MiddleSeventh;
        private SpriteFont TextFont;
        private SpriteFont TextFontStats;
        private Texture2D GradDitherBG;
        private Texture2D StarWrap;
        private Vector2 starV;
        private Vector2 starP;
        private double PassedSeconds;
        private EndingCutScene EndingScene;

        public WinnerState(GameMain game)
        {
            Game = game;
            GradDitherBG = game.Content.Load<Texture2D>("Backgrounds/GradDitherBG");
            StarWrap = game.Content.Load<Texture2D>("Backgrounds/StarWrap");
            starV = new Vector2(-20, -30);
            starP = new Vector2(Constants.ZERO,Constants.ZERO);
            SplashScreenDetails(Game);
            game.ResetElapsedTime();
            PassedSeconds =Constants.ZERO;
            EndingScene = new EndingCutScene(game);
        }

        public void GameOverTransition()
        {
            throw new NotImplementedException();
        }

        public void InitializeTransition()
        {
            throw new NotImplementedException();
        }
        public void BeginningTransition()
        {
            throw new NotImplementedException();
        }

        public void WinnerTransition()
        {
            var state = new WinnerState(Game);
            Game.currentGameState = state;
        }

        public void CoinRoomTransition()
        {
            throw new NotImplementedException();
        }

        public void GradeRoomTransition()
        {
            throw new NotImplementedException();
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            if (PassedSeconds > 3)
            {
                Game.FreezeControls = false;
                PlayerStats stats = Game.peach.PlayerStats;
                SplashScreenDetails(Game);
                Game.level.Continue = false;
                Game.level.Entities.Clear();
                SplashScreenDetails(Game);
                Game.level.Entities = new Collection<IEntity>();
                Game.level.Continue = false;

                spriteBatch.End();
                spriteBatch.GraphicsDevice.Clear(Color.Black);
                SamplerState pointFilter = new SamplerState();
                pointFilter.Filter = TextureFilter.Point;

                spriteBatch.Begin(SpriteSortMode.Deferred, null, pointFilter, null, null);
                spriteBatch.Draw(GradDitherBG, new Rectangle { X =Constants.ZERO, Y =Constants.ZERO, Width = Game.GraphicsDevice.Viewport.Width, Height = Game.GraphicsDevice.Viewport.Height }, Color.White);
                pointFilter.AddressW = TextureAddressMode.Mirror;
                spriteBatch.Draw(StarWrap, new Rectangle { X =Constants.ZERO, Y =Constants.ZERO, Width = Game.GraphicsDevice.Viewport.Width, Height = Game.GraphicsDevice.Viewport.Width }, new Rectangle { X = (int)starP.X, Y = (int)starP.Y, Width = 512, Height = 512 }, Color.FloralWhite);
                spriteBatch.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

                spriteBatch.DrawString(TextFont, "Winner", new Vector2(140, -5), new Color { R = 196, G = 213, B = 255 });
                spriteBatch.DrawString(TextFont, "Winner", new Vector2(440, 95), new Color { R = 199, G = 175, B = 255 });
                spriteBatch.DrawString(TextFont, "Chicken", new Vector2(740, 195), new Color { R = 255, G = 160, B = 251 });
                spriteBatch.DrawString(TextFont, "Dinner", new Vector2(1040, 295), new Color { R = 255, G = 150, B = 220 });

                spriteBatch.DrawString(TextFontStats, "PLAYER", MiddleSecond + new Vector2(Constants.ZERO, 100), Color.Lerp(Color.CornflowerBlue, Color.Transparent, .05f));
                spriteBatch.DrawString(TextFontStats, stats.PlayerName, MiddleSecond + new Vector2(215, 100), Color.Lerp(Color.CornflowerBlue, Color.Transparent, .05f));
                spriteBatch.DrawString(TextFontStats, "SCORE", MiddleThird + new Vector2(Constants.ZERO, 100), Color.Lerp(Color.CornflowerBlue, Color.Transparent, .05f));
                spriteBatch.DrawString(TextFontStats, stats.Score.ToString(), MiddleThird + new Vector2(215, 100), Color.Lerp(Color.CornflowerBlue, Color.Transparent, .05f));
                spriteBatch.DrawString(TextFontStats, "LIVES", MiddleFourth + new Vector2(Constants.ZERO, 100), Color.Lerp(Color.CornflowerBlue, Color.Transparent, .05f));
                spriteBatch.DrawString(TextFontStats, stats.Lives.ToString(), MiddleFourth + new Vector2(215, 100), Color.Lerp(Color.CornflowerBlue, Color.Transparent, .05f));
                spriteBatch.DrawString(TextFontStats, "COINS", MiddleFifth + new Vector2(Constants.ZERO, 100), Color.Lerp(Color.CornflowerBlue, Color.Transparent, .05f));
                spriteBatch.DrawString(TextFontStats, stats.Coins.ToString(), MiddleFifth + new Vector2(215, 100), Color.Lerp(Color.CornflowerBlue, Color.Transparent, .05f));
                spriteBatch.DrawString(TextFontStats, "TIME", MiddleSixth + new Vector2(Constants.ZERO, 100), Color.Lerp(Color.CornflowerBlue, Color.Transparent, .05f));
                spriteBatch.DrawString(TextFontStats, stats.TimeRemaining.ToString(), MiddleSixth + new Vector2(215, 100), Color.Lerp(Color.CornflowerBlue, Color.Transparent, .05f));
                spriteBatch.DrawString(TextFontStats, "Press R to replay or Q to quit", MiddleSeventh + new Vector2(Constants.ZERO, 100), Color.Lerp(Color.CornflowerBlue, Color.Transparent, .05f));
            }
        }


        public void Update(GameTime gameTime)
        {
            if (!EndingScene.Update()) // Cut scene in still in progress
            {
                PassedSeconds =Constants.ZERO;
            }
            PassedSeconds += gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0;
            starP += starV * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        private void SplashScreenDetails(GameMain game)
        {
            TextFont = game.Content.Load<SpriteFont>("Fonts/Header");
            TextFontStats = game.Content.Load<SpriteFont>("Fonts/File");           MiddleSecond = new Vector2((game.GraphicsDevice.Viewport.Width / 2) - 425, (game.GraphicsDevice.Viewport.Height / 2) - 200);
            MiddleThird = new Vector2((game.GraphicsDevice.Viewport.Width / 2) - 425, (game.GraphicsDevice.Viewport.Height / 2) - 100);
            MiddleFourth = new Vector2((game.GraphicsDevice.Viewport.Width / 2) - 425, (game.GraphicsDevice.Viewport.Height / 2));
            MiddleFifth = new Vector2((game.GraphicsDevice.Viewport.Width / 2) - 425, (game.GraphicsDevice.Viewport.Height / 2) + 100);
            MiddleSixth = new Vector2((game.GraphicsDevice.Viewport.Width / 2) - 425, (game.GraphicsDevice.Viewport.Height / 2) + 200);
            MiddleSeventh = new Vector2((game.GraphicsDevice.Viewport.Width / 2) - 425, (game.GraphicsDevice.Viewport.Height / 2) + 300);
        }

        public void GameTransitionStateTransistion()
        {
            throw new NotImplementedException();
        }
    }
}
