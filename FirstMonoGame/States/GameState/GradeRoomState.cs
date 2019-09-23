using FirstMonoGame.Collision;
using FirstMonoGame.Entity;
using FirstMonoGame.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace FirstMonoGame.States.GameState
{
    class GradeRoomState : IGameState
    {
        private GameMain Game;
        Random randomLocation;
        Random randomVelocity;
        private Vector2 TopLeft;
        private Vector2 Top2ndLeft;
        private Vector2 TopCenter;
        public SpriteFont TextFont;
        public SpriteFont TextFontBig;
        public SpriteFont TextFontResults;
        public GradeRoomState(GameMain game)
        {
            Game = game;
            randomLocation = new Random();
            randomVelocity = new Random();
            TextFont = Game.Content.Load<SpriteFont>("Fonts/File");
            TextFontBig = Game.Content.Load<SpriteFont>("Fonts/Header");
            TextFontResults = Game.Content.Load<SpriteFont>("Fonts/MiniGame");
            TopLeft = new Vector2(10, 1920);
            TopCenter = new Vector2(460, 1920);
            Top2ndLeft = new Vector2(910, 1920);
        }

        public void GameOverTransition()
        {
            var state = new GameOverState(Game);
            Game.currentGameState = state;
        }
        
        public void InitializeTransition()
        {
            var state = new InitialState(Game);
            Game.currentGameState = state;
        }
        public void BeginningTransition()
        {
            throw new NotImplementedException();
        }

        public void WinnerTransition()
        {
            var state = new WinnerState(Game);
            Game.currentGameState = state;
        }

        public void CoinRoomTransition()
        {
            throw new NotImplementedException();
        }

        public void GradeRoomTransition()
        {
            var state = new GradeRoomState(Game);
            Game.currentGameState = state;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int TimeLeft = Game.peach.PlayerStats.TimeRemainingGradeRoom;
            if (Game.level.DelayDraw >Constants.ZERO && Game.level.DelayDraw < 1355)
            {
                Game.gamePaused = true;
                spriteBatch.DrawString(TextFont, "You have 30 seconds to collect the grades to earn you a ", TopCenter + new Vector2(Constants.ZERO, 30), Color.White);
                spriteBatch.DrawString(TextFont, "3.2 GPA. A 3.2 will grant you acceptance into the CSE ", TopCenter + new Vector2(Constants.ZERO, 70), Color.White);
                spriteBatch.DrawString(TextFont, "program, and unlimited fireballs in the main level.", TopCenter + new Vector2(Constants.ZERO, 110), Color.White);           
                if (Game.level.DelayDraw > 1020 && Game.level.DelayDraw < 1110)
                {
                    spriteBatch.DrawString(TextFontBig, "STARTING IN...", TopCenter + new Vector2(-40, 190), Color.White);
                }
                else if (Game.level.DelayDraw > 1110 && Game.level.DelayDraw < 1190)
                {
                    spriteBatch.DrawString(TextFontBig, "3", TopCenter + new Vector2(430, 190), Color.White);
                }
                else if (Game.level.DelayDraw > 1190 && Game.level.DelayDraw < 1270)
                {
                    spriteBatch.DrawString(TextFontBig, "2", TopCenter + new Vector2(430, 190), Color.White);
                }
                else if (Game.level.DelayDraw > 1270 && Game.level.DelayDraw < 1350)
                {
                    spriteBatch.DrawString(TextFontBig, "1", TopCenter + new Vector2(430, 190), Color.White);
                }
            } else if (TimeLeft ==Constants.ZERO)
            {
                PlayerStats stats = Game.peach.PlayerStats;
                string gpa = stats.CalculateGPA();
                if (Double.Parse(gpa) >= 3.2)
                {
                    spriteBatch.DrawString(TextFontResults, "YASSSSS queen, you got a", TopCenter + new Vector2(-20, 190), Color.DeepPink);
                    spriteBatch.DrawString(TextFontResults, gpa + "!!! Fireballs granted.", TopCenter + new Vector2(-20, 290), Color.DeepPink);                  
                }
                else
                {
                    spriteBatch.DrawString(TextFontResults, "Yikes...you didn't get a", TopCenter + new Vector2(-20, 190), Color.DeepPink);
                    spriteBatch.DrawString(TextFontResults, "3.2. No fireballs for you.", TopCenter + new Vector2(-20, 290), Color.DeepPink);                  
                }
            }
            else
            {
                Game.gamePaused = false;
                PlayerStats stats = Game.peach.PlayerStats;
                spriteBatch.DrawString(TextFont, "PLAYER", TopLeft, Color.White);
                spriteBatch.DrawString(TextFont, stats.PlayerName, TopLeft + new Vector2(Constants.ZERO, 25), Color.Lerp(Color.White, Color.Transparent, .05f));
                spriteBatch.DrawString(TextFont, "GPA", Top2ndLeft, Color.Lerp(Color.White, Color.Transparent, .05f));
                spriteBatch.DrawString(TextFont, stats.CalculateGPA(), Top2ndLeft + new Vector2(Constants.ZERO, 25), Color.Lerp(Color.White, Color.Transparent, .05f));
                spriteBatch.DrawString(TextFont, "TIME", TopCenter, Color.Lerp(Color.White, Color.Transparent, .05f));
                spriteBatch.DrawString(TextFont, stats.TimeRemainingGradeRoom.ToString(), TopCenter + new Vector2(Constants.ZERO, 25), Color.Lerp(Color.White, Color.Transparent, .05f));
            }
        }

        public void Update(GameTime gameTime)
        {
            int TimeLeft = Game.peach.PlayerStats.TimeRemainingGradeRoom;
                if (Game.level.DelayUpdate > 180 && TimeLeft >Constants.ZERO)
                {
                    AGrade aGrade = new AGrade(Game.Content, Game.GraphicsDevice)
                    {
                        Location = new Vector2(randomLocation.Next(Constants.ZERO, 3000), 1900),
                        Visible = true,
                        Checkable = true
                    };

                    BGrade bGrade = new BGrade(Game.Content, Game.GraphicsDevice)
                    {
                        Location = new Vector2(randomLocation.Next(Constants.ZERO, 3000), 1900),
                        Visible = true,
                        Checkable = true
                    };

                    CGrade cGrade = new CGrade(Game.Content, Game.GraphicsDevice)
                    {
                        Location = new Vector2(randomLocation.Next(Constants.ZERO, 3000), 1900),
                        Visible = true,
                        Checkable = true
                    };

                    DGrade dGrade = new DGrade(Game.Content, Game.GraphicsDevice)
                    {
                        Location = new Vector2(randomLocation.Next(Constants.ZERO, 3000), 1900),
                        Visible = true,
                        Checkable = true
                    };

                    FGrade fGrade = new FGrade(Game.Content, Game.GraphicsDevice)
                    {
                        Location = new Vector2(randomLocation.Next(Constants.ZERO, 3000), 1900),
                        Visible = true,
                        Checkable = true
                    };
                    aGrade.Sprite.Velocity = new Vector2(Constants.ZERO, randomVelocity.Next(100, 500));
                    bGrade.Sprite.Velocity = new Vector2(Constants.ZERO, randomVelocity.Next(100, 500));
                    cGrade.Sprite.Velocity = new Vector2(Constants.ZERO, randomVelocity.Next(100, 500));
                    dGrade.Sprite.Velocity = new Vector2(Constants.ZERO, randomVelocity.Next(100, 500));
                    fGrade.Sprite.Velocity = new Vector2(Constants.ZERO, randomVelocity.Next(100, 500));

                    Game.level.Entities.Add(aGrade);
                    Game.level.Entities.Add(cGrade);
                    Game.level.Entities.Add(dGrade);
                    Game.level.Entities.Add(fGrade);
                    Game.level.Entities.Add(bGrade);

                    Collections.Instance.GetCollisionRef().ReRegister(aGrade);
                    Collections.Instance.GetCollisionRef().ReRegister(bGrade);
                    Collections.Instance.GetCollisionRef().ReRegister(cGrade);
                    Collections.Instance.GetCollisionRef().ReRegister(dGrade);
                    Collections.Instance.GetCollisionRef().ReRegister(fGrade);
                    Game.level.DelayUpdate =Constants.ZERO;
                }
            }

        public void GameTransitionStateTransistion()
        {
            throw new NotImplementedException();
        }
    }
}
