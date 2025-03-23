using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TwinStickFarm;

public class Game1 : Game
{
    private Texture2D ballTexture;
    private Vector2 ballPosition;
    private float ballSpeed;
    
    private Camera2D camera;

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
        ballPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2, 
                                   _graphics.PreferredBackBufferHeight / 2); // Sets ball start pos to half of screen's width and height (i.e. the center)
        ballSpeed = 200f;   // pixels per second

        // Set viewport for camera so it knows how to offset the drawing
        camera.Viewport = new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

        base.Initialize();
    }

    protected override void LoadContent() // Called once per game, loads all content (textures, sounds, etc.)
    {
        // Create a new SpriteBatch, which can be used to draw textures.
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        ballTexture = Content.Load<Texture2D>("ball");
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
        if (kstate.IsKeyDown(Keys.W))
        {
            inputDirection.Y -= 1;
        }

        if (kstate.IsKeyDown(Keys.S))
        {
            inputDirection.Y += 1;
        }

        if (kstate.IsKeyDown(Keys.A))
        {
            inputDirection.X -= 1;
        }

        if (kstate.IsKeyDown(Keys.D))
        {
            inputDirection.X += 1;
        }

        // Normalise input direction so that moving diagonally doesn't make the player move faster
        if (inputDirection != Vector2.Zero)
        {
            inputDirection.Normalize();
        }

        // Update ball position based on normalised input direction, speed and deltaTime
        ballPosition += inputDirection * ballSpeed * deltaTime;

        // Prevent ball from going off screen (x-axis)
        if (ballPosition.X > _graphics.PreferredBackBufferWidth - ballTexture.Width / 2)
        {
            ballPosition.X = _graphics.PreferredBackBufferWidth - ballTexture.Width / 2;
        }
        else if (ballPosition.X < ballTexture.Width / 2)
        {
            ballPosition.X = ballTexture.Width / 2;
        }

        // Prevent ball from going off screen (y-axis)
        if (ballPosition.Y > _graphics.PreferredBackBufferHeight - ballTexture.Height / 2)
        {
            ballPosition.Y = _graphics.PreferredBackBufferHeight - ballTexture.Height / 2;
        }
        else if (ballPosition.Y < ballTexture.Height / 2)
        {
            ballPosition.Y = ballTexture.Height / 2;
        }

        camera.Position = ballPosition;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) // Called multiple times per second, draws game visuals on screen
    {
        // Background
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _spriteBatch.Begin(transformMatrix: camera.Transform);

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