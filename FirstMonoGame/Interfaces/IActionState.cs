using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMonoGame
{
    public interface IActionState<T> 
    {
        void CrouchingStateTransition();
        void FallingStateTransition();
        void IdleStateTransition();
        void JumpingStateTransition();
        void RunningStateTransition();
        void WalkingStateTransition();
        void FaceLeftTransition();
        void FaceRightTransition();

    }
}
