using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using FirstMonoGame.WorldScrolling;

namespace FirstMonoGame
{
    public class BackgroundControl
    {
        private List<Background> Backgrounds;
        public Peach Peach { get; set; }
        public Rectangle OrignalCameraBounds { get; set; }
        public BackgroundControl(Viewport viewport, ContentManager content)
        {
            Backgrounds = new List<Background>();
            OrignalCameraBounds = viewport.Bounds;
            // Add background images here
            Backgrounds.Add(new Background(viewport, content.Load<Texture2D>("Backgrounds/MountainBackground"), 1.8f,0.3f));
            Backgrounds.Add(new Background(viewport, content.Load<Texture2D>("Backgrounds/clouds"), 1f,0.8f));
        }

        public void Update()
        {
            int positionChange = OrignalCameraBounds.Center.X - Camera.Instance.CameraBounds.Center.X;
            foreach (Background background in Backgrounds)
            {
                background.Update(positionChange);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Background background in Backgrounds)
            {
                background.Draw(spriteBatch);
            }
        }
    }
}