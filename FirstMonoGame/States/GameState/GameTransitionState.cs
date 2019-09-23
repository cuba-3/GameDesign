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
    class GameTransitionState : IGameState
    {
        private GameMain Game;
        private String LevelXMLFile;
        private double PassedSeconds;
        private SpriteFont TextFont;
        private Texture2D GradDitherBG;

        public GameTransitionState(GameMain game, String nextLevelXMLFile)
        {
            Game = game;
            LevelXMLFile = nextLevelXMLFile;
            PassedSeconds =Constants.ZERO;
            game.ResetElapsedTime();
            TextFont = game.Content.Load<SpriteFont>("Fonts/Header");
            GradDitherBG = game.Content.Load<Texture2D>("Backgrounds/GradDitherBG");
        }

        public void BeginningTransition()
        {
            throw new NotImplementedException();
        }
        public void Update(GameTime gameTime)
        {
            PassedSeconds += gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0;
        }
        public void GameTransitionStateTransistion()
        {
           
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (PassedSeconds > 3)
            {
                Game.level.Entities = new Collection<IEntity>();
                Game.level.Continue = false;

                spriteBatch.End();
                spriteBatch.GraphicsDevice.Clear(Color.Black);
                SamplerState pointFilter = new SamplerState();
                pointFilter.Filter = TextureFilter.Point;

                spriteBatch.Begin(SpriteSortMode.Deferred, null, pointFilter, null, null);
                spriteBatch.Draw(GradDitherBG, new Rectangle { X =Constants.ZERO, Y =Constants.ZERO, Width = spriteBatch.GraphicsDevice.Viewport.Width, Height = spriteBatch.GraphicsDevice.Viewport.Height }, Color.White);
                spriteBatch.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

                spriteBatch.DrawString(TextFont, "Next", new Vector2(140, -5), new Color { R = 196, G = 213, B = 255 });
                spriteBatch.DrawString(TextFont, "Level", new Vector2(440, 95), new Color { R = 199, G = 175, B = 255 });
                spriteBatch.DrawString(TextFont, "Starting", new Vector2(740, 195), new Color { R = 255, G = 160, B = 251 });
                spriteBatch.DrawString(TextFont, "Now...", new Vector2(1040, 295), new Color { R = 255, G = 150, B = 220 });
            }
            if (PassedSeconds > 8)
            {
                Debug.WriteLine("PASSING");
                Game.peach.PlayerStats.StartingPoint = Vector2.Zero;
                Game.currentGameState.InitializeTransition();
                Game.LoadLevel(LevelXMLFile);
                Camera.Instance.SwitchToVerticalScrollMode();
            }
        }

        public void GradeRoomTransition()
        {
            throw new NotImplementedException();
        }

        public void CoinRoomTransition()
        {
            throw new NotImplementedException();
        }

        public void WinnerTransition()
        {
            var state = new WinnerState(Game);
            Game.currentGameState = state;
        }

        public void InitializeTransition()
        {
            var state = new InitialState(Game);
            Game.currentGameState = state;
        }

        public void GameOverTransition()
        {
            var state = new GameOverState(Game);
            Game.currentGameState = state;
        }
    }
}
