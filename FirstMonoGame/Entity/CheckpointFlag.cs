using FirstMonoGame.Collision;
using FirstMonoGame.Interfaces;
using FirstMonoGame.States.ItemStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMonoGame.Entity
{
    class CheckpointFlag : IEntity
    {
        public Vector2 OriginalLocation { get; set; }
        public Vector2 Location { get { return Sprite.CurrentLocation; } set { Sprite.CurrentLocation = value; } }
        public IItemState<CheckpointFlag> FlagState { get; set; }
        public ISprite Sprite { get; set; }
        public bool Visible { get { return Sprite.Display; } set { Sprite.Display = value; } }
        public bool VisBounding { get { return Sprite.VisibleBounding; } set { Sprite.VisibleBounding = value; } }
        public bool Checkable { get; set; }
        public bool OnScreen { get; set; }
        public ContentManager Content;
        public GraphicsDevice Graphics;

        public CheckpointFlag(ContentManager content, GraphicsDevice graphics)
        {
            Content = content;
            Graphics = graphics;
            FlagState = new CheckpointFlagNewState();
            Sprite = CheckpointFlagSpriteFactory.Instance.FactoryMethod(content, graphics, this);
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

        public void DropFlagTransition()
        {
            FlagState = new CheckpointFlagDropState();
            Sprite = CheckpointFlagSpriteFactory.Instance.FactoryMethod(Content, Graphics, this);
        }

        public void DoCollision(IEntity entity)
        {
            if (entity is Peach)
            {
                DropFlagTransition();
            }
        }

        public bool CanCollide(IEntity other)
        {
            return FlagState is CheckpointFlagNewState && other is Peach;
        }
    }
}
