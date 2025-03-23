using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TwinStickFarm;

public class Game1 : Game
{
    Texture2D ballTexture;
    Vector2 ballPosition;
    float ballSpeed;
    
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize() // Called once per game, initializes game state (loads settings, sets up objects etc.)
    {
        // TODO: Add your initialization logic here
        ballPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2, 
                                   _graphics.PreferredBackBufferHeight / 2); // Sets ball start pos to half of screen's width and height (i.e. the center)
        ballSpeed = 100f;

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
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        // Time since last update
        float updatedBallSpeed = ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

        var kstate = Keyboard.GetState();
        
        // WASD movement
        if (kstate.IsKeyDown(Keys.W))
        {
            ballPosition.Y -= updatedBallSpeed;
        }

        if (kstate.IsKeyDown(Keys.S))
        {
            ballPosition.Y += updatedBallSpeed;
        }

        if (kstate.IsKeyDown(Keys.A))
        {
            ballPosition.X -= updatedBallSpeed;
        }

        if (kstate.IsKeyDown(Keys.D))
        {
            ballPosition.X += updatedBallSpeed;
        }

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

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) // Called multiple times per second, draws game visuals on screen
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        _spriteBatch.Begin();

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