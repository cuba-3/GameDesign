using FirstMonoGame.Collision;
using FirstMonoGame.WorldScrolling;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FirstMonoGame.Sprites
{
    class EnemySprite : ISprite
    {
        private float Scale;
        public Texture2D Texture { get; set; }
        private int Rows { get; set; }
        private int Columns { get; set; }
       // public bool IsUsed { get; set; }
        public Vector2 CurrentLocation { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }
        public bool Display { get; set; }
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }
        public bool LoopFrame { get; set; }
        public bool HReflect { get; set; }
        public bool VReflect { get; set; }
        //public bool Checkable { get; set; }
        private int CurrentFrame;
        private int TotalFrames;
        private int FrameDelay;
        public GraphicsDevice GraphicsDevice;
        public bool VisibleBounding { get; set; }
        public Texture2D RectangleTexture { get; set; }
        public bool Checkable { get; set; }
        public AABB BoundingBox
        {
            get
            {
                return new AABB(CurrentLocation.X, CurrentLocation.Y, Scale * (Texture.Width / Columns), Scale * (Texture.Height / Rows));
            }
        }

        public EnemySprite(GraphicsDevice graphicsDevice, Texture2D texture, int rows, int columns, Vector2 location, int rowIndex, int columnIndex)
        {
            CurrentFrame =Constants.ZERO;
            Display = true;
            LoopFrame = true;
            HReflect = false;
            FrameDelay =Constants.ZERO;
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            Texture = texture;
            Rows = rows;
            Columns = columns;
            CurrentLocation = location;
            TotalFrames = columnIndex;
            if (CurrentFrame >= TotalFrames) CurrentFrame =Constants.ZERO;
            GraphicsDevice = graphicsDevice;
            RectangleTexture = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            RectangleTexture.SetData<Color>(new Color[] { Color.White });
            VisibleBounding = false;
            Scale = 1;
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = RowIndex;
            int column = CurrentFrame % ColumnIndex;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)CurrentLocation.X, (int)CurrentLocation.Y, (int)Scale * width, (int)Scale * height);
            
           /* if (VisibleBounding)
            {
                spriteBatch.Draw(RectangleTexture, destinationRectangle, sourceRectangle, Color.Red);
            }
            
            if (HReflect is true)
            {
                spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White,Constants.ZERO, new Vector2(Constants.ZERO,Constants.ZERO), SpriteEffects.FlipHorizontally, 1);
            }
            else
            {
                spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
            }*/



            if (HReflect && !VReflect)
            {
                if (VisibleBounding)
                {
                    spriteBatch.Draw(RectangleTexture, new Rectangle { X = (int)BoundingBox.X, Y = (int)BoundingBox.Y, Width = (int)BoundingBox.Width, Height = (int)BoundingBox.Height }, sourceRectangle, Color.White);
                }
                else
                {
                    spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White,Constants.ZERO, new Vector2(Constants.ZERO,Constants.ZERO), SpriteEffects.FlipHorizontally, 1);
                }
            }
            else if (!VReflect)
            {
                if (VisibleBounding)
                {
                    spriteBatch.Draw(RectangleTexture, new Rectangle { X = (int)BoundingBox.X, Y = (int)BoundingBox.Y, Width = (int)BoundingBox.Width, Height = (int)BoundingBox.Height}, sourceRectangle, Color.White);
                }
                spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
            }
            else
            {
                if (VisibleBounding)
                {
                    spriteBatch.Draw(RectangleTexture, new Rectangle { X = (int)BoundingBox.X, Y = (int)BoundingBox.Y, Width = (int)BoundingBox.Width, Height = (int)BoundingBox.Height }, sourceRectangle, Color.White);
                }
                else
                {
                    spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White,Constants.ZERO, new Vector2(Constants.ZERO,Constants.ZERO), SpriteEffects.FlipVertically, 1);
                }
            }
           //bool onScreen = camera.IsOnScreen(destinationRectangle);
        }

        public void InitializeSprite(Texture2D texture, GraphicsDevice graphicsDevice, int rows, int columns)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            TotalFrames = Rows * Columns;
        }

        public Vector2 Position()
        {
            return CurrentLocation;
        }

        public void Toggle()
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            if (Display)
            {
                FrameDelay++;
                KeepLocationOnScreen(gameTime);
                if (FrameDelay >= 10)
                {
                    FrameDelay =Constants.ZERO;
                    CurrentFrame++;
                    if (CurrentFrame == TotalFrames)
                    {
                        CurrentFrame--;
                        if (LoopFrame) CurrentFrame =Constants.ZERO;
                    }
                }
            }
        }

        public void SwitchAnimation(int rowIndex, int columnIndex)
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            TotalFrames = columnIndex;
            if (CurrentFrame >= TotalFrames) CurrentFrame =Constants.ZERO; // Set CurrentFrame only if out of bounds
        }

        private void KeepLocationOnScreen(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //int width = GraphicsDevice.Viewport.Width - 2 * (Texture.Width / Columns);
            //int height = GraphicsDevice.Viewport.Height - 2 * (Texture.Height / Rows);

            // Keeps sprite on screen as suggested in class
            //CurrentLocation = new Vector2(CurrentLocation.X + Velocity.X * dt, CurrentLocation.Y + Velocity.Y * dt);

            if (CurrentLocation + Velocity * dt != CurrentLocation)
            {
                RectangleTexture.SetData<Color>(new Color[] { Color.Red });
            }
            Velocity += Acceleration * dt;
        }

        public void ChangeTexture(Texture2D texture, int rows, int columns)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
        }

        public void SetFrame(int frame)
        {
            throw new NotImplementedException();
        }

        public void SetLevelBound(int farRightSide)
        {
            throw new NotImplementedException();
        }

        public void ScaleSprite(float scale)
        {
            Scale = scale;
        }
    }
}
