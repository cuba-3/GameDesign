using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace FirstMonoGame.WorldScrolling
{
    public sealed class Camera
    {
        static readonly Camera _instance = new Camera();
        public static Camera Instance
        {
            get
            {
                return _instance;
            }
        }

        private Vector2 Position;
        private Vector2 Origin;
        public float Zoom { get; set; }
        public float Rotation { get; set; }
        public Peach Peach { get; set; }
        private Viewport Viewport;
        public bool VerticalScrollMode { get; set; }

        public Rectangle CameraBounds
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, Viewport.Width, Viewport.Height);
            }
        }

        public Rectangle LevelBounds { get; set; }

        Camera()
        {
            Zoom = 1.0f;
            LevelBounds = new Rectangle(Constants.ZERO,Constants.ZERO, Viewport.Width, Viewport.Height);
            VerticalScrollMode = false;
        }

        public void SetViewport(Viewport viewport)
        {
            Origin = new Vector2(viewport.Width / 2.0f, viewport.Height / 2.0f);
            Viewport = viewport;
        }

        public Matrix GetViewMatrix(Vector2 parallax)
        {
            return Matrix.CreateTranslation(new Vector3(-Position * parallax,0.0f)) *
                Matrix.CreateTranslation(new Vector3(-Origin,0.0f)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(Zoom, Zoom, 1) *
                Matrix.CreateTranslation(new Vector3(Origin,0.0f));
        }

        public void Update()
        {
            if (VerticalScrollMode && IsOnScreen(new Rectangle { X = (int)Peach.BoundingBox().Left, Y = (int)Peach.BoundingBox().Top, Width = (int)Peach.BoundingBox().Width, Height = (int)Peach.BoundingBox().Height }))
            {
                float oldPosition = Position.Y;
                if(Peach.Sprite.Position().Y - Origin.Y < oldPosition)
                {
                    Position = Peach.Sprite.Position() - Origin;
                }
            }
            else
            {
                Position = Peach.Sprite.Position() - Origin;

            }
            // Screen bounds of level
            Position = new Vector2(MathHelper.Clamp(Position.X,Constants.ZERO, LevelBounds.Right - Viewport.Width), MathHelper.Clamp(Position.Y, LevelBounds.Top, LevelBounds.Bottom - Viewport.Height));
        }

        public bool IsOnScreen(Rectangle spriteBoundingBox)
        {
            return spriteBoundingBox.Intersects(CameraBounds);
        }

        public void SetLevelBounds(int minX, int maxX, int minY, int maxY)
        {
            LevelBounds = new Rectangle(new Point(minX, minY), new Point(maxX - minX, maxY - minY));
            int width = System.Math.Max(LevelBounds.Width, Viewport.Width);
            int height = System.Math.Max(LevelBounds.Height, Viewport.Height);
            LevelBounds = new Rectangle(LevelBounds.X, LevelBounds.Y, width, height);
            Peach.Sprite.SetLevelBound(LevelBounds.Right);
        }

        public void SwitchToVerticalScrollMode()
        {
            VerticalScrollMode = true;
        }
    }
}
