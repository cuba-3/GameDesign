using FirstMonoGame.Collision;
using FirstMonoGame.Commands;
using FirstMonoGame.Entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FirstMonoGame
{
    class EndingCutScene
    {
        private Peach Peach;
        private Vector2 GoalLocation;
            
        public EndingCutScene(GameMain game)
        {
            Peach = game.peach;
            Peach.Location = new Vector2(110, Peach.Location.Y);
            GoalLocation = new Vector2(300, 5005);
            game.FreezeControls = true;
        }

        public bool Update()
        {
            if(Math.Abs(Peach.Location.X - GoalLocation.X) < 5)
            {
                Peach.IdleTransition();
                foreach (IEntity entity in Collections.Instance.GetLevelRef().Entities) // explode all remaining brick blocks on ceiling
                {
                    if(entity is GlassBlock glassBlock)
                    {
                        int count = glassBlock.ContainedItems.Count;
                        glassBlock.ExplodeBrickBlockTransition();
                        for (int i =Constants.ZERO; i < count; i++)
                        {
                            ExplodingBlock child = (ExplodingBlock)glassBlock.ContainedItems[i]; // must do following for all four exploding blocks
                            child.Visible = true;
                            if (i == 1) child.Sprite.Velocity = new Vector2(-300, -300); // make the blocks go in different directions
                            else if (i == 2) child.Sprite.Velocity = new Vector2(300, 300);
                            else if (i == 3) child.Sprite.Velocity = new Vector2(300, -300);
                            else if (i == 4) child.Sprite.Velocity = new Vector2(-300, 300);
                            child.ExplodingBlockTransition();
                        }
                    }
                }
                return true;
            }
            else
            {
                if(Peach.Location.X > GoalLocation.X)
                {
                    ICommand moveLeft = new MoveLeftCommand(Peach);
                    moveLeft.Execute();
                } else
                {
                    ICommand moveRight = new MoveRightCommand(Peach);
                    moveRight.Execute();
                }
            }
            return false;
        }
    }
}
