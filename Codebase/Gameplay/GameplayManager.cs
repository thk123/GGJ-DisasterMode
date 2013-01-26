using System;
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
        const int uiOffset = 585;

        private Vector2 cursorPosition;
        private GameMode gameMode;

        private SpriteFont gameFont;
        private ContentManager content;

        private Texture2D cursor;
        private Texture2D backgroundTexture;
        private Texture2D uiTexture;

        private bool missionRunning;

        private GraphicsDevice graphics;


        TimeSpan realTimeMaxDuration = new TimeSpan(0, 1, 10);
        TimeSpan decisionMaxDuration = new TimeSpan(0, 1, 30);

        TimeSpan remainingTime;

        Draggable currentlyDragging;

        enum DragState
        {
            Idle,
            Dragging,
        }
        DragState currentState;


        public GameplayManager(ContentManager content)
        {
            this.gameMode = GameMode.REALTIME;
            this.missionRunning = true;
            remainingTime = realTimeMaxDuration;

            ConstructReal();
        }
  
        public void LoadContent(ContentManager content)
        {
            

            LoadContentReal(content);
            LoadContentDecision(content);
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

            UpdateTime(gameTime);
            
            missionRunning = this.missionRunning;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            grid.Draw(spriteBatch);
            if (this.gameMode == GameMode.REALTIME)
            {
                DrawReal(gameTime, spriteBatch);
            }
            else if (this.gameMode == GameMode.DECISION)
            {
                DrawDecision(gameTime, spriteBatch);
            }
            spriteBatch.End();
        }

        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            /*if (gamePadState.Buttons.Y == ButtonState.Pressed)
                this.missionRunning = false;*/

            IEnumerable<Draggable> actions = null;
            if (this.gameMode == GameMode.REALTIME)
            {
                actions = GetRealDraggables();
            }
            else
            {
                actions = GetDecisionDraggble();
            }

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
                        currentlyDragging.EndDrag(grid.GetGridRectangleFromGridPoint(gridPoint.Value), null);
                        currentState = DragState.Idle;

                        if (gameMode == GameMode.REALTIME)
                        {
                            ActionPlaced((Actions.Action)currentlyDragging);
                        }
                        else
                        {
                            DropoffPlaced((Dropoffs.Dropoff)currentlyDragging);
                        }
                    }
                    else
                    {
                        //Return the object to the shop
                        currentState = DragState.Idle;
                        currentlyDragging.EndDrag(null, null);
                    }
                }
            }
            
        }

        private void UpdateTime(GameTime gameTime)
        {
            switch (gameMode)
            {
                case GameMode.REALTIME:
                    remainingTime = remainingTime - gameTime.ElapsedGameTime;
                    if (remainingTime.TotalSeconds < 0.0)
                    {
                        Console.WriteLine("Switching to decsision");
                        gameMode = GameMode.DECISION;
                        remainingTime = decisionMaxDuration;
                    }
                    break;
                case GameMode.DECISION:
                    remainingTime = remainingTime - gameTime.ElapsedGameTime;
                    if (remainingTime.TotalSeconds < 0.0)
                    {
                        Console.WriteLine("Switching to realtime");
                        gameMode = GameMode.REALTIME;
                        remainingTime = realTimeMaxDuration;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
