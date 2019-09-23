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
    class BrickBlock : Block
    {
        public IBlockState<BrickBlock> BlockState { get; set; }
        private GraphicsDevice GraphicsDevice;
        public ContentManager Content { get; set; }
        public Collection<IEntity> ContainedItems { get; set; }
        public bool Bump { get; set; }
        public bool Underworld { get; set; }
        public SoundEffect sound;
        private bool consumed;
        public Peach BumpedBy { get; set; }

        public BrickBlock(ContentManager content, GraphicsDevice graphicsDevice, bool underworld)
        {
            Content = content;
            BlockState = new NewBrickBlockState(graphicsDevice); // init state
            GraphicsDevice = graphicsDevice;
            ContainedItems = new Collection<IEntity>();
            Bump = true;
            Underworld = underworld;
            Sprite = BrickBlockSpriteFactory.Instance.FactoryMethod(content, this, GraphicsDevice, Underworld);
            sound = content.Load<SoundEffect>("Sound Effects/Item Appears");
        }

        public void BumpBrickBlockTransition()
        {
            if (Bump)
            {
                BlockState.BumpTransition(this);
            }
        }

        public void ExplodeBrickBlockTransition()
        {
            // controls block delete
            consumed = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (Sprite.CurrentLocation.Y <= OriginalLocation.Y - 30)
            {
                Sprite.Velocity *= new Vector2(1, -1);
                if (Sprite.CurrentLocation.Y < OriginalLocation.Y - 30) Sprite.CurrentLocation = new Vector2(Sprite.CurrentLocation.X, OriginalLocation.Y - 30);
            }
            else if (Sprite.CurrentLocation.Y >= OriginalLocation.Y)
            {
                Sprite.Velocity *= new Vector2(1,Constants.ZERO);
                if (Sprite.CurrentLocation.Y > OriginalLocation.Y) Sprite.CurrentLocation = new Vector2(Sprite.CurrentLocation.X, OriginalLocation.Y);
            }
            Sprite.Update(gameTime);
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

        public void RemoveContainedItem(IEntity item)
        {
            ContainedItems.Remove(item);
        }

        public override void DoCollision(IEntity entity)
        {
            if (entity is Peach peach)
            {
                AABB peachBounds = peach.BoundingBox();
                AABB blockBounds = BoundingBox();
                if (blockBounds.Bottom - peachBounds.Top <= 10 && peach.ActionState is JumpingState)
                {
                    if (Bump && peach.PowerUpState is StandardState)
                    {
                        BumpBrickBlockTransition();
                        BumpedBy = peach;
                        if (ContainedItems.Count !=Constants.ZERO)
                        {
                            IEntity child = ContainedItems[0];
                            if (!(child is ExplodingBlock))
                            {
                                child.Checkable = true;
                                child.Visible = true;
                            }
                            if (child is SuperMushroom)
                            {
                                sound.Play();
                                if (peachBounds.Center.X > blockBounds.Center.X)
                                {
                                    child.Sprite.Velocity = new Vector2(100,Constants.ZERO);
                                }
                                else
                                {
                                    child.Sprite.Velocity = new Vector2(-100,Constants.ZERO);
                                }
                            }
                            else if (child is OneUpMushroom)
                            {
                                sound.Play();
                                if (peachBounds.Center.X > blockBounds.Center.X)
                                {
                                    child.Sprite.Velocity = new Vector2(-100,Constants.ZERO);
                                }
                                else
                                {
                                    child.Sprite.Velocity = new Vector2(100,Constants.ZERO);
                                }
                            }
                            else if (child is Star)
                            {
                                sound.Play();
                                if (peach.Sprite.HReflect)
                                {
                                    child.Sprite.Velocity = new Vector2(-200, -300);
                                }
                                else
                                {
                                    child.Sprite.Velocity = new Vector2(200, -300);
                                }
                            }
                            RemoveContainedItem(child);
                        }
                    }
                    else if (Bump)
                    {
                        int count = ContainedItems.Count;
                        for (int i =Constants.ZERO; i < count; i++)
                        {
                            if (ContainedItems[i] is ExplodingBlock)
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
}

