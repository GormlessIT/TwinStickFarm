using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace TwinStickFarm.States
{
    public class  PlayingState : IGameState
    {
        private readonly Game1 game;
        private readonly SpriteBatch spriteBatch;
        private readonly SpriteFont font;
        private Player player;
        private Camera2D camera;
        private Texture2D pixelTexture;
        private readonly int worldWidth, worldHeight, tileSize;
        private KeyboardState previousKState;

        public PlayingState(
            Game1 game,
            SpriteBatch spriteBatch,
            SpriteFont font,
            Player player,
            Camera2D camera,
            Texture2D pixelTexture,
            int worldWidth, 
            int worldHeight,
            int tileSize
            )
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
            this.font = font;
            this.player = player;
            this.camera = camera;
            this.pixelTexture = pixelTexture;
            this.worldWidth = worldWidth;
            this.worldHeight = worldHeight;
            this.tileSize = tileSize;
            previousKState = Keyboard.GetState();
        }

        public void Enter()
        {
            // (Re)position camera on player
            camera.Position = player.Position;

            previousKState = Keyboard.GetState();
        }

        public void Exit()
        {
            // Clean up resources if needed
        }

        public void Update(GameTime gameTime)
        {
            // Time since last update in seconds
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            KeyboardState kstate = Keyboard.GetState();

            // Player movement + clamping to world bounds
            player.Update(gameTime);

            // Camera zoom in/out 
            if (kstate.IsKeyDown(Keys.Q) && previousKState.IsKeyUp(Keys.Q))
            {
                // Zoom out: Move to next lower zoom level, if not at FAR
                if (camera.CurrentZoomLevel != "FAR")
                {
                    // Get the next lower zoom level
                    var zoomLevels = camera.ZoomLevels.Keys.ToList();
                    int currentIndex = zoomLevels.IndexOf(camera.CurrentZoomLevel);
                    if (currentIndex > 0)
                    {
                        camera.CurrentZoomLevel = zoomLevels[currentIndex - 1];
                    }
                }
            }
            else if (kstate.IsKeyDown(Keys.E) && previousKState.IsKeyUp(Keys.E))
            {
                // Zoom in: Move to next higher zoom level, if not at CLOSE
                if (camera.CurrentZoomLevel != "CLOSE")
                {
                    // Get the next higher zoom level
                    var zoomLevels = camera.ZoomLevels.Keys.ToList();
                    int currentIndex = zoomLevels.IndexOf(camera.CurrentZoomLevel);
                    if (currentIndex < zoomLevels.Count - 1)
                    {
                        camera.CurrentZoomLevel = zoomLevels[currentIndex + 1];
                    }
                }
            }

            // Update camera with new position
            camera.Update(player.Position, deltaTime);

            // Pause game if ESC is pressed
            if (kstate.IsKeyDown(Keys.Escape) && previousKState.IsKeyUp(Keys.Escape))
            {
                game.ChangeState(GameState.Paused);
                return;
            }

            // Update previous key press
            previousKState = kstate;
        }

        public void Draw(GameTime gameTime)
        {
            var fullMatrix = game.ScaleMatrix * camera.Transform;
            spriteBatch.Begin(
                transformMatrix: fullMatrix,
                samplerState: SamplerState.PointClamp
                );

            // Draw the checkered background 
            for (int x = 0; x < worldWidth; x += tileSize)
            {
                for (int y = 0; y < worldHeight; y += tileSize)
                {
                    // Alternate colors based on the tile's position
                    // This creates the checkered effect: if the sum of the grid indices is even, use one color; otherwise, another.
                    Color tileColor = ((x / tileSize + y / tileSize) % 2 == 0) ? Color.PaleTurquoise : Color.PaleVioletRed;

                    // Draw the tile as a rectangle
                    spriteBatch.Draw(pixelTexture,
                        new Rectangle(x, y, tileSize, tileSize),
                        tileColor);
                }
            }

            player.Draw(spriteBatch); // Draw player character

            spriteBatch.End();
        }
    }
}