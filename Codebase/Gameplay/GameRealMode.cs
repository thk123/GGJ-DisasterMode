﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GGJ_DisasterMode.Codebase.Screens;
using GGJ_DisasterMode.Codebase.Characters;
using GGJ_DisasterMode.Codebase.Actions;
using GGJ_DisasterMode.Codebase.Dropoffs;
using GGJ_DisasterMode.Codebase.Gameplay.Grid;
using GGJ_DisasterMode.Codebase.Gameplay.Grid.Resources;

namespace GGJ_DisasterMode.Codebase.Gameplay
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    public partial class GameplayManager
    {
        private int realVariable;
        private GameGrid grid;
        private Matrix gridTransformMatrix;

        private List<Civilian> civilians;

        SpriteFont defaultFont;

        Buckets buckets;        
        

        private const int actionCount = 2;
        private List<Actions.GameAction> actions;

        const int totalActionsPerDay = 12;
        int actionsReaminings;

        private void ConstructReal()
        {            
            int actionCount =Enum.GetNames(typeof(Actions.ActionType)).Length;
            actions = new List<Actions.GameAction>(actionCount);

            int i = 0;
            foreach (Actions.ActionType type in Enum.GetValues(typeof(Actions.ActionType)))
            {
                actions.Add(new Actions.GameAction(type, new Rectangle(uiOffset + 135 - 75 + (200 * i), 155, 150, 150)));
                ++i;
            }

            currentState = DragState.Idle;
            currentlyDragging = null;
        }

        public void StartDay()
        {
            actionsReaminings = totalActionsPerDay;
        }

        public void EndDay()
        {

        }

        private void LoadContentReal(ContentManager content)
        {
            Texture2D pixelTexture = content.Load<Texture2D>("graphics//pixel");
            this.grid = new GameGrid(pixelTexture, 9, 9, 32, 31, 18, 18, 1, Color.Black);
            
            foreach (Actions.GameAction action in actions)
            {
                action.LoadContent(content);
            }
            this.civilians = new List<Civilian>();          
            

            defaultFont = content.Load<SpriteFont>("Fonts//gamefont");

            this.buckets = new Buckets(this.grid, new List<GameAction>(), new List<Civilian>(), 
                new List<Dropoff>(), new List<Water>());

            PopulateCivilians(this.civilians, pixelTexture);
           
        }

        private void PopulateCivilians(List<Civilian> civilians, Texture2D texture)
        {
            Random randomGen = new Random(1000);

            for (int i = 0; i < 500; i++)
            {
                civilians.Add(new ChildCharacter(randomGen.Next(0, Civilian.SCALE_FACTOR), 
                    randomGen.Next(0, Civilian.SCALE_FACTOR), texture));
            }

            civilians.Add(new ChildCharacter(0, 0, texture));
            civilians.Add(new ChildCharacter(1800, 0, texture));
            civilians.Add(new ChildCharacter(0, 1800, texture));
            civilians.Add(new ChildCharacter(1800, 1800, texture));

            foreach (Civilian civilian in civilians)
            {
                buckets.addNewCivilian(civilian);
            }
        }

        public void UpdateReal(GameTime gameTime, out bool missionRunning)
        {
            
            foreach (Actions.GameAction action in actions)
            {
                action.Update(gameTime);
            }

            buckets.PopulateBuckets();

            foreach (Civilian civilian in civilians)
            {
                civilian.Update(gameTime);
            }


            missionRunning = this.missionRunning;
        }
        
        private void DrawReal(GameTime gameTime, SpriteBatch spriteBatch)
        {           

            foreach (Civilian civilian in civilians)
            {
                civilian.Draw(spriteBatch, this.gridTransformMatrix);
            }
            
            foreach (Actions.GameAction action in actions)
            {
                action.Draw(spriteBatch);
            }

            Vector2 halfTextLength = defaultFont.MeasureString(actionsReaminings.ToString()) * 0.5f;
            spriteBatch.DrawString(defaultFont, actionsReaminings.ToString(), new Vector2(uiOffset + 230 - halfTextLength.X, 200 - halfTextLength.Y), Color.Red);

        }

        private List<Draggable> GetRealDraggables()
        {
            List<Draggable> draggables = new List<Draggable>();
            foreach (Draggable d in actions)
            {
                draggables.Add(d);
            }
            return draggables;
        }

        private void HandleInputReal(InputState input)
        {
            
        }

    }
}
