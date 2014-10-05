using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FiascoRL.Display;
using FiascoRL.Display.Animation;
using FiascoRL.Display.UI;
using FiascoRL.Display.UI.Controls;
using FiascoRL.Entities;
using FiascoRL.Etc;
using FiascoRL.Etc.WeightedRandom;
using FiascoRL.Input;
using FiascoRL.World;
using FiascoRL.Etc.Targeting;
using FiascoRL.Entities.Util;
using FiascoRL.Display.UI.Controls.Coordinates;
using FiascoRL.Etc.GameState;

namespace FiascoRL
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class FiascoGame : Microsoft.Xna.Framework.Game
    {
        public GraphicsDeviceManager graphics;
        public Level firstLevel;
        private Texture2D cursorTex;
        private Vector2 cursorPos;

        public FiascoGame()
        {
            this.graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 700;
            graphics.PreferredBackBufferWidth = 1200;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Window.SetPosition(new Point(200, 50));
            
            // Initialize random generator.
            RandomGenerator.Rand = new Random();

            // Load textures into active content.
            SpriteGraphic.Initialize(this);
            FiascoRL.Display.UI.UIGraphic.Initialize(this);

            // Load first level.
            firstLevel = new Cave(50, 50);
            firstLevel.Depth = 1;
            firstLevel.GenerateLevel();

            // TODO: Remove this.
            Session.Player = new Player(SpriteGraphic.Creatures, 0)
            {
                CurrentLevel = firstLevel,
                HP = new Stat(20, 20),
                SP = new Stat(10, 10),
                Coords = firstLevel.GetRandomOpenTile(),
            };
            firstLevel.ActorList.Add(Session.Player);
            firstLevel.AddRandomAccessibleStaircase(Session.Player.Coords, Staircase.StairType.Down);

            Session.Graphics = GraphicsDevice;
            
            Session.MessageLog = new MessageLog(graphics);
            Session.Game = this;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            Session.SpriteBatch = new SpriteBatch(GraphicsDevice);

            cursorTex = Content.Load<Texture2D>("UI/mouse_pointer");

            // Load UI texture.
            Control.UITexture = Content.Load<Texture2D>("UI/ui");
            Control.GraphicsDeviceManager = this.graphics;

            // Set up UI.
            var minimapControl = new MinimapControl() {Enabled = true};
            var healthBarControl = new PowerBarControl(PowerBarControl.BarColor.Red, Session.Player.HP, "HP") { Enabled = true };
            var powerBarControl = new PowerBarControl(PowerBarControl.BarColor.Blue, Session.Player.SP, "SP")
            {
                Enabled = true,
                Coords = new RelativeRectangle(new RelativeVector(0.04f, 0.0f),
                new RelativeVector(0.04f, 26.0f), 0.12f, 0.01f),
            };

            var inventoryControl = new InventoryControl();
            var characterSelectControl = new CharacterSelectControl() { Enabled = true };
            Session.UIManager = new UIManager();
            Session.UIManager.Controls.Add(healthBarControl);
            Session.UIManager.Controls.Add(minimapControl);
            Session.UIManager.Controls.Add(inventoryControl);
            Session.UIManager.Controls.Add(powerBarControl);
            Session.UIManager.Controls.Add(characterSelectControl);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            Session.GameStateManager.Update(gameTime);

            // Update mouse position.
            MouseState mouse = Mouse.GetState();
            cursorPos = new Vector2(mouse.X, mouse.Y);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Session.GameStateManager.Draw(gameTime);

            // Draw mouse.
            Session.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null);
            Session.SpriteBatch.Draw(cursorTex, new Rectangle((int)cursorPos.X, (int)cursorPos.Y, cursorTex.Width * 2, cursorTex.Height * 2), Color.White);
            Session.SpriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
