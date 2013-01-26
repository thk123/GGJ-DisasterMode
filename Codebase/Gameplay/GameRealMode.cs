using System;
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


        enum RealTimeState
        {
            Idle,
            SelectingDestionation,
        }
        RealTimeState realTimeState;
        GameAction actionToPoint;
        Point actionToPointLocation;

        private const int actionCount = 2;
        private List<Actions.GameAction> actions;

        const int totalActionsPerDay = 12;
        int actionsRemaining;

        private void ConstructReal()
        {
            int actionCount =Enum.GetNames(typeof(Actions.ActionType)).Length;
            actions = new List<Actions.GameAction>(actionCount);

            int i = 0;
            foreach (Actions.ActionType type in Enum.GetValues(typeof(Actions.ActionType)))
            {
                actions.Add(new Actions.GameAction(type, GetUiPosition(type)));
                ++i;
            }

            currentState = DragState.Idle;
            currentlyDragging = null;

            actionsRemaining = totalActionsPerDay;
        }

        public void RealTimeProcessStartDay()
        {
            actionsRemaining = totalActionsPerDay;
            realTimeState = RealTimeState.Idle;
        }

        public void RealTimeProcessEndDay()
        {
            TemperatureManager.ProcessDay();
            foreach (Civilian civ in civilians)
            {
                civ.ProcessDay();
                civ.UpdateTemperature(TemperatureManager.Temperature);
            }
        }

        public void RealTimeProcessStartNight()
        {

        }

        public void RealTimeProcessEndNight()
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
            PopulateCivilians(this.civilians, pixelTexture);

            defaultFont = content.Load<SpriteFont>("Fonts//gamefont");
           
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
            foreach (Actions.GameAction action in actions)
            {
                action.Update(gameTime);
            }

            foreach (Civilian civilian in civilians)
            {
                civilian.Update(gameTime);
            }


            missionRunning = this.missionRunning;
        }

        private void ActionPlaced(GameAction droppedAction, Point gridLocation)
        {
            if (droppedAction.ActionType == ActionType.DirectAction)
            {
                //Get some more input
                realTimeState = RealTimeState.SelectingDestionation;
                actionToPoint = droppedAction;
                actionToPointLocation = gridLocation;

            }
            else
            {
                //we are done for this action
                droppedAction.PlaceAction(gridLocation);

                // Decrease remaining actions
                --actionsRemaining;

                //And replensh the ui
                actions.Add(GameAction.CreateNewActionFromAction(droppedAction, GetUiPosition(droppedAction.ActionType)));

                //Are we done for today?
                if (actionsRemaining == 0)
                {
                    EndDay();
                }
            }
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

            Vector2 halfTextLength = defaultFont.MeasureString(actionsRemaining.ToString()) * 0.5f;
            spriteBatch.DrawString(defaultFont, actionsRemaining.ToString(), new Vector2(uiOffset + 230 - halfTextLength.X, 200 - halfTextLength.Y), Color.Red);

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
            if (realTimeState == RealTimeState.SelectingDestionation)
            {
                if (input.GetMouseDown())
                {
                    Point? mousePoint = grid.GetGridPointFromMousePosition(input.GetMousePosition());
                    if(mousePoint.HasValue)
                    {
                        actionToPoint.PlaceAction(actionToPointLocation, mousePoint.Value);

                        realTimeState = RealTimeState.Idle;

                        --actionsRemaining;

                        //And replensh the ui
                        actions.Add(GameAction.CreateNewActionFromAction(actionToPoint, GetUiPosition(actionToPoint.ActionType)));

                        //Are we done for today?
                        if (actionsRemaining == 0)
                        {
                            EndDay();
                        }
                    }
                }
            }
        }

        private Rectangle GetUiPosition(ActionType actionType)
        {
            switch (actionType)
            {
                case ActionType.ListenAction:
                    return new Rectangle(uiOffset + 135 - 75 + 200, 155, 150, 150);
                case ActionType.DirectAction:
                    return new Rectangle(uiOffset + 135 - 75, 155, 150, 150);
                default:
                    throw new Exception("Unrecognised action type");
            }
        }

    }
}
