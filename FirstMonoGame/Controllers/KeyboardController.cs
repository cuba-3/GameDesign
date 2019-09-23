using FirstMonoGame.Commands;
using FirstMonoGame.Interfaces;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMonoGame.Controllers
{
    class KeyboardController
    {
        private KeyboardState KeyboardState;
        private KeyboardState PreviousKeyboardState;
        private Dictionary<Keys, IKeyState> KeyStates;
        private Dictionary<Keys, ICommand> Commands;

        public KeyboardController()
        {
            KeyStates = new Dictionary<Keys, IKeyState>();
            KeyboardState = new KeyboardState();
            PreviousKeyboardState = new KeyboardState();
            Commands = new Dictionary<Keys, ICommand>();
        }

        public void UpdateKeyboard()
        {
            KeyboardState = Keyboard.GetState();
            foreach (Keys key in Commands.Keys)
            {
               if (KeyboardState.IsKeyDown(key) && !PreviousKeyboardState.GetPressedKeys().Contains(key))
               {
                        KeyStates[key].SetPressed();
                        Commands[key].Execute();
               }
               else if (KeyboardState.IsKeyDown(key) && PreviousKeyboardState.IsKeyDown(key))
               {
                    KeyStates[key].SetHeld();
                    if (KeyStates[key].IsHeld())
                    {
                        Commands[key].HeldKey();       // if we want peach to move only when key is being held uncomment this and line below
                    }
                }
               else if (PreviousKeyboardState.IsKeyDown(key) && KeyboardState.IsKeyUp(key))
               {
                    KeyStates[key].SetReleased();
                    Commands[key].Release();            // if we want peach to move only when key is being held uncomment this
                }
            }
            PreviousKeyboardState = KeyboardState;
        }

        public void AddNewKeyState(Keys key)
        {
            KeyStates.Add(key, new KeyState());
        }

        public void UpdateCommands(Dictionary<Keys, ICommand> commands)
        {
            this.Commands = commands;
        }
    }
}
