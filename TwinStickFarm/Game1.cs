using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace TwinStickFarm;

public class Game1 : Game
{
    private Texture2D ballTexture;
    private Vector2 ballPosition;
    private float ballSpeed;

    private Camera2D camera;

    // World size in pixels
    private int worldWidth = 2000;
    private int worldHeight = 2000;

    // Tiles for checkered background
    private int tileSize = 50;
    private Texture2D pixelTexture;

    // Prevents rapid cycling through zoom levels
    private bool qPressed = false;
    private bool ePressed = false;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        camera = new Camera2D();
    }

    protected override void Initialize() // Called once per game, initializes game state (loads settings, sets up objects etc.)
    {
        try
        {
            Console.WriteLine("Initializing game...");
            // TODO: Add your initialization logic here

            // Sets ball start pos to half of world's width and height (center of game world)
            ballPosition = new Vector2(worldWidth / 2, worldHeight / 2);
            ballSpeed = 200f;

            // Set viewport for camera so it knows how to offset the drawing
            camera.Viewport = new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            // Define world bounds for camera so it knows how far it can move
            camera.WorldBounds = new Rectangle(0, 0, worldWidth, worldHeight);
            camera.Position = ballPosition;

            base.Initialize();

            Console.WriteLine("Initialization complete.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Initialization error: {ex.Message}");
        }
    }

    protected override void LoadContent() // Called once per game, loads all content (textures, sounds, etc.)
    {
        try
        {
            Console.WriteLine("Loading content...");

            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            ballTexture = Content.Load<Texture2D>("ball");
            pixelTexture = new Texture2D(GraphicsDevice, 1, 1);
            pixelTexture.SetData(new Color[] { Color.White });

            Console.WriteLine("Content loaded successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Content loading error: {ex.Message}");
        }
    }

    // Update/Draw is the main game loop
    protected override void Update(GameTime gameTime) // Called once per frame, updates game state (input, physics, etc.)
    {
        try
        {
            // Time since last update in seconds (ensures movement is consistent regardless of framerate)
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            // Get current state of keyboard
            var kstate = Keyboard.GetState();
            // Create vector to hold the input direction
            Vector2 inputDirection = Vector2.Zero;

            // WASD movement
            if (kstate.IsKeyDown(Keys.W)) inputDirection.Y -= 1;
            if (kstate.IsKeyDown(Keys.S)) inputDirection.Y += 1;
            if (kstate.IsKeyDown(Keys.A)) inputDirection.X -= 1;
            if (kstate.IsKeyDown(Keys.D)) inputDirection.X += 1;

            // Normalize input direction so diagonal movement isn't faster
            if (inputDirection != Vector2.Zero)
            {
                inputDirection.Normalize();
            }

            // Udapte ball position based on input direction, speed and deltaTime
            ballPosition += inputDirection * ballSpeed * deltaTime;

            // Prevent ball from moving outside of world bounds
            if (ballPosition.X > worldWidth - ballTexture.Width / 2)
            {
                ballPosition.X = worldWidth - ballTexture.Width / 2;
            }
            else if (ballPosition.X < ballTexture.Width / 2)
            {
                ballPosition.X = ballTexture.Width / 2;
            }

            if (ballPosition.Y > worldHeight - ballTexture.Height / 2)
            {
                ballPosition.Y = worldHeight - ballTexture.Height / 2;
            }
            else if (ballPosition.Y < ballTexture.Height / 2)
            {
                ballPosition.Y = ballTexture.Height / 2;
            }

            // Camera zoom in/out
            if (kstate.IsKeyDown(Keys.Q) && !qPressed)
            {
                // Zoom out: Move to next lower zoom level
                if (camera.CurrentZoomLevel != "FAR")
                {
                    // Get next lower zoom level
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
                // Zoom in: Move to next higher zoom level
                if (camera.CurrentZoomLevel != "CLOSE")
                {
                    // Get next higher zoom level
                    var zoomLevels = camera.ZoomLevels.Keys.ToList();
                    int currentIndex = zoomLevels.IndexOf(camera.CurrentZoomLevel);
                    if (currentIndex < zoomLevels.Count - 1)
                    {
                        camera.CurrentZoomLevel = zoomLevels[currentIndex + 1];
                    }
                }

                ePressed = true;
            }

            // If Q or E are released, reset the pressed state
            if (kstate.IsKeyUp(Keys.Q))
            {
                qPressed = false;
            }
            if (kstate.IsKeyUp(Keys.E))
            {
                ePressed = false;
            }

            // Update camera with new ball position and deltaTime
            camera.Update(ballPosition, deltaTime);

            base.Update(gameTime);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Update error: {ex.Message}");
        }
    }

    protected override void Draw(GameTime gameTime) // Called once per frame, draws game visuals (sprites, textures, etc.)
    {
        try
        {
            // Background colour
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(transformMatrix: camera.Transform);

            // Draw checkered background
            for (int x = 0; x < worldWidth; x += tileSize)
            {
                for (int y = 0; y < worldHeight; y += tileSize)
                {
                    // Alternate colours based on tile position
                    // This craetes the checkerboard pattern: If the sum of the grid indices is even, the tile is one colour, otherwise it's the other colour
                    Color tileColor = ((x / tileSize + y / tileSize) % 2 == 0) ? Color.PaleTurquoise : Color.PaleVioletRed;
                    // Draw tile as a rectangle
                    _spriteBatch.Draw(pixelTexture, new Rectangle(x, y, tileSize, tileColor);
                }
            }

            // Draw ball
            _spriteBatch.Draw(
                ballTexture,
                ballPosition,
                null,
                Color.White,
                0f,
                new Vector2(ballTexture.Width / 2, ballTexture.Height / 2), // Sets origin of drawing point of sprite to its center (by default sprites are drawn from top-left corner)
                Vector2.One,
                SpriteEffects.None,
                0f
            );

            _spriteBatch.End();

            base.Draw(gameTime);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Draw error: {ex.Message}");
        }
    }
}