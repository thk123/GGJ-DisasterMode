using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using GGJ_DisasterMode.Codebase.Screens;

namespace GGJ_DisasterMode.Codebase
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private ScreenManager screenManager;

        #region Init

        public Game()
        {
            Content.RootDirectory = "Content";

            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 576;
            base.IsMouseVisible = true;

            // Create the screen manager component.
            screenManager = new ScreenManager(this);

            Components.Add(screenManager);

            

            // Activate the first screens.

            screenManager.AddScreen(new GameplayScreen(), PlayerIndex.One);
            //screenManager.AddScreen(new BackgroundScreen(), null);
            //screenManager.AddScreen(new MainMenuScreen(), null);

            //
            Components.Add(new FrameRateCounter(this));
        }

        #endregion

        #region Draw


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);

            // The real drawing happens inside the screen manager component.
            base.Draw(gameTime);
        }


        #endregion
    }
}
