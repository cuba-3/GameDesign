using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstMonoGame.Commands;
using Microsoft.Xna.Framework.Input;

namespace FirstMonoGame.Interfaces
{
    public interface IController
    {
        void Update();
        void AddCommand(dynamic input, ICommand command);
        void ResetCommands();
    }
}
