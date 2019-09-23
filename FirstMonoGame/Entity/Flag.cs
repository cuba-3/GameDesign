using FirstMonoGame.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstMonoGame.Collision;
using FirstMonoGame.Levels;
using FirstMonoGame.States.ItemStates;
using FirstMonoGame.Interfaces;
using System.Diagnostics;

namespace FirstMonoGame.Entity
{
    class Flag : IEntity
    {
        public Vector2 OriginalLocation { get; set; }
        public IItemState<Flag> FlagState { get; set; }
        public ISprite Sprite { get; set; }
        public Vector2 Location { get { return Sprite.CurrentLocation; } set { Sprite.CurrentLocation = value; } }
        public bool Visible { get { return Sprite.Display; } set { Sprite.Display = value; } }
        public bool VisBounding { get { return Sprite.VisibleBounding; } set { Sprite.VisibleBounding = value; } }
        public bool Checkable { get; set; }
        public ContentManager Content { get; set; }
        public bool OnScreen { get; set; }
        public GraphicsDevice Graphics;
        public Flag(ContentManager content, GraphicsDevice graphics)
        {
            Graphics = graphics;
            Content = content;
            Sprite = FlagSpriteFactory.Instance.FactoryMethod(content, graphics, this);
            FlagState = new FlagNewState();
            Sprite.HReflect = true;
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
            //FlagState = new FlagDropState();
            FlagState.DoAction(this);
        }

        public void DoCollision(IEntity entity)
        {
            if(entity is Peach)
            {
                DropFlagTransition();
            } 
            
        }

        public bool CanCollide(IEntity other)
        {
            return other is Peach && FlagState is FlagNewState;
        }
    }
}
