using FirstMonoGame.Entity;
using FirstMonoGame.Levels;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMonoGame.Collision
{
    class GridMap
    {
        readonly int gridWidth;
        readonly int gridHeight;
        private readonly int tileWidth = 60;
        private readonly int tileHeight = 60;
        private static Rectangle[,] tiles;
        private Dictionary<Rectangle, List<IEntity>> tileToEntityList; // for finding all entities in a given tile
        private List<Rectangle> nonEmptyTiles; // set of non-empty tiles
        private HashSet<IEntity> entities; // all entities gridded

        public GridMap(int gridWidth, int gridHeight)
        {
            this.gridWidth = gridWidth; // set grid width
            this.gridHeight = gridHeight; // set grid height
            tiles = new Rectangle[gridHeight / tileHeight, gridWidth / tileWidth];
            tileToEntityList = new Dictionary<Rectangle, List<IEntity>>();
            nonEmptyTiles = new List<Rectangle>();
            entities = new HashSet<IEntity>();
            GenerateTiles(); // generate tiles
        }

        private void GenerateTiles()
        {
            int columns = gridWidth / tileWidth;
            int rows = gridHeight / tileHeight;
            // generate tiles; O(n) but only runs once at start, n ~ 500
            for (int i =Constants.ZERO; i < rows; i++)
            {
                for (int j =Constants.ZERO; j < columns; j++)
                {
                    tiles[i, j] = new Rectangle(j * tileWidth, i * tileHeight, tileWidth, tileHeight);
                    tileToEntityList.Add(tiles[i, j], new List<IEntity>());
                    //de Debug.WriteLine("Tile generated: " + tiles[i, j].ToString());
                }
            }
        }

        public void Add(IEntity entity)
        {
            // brand new entity in grid
            entities.Add(entity);
            AABB boundingBox = entity.BoundingBox();
            float minX = boundingBox.Left;
            float maxX = boundingBox.Right;
            float maxY = boundingBox.Bottom;
            float minY = boundingBox.Top;
            int columnStart = (int)(minX / tileWidth);
            int rowStart = (int)(minY / tileHeight);
            int columnEnd = (int)(maxX / tileWidth);
            int rowEnd = (int)(maxY / tileHeight);
            for (int i = rowStart; i < rowEnd + 1; i++)
            {
                for (int j = columnStart; j < columnEnd + 1; j++)
                {
                    if (i > -1 && j > -1 && i < gridHeight / tileHeight && j < gridWidth / tileWidth)
                    {
                        tileToEntityList[tiles[i, j]].Add(entity);
                        if (!nonEmptyTiles.Contains(tiles[i, j])) nonEmptyTiles.Add(tiles[i, j]); // if the tile used to be empty, add to non-empty
                                                                                                  //  Debug.WriteLine("entity " + entity.ToString() + " tile " + tiles[i, j].ToString());
                    }
                }
            }
        }

        public void Remove(IEntity entity)
        {
            // remove entity from grid, update mappings accordingly
            int count = nonEmptyTiles.Count;
            for (int i = count - 1; i > -1; i--)
            {
                tileToEntityList[nonEmptyTiles.ElementAt(i)].Remove(entity);
                if (tileToEntityList[nonEmptyTiles.ElementAt(i)].Count ==Constants.ZERO) nonEmptyTiles.Remove(nonEmptyTiles.ElementAt(i));
            }
            entities.Remove(entity);
        }

        public void FindProcessCollisions(GameTime gameTime, bool con)
        {
            double dt = gameTime.ElapsedGameTime.TotalSeconds; // total t change from last frame [s]
            if (con)
            {
                List<Tuple<double, Tuple<IEntity, IEntity>>> pairList = FindCollisionPairs(entities, dt);
                // process pairs, earliest first
                pairList.Sort(CompareEntityPairByDouble);
                HashSet<IEntity> collidedEntities = new HashSet<IEntity>();
                double totalPassedFrac =Constants.ZERO;
                while (pairList.Count >Constants.ZERO)
                {
                    double firstFrac = pairList.First().Item1;
                    totalPassedFrac += firstFrac;
                    foreach (IEntity movingEntity in entities.Where(entity => entity.Sprite.Velocity != Vector2.Zero).ToArray())
                    {
                        Remove(movingEntity); // pre-param change
                        movingEntity.Sprite.CurrentLocation += movingEntity.Sprite.Velocity * (float)(firstFrac * dt); // move to time of first collision
                        Add(movingEntity); // post-param change
                    }
                    foreach (Tuple<double, Tuple<IEntity, IEntity>> firstPair in pairList)
                    {
                        Tuple<IEntity, IEntity> entityPair = firstPair.Item2;
                        if (!collidedEntities.Contains(entityPair.Item1)) collidedEntities.Add(entityPair.Item1);
                        if (!collidedEntities.Contains(entityPair.Item2)) collidedEntities.Add(entityPair.Item2);
                        if (firstPair.Item1 == firstFrac) // if time of first collision
                        {
                            Remove(entityPair.Item1); // remove entities pre-param change
                            Remove(entityPair.Item2);
                            entityPair.Item1.DoCollision(entityPair.Item2);
                            entityPair.Item2.DoCollision(entityPair.Item1);
                            Add(entityPair.Item1); // add entities post-param change
                            Add(entityPair.Item2);
                        }
                    }
                    dt = dt * (1 - totalPassedFrac);
                    pairList = FindCollisionPairs(collidedEntities, dt);
                    pairList.Sort(CompareEntityPairByDouble);
                    collidedEntities.Clear(); // clear collided entity collection
                }
                // no more collisions -> update to end of frame ᕕ( ᐛ )ᕗ
                foreach (IEntity movingEntity in entities.Where(entity => entity.Sprite.Velocity != Vector2.Zero).ToArray())
                {
                    Remove(movingEntity); // pre-param change
                    movingEntity.Sprite.CurrentLocation += movingEntity.Sprite.Velocity * (float)dt;
                    Add(movingEntity); // post-param change
                }
            }
        }

        private List<Tuple<double, Tuple<IEntity, IEntity>>> FindCollisionPairs(IEnumerable<IEntity> entityColn, double dt)
        {
            List<Tuple<double, Tuple<IEntity, IEntity>>> pairList = new List<Tuple<double, Tuple<IEntity, IEntity>>>();
            HashSet<Tuple<IEntity, IEntity>> pairSet = new HashSet<Tuple<IEntity, IEntity>>(); // set of unique collision pairs
            // for all moving entities, find entities that might be colliders
            foreach (IEntity collidee in entityColn.Where(entity => entity.Sprite.Velocity != Vector2.Zero && entity.Checkable))
            {
                AABB collideeBox = collidee.BoundingBox();
                float dx = collidee.Sprite.Velocity.X * (float)dt; // total y change this frame [px]
                float dy = collidee.Sprite.Velocity.Y * (float)dt; // total x change this frame [px]
                Tuple<int, int, int, int> neighborTiles = GetNeighborTiles(collideeBox, dx, dy);
                for (int i = neighborTiles.Item1; i <= neighborTiles.Item2; i++)
                {
                    for (int j = neighborTiles.Item3; j <= neighborTiles.Item4; j++)
                    {
                        if (i > -1 && j > -1 && i < gridHeight / tileHeight && j < gridWidth / tileHeight)
                        {
                            foreach (IEntity collider in tileToEntityList[tiles[i, j]].Where(entity => entity.Checkable))
                            {
                                if (collidee != collider)
                                {
                                    var pair = Tuple.Create(collidee, collider);
                                    var pairRev = Tuple.Create(collider, collidee);
                                    if (!pairSet.Contains(pairRev) && !pairSet.Contains(pair))
                                    {
                                        if (collidee.CanCollide(collider) && collider.CanCollide(collidee))
                                        {
                                            pairSet.Add(pair);
                                            // find distance fraction t of collision between pair
                                            AABB colliderBox = collider.BoundingBox();
                                            AABB minkDiff = collideeBox.MinkowskiDifference(colliderBox); // B - A [minkowski]
                                            if (colliderBox.MinkowskiDifferenceContainsOrigin(collideeBox))
                                            {
                                                // t =Constants.ZERO
                                                pairList.Add(Tuple.Create((double)0, pair));
                                            }
                                            else
                                            {
                                                // find t, t >Constants.ZERO
                                                Vector2 relMotion = collider.Sprite.Velocity - collidee.Sprite.Velocity; // A's velocity in B's frame
                                                relMotion *= (float)dt;
                                                double t = VectorIntersectFraction(Vector2.Zero, relMotion, minkDiff); // raycast relative motion against mink aabb
                                                if (t < double.PositiveInfinity)
                                                {
                                                    // pair collides this frame
                                                    pairList.Add(Tuple.Create(t, pair));
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return pairList;
        }

        private Tuple<int, int, int, int> GetNeighborTiles(AABB aabb, float dx, float dy)
        {
            int rowStart;
            int rowEnd;
            int colStart;
            int colEnd;
            if (dx <Constants.ZERO)
            {
                colStart = (int)Math.Ceiling(aabb.Left + dx) - 1; // include left tile if left edge on tile edge
                colStart /= tileWidth;
                colEnd = (int)(aabb.Right / tileWidth);
            }
            else
            {
                colStart = (int)(aabb.Left / tileWidth);
                if (aabb.Left % tileWidth ==Constants.ZERO) colStart--; // include left tile if left edge on tile edge
                colEnd = (int)(aabb.Right + dx) / tileWidth;
            }
            if (dy <Constants.ZERO)
            {
                rowStart = (int)Math.Ceiling(aabb.Top + dy) - 1; // include top tile if top edge on tile edge
                rowStart /= tileHeight;
                rowEnd = (int)(aabb.Bottom / tileHeight);
            }
            else
            {
                rowStart = (int)(aabb.Top / tileHeight);
                if (aabb.Top % tileHeight ==Constants.ZERO) rowStart--; // include top tile if top edge on tile edge
                rowEnd = (int)(aabb.Bottom + dy) / tileHeight;
            }
            return Tuple.Create(rowStart, rowEnd, colStart, colEnd);
        }

        private static double VectorIntersectFraction(Vector2 originA, Vector2 endA, Vector2 originB, Vector2 endB)
        {
            Vector2 u = endA - originA;
            Vector2 v = endB - originB;

            double numerator = Cross(originB - originA, u);
            double denominator = Cross(u, v);

            if (numerator == Constants.ZERO && denominator == Constants.ZERO)
            {
                // colinear
                return double.PositiveInfinity;
            }

            if (denominator ==Constants.ZERO)
            {
                // parallel
                return double.PositiveInfinity;
            }

            double d = numerator / denominator;
            double h = Cross(originB - originA, v) / denominator;
            if (d >=Constants.ZERO && d <= 1 && h >=Constants.ZERO && h <= 1)
            {
                // intersecting
                return h;
            }
            return double.PositiveInfinity;
        }

        private static double VectorIntersectFraction(Vector2 origin, Vector2 v, AABB aabb)
        {
            Vector2 end = origin + v;
            // raycast to find minimum intersect fraction
            double minT = VectorIntersectFraction(origin, end, new Vector2(aabb.Left, aabb.Top), new Vector2(aabb.Left, aabb.Bottom));
            double x;
            x = VectorIntersectFraction(origin, end, new Vector2(aabb.Left, aabb.Bottom), new Vector2(aabb.Right, aabb.Bottom));
            if (x < minT) minT = x;
            x = VectorIntersectFraction(origin, end, new Vector2(aabb.Right, aabb.Bottom), new Vector2(aabb.Right, aabb.Top));
            if (x < minT) minT = x;
            VectorIntersectFraction(origin, end, new Vector2(aabb.Right, aabb.Top), new Vector2(aabb.Left, aabb.Top));
            if (x < minT) minT = x;
            return minT;
        }

        private static int CompareEntityPairByDouble(Tuple<double, Tuple<IEntity, IEntity>> x, Tuple<double, Tuple<IEntity, IEntity>> y)
        {
            return x.Item1.CompareTo(y.Item1);
        }

        private static double Cross(Vector2 a, Vector2 b)
        {
            return a.X * b.Y - a.Y * b.X;
        }
    }
}
