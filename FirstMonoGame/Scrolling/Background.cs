using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FirstMonoGame
{
    class Background
    {
        private Texture2D Texture;
        private Vector2 Offset;
        public float Zoom;
        private Viewport Viewport;
        public float ScrollFactor;

        private Rectangle Rectangle
        {
            get { return new Rectangle((int)Offset.X, (int)Offset.Y, (int)(Viewport.Width / Zoom), (int)(Viewport.Height / Zoom)); }
        }

        public Background(Viewport viewport, Texture2D texture, float zoom, float scrollFactor)
        {
            Texture = texture;
            Offset = Vector2.Zero;
            Zoom = zoom;
            Viewport = viewport;
            ScrollFactor = scrollFactor;
        }

        public void Update(int positionChange)
        {
            Offset = new Vector2(-positionChange * ScrollFactor,Constants.ZERO);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, new Vector2(Viewport.X, Viewport.Y), Rectangle, Color.White,Constants.ZERO, Vector2.Zero, Zoom, SpriteEffects.None, 1);
        }
    }
}
