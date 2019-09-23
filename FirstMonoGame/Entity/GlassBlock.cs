using FirstMonoGame.Collision;
using FirstMonoGame.Interfaces;
using FirstMonoGame.Levels;
using FirstMonoGame.Sprites;
using FirstMonoGame.States.BlockStates;
using FirstMonoGame.States.PeachStates;
using FirstMonoGame.States2.PowerUpStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace FirstMonoGame.Entity
{
    class GlassBlock : Block
    {
        public IBlockState<GlassBlock> BlockState { get; set; }
        private GraphicsDevice GraphicsDevice;
        public ContentManager Content { get; set; }
        public Collection<IEntity> ContainedItems { get; set; }
        public bool Bump { get; set; }
        private bool consumed;

        public GlassBlock(ContentManager content, GraphicsDevice graphicsDevice)
        {
            Content = content;
            BlockState = new NewGlassBlockState(graphicsDevice); // init state
            GraphicsDevice = graphicsDevice;
            ContainedItems = new Collection<IEntity>();
            Bump = true;
            Sprite = GlassBlockSpriteFactory.Instance.FactoryMethod(content, this, GraphicsDevice, false);
        }

        public void BumpBrickBlockTransition()
        {
            BlockState.BumpTransition(this);
        }

        public void ExplodeBrickBlockTransition()
        {
            // controls block delete
            consumed = true;
            Debug.WriteLine("Explode glass block transition");
        }

        public override void Update(GameTime gameTime)
        {
            Sprite.Update(gameTime);
//            this.Location = this.OriginalLocation;
            if (consumed)
            {
                Collections.Instance.GetCollisionRef().Deregister(this);
                Collections.Instance.GetLevelRef().Entities.Remove(this);
            }
        }

        public void AddContainedItem(IEntity item)
        {
            ContainedItems.Add(item);
        }


        public override void DoCollision(IEntity entity)
        {
            if (entity is Peach peach)
            {
                AABB peachBounds = peach.BoundingBox();
                AABB blockBounds = BoundingBox();
                if (blockBounds.Bottom - peachBounds.Top <= 10 && peach.ActionState is JumpingState)
                {
                    Debug.WriteLine("Glass state: " + this.BlockState);
                    if (Bump && this.BlockState is NewGlassBlockState)
                    {
                        BumpBrickBlockTransition();
                    }
                    else if(this.BlockState is CrackedGlassBlockState)
                    {
                        Debug.WriteLine("Block state is Cracked Glass Block State");
                        int count = ContainedItems.Count;
                        for (int i =Constants.ZERO; i < count; i++)
                        {
                                ExplodeBrickBlockTransition(); //remove the brickblock from screen and deregister it
                                ExplodingBlock child = (ExplodingBlock)ContainedItems[i]; // must do following for all four exploding blocks
                                child.Visible = true;
                                if (i == 1) child.Sprite.Velocity = new Vector2(-300, -300); // make the blocks go in different directions
                                else if (i == 2) child.Sprite.Velocity = new Vector2(300, 300);
                                else if (i == 3) child.Sprite.Velocity = new Vector2(300, -300);
                                else if (i == 4) child.Sprite.Velocity = new Vector2(-300, 300);
                                child.ExplodingBlockTransition();
                        }
                    }
                }
            }
        }

    }
}


