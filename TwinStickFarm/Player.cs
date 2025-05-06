using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TwinStickFarm
{
    public class  Player
    {
        // Instance variables
        private Texture2D texture;
        private float speed;
        private int worldWidth, worldHeight;

        public Vector2 Position { get; private set; }

        // Constructor
        public Player(Texture2D texture, Vector2 startPosition, float speed, int worldWidth, int worldHeight)
        {
            this.texture = texture;
            this.speed = speed;
            this.worldWidth = worldWidth;
            this.worldHeight = worldHeight;
            Position = startPosition;
        }

        public void Update(GameTime gameTime)
        {
            // Get current keyboard state
            KeyboardState kstate = Keyboard.GetState();

            Vector2 inputDirection = Vector2.Zero;

            if (kstate.IsKeyDown(Keys.W)) inputDirection.Y -= 1;
            if (kstate.IsKeyDown(Keys.S)) inputDirection.Y += 1;
            if (kstate.IsKeyDown(Keys.A)) inputDirection.X -= 1;
            if (kstate.IsKeyDown(Keys.D)) inputDirection.X += 1;
            if (inputDirection != Vector2.Zero) inputDirection.Normalize(); // Normalize the input direction to ensure consistent speed

            // Update position based on input direction, speed, and elapsed time
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position += inputDirection * speed * deltaTime;

            // Keep player within world bounds
            float halfWidth = texture.Width / 2f;
            float halfHeight = texture.Height / 2f;

            Position = new Vector2(
                MathHelper.Clamp(Position.X, halfWidth, worldWidth - halfWidth),
                MathHelper.Clamp(Position.Y, halfHeight, worldHeight - halfHeight)
                );
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw player texture at its current position
            spriteBatch.Draw(
            texture,
            Position,
            null,
            Color.White,
            0f,
            new Vector2(texture.Width / 2f, texture.Height / 2f),  // Sets origin of drawing point of sprite to its center (by default sprites are drawn from top-left corner)
            Vector2.One,
            SpriteEffects.None,
            0f
            );
        }
    }
}