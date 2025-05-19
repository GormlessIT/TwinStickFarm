using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using TwinStickFarm.States;

namespace TwinStickFarm
{
    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        GameOver
    }

    public class Game1 : Game
    {
        // Instance variables

        // 1. Core MonoGame objects
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        // 2. Game state management
        private Dictionary<GameState, IGameState> states;
        private IGameState currentState;

        // 3. Game objects
        private Camera2D camera;
        private Player player;
        private int worldWidth = 2000;  // Size in pixels
        private int worldHeight = 2000;
        private int tileSize = 50;
        private Texture2D pixelTexture;

        // 4. Resolution scaling
        private const int virtualWidth = 640;
        private const int virtualHeight = 360;
        public Matrix ScaleMatrix { get; private set; }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Set fullscreen mode and adapt to the current screen resolution
            var mode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
            graphics.PreferredBackBufferWidth = mode.Width;
            graphics.PreferredBackBufferHeight = mode.Height;
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();

            // Get real screen resolution (backbuffer size)
            var bb = GraphicsDevice.Viewport;
            float scaleX = bb.Width / (float)virtualWidth;
            float scaleY = bb.Height / (float)virtualHeight;
            float scale = Math.Min(scaleX, scaleY);

            // Letterbox offsets
            float viewW = virtualWidth * scale;
            float viewH = virtualHeight * scale;
            float offsetX = (bb.Width - viewW) / 2f;
            float offsetY = (bb.Height - viewH) / 2f;

            // Build matrix: First scale, then translate
            ScaleMatrix = Matrix.CreateScale(scale, scale, 1f) *
                          Matrix.CreateTranslation(offsetX, offsetY, 0f);

        }

        protected override void Initialize() // Called once per game, initializes game state (loads settings, sets up objects etc.)
        {
            // Camera Setup
            camera = new Camera2D();
            camera.Viewport = new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            camera.WorldBounds = new Rectangle(0, 0, worldWidth, worldHeight);

            base.Initialize();
        }

        protected override void LoadContent() // Called once per game, loads all content (textures, sounds, etc.)
        {
            // 1. Core MonoGame objects

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // 2. Shared assets for all states
            var font = Content.Load<SpriteFont>("DefaultFont");

            // 3. Create game objects
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
            pixelTexture.SetData(new[] { Color.White });

            // 4. Initialize game states
            states = new Dictionary<GameState, IGameState>
            {
                { GameState.MainMenu, new MainMenuState(this, spriteBatch, font, pixelTexture) },
                { GameState.Playing,
                    new PlayingState(
                        this,
                        spriteBatch,
                        font,
                        player,
                        camera,
                        pixelTexture,
                        worldWidth,
                        worldHeight,
                        tileSize
                    )
                },
                { GameState.Paused, new PausedState(this, spriteBatch, font, pixelTexture) },
                { GameState.GameOver, new GameOverState(this, spriteBatch, font) }
            };

            // 5. Set the initial state
            ChangeState(GameState.MainMenu);
        }

        /// <summary>
        /// Switches between states, calling Exit on the old state and Enter on the new one
        /// </summary>
        public void ChangeState(GameState newState)
        {
            currentState?.Exit();
            currentState = states[newState];
            currentState.Enter();
        }

        // Update/Draw is the main game loop
        protected override void Update(GameTime gameTime) // Called multiple times per second, updates game state (checks collisions, gets input, plays audio etc.)
        {
            currentState.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) // Called multiple times per second, draws game visuals on screen
        {
            // Clear the screen
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // Delegate all drawing to the current state
            currentState.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}