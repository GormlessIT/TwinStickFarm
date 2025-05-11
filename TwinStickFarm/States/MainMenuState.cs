using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TwinStickFarm.UI;

namespace TwinStickFarm.States
{
    public class MainMenuState : IGameState
    {
        private SpriteBatch spriteBatch;
        private SpriteFont font;
        private Game1 game;
        private readonly Texture2D pixelTexture;

        private Button playButton, quitButton;

        private MouseState previousMState;
        private bool inQuitConfirm;
        private Button yesButton, noButton;

        public MainMenuState(Game1 game, SpriteBatch spriteBatch, SpriteFont font, Texture2D pixelTexture)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
            this.font = font;
            this.pixelTexture = pixelTexture;

            // Lay out buttons
            playButton = new Button(spriteBatch, font, pixelTexture, "Play", new Rectangle(300, 200, 200, 50));
            quitButton = new Button(spriteBatch, font, pixelTexture, "Quit", new Rectangle(300, 300, 200, 50));

            // Confirmation buttons
            yesButton = new Button(spriteBatch, font, pixelTexture, "Yes", new Rectangle(300, 200, 200, 50));
            noButton = new Button(spriteBatch, font, pixelTexture, "No", new Rectangle(300, 300, 200, 50));
        }

        public void Enter()
        {
            previousMState = Mouse.GetState();
            inQuitConfirm = false;
        }
        public void Exit()
        {
            // Clean up resources if needed
        }

        public void Update(GameTime gameTime)
        {
            var mstate = Mouse.GetState();

            if (inQuitConfirm)
            {
                if (yesButton.IsClicked(mstate, previousMState))
                {
                    game.Exit();
                }
                else if (noButton.IsClicked(mstate, previousMState))
                {
                    inQuitConfirm = false;
                }
            }
            else
            {
                if (playButton.IsClicked(mstate, previousMState))
                {
                    game.ChangeState(GameState.Playing);
                    return;
                }
                else if (quitButton.IsClicked(mstate, previousMState))
                {
                    inQuitConfirm = true;
                }
            }

            previousMState = mstate;
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            if (inQuitConfirm)
            {

                // Dim Background
                var viewport = spriteBatch.GraphicsDevice.Viewport;
                spriteBatch.Draw(pixelTexture, new Rectangle(0, 0, viewport.Width, viewport.Height), Color.Black * 0.5f);

                spriteBatch.DrawString(font, "Quit the game?", new Vector2(300, 150), Color.White);
                yesButton.Draw(Color.DarkRed, Color.White);
                noButton.Draw(Color.DarkGreen, Color.White);
            }
            else
            {
                spriteBatch.DrawString(font, "Dillweed Ranch", new Vector2(280, 120), Color.White);
                playButton.Draw(Color.DarkSlateGray, Color.White);
                quitButton.Draw(Color.DarkRed, Color.White);
            }
                spriteBatch.End();
        }
    }
}