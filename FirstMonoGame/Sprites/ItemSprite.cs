using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstMonoGame.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FirstMonoGame.Sprites
{
    class ItemSprite : ISprite
    {
        public bool Checkable { get; set; }
        public Texture2D Texture { get; set; }
        public bool HReflect { get; set; }
        public bool VReflect { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public bool Display { get; set; }
        public Vector2 CurrentLocation { get; set; }
        public bool LoopFrame { get; set; }
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }
        public Texture2D RectangleTexture { get; set; }
        //public bool Checkable { get; set; }
        //public bool IsUsed { get; set; }
        public bool VisibleBounding { get; set; }
        public Vector2 Acceleration { get; set; }

        public GraphicsDevice GraphicsDevice;
        private int CurrentFrame;
        private int TotalFrames;
        private int FrameDelay;
        public Vector2 Velocity { get; set; }
        private float Scale;
        public AABB BoundingBox
        {
            get
            {
                return new AABB(CurrentLocation.X, CurrentLocation.Y, Scale *(Texture.Width / Columns), Scale * (Texture.Height / Rows));
            }
        }


        public ItemSprite(GraphicsDevice graphicsDevice, Texture2D texture, int rows, int columns, int rowIndex, int columnIndex)
        {
            CurrentFrame =Constants.ZERO;
            Display = true;
            LoopFrame = true;
            FrameDelay =Constants.ZERO;
            Scale = 1;
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            Texture = texture;
            HReflect = false;
            Rows = rows;
            Columns = columns;
            TotalFrames = columnIndex;
            GraphicsDevice = graphicsDevice;
            if (CurrentFrame >= TotalFrames) CurrentFrame =Constants.ZERO;
            RectangleTexture = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            RectangleTexture.SetData<Color>(new Color[] { Color.White });
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

                //RectangleTexture = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                //RectangleTexture.SetData<Color>(new Color[] { Color.White });
                Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
                Rectangle destinationRectangle = new Rectangle((int)CurrentLocation.X, (int)CurrentLocation.Y, (int)(Scale * width), (int)(Scale * height));

                if (VisibleBounding)
                {
                    spriteBatch.Draw(RectangleTexture, destinationRectangle, sourceRectangle, Color.Green);
                }

                if (HReflect)
                {
                    spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White,Constants.ZERO, new Vector2(Constants.ZERO,Constants.ZERO), SpriteEffects.FlipHorizontally, 1);
                }
                else
                {
                    spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
                }
            }
        }

        public void InitializeSprite(Texture2D texture, GraphicsDevice graphicsDevice, int rows, int columns)
        {
            throw new NotImplementedException();
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
        }

        private void KeepLocationOnScreen(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //CurrentLocation += Velocity * dt;

            if (CurrentLocation + Velocity * dt != CurrentLocation)
            {
                RectangleTexture.SetData<Color>(new Color[] { Color.Red });
            }
            Velocity += Acceleration * dt;
        }

        public void SwitchAnimation(int rowIndex, int columnIndex)
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            TotalFrames = columnIndex;
            if (CurrentFrame >= TotalFrames) CurrentFrame =Constants.ZERO; // Set CurrentFrame only if out of bounds
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
