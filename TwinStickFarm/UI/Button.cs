using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TwinStickFarm;

namespace TwinStickFarm.UI
{
    public class Button
    {
        public Rectangle Bounds;
        public string Text { get; private set; }

        private SpriteFont font;
        private SpriteBatch spriteBatch;
        private Texture2D backgroundTexture;

        public Button(SpriteBatch spriteBatch, SpriteFont font, Texture2D backgroundTexture, string text, Rectangle bounds)
        {
            this.spriteBatch = spriteBatch;
            this.font = font;
            this.backgroundTexture = backgroundTexture;
            this.Text = text;
            this.Bounds = bounds;
        }

        public void Draw(Color background, Color foreground)
        {
            // Draw background
            spriteBatch.Draw(
                texture: backgroundTexture,
                destinationRectangle: Bounds,
                color: background
            );

            // Draw centered text
            spriteBatch.DrawCenteredString(
                font: font,
                text: Text,
                Bounds.Center.ToVector2(),
                foreground
                );
        }

        public bool IsClicked(MouseState mstate, MouseState previousMState)
        {
            return Bounds.Contains(mstate.Position)
                && mstate.LeftButton == ButtonState.Pressed
                && previousMState.LeftButton == ButtonState.Released;
        }
    }
}