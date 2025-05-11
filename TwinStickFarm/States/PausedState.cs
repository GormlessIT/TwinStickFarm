using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TwinStickFarm.States;

namespace TwinStickFarm.States
{
    public class PausedState : IGameState
    {
        private readonly Game1 game;
        private readonly SpriteBatch spriteBatch;
        private readonly SpriteFont font;
        private bool escReleased;
        private KeyboardState previousKState;

        public PausedState(Game1 game, SpriteBatch spriteBatch, SpriteFont font)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
            this.font = font;
        }

        public void Enter() 
        {
            previousKState = Keyboard.GetState();
            escReleased = false;
        }
        public void Exit()
        {
            
        }
        public void Update(GameTime gameTime) 
        { 
            var kstate = Keyboard.GetState();

           if (!escReleased)
            {
                if(kstate.IsKeyUp(Keys.Escape) && previousKState.IsKeyDown(Keys.Escape))
                {
                    escReleased = true;
                }
            }
           else
            {
                if (kstate.IsKeyDown(Keys.Escape) && previousKState.IsKeyUp(Keys.Escape))
                {
                    game.ChangeState(GameState.Playing);
                    return;
                }
            }

                previousKState = kstate;
        }
        public void Draw(GameTime gameTime) 
        { 
            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Paused - esc to Resume", new Vector2(100, 100), Color.White);
            spriteBatch.End();
        }
    }
}