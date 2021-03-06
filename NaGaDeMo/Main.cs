﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Configuration;
using System.Collections.Specialized;

namespace NaGaDeMo
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Main : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            string screen_mode = ConfigurationManager.AppSettings.Get("screen_mode");

            if (screen_mode == "fullscreen")
            {
                graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;  // set this value to the desired width of your window
                graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;   // set this value to the desired height of your window

                UI.GameView.X = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2 - UI.GameView.Width / 2;
                UI.GameView.Y = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2 - UI.GameView.Height / 2;

             //   graphics.IsFullScreen = true;
            }
            else
            {

            }

            graphics.PreferMultiSampling = true;

            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Engine.Initialize(this);

            UI.Initialize();

            //Templates.Interfaces.DefaultInterface.Init();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            UI.Pixel = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            UI.Pixel.SetData(new[] {
                Color.White
            }); // so that we can draw whatever color we want on top of it

            Engine.LoadContent(Content);
            UI.LoadContent(Content);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            UI.Update(gameTime);

            foreach (Character character in Engine.Characters)
            {
                character.Update(gameTime);
            }

            foreach (Tile tile in Engine.CurrentBattle.GameMap.Tiles)
            {
                tile.Update(gameTime);
            }

            UI.Overlay.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Engine.BufferMap(GraphicsDevice, spriteBatch);

            Engine.BufferCollisionMap(GraphicsDevice, spriteBatch);

            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();


            //Draw the UI
            UI.Draw(spriteBatch);

            // Render the game
            Engine.Draw(spriteBatch);

            //Render any overlays
            UI.Overlay.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
