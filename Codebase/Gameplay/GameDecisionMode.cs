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

            Dropoff.InitTypes(content);
            foreach (KeyValuePair<DropoffType, DropoffProperties> entry in Dropoff.dropoffTypes)
            {
                dropoffs.Add(new Dropoff(entry.Value, GetStoreSlot(entry.Key)));
            }

            foreach (Dropoff dropoff in dropoffs)
            {
                dropoff.LoadContent(content);
            }
        }

        private void DropoffPlaced(Dropoffs.Dropoff dropoff, Rectangle dropoffRectangle, Point dropoffPoint)
        {
            dropoff.PlaceDropoff(dropoffRectangle, buckets, dropoffPoint);
            dropoffs.Add(Dropoffs.Dropoff.CreateNewDropoffFromDropoff(dropoff, GetStoreSlot(dropoff.DropoffType)));
        }

        private void UpdateDecision(GameTime gameTime, out bool missionRunning)
        {

            missionRunning = this.missionRunning;
        }
        
        private void DrawDecision(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Dropoffs.Dropoff dropoff in dropoffs)
            {
                dropoff.Draw(gameTime, spriteBatch, gameMode == GameMode.DECISION);
            }
        }

        private void DecisionProcessStartDay()
        {

        }

        private void DecisionProcessEndDay()
        {
            List<Dropoff> decayedDropoffs = new List<Dropoff>();
            foreach (Dropoffs.Dropoff dropoff in dropoffs)
            {
                dropoff.ProcessDay();
                if (dropoff.CurrentState == DropoffState.Decayed)
                {
                    decayedDropoffs.Add(dropoff);
                }
            }

            foreach (Dropoff decayedDropoff in decayedDropoffs)
            {
                dropoffs.Remove(decayedDropoff);
            }
        }

        private void DecisionProcessStartNight()
        {

        }

        private void DecisionProcessEndNight()
        {
            foreach (Dropoff dropoff in dropoffs)
            {
                //if we have placed these drops, they then have been ordered
                if (dropoff.CurrentState == DropoffState.Placed)
                {
                    dropoff.FixLocation();
                }
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

        private Rectangle GetStoreSlot(DropoffType dropoffType)
        {
            int i = -1;
            int j = -1;
            switch (dropoffType)
            {
                case DropoffType.Dropoff_Health_Low:
                    i = 3;
                    j = 0;
                    break;
                case DropoffType.Dropoff_Food_Low:
                    i = 1;
                    j = 0;
                    break;
                case DropoffType.Dropoff_Water_Low:
                    i = 2;
                    j = 0;
                    break;
                case DropoffType.Dropoff_Temperature_Low:
                    i = 0;
                    j = 0;
                    break;
                case DropoffType.Dropoff_Health_Medium:
                    i = 3;
                    j = 1;
                    break;
                case DropoffType.Dropoff_Food_Medium:
                    i = 1;
                    j = 1;
                    break;
                case DropoffType.Dropoff_Water_Medium:
                    i = 2;
                    j = 1;
                    break;
                case DropoffType.Dropoff_Temperature_Medium:
                    i = 0;
                    j = 1;
                    break;
                case DropoffType.Dropoff_Health_High:
                    i = 3;
                    j = 2;
                    break;
                case DropoffType.Dropoff_Food_High:
                    i = 1;
                    j = 2;
                    break;
                case DropoffType.Dropoff_Water_High:
                    i = 2;
                    j = 2;
                    break;
                case DropoffType.Dropoff_Temperature_High:
                    i = 0;
                    j = 2;
                    break;
                default:
                    break;
            }
            return new Rectangle(uiOffset + 130 + 2 +((34 + 25) * i), 393 + 2 + ((34 + 16)* j), 32, 31);
        }

    }
}
