using FirstMonoGame.Entity;
using Microsoft.Xna.Framework;
using FirstMonoGame.States2.PowerUpStates;
using FirstMonoGame.Levels;
using FirstMonoGame.States.PeachStates;
using FirstMonoGame.States.BlockStates;
using FirstMonoGame.States2.PeachStates;
using FirstMonoGame.States.EnemyStates;
using System.Diagnostics;
using System;
using System.Collections.ObjectModel;
using FirstMonoGame.States.ItemStates;
using Microsoft.Xna.Framework.Audio;
using FirstMonoGame.States.PowerUpStates;
using FirstMonoGame.WorldScrolling;
using FirstMonoGame.Commands;

namespace FirstMonoGame.Collision
{
    // Singleton for exposing collisions and level
    class Collections
    {
        private Level Level;
        private CollisionDetection Collisions;
        private static Collections _instance;
        
        public static Collections Instance
        {
            get
            {
                if (_instance == null) _instance = new Collections();
                return _instance;
            }
        }

        public void SetCollisionRef(ref CollisionDetection collisions)
        {
            this.Collisions = collisions;
        }

        public ref Level GetLevelRef()
        {
            return ref Level;
        }

        public ref CollisionDetection GetCollisionRef()
        {
            return ref Collisions;
        }

        public void SetLevelRef(ref Level level)
        {
            this.Level = level;
        }
    }
}
