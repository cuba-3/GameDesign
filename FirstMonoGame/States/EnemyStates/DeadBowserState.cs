using FirstMonoGame.Entity;
using FirstMonoGame.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace FirstMonoGame.States.EnemyStates
{
    class DeadBowserState : IEnemyState<Bowser>
    {
        public DeadBowserState()
        {
        }

        public void StandardTransition(Bowser enemy)
        {
            throw new NotImplementedException();
        }

        public void PunchingTransition(Bowser enemy)
        {
            throw new NotImplementedException();
        }

        public void WalkingTransition(Bowser enemy)
        {
            throw new NotImplementedException();
        }

        public void TakeDamageTransition(Bowser enemy)
        {
            enemy.isConsumed = true;
        }

        public void BreatheFireTransition(Bowser enemy)
        {
            throw new NotImplementedException();
        }
    }
}
