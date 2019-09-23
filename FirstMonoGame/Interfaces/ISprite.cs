using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FirstMonoGame.Collision;

namespace FirstMonoGame
{
    public interface ISprite
    {
        Texture2D Texture { get; set; }
        Vector2 Velocity { get; set; }
        Vector2 CurrentLocation { get; set; }
        bool Display { get; set; }
        bool VisibleBounding { get; set; }
        AABB BoundingBox { get; }
        Vector2 Acceleration { get; set; }
        Texture2D RectangleTexture { get; set; }
        bool Checkable { get; set; }

        void SetFrame(int frame);
        bool LoopFrame { get; set; }
        bool HReflect { get; set; }
        bool VReflect { get; set; }
       // bool IsUsed { get; }

        void Update(GameTime gameTime);
        void Draw(SpriteBatch spritebatch);
        void Toggle();
        //void SetVelocity(float x, float y);
        void InitializeSprite(Texture2D texture, GraphicsDevice graphics, int rows, int columns);
        Vector2 Position();
        void SwitchAnimation(int rowIndex, int columnIndex);
        void ChangeTexture(Texture2D texture, int rows, int columns);
        void SetLevelBound(int farRightSide);
        void ScaleSprite(float scalePercentage);
        
    }
}
