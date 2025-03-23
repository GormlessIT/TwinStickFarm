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
        // TODO: Add your initialization logic here

        // Sets ball start pos to half of world's width and height (center of game world)
        ballPosition = new Vector2(worldWidth / 2, worldHeight / 2); 
        ballSpeed = 200f;   // pixels per second

        // Set viewport for camera so it knows how to offset the drawing
        camera.Viewport = new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

        // Define world bounds for camera
        camera.WorldBounds = new Rectangle(0, 0, worldWidth, worldHeight);

        camera.Position = ballPosition;

        base.Initialize();
    }

    protected override void LoadContent() // Called once per game, loads all content (textures, sounds, etc.)
    {
        // Create a new SpriteBatch, which can be used to draw textures.
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        ballTexture = Content.Load<Texture2D>("ball");
        pixelTexture = new Texture2D(GraphicsDevice, 1, 1);
        pixelTexture.SetData(new Color[] { Color.White });
    }

    // Update/Draw is the main game loop
    protected override void Update(GameTime gameTime) // Called multiple times per second, updates game state (checks collisions, gets input, plays audio etc.)
    {
        // TODO: Add your update logic here

        // Time since last update in seconds (ensures movement is consistent regardless of framerate)
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Get keyboard state
        var kstate = Keyboard.GetState();

        // Create vector to hold the input direction
        Vector2 inputDirection = Vector2.Zero;

        // WASD movement
        if (kstate.IsKeyDown(Keys.W)) inputDirection.Y -= 1;
        if (kstate.IsKeyDown(Keys.S)) inputDirection.Y += 1;
        if (kstate.IsKeyDown(Keys.A)) inputDirection.X -= 1;
        if (kstate.IsKeyDown(Keys.D)) inputDirection.X += 1;
       
        // Normalise input direction so that moving diagonally doesn't make the player move faster
        if (inputDirection != Vector2.Zero)
        {
            inputDirection.Normalize();
        }

        // Update ball position based on normalised input direction, speed and deltaTime
        ballPosition += inputDirection * ballSpeed * deltaTime;

        // Prevent ball from going off the edge of the world (x-axis)
        if (ballPosition.X > worldWidth - ballTexture.Width / 2)
        {
            ballPosition.X = worldWidth - ballTexture.Width / 2;
        }
        else if (ballPosition.X < ballTexture.Width / 2)
        {
            ballPosition.X = ballTexture.Width / 2;
        }

        // Prevent ball from going off the edge of the world (y-axis)
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
        if (kstate.IsKeyUp(Keys.Q))
        {
            qPressed = false;
        }
        if (kstate.IsKeyUp(Keys.E))
        {
            ePressed = false;
        }

        // Update camera with new position
        camera.Update(ballPosition, deltaTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) // Called multiple times per second, draws game visuals on screen
    {
        // Background
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _spriteBatch.Begin(transformMatrix: camera.Transform);

        // Draw the checkered background first
        for (int x = 0; x < worldWidth; x += tileSize)
        {
            for (int y = 0; y < worldHeight; y += tileSize)
            {
                // Alternate colors based on the tile's position
                // This creates the checkered effect: if the sum of the grid indices is even, use one color; otherwise, another.
                Color tileColor = ((x / tileSize + y / tileSize) % 2 == 0) ? Color.PaleTurquoise : Color.PaleVioletRed;

                // Draw the tile as a rectangle
                _spriteBatch.Draw(pixelTexture,
                    new Rectangle(x, y, tileSize, tileSize),
                    tileColor);
            }
        }

        // Draw the ball
        _spriteBatch.Draw(
            ballTexture, 
            ballPosition, 
            null, 
            Color.White,
            0f,
            new Vector2(ballTexture.Width / 2, ballTexture.Height /2),  // Sets origin of drawing point of sprite to its center (by default sprites are drawn from top-left corner)
            Vector2.One,
            SpriteEffects.None,
            0f
        );

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}