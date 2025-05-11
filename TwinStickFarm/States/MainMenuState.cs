using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TwinStickFarm.States
{
    public class MainMenuState : IGameState
    {
        private SpriteBatch spriteBatch;
        private SpriteFont font;
        private Game1 game;

        public MainMenuState(Game1 game, SpriteBatch spriteBatch, SpriteFont font)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
            this.font = font;
        }

        public void Enter()
        {
            // Initialize resources needed for the main menu
        }
        public void Exit()
        {
            // Clean up resources if needed
        }

        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                game.ChangeState(GameState.Playing);
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Dillweed Ranch", new Vector2(100, 100), Color.White);
            spriteBatch.DrawString(font, "Press Enter to Start", new Vector2(100, 200), Color.White);
            spriteBatch.End();
        }
    }
}