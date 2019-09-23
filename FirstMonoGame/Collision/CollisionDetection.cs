using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Windows.Forms;
using FirstMonoGame.Entity;
using FirstMonoGame.Levels;
using System.Diagnostics;

namespace FirstMonoGame.Collision
{
    // class for collision detection using IGriddedEntity objects within a grid
    public class CollisionDetection
    {
        private GridMap collisionMap;
        private List<IEntity> collisionEntites;

        public CollisionDetection()
        {
            collisionEntites = new List<IEntity>();
        }

        public void FindCollisions(GameTime gameTime, bool con)
        {
            collisionMap.FindProcessCollisions(gameTime, con);
        }

        public void Register(IEntity entity)
        {
            collisionEntites.Add(entity);
        }

        public void ReRegister(IEntity entity)
        {
            collisionMap.Add(entity);
        }

        public void Deregister(IEntity entity)
        {
            collisionMap.Remove(entity);
        }

        public void StartDetection(int levelWidth, int levelHeight)
        {
            collisionMap = new GridMap(levelWidth, levelHeight);
            foreach (IEntity entity in collisionEntites)
            {
                collisionMap.Add(entity);
            }
        }
    }
}
