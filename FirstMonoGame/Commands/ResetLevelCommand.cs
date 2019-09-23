using FirstMonoGame.States.GameState;
using FirstMonoGame.WorldScrolling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMonoGame.Commands
{
    class ResetLevelCommand : ICommand
    {
        GameMain Game;

        public ResetLevelCommand(GameMain game)
        {
            Game = game;
        }

        public void Execute()
        {
            if(Game.currentGameState is WinnerState || Game.currentGameState is GameOverState)
            {
                Camera.Instance.VerticalScrollMode = false;
                Game.peachStatsInstance = null;
                Game.LoadLevel("level.xml");
            }
            else
            {
                Game.peachStatsInstance = null;
                Game.LoadLevel(Game.CurrentLevelXML);
            }
        }

        public void HeldKey()
        {
            return;
        }

        public void Release()
        {
            return;
        }
    }
}
