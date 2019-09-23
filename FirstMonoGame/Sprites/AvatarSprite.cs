using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using FirstMonoGame.States;
using FirstMonoGame.States2.PowerUpStates;
using FirstMonoGame.WorldScrolling;
using FirstMonoGame.Collision;

namespace FirstMonoGame
{
    public class AvatarSprite : ISprite
    {
        public bool Checkable { get; set; }
        public Texture2D Texture { get; set; }
        //public bool IsUsed { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public int ColumnIndex { get; set; }
        public bool Display { get; set; }
        public bool LoopFrame { get; set; }
        public bool HReflect { get; set; }
        public bool VReflect { get; set; }
        public Vector2 CurrentLocation { get; set; }
        public int CurrentFrame { get; set; }
        private int TotalFrames;
        private int FrameDelay;
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }
        public int RowIndex { get; set; }
        public Texture2D RectangleTexture { get; set; }
        public bool VisibleBounding { get; set; }
        public float Scale { get; set; }
        public AABB BoundingBox
        {
            get
            {
                return new AABB(CurrentLocation.X + 25, CurrentLocation.Y + 18, Scale * 45, Scale * 80);
            }
        }

        public AvatarSprite(GraphicsDevice graphicsDevice)
        {
            Rows =Constants.ZERO;
            Columns =Constants.ZERO;
            CurrentFrame =Constants.ZERO;
            Display = true;
            LoopFrame = true;
            HReflect = false;
            FrameDelay =Constants.ZERO;
            Scale = 1;
            RowIndex =Constants.ZERO;
            ColumnIndex =Constants.ZERO;
            RectangleTexture = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            RectangleTexture.SetData<Color>(new Color[] { Color.Yellow });
            VisibleBounding = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Display)
            {
                int width = Texture.Width / Columns;
                int height = Texture.Height / Rows;
                int row = RowIndex;
                int column = CurrentFrame % ColumnIndex;

               // RectangleTexture = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
               // RectangleTexture.SetData<Color>(new Color[] { Color.White });
                
                Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
                Rectangle destinationRectangle = new Rectangle((int)CurrentLocation.X, (int)CurrentLocation.Y, (int)(Scale * 2 * width),(int)(Scale * 2 * height));

                if (HReflect)
                {
                    if (VisibleBounding)
                    {
                        spriteBatch.Draw(RectangleTexture, new Rectangle { X = (int)BoundingBox.X, Y = (int)BoundingBox.Y, Width = (int)BoundingBox.Width, Height = (int)BoundingBox.Height }, sourceRectangle, Color.White);
                    }
                    spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White,Constants.ZERO, new Vector2(Constants.ZERO,Constants.ZERO), SpriteEffects.FlipHorizontally, 1);
                }
                else
                {
                    if (VisibleBounding)
                    {
                        spriteBatch.Draw(RectangleTexture, new Rectangle { X = (int)BoundingBox.X, Y = (int)BoundingBox.Y, Width = (int)BoundingBox.Width, Height = (int)BoundingBox.Height}, sourceRectangle, Color.White);
                    }
                    spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            KeepLocationOnScreen(gameTime);
            FrameDelay++;
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

        public void Toggle()
        {
            if (!Display)
            {
                Display = true;
            }
            else
            {
                Display = false;
            }
        }

        public Vector2 Position()
        {
            return CurrentLocation;
        }

        // Row and Column Dimensions of entire sprite sheet; updates Rows, Columns, Textures, changes no other anim data
        // If changing to sprite sheet with different 2D texture array (sprites aren't same position as last sheet) need SwitchAnim
        public void ChangeTexture(Texture2D texture, int rows, int columns)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
        }

        // RowIndex = Row number of animation you want shown. INDEX STARTS ATConstants.ZERO
        // ColumnIndex = Number of animation frames in the row you have choosen
        public void SwitchAnimation(int rowIndex, int columnIndex)
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            TotalFrames = columnIndex;
            if (CurrentFrame >= TotalFrames) CurrentFrame =Constants.ZERO; // Set CurrentFrame only if out of bounds
        }

        // Set CurrentFrame
        public void SetFrame(int frame)
        {
            if (frame < TotalFrames)
            {
                CurrentFrame = frame;
            }
        }

        //public void SetVelocity(float x, float y)
        //{
        //    Velocity.X = x;
        //    Velocity.Y = y;
        //}

        //public void SetAcceleration(float x, float y)
        //{
        //    Acceleration.X = x;
        //    Acceleration.Y = y;
        //}

        private void KeepLocationOnScreen(GameTime gameTime)
        {
            //int width = GraphicsDevice.Viewport.Width - 2*(Texture.Width / Columns);
           // int height = GraphicsDevice.Viewport.Height - 2*(Texture.Height / Rows);
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Keeps sprite on screen as suggested in class
            //CurrentLocation = new Vector2(MathHelper.Clamp(CurrentLocation.X + (Velocity.X * dt),Constants.ZERO, FarRightSide - BoundingBox.Width), MathHelper.Clamp(CurrentLocation.Y + Velocity.Y * dt,Constants.ZERO, Camera.Instance.LevelBounds.Bottom + 100));

            if (CurrentLocation + (Velocity * dt) != CurrentLocation)
            {
                RectangleTexture.SetData<Color>(new Color[] { Color.Yellow });
            }
            Velocity += Acceleration * dt;
        }

        public void InitializeSprite(Texture2D texture, GraphicsDevice graphics, int rows, int columns)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            TotalFrames = Rows * Columns;
        }

        public void SetLevelBound(int farRightSide)
        {
            
        }

        public void ScaleSprite(float scale)
        {
            Scale = scale;
        }
    }
}