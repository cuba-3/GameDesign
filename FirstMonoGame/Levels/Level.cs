using FirstMonoGame.Collision;
using FirstMonoGame.Entity;
using FirstMonoGame.States.GameState;
using FirstMonoGame.WorldScrolling;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.ObjectModel;
using System.Linq;

namespace FirstMonoGame.Levels
{
    public class Level
    {
        private SpriteBatch SpriteBatch;
        public Collection<IEntity> Entities { get; set; }
        public bool Continue { get; set; }
        public int DelayDraw { get; set; }
        public int DelayUpdate { get; set; }
        public Level(Collection<IEntity> entities)
        {
            Entities = entities;
            Continue = true;
            DelayDraw =Constants.ZERO;
            DelayUpdate =Constants.ZERO;
        }

        public void Draw(SpriteBatch spriteBatch, GameMain game)
        {
            PlayerStats stats = game.peach.PlayerStats;
            SpriteBatch = new SpriteBatch(game.GraphicsDevice);
            SpriteBatch.Begin();
            
            foreach (IEntity entity in Entities)
            {
                AABB bounds = entity.BoundingBox();
                if (Camera.Instance.IsOnScreen(new Rectangle { X = (int)bounds.Left, Y = (int)bounds.Top, Width = (int)bounds.Width, Height = (int)bounds.Height }))
                {
                    entity.Draw(spriteBatch);
                }                
            }
            if (!(game.currentGameState is InitialState))
            {
                DelayDraw++;
            }
            if (!stats.GradeRoom && !stats.CoinRoom)
            {
                DelayDraw =Constants.ZERO;
                DelayUpdate =Constants.ZERO;
            }
            SpriteBatch.End();
        }

        public void Update(GameTime gameTime, GameMain game)
        {
            PlayerStats stats = game.peach.PlayerStats;
            if (game.currentGameState is GradeRoomState || game.currentGameState is GameTransitionState || game.currentGameState is CoinRoomState)
            {
                DelayUpdate++;
                game.currentGameState.Update(gameTime);
            }
            int length = Entities.Count;
            for (int i = length - 1; i > -1; i--)
            {
                if (Continue)
                {
                    AABB bounds = Entities.ElementAt(i).BoundingBox();
                    if (Camera.Instance.IsOnScreen(new Rectangle { X = (int)bounds.Left, Y = (int)bounds.Top, Width = (int)bounds.Width, Height = (int)bounds.Height }))
                    {
                        Entities.ElementAt(i).Update(gameTime);
                        if (stats.CoinMultiplier)
                        {
                            if (i > -1 && i < Entities.Count - 1 && Entities.ElementAt(i) is Coin)
                            {
                                Entities.ElementAt(i).Sprite.Texture = game.Content.Load<Texture2D>("Dollar Bill");
                                Entities.ElementAt(i).Sprite.ChangeTexture(Entities.ElementAt(i).Sprite.Texture, 1, 3);
                                Entities.ElementAt(i).Sprite.SwitchAnimation(Constants.ZERO, 3);
                            }
                        }
                    }
                    else if (Entities.ElementAt(i) is Peach p) p.Update(gameTime);
                    else if (Entities.ElementAt(i) is FireBar f) f.Update(gameTime);
                    else if (Entities.ElementAt(i) is FireBall fb) fb.Update(gameTime);              
                }
            }
        }
    }
}
