using FirstMonoGame.Entity;
using FirstMonoGame.Interfaces;
using FirstMonoGame.Sprites;
using FirstMonoGame.States.BlockStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstMonoGame.Collision;
using FirstMonoGame.States.PeachStates;
using System.Collections.ObjectModel;
using FirstMonoGame.States2.PowerUpStates;
using Microsoft.Xna.Framework.Audio;

namespace FirstMonoGame.Entity
{
    class HiddenBlock : Block
    {
        public IBlockState<HiddenBlock> BlockState { get; set; }
        public ContentManager Content { get; set; }
        public GraphicsDevice GraphicsDevice;
        public Collection<IEntity> ContainedItems { get; set; }
        public bool Bump;
        public SoundEffect sound;
        private bool isConsumed { get; set; }
        private bool Bumping { get; set; }
        public Peach BumpedBy { get; set; }

        public HiddenBlock(ContentManager content, GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
            Content = content;
            ContainedItems = new Collection<IEntity>();
            BlockState = new HiddenBlockState(graphicsDevice);
            Sprite = new BlockSprite(graphicsDevice);
            Bump = true;
            Bumping = false;
            Sprite = HiddenBlockSpriteFactory.Instance.FactoryMethod(this.Content, this, GraphicsDevice, false);
            sound = content.Load<SoundEffect>("Sound Effects/Item Appears");
            isConsumed = false;
            //Sprite.Toggle(); // Turns initial display value to false
        }
        public override void Update(GameTime gameTime)
        {
            Sprite.Update(gameTime);
            if (Location.Y <= OriginalLocation.Y - 30)
            {
                Sprite.Velocity *= new Vector2(1, -1);
                if (Sprite.CurrentLocation.Y < OriginalLocation.Y - 30) Sprite.CurrentLocation = new Vector2(Sprite.CurrentLocation.X, OriginalLocation.Y - 30);
            }
            else if (Location.Y >= OriginalLocation.Y)
            {
                Sprite.Velocity *= new Vector2(1,Constants.ZERO);
                if (Sprite.CurrentLocation.Y > OriginalLocation.Y) Sprite.CurrentLocation = new Vector2(Sprite.CurrentLocation.X, OriginalLocation.Y);
                Bumping = false;
            }
            if (isConsumed)
            {
                Collections.Instance.GetCollisionRef().Deregister(this);
                Collections.Instance.GetLevelRef().Entities.Remove(this);
            }
        }

        public void BumpHiddenBlockTransition()
        {
            if (Bumping) return;
            BlockState.NewTransition(this);
            Sprite.Display = true;
            BlockState.BumpTransition(this);
            Bumping = true;
        }
        public void AddContainedItem(IEntity item)
        {
            ContainedItems.Add(item);
        }

        public void RemoveContainedItem(IEntity item)
        {
            ContainedItems.Remove(item);
        }
        public void ExplodeBrickBlockTransition()
        {
            // controls block going up
            isConsumed = true;
        }

        public override void DoCollision(IEntity entity)
        {
            if (entity is Peach peach)
            {
                AABB peachBounds = peach.BoundingBox();
                AABB blockBounds = BoundingBox();
                if (blockBounds.Bottom - peachBounds.Top <= 10 && peach.ActionState is JumpingState)
                {
                    BumpHiddenBlockTransition();
                    if (Bump && peach.PowerUpState is StandardState)
                    {
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

        public override bool CanCollide(IEntity other)
        {
            // either visible, or peach is colliding with bottom of invisible block [intersecting, condition for bottom, peach is jumping up]
            return Visible || (other is Peach p && BoundingBox().MinkowskiDifferenceContainsOrigin(p.BoundingBox()) && BoundingBox().Bottom - p.BoundingBox().Top <= 10 && p.ActionState is JumpingState);
        }
    }
}
