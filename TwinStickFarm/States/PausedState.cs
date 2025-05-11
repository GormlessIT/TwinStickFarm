using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TwinStickFarm.States;
using TwinStickFarm.UI;

namespace TwinStickFarm.States
{
    public class PausedState : IGameState
    {
        private readonly Game1 game;
        private readonly SpriteBatch spriteBatch;
        private readonly SpriteFont font;
        private readonly Texture2D pixelTexture;

        private bool escReleased;

        private KeyboardState previousKState;
        private MouseState previousMState;

        private Button resumeButton, mainMenuButton, quitButton;
        private bool inQuitConfirm = false;
        private enum ConfirmAction { None, QuitGame, MainMenu }
        private ConfirmAction confirmAction;
        private Button yesButton, noButton;

        public PausedState(Game1 game, SpriteBatch spriteBatch, SpriteFont font, Texture2D pixelTexture)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
            this.font = font;
            this.pixelTexture = pixelTexture;

            // Lay out buttons
            resumeButton = new Button(spriteBatch, font, pixelTexture, "Resume", new Rectangle(300, 150, 200, 50));
            mainMenuButton = new Button(spriteBatch, font, pixelTexture, "Main Menu", new Rectangle(300, 250, 200, 50));
            quitButton = new Button(spriteBatch, font, pixelTexture, "Exit", new Rectangle(300, 350, 200, 50));

            // Confirmation buttons
            yesButton = new Button(spriteBatch, font, pixelTexture, "Yes", new Rectangle(300, 150, 200, 50));
            noButton = new Button(spriteBatch, font, pixelTexture, "No", new Rectangle(300, 250, 200, 50));
        }

        public void Enter() 
        {
            previousKState = Keyboard.GetState();
            previousMState = Mouse.GetState();

            escReleased = false;

            inQuitConfirm = false;
            confirmAction = ConfirmAction.None;
        }
        public void Exit()
        {
            
        }
        public void Update(GameTime gameTime) 
        { 
            var kstate = Keyboard.GetState();
            var mstate = Mouse.GetState();

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

            if (!inQuitConfirm)
            {
                if (resumeButton.IsClicked(mstate, previousMState))
                {
                    game.ChangeState(GameState.Playing);
                    return;
                }
                else if (mainMenuButton.IsClicked(mstate, previousMState))
                {
                    inQuitConfirm = true;
                    confirmAction = ConfirmAction.MainMenu;
                    return;
                }
                else if (quitButton.IsClicked(mstate, previousMState))
                {
                    inQuitConfirm = true;
                    confirmAction = ConfirmAction.QuitGame;
                    return;
                }
            }
            else
            {
                if (yesButton.IsClicked(mstate, previousMState))
                {
                    if (confirmAction == ConfirmAction.QuitGame)
                        game.Exit();
                    else
                        game.ChangeState(GameState.MainMenu);

                    return;
                }
                else if (noButton.IsClicked(mstate, previousMState))
                {
                    inQuitConfirm = false;
                    confirmAction = ConfirmAction.None;
                    return;
                }
            }

            previousKState = kstate;
            previousMState = mstate;
        }
        public void Draw(GameTime gameTime) 
        { 
            spriteBatch.Begin();

            if (inQuitConfirm)
            {
                string prompt = confirmAction == ConfirmAction.MainMenu
                    ? "Return to Main Menu?"
                    : "Quit Game?";
                
                // Center screen
                var viewport = spriteBatch.GraphicsDevice.Viewport;
                int centerX = viewport.Width / 2, centerY = viewport.Height / 2;

                Vector2 textSize = font.MeasureString(prompt);

                // Prompt above center
                Vector2 promptPos = new Vector2(centerX - textSize.X / 2, centerY - 60);
                spriteBatch.DrawString(font, prompt, promptPos, Color.White);

                // Buttons below prompt
                yesButton.Bounds = new Rectangle(centerX - 110, (int)promptPos.Y + (int)textSize.Y + 10, 100, 40);
                noButton.Bounds = new Rectangle(centerX + 10, (int)promptPos.Y + (int)textSize.Y + 10, 100, 40);

                yesButton.Draw(Color.DarkRed, Color.White);
                noButton.Draw(Color.DarkGreen, Color.White);
            }
            else
            {
                resumeButton.Draw(Color.DarkSlateGray, Color.White);
                mainMenuButton.Draw(Color.DarkSlateGray, Color.White);
                quitButton.Draw(Color.DarkSlateGray, Color.White);
            }
            
            spriteBatch.End();
        }
    }
}