using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GGJ_DisasterMode.Codebase.Screens;
using GGJ_DisasterMode.Codebase.Dropoffs;

namespace GGJ_DisasterMode.Codebase.Gameplay
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    public partial class GameplayManager
    {
        private int decisionVariable;

        private List<Dropoff> dropoffs;

        private void LoadContentDecision(ContentManager content)
        {
            dropoffs = new List<Dropoff>();
            dropoffs.Add(new BasicFoodDropoff(GetNextStoreSlot()));
            dropoffs.Add(new BasicFoodDropoff(GetNextStoreSlot()));
            dropoffs.Add(new BasicFoodDropoff(GetNextStoreSlot()));

            foreach (Dropoff dropoff in dropoffs)
            {
                dropoff.LoadContent(content);
            }
        }

        private void UpdateDecision(GameTime gameTime, out bool missionRunning)
        {




            missionRunning = this.missionRunning;
        }
        
        private void DrawDecision(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Dropoffs.Dropoff dropoff in dropoffs)
            {
                dropoff.Draw(gameTime, spriteBatch);
            }
        }

        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        private void HandleInputDecision(InputState gamePadState)
        {

        }

        private List<Draggable> GetDecisionDraggble()
        {
            List<Draggable> draggables = new List<Draggable>();
            foreach (Draggable d in dropoffs)
            {
                draggables.Add(d);
            }
            return draggables;
        }

        int i = 0;
        int j = 0;
        private Rectangle GetNextStoreSlot()
        {
            Rectangle r = new Rectangle(uiOffset + 128 + ((34 + 16) * i), 383 + ((34 + 16)* j), 17, 17);
            i += 1 % 5;
            if (i == 0)
            {
                ++j;
            }

            return r;
        }

    }
}
