using System.Collections.Generic;
using System.Threading.Tasks;
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

        private readonly Button playButton, quitButton, yesButton, noButton;

        private List<Button> menuButtons;
        private List<Button> confirmButtons;
        private int selectedIndex;
        private bool inQuitConfirm;
        private KeyboardState previousKState;

        public MainMenuState(Game1 game, SpriteBatch spriteBatch, SpriteFont font, Texture2D pixelTexture)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
            this.font = font;
            this.pixelTexture = pixelTexture;

            const int btnWidth = 200, btnHeight = 50;
            // Center X in virtual coordinate space
            int centerX = (Config.VirtualWidth - btnWidth) / 2;

            // Lay out buttons
            playButton = new Button(spriteBatch, font, pixelTexture, "Play", new Rectangle(centerX, 200, btnWidth, btnHeight));
            quitButton = new Button(spriteBatch, font, pixelTexture, "Quit", new Rectangle(centerX, 300, btnWidth, btnHeight));

            // Confirmation buttons
            yesButton = new Button(spriteBatch, font, pixelTexture, "Yes", new Rectangle(centerX, 200, btnWidth, btnHeight));
            noButton = new Button(spriteBatch, font, pixelTexture, "No", new Rectangle(centerX, 300, btnWidth, btnHeight));

            menuButtons = new List<Button> { playButton, quitButton };
            confirmButtons = new List<Button> { yesButton, noButton };
            selectedIndex = 0;  // Start on "Play"
            inQuitConfirm = false;
        }

        public void Enter()
        {
            previousKState = Keyboard.GetState();
            inQuitConfirm = false;
            selectedIndex = 0;
        }
        public void Exit()
        {
            // Clean up resources if needed
        }

        public void Update(GameTime gameTime)
        {
            var kstate = Keyboard.GetState();

            // Up Arrow: Move selection up
            if (kstate.IsKeyDown(Keys.Up) && previousKState.IsKeyUp(Keys.Up))
            {
                selectedIndex =
                    (selectedIndex - 1 + (inQuitConfirm ? confirmButtons.Count : menuButtons.Count))
                    % (inQuitConfirm ? confirmButtons.Count : menuButtons.Count);
            }
            // Down Arrow: Move selection down
            else if (kstate.IsKeyDown(Keys.Down) && previousKState.IsKeyUp(Keys.Down))
            {
                selectedIndex = (selectedIndex + 1) % (inQuitConfirm ? confirmButtons.Count : menuButtons.Count);
            }

            // Enter: Use whichever button is selected
            if (kstate.IsKeyDown(Keys.Enter) && previousKState.IsKeyUp(Keys.Enter))
            {
                if (!inQuitConfirm)
                {
                    // Main menu
                    switch (selectedIndex)
                    {
                        case 0: // Play
                            game.ChangeState(GameState.Playing);
                            break;
                        case 1: // Quit
                            inQuitConfirm = true;
                            selectedIndex = 0;  // default to “Yes”
                            break;
                    }
                }
                else
                {
                    // Confirmation
                    switch (selectedIndex)
                    {
                        case 0: // Yes
                            game.Exit();
                            break;
                        case 1: // No
                            inQuitConfirm = false;
                            selectedIndex = 1; // back to “Quit” in the main menu
                            break;
                    }
                }
            }

            previousKState = kstate;
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(
                transformMatrix: game.ScaleMatrix,
                samplerState: SamplerState.PointClamp
                );

            if (inQuitConfirm)
            {
                // Dim virtual screen
                spriteBatch.Draw(pixelTexture, new Rectangle(0, 0, Config.VirtualWidth, Config.VirtualHeight), Color.Black * 0.5f);

                // Prompt
                spriteBatch.DrawCenteredString(font, "Are you sure you want to quit?", new Vector2(Config.VirtualWidth/2, 150), Color.White);

                // Draw Yes/No with higlight
                for (int i = 0; i < confirmButtons.Count; i++)
                {
                    var foreground = (i == selectedIndex) ? Color.Yellow : Color.White;
                    confirmButtons[i].Draw(Color.DarkSlateGray, foreground);
                }
            }
            else
            {
                // Title
                spriteBatch.DrawCenteredString(font, "Dillweed Ranch", new Vector2(Config.VirtualWidth/2, 100), Color.White);

                // Draw Play/Quit with highlight
                for (int i = 0; i < menuButtons.Count; i++)
                {
                    var foreground = (i == selectedIndex) ? Color.Yellow : Color.White;
                    menuButtons[i].Draw(Color.DarkSlateGray, foreground);
                }
            }
                spriteBatch.End();
        }
    }
}