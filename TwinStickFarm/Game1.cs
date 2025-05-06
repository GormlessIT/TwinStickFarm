using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace TwinStickFarm
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Camera2D camera;
        private Player player;
        // World size in pixels
        private int worldWidth = 2000;
        private int worldHeight = 2000;
        // Tiles for checkered background
        private int tileSize = 50;
        private Texture2D pixelTexture;
        // Prevents rapid cycling through zoom levels
        private bool qPressed, ePressed = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            camera = new Camera2D();
        }

        protected override void Initialize() // Called once per game, initializes game state (loads settings, sets up objects etc.)
        {
            // Set viewport for camera so it knows how to offset the drawing
            camera.Viewport = new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            // Define world bounds for camera
            camera.WorldBounds = new Rectangle(0, 0, worldWidth, worldHeight);

            base.Initialize();
        }

        protected override void LoadContent() // Called once per game, loads all content (textures, sounds, etc.)
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            var ballTexture = Content.Load<Texture2D>("ball");
            player = new Player(
                ballTexture,
                new Vector2(worldWidth / 2, worldHeight / 2),
                200f,
                worldWidth,
                worldHeight
                );

            camera.Position = player.Position;

            pixelTexture = new Texture2D(GraphicsDevice, 1, 1);
            pixelTexture.SetData(new Color[] { Color.White });
        }

        // Update/Draw is the main game loop
        protected override void Update(GameTime gameTime) // Called multiple times per second, updates game state (checks collisions, gets input, plays audio etc.)
        {
            // Time since last update in seconds
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            KeyboardState kstate = Keyboard.GetState();

            // Player movement + clamping to world bounds
            player.Update(gameTime);

            // Camera zoom in/out 
            if (kstate.IsKeyDown(Keys.Q) && !qPressed)
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

                qPressed = true;
            }
            else if (kstate.IsKeyDown(Keys.E) && !ePressed)
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

                ePressed = true;
            }

            // If Q or E is not pressed, reset the flag
            if (kstate.IsKeyUp(Keys.Q)) qPressed = false;
            if (kstate.IsKeyUp(Keys.E)) ePressed = false;

            // Update camera with new position
            camera.Update(player.Position, deltaTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) // Called multiple times per second, draws game visuals on screen
        {
            // Background
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(transformMatrix: camera.Transform);

            // Draw the checkered background first
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
            base.Draw(gameTime); // Draw the game visuals
        }
    }
}