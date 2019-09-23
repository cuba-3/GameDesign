using FirstMonoGame.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using FirstMonoGame.WorldScrolling;
using System.Diagnostics;
using FirstMonoGame.States.PowerUpStates;

namespace FirstMonoGame.Commands
{
    class AscendCommand : ICommand
    {
        private Peach Peach;
        private WarpPipe WarpPipe;
        public AscendCommand(Peach peach, WarpPipe pipe)
        {
            Peach = peach;
            WarpPipe = pipe;
        }

        public void Execute()
        {
            if (Peach.Ascending)
            {
                Peach.PowerUpState = new WarpingState();
                Peach.Location = new Vector2(WarpPipe.Location.X - 5, WarpPipe.Location.Y + 10);
                Peach.LocationBelow = Peach.Location;
                if (WarpPipe.MiniGame == 1)
                {
                    Camera.Instance.SetLevelBounds(Constants.ZERO, 1720, 1920, 3000);
                }
                else if (WarpPipe.MiniGame == 2)
                {
                    Camera.Instance.SetLevelBounds(Constants.ZERO, 1720,Constants.ZERO, 5000);
                }
                else
                {
                    Camera.Instance.SetLevelBounds(Constants.ZERO, 10000,Constants.ZERO, 1080);
                }

                Camera.Instance.Update();
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
