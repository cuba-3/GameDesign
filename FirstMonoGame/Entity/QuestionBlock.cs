using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstMonoGame.Entity;
using FirstMonoGame.Interfaces;
using FirstMonoGame.Sprites;
using FirstMonoGame.States.BlockStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using FirstMonoGame.Collision;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework.Audio;
using FirstMonoGame.States.PeachStates;

namespace FirstMonoGame
{
    class QuestionBlock : Block
    {
        public IBlockState<QuestionBlock> BlockState { get; set; }
        public ContentManager Content { get; set; }
        public SoundEffect sound;
        public Collection<IEntity> ContainedItems { get; set; }
        public Peach BumpedBy { get; set; }
        public QuestionBlock(ContentManager content, GraphicsDevice graphicsDevice) 
        {
            Content = content;
            BlockState = new NewQuestionBlockState(graphicsDevice); // init state
            Sprite = QuestionBlockSpriteFactory.Instance.FactoryMethod(content, this, graphicsDevice, false);
            ContainedItems = new Collection<IEntity>();
            sound = content.Load<SoundEffect>("Sound Effects/Item Appears");
        }

        public override void Update(GameTime gameTime)
        {
            Sprite.Update(gameTime);
            if(Location.Y <= OriginalLocation.Y - 30 && BlockState is BumpQuestionBlockState)
            {
                Sprite.Velocity *= new Vector2(1, -1);
                if (Sprite.CurrentLocation.Y < OriginalLocation.Y - 30) Sprite.CurrentLocation = new Vector2(Sprite.CurrentLocation.X, OriginalLocation.Y - 30);
            }
            else if(Location.Y >= OriginalLocation.Y && BlockState is BumpQuestionBlockState)
            {
                Sprite.Velocity *= new Vector2(1,Constants.ZERO);
                if (Sprite.CurrentLocation.Y > OriginalLocation.Y) Sprite.CurrentLocation = new Vector2(Sprite.CurrentLocation.X, OriginalLocation.Y);
                BlockState.NewTransition(this);
            }
        }

        public void BumpQuestionTransition()
        {
            BlockState.BumpTransition(this);
        }

        public void AddContainedItem(IEntity item)
        {
            ContainedItems.Add(item);
        }

        public void RemoveContaindItem(IEntity item)
        {
            ContainedItems.Remove(item);
        }

        public override void DoCollision(IEntity entity)
        {
            if (entity is Peach peach)
            {
                AABB peachBounds = peach.BoundingBox();
                AABB blockBounds = BoundingBox();
                if (blockBounds.Bottom - peachBounds.Top <= 10 && BlockState is NewQuestionBlockState && peach.ActionState is JumpingState)
                {
                    BumpQuestionTransition();
                    BumpedBy = peach;
                    if (ContainedItems.Count !=Constants.ZERO)
                    {
                        IEntity child = ContainedItems[0];
                        child.Visible = true;
                        child.Checkable = true;
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
                                child.Sprite.Velocity = new Vector2(200, -300);
                            }
                            else
                            {
                                child.Sprite.Velocity = new Vector2(-200, -300);
                            }
                        }
                        RemoveContaindItem(child);
                    }
                }
            }
        }
    }
}
