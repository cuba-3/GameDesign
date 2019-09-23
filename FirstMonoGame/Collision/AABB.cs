using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMonoGame.Collision
{
    // Implementation of AABB using (X, Y) [top left], Width, Height: float[px] coordinates, (Constants.ZERO,Constants.ZERO) is top left point and origin.
    // Convention: Width >=Constants.ZERO, Height >=Constants.ZERO, x >=Constants.ZERO, y >=Constants.ZERO.
    public struct AABB
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float Left { get { return X; } }
        public float Right { get { return X + Width; } }
        public float Top { get { return Y; } }
        public float Bottom { get { return Y + Height; } }
        public Vector2 Center { get { return new Vector2(Left +0.5f * Width, Top +0.5f * Height); } }

        public AABB(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public bool Contains(float x, float y)
        {
            return x >= Left && x <= Right && y >= Top && y <= Bottom;
        }

        public AABB MinkowskiDifference(AABB other)
        {
            return new AABB(Left - other.Right, Top - other.Bottom, Width + other.Width, Height + other.Height); // this - other
        }

        public bool MinkowskiDifferenceContainsOrigin(AABB other)
        {
            AABB md = MinkowskiDifference(other);
            return md.Contains(Constants.ZERO, Constants.ZERO);
        }
    }
}
