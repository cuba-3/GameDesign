

using FirstMonoGame.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMonoGame
{
    public class KeyState : IKeyState
    {
        private string State;
        /// Idle means key remains untouched
        public int DownTime { get; set; }

        public KeyState()
        {
            State = "IDLE";
            DownTime =Constants.ZERO;
        }

        // IF a key is idle it has never been pressed. Can use setIdle() to reset this
        public void SetIdle()
        {
            State = "IDLE";
            DownTime =Constants.ZERO;
            return;
        }

        public void SetPressed()
        {
            State = "PRESSED";
            return;
        }

        public void SetHeld()
        {
            if (State != "HELD")
            {
                State = "HELD";
            }
            else
            {
                DownTime++;
            }
            return;
        }

        public void SetReleased()
        {
            State = "RELEASED";
            DownTime =Constants.ZERO;
            return;
        }

        public bool IsHeld()
        {
            if (State == "HELD" && DownTime > 30) // Change 30 if hold timeout too long/short
            {
                return true;
            }
            return false;
        }
    }
}