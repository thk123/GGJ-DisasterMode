﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GGJ_DisasterMode.Codebase.Screens;

namespace GGJ_DisasterMode.Codebase.Gameplay
{
    public partial class GameplayManager
    {
        private Vector2 cursorPosition;
        private GameMode gameMode;

        private SpriteFont gameFont;
        private ContentManager content;

        private Texture2D cursor;
        private Texture2D backgroundTexture;
        private Texture2D uiTexture;

        private bool missionRunning;

        public GameplayManager(ContentManager content)
        {
            this.gameMode = GameMode.REALTIME;
            this.missionRunning = true;
        }

        private enum GameMode
        {
            REALTIME, DECISION
        }

        public void Update(GameTime gameTime, out bool missionRunning)
        {
            if (this.gameMode == GameMode.REALTIME)
            {
                UpdateReal(gameTime, out missionRunning);
            }
            else if (this.gameMode == GameMode.DECISION)
            {
                UpdateDecision(gameTime, out missionRunning);
            }
            
            missionRunning = this.missionRunning;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (this.gameMode == GameMode.REALTIME)
            {
                DrawReal(gameTime, spriteBatch);
            }
            else if (this.gameMode == GameMode.DECISION)
            {
                DrawDecision(gameTime, spriteBatch);
            }
        }

        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public void HandleInput(GamePadState gamePadState)
        {
            if (gamePadState == null)
                throw new ArgumentNullException("input");

            if (gamePadState.Buttons.Y == ButtonState.Pressed)
                this.missionRunning = false;

            if (this.gameMode == GameMode.DECISION)
            {
                HandleInputDecision(gamePadState);
            }
            
        }
    }
}
