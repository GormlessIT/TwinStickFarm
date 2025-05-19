using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TwinStickFarm
{
    public static class SpriteBatchExtensions
    {
        // Draws given text so that its center is at 'position'
        public static void DrawCenteredString(
            this SpriteBatch sb,
            SpriteFont font,
            string text,
            Vector2 position,   // This will be center of text
            Color color, 
            float rotation = 0f,
            Vector2? scale = null,
            SpriteEffects effects = SpriteEffects.None,
            float layerDepth = 0f
            )
        {
            // Measure string in pixels
            Vector2 size = font.MeasureString(text);
            // Center of text
            Vector2 origin = size * 0.5f;

            sb.DrawString(
                font,
                text,
                position,
                color: color,
                rotation: rotation,
                origin: origin,
                scale: scale ?? Vector2.One,
                effects: effects,
                layerDepth: layerDepth
            );
        }
    }
}