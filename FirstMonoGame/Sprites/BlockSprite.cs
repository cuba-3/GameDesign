using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FirstMonoGame.Entity;
using FirstMonoGame.Collision;

namespace FirstMonoGame.Sprites
{
    public class BlockSprite : ISprite
    {
        public Texture2D Texture { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public bool Display { get; set; }
        public bool LoopFrame { get; set; }
        //public bool IsUsed { get; set; }
        public Vector2 CurrentLocation { get; set; }
        private int CurrentFrame;
        private int TotalFrames;
        private int FrameDelay;
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }
        public Texture2D RectangleTexture { get; set; }
        private GraphicsDevice GraphicsDevice;
        public bool Checkable { get; set; }
        public bool VisibleBounding { get; set; }

        public AABB BoundingBox
        {
            get
            {
                return new AABB(CurrentLocation.X, CurrentLocation.Y, Texture.Width / Columns,  Texture.Height / Rows);
            }
        }

        public bool HReflect { get; set; }
        public bool VReflect { get; set; }

        public BlockSprite(GraphicsDevice graphicsDevice)
        {
            Rows =Constants.ZERO;
            Columns =Constants.ZERO;
            CurrentFrame =Constants.ZERO;
            Display = true;
            LoopFrame = true;
            FrameDelay =Constants.ZERO;
            RowIndex =Constants.ZERO;
            ColumnIndex =Constants.ZERO;
            Velocity = new Vector2(Constants.ZERO,Constants.ZERO);
            GraphicsDevice = graphicsDevice;
            RectangleTexture = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            RectangleTexture.SetData<Color>(new Color[] { Color.White });
            CurrentLocation = this.Position();
            VisibleBounding = false;
            //IsUsed = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Display && CheckProperSprite())
            {

                double frameWidthD = (Texture.Width / Columns) * 1.2;
                double frameHeightD = (Texture.Height / Rows) * 1.2;
                int frameWidth = (Texture.Width / Columns);
                int frameHeight = (Texture.Height / Rows);
                Rectangle sourceRectangle = new Rectangle((int)frameWidth * CurrentFrame,Constants.ZERO, (int)frameWidth, (int)frameHeight);
                Rectangle destinationRectangle = new Rectangle((int)CurrentLocation.X, (int)CurrentLocation.Y, (int)frameWidthD, (int)frameHeightD);
                if (VisibleBounding)
                {
                    spriteBatch.Draw(RectangleTexture, destinationRectangle, sourceRectangle, Color.Blue);
                }
                spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
            }
        }

        public void InitializeSprite(Texture2D texture, GraphicsDevice graphicsDevice, int rows, int columns)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            TotalFrames = Rows * Columns;
            GraphicsDevice = graphicsDevice;
        }

        public Vector2 Position()
        {
            return CurrentLocation;
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

        public void Update(GameTime gameTime)
        {
            if (Display && CheckProperSprite())
            {
                UpdateLocation(gameTime);
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

        public void SwitchAnimation(int rowIndex, int columnIndex)
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            TotalFrames = columnIndex;
            if (CurrentFrame >= TotalFrames) CurrentFrame =Constants.ZERO; // Set CurrentFrame only if out of bounds
        }

        private void UpdateLocation(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //CurrentLocation += Velocity * dt;
            Velocity += Acceleration * dt;
        }

        public bool CheckProperSprite()
        {
            bool isProper = true;
            if (Texture == null)
            {
                isProper = false;
            }
            if (Rows ==Constants.ZERO)
            {
                isProper = false;
            }
            if (Columns ==Constants.ZERO)
            {
                isProper = false;
            }
            if (CurrentLocation == new Vector2(-1, -1))
            {
                isProper = false;
            }
            return isProper;
        }


        //public void SetVelocity(float x, float y)
        //{
        //    Velocity.X = x;
        //    Velocity.Y = y;
        //}

        public void ChangeTexture(Texture2D texture, int rows, int columns)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
