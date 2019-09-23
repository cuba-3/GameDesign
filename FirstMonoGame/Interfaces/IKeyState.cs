using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMonoGame.Interfaces
{
    interface IKeyState
    {
        void SetPressed();
        void SetReleased();
        void SetHeld();
        void SetIdle();
        bool IsHeld();
    }
}