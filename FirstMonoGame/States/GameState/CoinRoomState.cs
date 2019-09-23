using FirstMonoGame.Collision;
using FirstMonoGame.Entity;
using FirstMonoGame.Interfaces;
using FirstMonoGame.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace FirstMonoGame.States.GameState
{
    class CoinRoomState : IGameState
    {
        private GameMain Game;
        Random randomLocation;
        Random randomVelocity;
        private Vector2 TopLeft;
        private Vector2 Top2ndLeft;
        private Vector2 Top3rdLeft;
        private Vector2 Top4thLeft;
        private Vector2 Top5thLeft;
        private Vector2 TopCenter;
        public SpriteFont TextFont;
        public SpriteFont TextFontBig;
        public SpriteFont TextFontResults;

        public CoinRoomState(GameMain game)
        {
            Game = game;
            randomLocation = new Random();
            randomVelocity = new Random();
            TextFont = Game.Content.Load<SpriteFont>("Fonts/File");
            TextFontBig = game.Content.Load<SpriteFont>("Fonts/Header");
            TextFontResults = Game.Content.Load<SpriteFont>("Fonts/MiniGame");
            TopLeft = new Vector2(10, 3919);
            TopCenter = new Vector2(460, 3919);
            Top2ndLeft = new Vector2(320, 3919);
            Top3rdLeft = new Vector2(628, 3919);
            Top4thLeft = new Vector2(938, 3919);
            Top5thLeft = new Vector2(1248, 3919);
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

        public void WinnerTransition()
        {
            var state = new WinnerState(Game);
            Game.currentGameState = state;
        }

        public void CoinRoomTransition()
        {
            var state = new CoinRoomState(Game);
            Game.currentGameState = state;
        }

        public void GradeRoomTransition()
        {
            throw new NotImplementedException();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            PlayerStats stats = Game.peach.PlayerStats;
            PlayerStats marioStats = Game.mario.PlayerStats;
            int TimeLeft = Game.peach.PlayerStats.TimeRemainingCoinRoom;
            if (Game.level.DelayDraw >Constants.ZERO && Game.level.DelayDraw < 500)
            {
                Game.gamePaused = true;
                spriteBatch.DrawString(TextFont, "You have 30 seconds to collect as many coins as possible.", TopCenter + new Vector2(Constants.ZERO, 30), Color.White);
                spriteBatch.DrawString(TextFont, "If you exceed the number of coins that Mario collects, ", TopCenter + new Vector2(Constants.ZERO, 70), Color.White);
                spriteBatch.DrawString(TextFont, "all existing coins in the main level will be replaced with", TopCenter + new Vector2(Constants.ZERO, 110), Color.White);
                spriteBatch.DrawString(TextFont, "$3 bills. PLUS, you will have evened out the wage gap!", TopCenter + new Vector2(Constants.ZERO, 150), Color.White);
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
            }
            else
            {
                Game.gamePaused = false;
                //line below needs to be uncommented but mario is null rn cuz he not on screen
                //PlayerStats statsMario = Game.mario.PlayerStats;
                spriteBatch.DrawString(TextFont, "PLAYER", TopLeft, Color.White);
                spriteBatch.DrawString(TextFont, stats.PlayerName, TopLeft + new Vector2(Constants.ZERO, 25), Color.Lerp(Color.White, Color.Transparent, .05f));
                spriteBatch.DrawString(TextFont, "Coins", Top2ndLeft, Color.Lerp(Color.White, Color.Transparent, .05f));
                spriteBatch.DrawString(TextFont, stats.CoinRoomCoin.ToString(), Top2ndLeft + new Vector2(Constants.ZERO, 25), Color.Lerp(Color.White, Color.Transparent, .05f));
                //change stats to statsMario below for player and coins. not time remaining
                spriteBatch.DrawString(TextFont, "PLAYER", Top3rdLeft, Color.White);
                spriteBatch.DrawString(TextFont, "Mario", Top3rdLeft + new Vector2(Constants.ZERO, 25), Color.Lerp(Color.White, Color.Transparent, .05f));
                spriteBatch.DrawString(TextFont, "Coins", Top4thLeft, Color.Lerp(Color.White, Color.Transparent, .05f));
                spriteBatch.DrawString(TextFont, marioStats.CoinRoomCoin.ToString(), Top4thLeft + new Vector2(Constants.ZERO, 25), Color.Lerp(Color.White, Color.Transparent, .05f));
                spriteBatch.DrawString(TextFont, "TIME", Top5thLeft, Color.Lerp(Color.White, Color.Transparent, .05f));
                spriteBatch.DrawString(TextFont, stats.TimeRemainingCoinRoom.ToString(), Top5thLeft + new Vector2(Constants.ZERO, 25), Color.Lerp(Color.White, Color.Transparent, .05f));
            }
            if (TimeLeft ==Constants.ZERO)
            {
                if (stats.CoinRoomCoin > marioStats.CoinRoomCoin)
                {
                    stats.CoinMultiplier = true;
                    spriteBatch.DrawString(TextFontResults, "YASSSSS queen, you collected ", TopCenter + new Vector2(-20, 190), Color.DeepPink);
                    spriteBatch.DrawString(TextFontResults, (stats.CoinRoomCoin - marioStats.CoinRoomCoin) + " more coins than Mario!!!", TopCenter + new Vector2(-20, 290), Color.DeepPink);
                    spriteBatch.DrawString(TextFontResults, "Coins exchanged for $5 bills.", TopCenter + new Vector2(-20, 390), Color.DeepPink);
                }
                else if (stats.CoinRoomCoin < marioStats.CoinRoomCoin)
                {
                    spriteBatch.DrawString(TextFontResults, "Yikes...Mario collected " + (marioStats.CoinRoomCoin - stats.CoinRoomCoin), TopCenter + new Vector2(-20, 190), Color.DeepPink);
                    spriteBatch.DrawString(TextFontResults, " more coins than you.", TopCenter + new Vector2(-20, 290), Color.DeepPink);
                }
                else
                {
                    stats.CoinMultiplier = true;
                    spriteBatch.DrawString(TextFontResults, "OoOoOoOoOoOoO! You and Mario tied." + (marioStats.CoinRoomCoin - stats.CoinRoomCoin), TopCenter + new Vector2(-20, 190), Color.DeepPink);
                    spriteBatch.DrawString(TextFontResults, "Coins exchanged for $5 bills still.", TopCenter + new Vector2(-20, 290), Color.DeepPink);
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            int TimeLeft = Game.peach.PlayerStats.TimeRemainingCoinRoom;
            if (Game.level.DelayUpdate > 70 && TimeLeft >Constants.ZERO)
            {
                Coin coinOne = new Coin(Game.Content, Game.GraphicsDevice)
                {
                    Location = new Vector2(1700, 3900),
                    Visible = true,
                    Checkable = true
                };
                Coin coinTwo = new Coin(Game.Content, Game.GraphicsDevice)
                {
                    Location = new Vector2(randomLocation.Next(Constants.ZERO, 3000), 3900),
                    Visible = true,
                    Checkable = true
                };
                Coin coinThree = new Coin(Game.Content, Game.GraphicsDevice)
                {
                    Location = new Vector2(randomLocation.Next(Constants.ZERO, 3000), 3900),
                    Visible = true,
                    Checkable = true
                };
                coinOne.Sprite.ScaleSprite(3f);
                coinOne.Sprite.Velocity = new Vector2(Constants.ZERO, randomVelocity.Next(300, 600));
                Game.level.Entities.Add(coinOne);
                Collections.Instance.GetCollisionRef().ReRegister(coinOne);

                coinTwo.Sprite.ScaleSprite(3f);
                coinTwo.Sprite.Velocity = new Vector2(Constants.ZERO, randomVelocity.Next(300, 600));
                Game.level.Entities.Add(coinTwo);
                Collections.Instance.GetCollisionRef().ReRegister(coinTwo);

                coinThree.Sprite.ScaleSprite(3f);
                coinThree.Sprite.Velocity = new Vector2(Constants.ZERO, randomVelocity.Next(300, 600));
                Game.level.Entities.Add(coinThree);
                Collections.Instance.GetCollisionRef().ReRegister(coinThree);
                Game.level.DelayUpdate =Constants.ZERO; 
            }
        }

        public void GameTransitionStateTransistion()
        {
            throw new NotImplementedException();
        }

        public void BeginningTransition()
        {
            throw new NotImplementedException();
        }
    }
}