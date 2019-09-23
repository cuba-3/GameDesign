using FirstMonoGame.Entity;
using FirstMonoGame.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMonoGame.Collision
{
    // barebones methods for a class responsible for tracking AABB entities and finding collisions among them
    interface ICollisionFinder
    {
        void Add(IEntity entity);
        void Remove(IEntity entity);
        void Update();
        void FindCollisions();
    }
}
