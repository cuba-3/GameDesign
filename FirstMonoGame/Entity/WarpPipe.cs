using FirstMonoGame.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace FirstMonoGame.Entity
{
    public class WarpPipe : IEntity
    {
        public Vector2 OriginalLocation { get; set; }
        public Vector2 Location { get { return Sprite.CurrentLocation; } set { Sprite.CurrentLocation = value; } }
        public bool Visible { get { return Sprite.Display; } set { Sprite.Display = value; } }
        public bool Checkable { get { return Sprite.Checkable; } set { Sprite.Checkable = value; } }
        public Collection<IEntity> ContainedItems { get; } 
        public ISprite Sprite { get; set; }
        public bool VisBounding { get { return Sprite.VisibleBounding; } set { Sprite.VisibleBounding = value; } }
        public bool OnScreen { get; set; }
        public bool ExitPipe { get; set; }
        public bool Underworld { get; set; }
        public int MiniGame { get; set; }

        public WarpPipe(ContentManager content, GraphicsDevice graphics, bool exitPipe, bool underworld, int minigame)
        {
            Sprite = PipeSpriteFactory.Instance.FactoryMethod(content, this, graphics, false);
            ContainedItems = new Collection<IEntity>();
            ExitPipe = exitPipe;
            Underworld = underworld;
            MiniGame = minigame;
        }

        public void Update(GameTime gameTime)
        {

            Sprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch);
        }

        public AABB BoundingBox()
        {
            return Sprite.BoundingBox;
        }

        public void DoCollision(IEntity entity)
        {
            return;
        }

        public bool CanCollide(IEntity other)
        {
            return !(other is Block || other is Piranha);
        }

        public void AddContainedItem(IEntity item)
        {
            ContainedItems.Add(item);
        }

        public void RemoveContainedItem(IEntity item)
        {
            ContainedItems.Remove(item);
        }
    }
}

