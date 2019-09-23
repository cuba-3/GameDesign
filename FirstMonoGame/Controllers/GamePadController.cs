using FirstMonoGame.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMonoGame.Controllers
{
    class GamePadController
    {
        private Dictionary<PlayerIndex, GamePadState> GamePadStates;
        private Dictionary<PlayerIndex, GamePadState> PreviousGamePadStates;
        private Dictionary<PlayerIndex, Dictionary<Buttons, IKeyState>> KeyStates;
        private Dictionary<Buttons, ICommand> Commands;

        public GamePadController()
        {
            GamePadStates = new Dictionary<PlayerIndex, GamePadState>();
            PreviousGamePadStates = new Dictionary<PlayerIndex, GamePadState>();
            KeyStates = new Dictionary<PlayerIndex, Dictionary<Buttons, IKeyState>>();
            Commands = new Dictionary<Buttons, ICommand>();

            foreach (PlayerIndex playerIndex in Enum.GetValues(typeof(PlayerIndex)))
            {
                Dictionary<Buttons, IKeyState> newKeyStateDictionary = new Dictionary<Buttons, IKeyState>();
                GamePadStates.Add(playerIndex, new GamePadState());
                PreviousGamePadStates.Add(playerIndex, new GamePadState());
                KeyStates.Add(playerIndex, newKeyStateDictionary);
            }
        }
    
        public void UpdateGamePad()
        {
            foreach (PlayerIndex playerIndex in Enum.GetValues(typeof(PlayerIndex)))
            {
                GamePadStates[playerIndex] = GamePad.GetState(playerIndex);
            }
            foreach (Buttons button in Commands.Keys)
            {
                foreach (PlayerIndex playerIndex in GamePadStates.Keys)
                {
                    if (GamePadStates[playerIndex].IsButtonDown(button) && !PreviousGamePadStates[playerIndex].IsButtonDown(button))
                    {
                        KeyStates[playerIndex][button].SetPressed();
                        Commands[button].Execute();
                    }
                    else if (GamePadStates[playerIndex].IsButtonDown(button) && PreviousGamePadStates[playerIndex].IsButtonDown(button))
                    {
                        KeyStates[playerIndex][button].SetHeld();
                        if (KeyStates[playerIndex][button].IsHeld())
                        {
                            Commands[button].HeldKey();     // if we want peach to move only when key is being held uncomment this and line below
                        }
                    }
                    else if (PreviousGamePadStates[playerIndex].IsButtonDown(button) && GamePadStates[playerIndex].IsButtonUp(button))
                    {
                        KeyStates[playerIndex][button].SetReleased();
                        Commands[button].Release();        // if we want peach to move only when key is being held uncomment this
                    }
                }
            }
            foreach (PlayerIndex playerIndex in GamePadStates.Keys)
            {
                PreviousGamePadStates[playerIndex] = GamePadStates[playerIndex];
            }
        }

        public void AddNewButtonState(Buttons button)
        {
            foreach (PlayerIndex playerIndex in Enum.GetValues(typeof(PlayerIndex)))
            {
                KeyStates[playerIndex].Add(button, new KeyState());
            }
        }

        public void UpdateCommands(Dictionary<Buttons, ICommand> commands)
        {
            this.Commands = commands;
        }
    }
}
