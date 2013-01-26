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

        
        

        private const int actionCount = 2;
        private Actions.Action[] actions;

        Draggable currentlyDragging;

        enum DragState
        {
            Idle,
            Dragging,
        }
        DragState currentState;

        private void ConstructReal()
        {
            int actionCount =Enum.GetNames(typeof(Actions.ActionType)).Length;
            actions = new Actions.Action[actionCount];

            int i = 0;
            foreach (Actions.ActionType type in Enum.GetValues(typeof(Actions.ActionType)))
            {
                actions[i] = new Actions.Action(type, new Rectangle(585 + 60 + (215 * i), 155, 150, 150));
                ++i;
            }

            currentState = DragState.Idle;
            currentlyDragging = null;
        }

        private void LoadContentReal(ContentManager content)
        {
            Texture2D pixelTexture = content.Load<Texture2D>("graphics//pixel");
            this.grid = new GameGrid(pixelTexture, 9, 9, 32, 31, 18, 18, 1, Color.Black);
            
            foreach (Actions.Action action in actions)
            {
                action.LoadContent(content);
            }
            this.civilians = new List<Civilian>();          
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

            
        }

        public void UpdateReal(GameTime gameTime, out bool missionRunning)
        {
            foreach (Actions.Action action in actions)
            {
                action.Update(gameTime);
            }



            missionRunning = this.missionRunning;
        }
        
        private void DrawReal(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            grid.Draw(spriteBatch);

            foreach (Civilian civilian in civilians)
            {
                civilian.Draw(spriteBatch, this.gridTransformMatrix);
            }
            
            foreach (Actions.Action action in actions)
            {
                action.Draw(spriteBatch);
            }

            spriteBatch.End();

        }

        private void HandleInputReal(InputState input)
        {
            if (currentState == DragState.Idle)
            {
                if (input.GetMouseDown())
                {
                    //Check if mouse is within any of the dropoff boxes
                    foreach (Draggable draggable in actions)
                    {
                        if (draggable.AttemptBeginDrag(input.GetMousePosition()))
                        {
                            currentState = DragState.Dragging;
                            currentlyDragging = draggable;
                            break;
                        }
                    }
                }
            }
            else if (currentState == DragState.Dragging)
            {
                if (input.GetMouseDown())
                {
                    //Move the one we are dragging
                    Point? gridPoint = grid.GetGridPointFromMousePosition(input.GetMousePosition());
                    Rectangle? lockedRectangle = null;
                    if (gridPoint.HasValue)
                    {
                        lockedRectangle = grid.GetGridRectangleFromGridPoint(gridPoint.Value);
                    }
                    currentlyDragging.UpdateDrag(input.GetMouseDelta(), lockedRectangle);
                }
                else
                {
                    Point? gridPoint = grid.GetGridPointFromMousePosition(input.GetMousePosition());
                    if (gridPoint.HasValue)
                    {
                        //Drop the dropoff in this grid
                        currentlyDragging.EndDrag(grid.GetGridRectangleFromGridPoint(gridPoint.Value));
                        currentState = DragState.Idle;
                    }
                    else
                    {
                        //Return the object to the shop
                        currentState = DragState.Idle;
                        currentlyDragging.EndDrag();
                    }
                }
            }
        }

    }
}
