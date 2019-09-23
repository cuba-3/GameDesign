using FirstMonoGame.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FirstMonoGame.Entity
{
    // contains all entities (objects in game world)
    public interface IEntity
    {
        Vector2 Location { get; set; }
        bool Visible { get; set; }
        bool Checkable { get; set; }
        bool VisBounding { get; set; }
        Vector2 OriginalLocation { get; set; }
        ISprite Sprite { get; set; }
        bool OnScreen { get; set; }
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
        AABB BoundingBox();
        void DoCollision(IEntity collider);
        bool CanCollide(IEntity other);
    }
}
