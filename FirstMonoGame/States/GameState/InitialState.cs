using FirstMonoGame.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMonoGame.States.GameState
{
    class InitialState : IGameState
    {
        private GameMain Game;        

        public InitialState(GameMain game)
        {
            Game = game;
        }

        public void BeginningTransition()
        {
            var state = new BeginningState(Game);
            Game.currentGameState = state;

        }
        public void GameOverTransition()
        {
            var state = new GameOverState(Game);
            Game.currentGameState = state;
        }

        public void GradeRoomTransition()
        {
            var state = new GradeRoomState(Game);
            Game.currentGameState = state;
        }

        public void InitializeTransition()
        {
            var state = new InitialState(Game);
            Game.currentGameState = state;
        }

        public void WinnerTransition()
        {
            var state = new WinnerState(Game);
            Game.currentGameState = state;
        }

        public void CoinRoomTransition()
        {
            var state = new CoinRoomState(Game);
            Game.currentGameState = state;
            if (Game.mario != null) Game.mario.PlayerStats = new PlayerStats(Game.Content, Game.GraphicsDevice, "Mario", Game); 
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }

        public void Update(GameTime gameTime)
        {

        }

        public void GameTransitionStateTransistion()
        {
            var state = new GameTransitionState(Game, "verticalLevel.xml");
            Game.currentGameState = state;
        }
    }
}
