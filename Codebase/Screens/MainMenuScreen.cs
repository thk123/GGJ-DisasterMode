#region File Description
//-----------------------------------------------------------------------------
// MainMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
#endregion

namespace GGJ_DisasterMode.Codebase.Screens
{
    /// <summary>
    /// The main menu screen is the first thing displayed when the game starts up.
    /// </summary>
    class MainMenuScreen : MenuScreen
    {
        #region Initialization

        private Song backgroundMusic;
        private ContentManager content;

        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public MainMenuScreen()
            : base("Main Menu")
        {
            // Create our menu entries.
            /*MenuEntry playGameMenuEntry = new MenuEntry("Launch Game");
            MenuEntry instructionsEntry = new MenuEntry("Instructions");*/
            //MenuEntry optionsMenuEntry = new MenuEntry("Options");
            //MenuEntry exitMenuEntry = new MenuEntry("Quit");

            // Hook up menu event handlers.
            /*playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            instructionsEntry.Selected += new EventHandler<PlayerIndexEventArgs>(instructionsEntry_Selected);*/
            /*optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;*/

            // Add entries to the menu.
            //MenuEntries.Add(playGameMenuEntry);
            //MenuEntries.Add(instructionsEntry);
            /*MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(exitMenuEntry);       */
        }

        void instructionsEntry_Selected(object sender, PlayerIndexEventArgs e)
        {
            BriefingGameplayScreen instructionScreen = new BriefingGameplayScreen();
            ScreenManager.AddScreen(instructionScreen, ControllingPlayer);
        }
        Texture2D background;
        public override void LoadContent()
        {
            base.LoadContent();

            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");


            background = content.Load<Texture2D>("Graphics//StartAndEndScreens//mainMenu");
            //this.backgroundMusic = content.Load<Song>("Audio//Songs//menu");
            /*MediaPlayer.Play(backgroundMusic);
            MediaPlayer.IsRepeating = true;*/
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0,0, 1024, 576), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, false,
                               new GameplayScreen());
        }


        /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>
        void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(), e.PlayerIndex);
        }


        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            const string message = "Are you sure you want to exit this sample?";

            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
        }

        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to exit" message box.
        /// </summary>
        void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }

        public override void UnloadContent()
        {
            //MediaPlayer.Stop();
            base.UnloadContent();
        }

        public override void HandleInput(InputState input)
        {
            base.HandleInput(input);
            if (input.GetMouseJustDown())
            {
                LoadingScreen.Load(ScreenManager, true, PlayerIndex.One, false,
                               new GameplayScreen());
            }
        }


        #endregion
    }
}
