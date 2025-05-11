using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TwinStickFarm.States;

namespace TwinStickFarm.States
{
    public class GameOverState : IGameState
    {
        private readonly Game1 game;
        private readonly SpriteBatch spriteBatch;
        private readonly SpriteFont font;

        public GameOverState(Game1 game, SpriteBatch spriteBatch, SpriteFont font)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
            this.font = font;
        }

        public void Enter()
        {

        }
        public void Exit()
        {

        }
        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                game.ChangeState(GameState.MainMenu);
            }
        }
        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Game Over - esc for Main Menu", new Vector2(100, 100), Color.White);
            spriteBatch.End();
        }
    }
}
