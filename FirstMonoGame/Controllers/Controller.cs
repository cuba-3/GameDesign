using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using FirstMonoGame.Interfaces;
using System.Diagnostics;

namespace FirstMonoGame.Controllers
{
    class Controller : IController
    {
        private KeyboardController KeyboardController;
        private GamePadController GamePadController;
        private Dictionary<Buttons, ICommand> GamePadCommands;
        private Dictionary<Keys, ICommand> KeyboardCommands;

        public Controller()
        {
            KeyboardController = new KeyboardController();
            GamePadController = new GamePadController();
            GamePadCommands = new Dictionary<Buttons, ICommand>();
            KeyboardCommands = new Dictionary<Keys, ICommand>();
        }

        public void Update()
        {
            KeyboardController.UpdateKeyboard();
            GamePadController.UpdateGamePad();
        }

        public void AddCommand(dynamic input, ICommand command)
        {
            if (input is Keys key)
            {
                KeyboardCommands.Add(key, command);
                KeyboardController.AddNewKeyState(key);
                KeyboardController.UpdateCommands(KeyboardCommands);
            }
            else if (input is Buttons button)
            {
                GamePadCommands.Add(button, command);
                GamePadController.AddNewButtonState(button);
                GamePadController.UpdateCommands(GamePadCommands);
            }
        }

        public void ResetCommands()
        {
            GamePadCommands.Clear();
            GamePadController.UpdateCommands(GamePadCommands);
            KeyboardCommands.Clear();
            KeyboardController.UpdateCommands(KeyboardCommands);
        }
    }
}