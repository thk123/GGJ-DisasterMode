#region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using GGJ_DisasterMode.Codebase.Gameplay;
#endregion

namespace GGJ_DisasterMode.Codebase.Screens
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class GameplayScreen : GameScreen
    {
        #region Fields

        private ContentManager content;
        private GameplayManager gameController;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            
        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            // A real game would probably have more content than this sample, so
            // it would take longer to load. We simulate that by delaying for a
            // while, giving you a chance to admire the beautiful loading screen.
            for (int i = 0; i < 10; i++)
            {
                LoadingScreen.SetLoadProgress(i * 10);
                Thread.Sleep(50);
            }
            
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            this.gameController = new GameplayManager(content);
            this.gameController.LoadContent(content);



            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            MediaPlayer.Stop();
            content.Unload();
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            bool missionRunning = true;

            if (IsActive)
            {
                gameController.Update(gameTime, out missionRunning);
            }

            if (!missionRunning)
            {
                LoadingScreen.Load(ScreenManager, true, PlayerIndex.One, false,
                    new BackgroundScreen(), new MainMenuScreen());
            }

        }

        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       input.GamePadWasConnected[playerIndex];

            if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {
                gameController.HandleInput(input);
            }
        }

        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                               Color.White, 0, 0);

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);
            byte fade = TransitionAlpha;

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            
            gameController.Draw(gameTime, spriteBatch);

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0)
                ScreenManager.FadeBackBufferToBlack(255 - TransitionAlpha);
        }


        #endregion
    }
}
