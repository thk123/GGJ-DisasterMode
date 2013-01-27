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

        enum RealTimeState
        {
            Idle,
            SelectingDestionation,
            DisplayingListen,
        }
        RealTimeState realTimeState;
        GameAction actionToPoint;
        Point actionToPointLocation;
        AudioResultsScreen currentResultsScreen;

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

            buckets.ApplyDensityIllnessPenalty();

            foreach (Civilian civ in civilians)
            {
                civ.UpdateTemperature(TemperatureManager.Temperature);
                civ.ProcessDay();
            }

            if (realTimeState == RealTimeState.SelectingDestionation)
            {
                actionToPoint.CancelPlaceAction();
                realTimeState = RealTimeState.Idle;
            }
            else if (realTimeState == RealTimeState.DisplayingListen)
            {
                currentResultsScreen.ExitScreen();
            }
        }

        public void RealTimeProcessStartNight()
        {
            //Get actions on the board
            List<GameAction> redundantActions = actions.Where(action => action.ActionState == ActionState.Active).ToList(); ;
            foreach (GameAction action in redundantActions)
            {
                actions.Remove(action);
            }
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
            

            defaultFont = content.Load<SpriteFont>("Fonts//gamefont");

            this.buckets = new Buckets(this.grid, new List<GameAction>(), new List<Civilian>(), 
                new List<Dropoff>(), new List<Water>());

            PopulateCivilians(this.civilians, pixelTexture, content);
           
        }

        private void PopulateCivilians(List<Civilian> civilians, Texture2D texture, ContentManager content)
        {
            Random randomGen = new Random(1000);

            for (int i = 0; i < 500; i++)
            {
                int type = randomGen.Next(6);
                switch (type)
                {
                    case 0:
                        civilians.Add(new ChildMaleCharacter(randomGen.Next(0, Civilian.SCALE_FACTOR),
                    randomGen.Next(0, Civilian.SCALE_FACTOR)));
                        break;
                    case 1:
                        civilians.Add(new ChildFemaleCharacter(randomGen.Next(0, Civilian.SCALE_FACTOR),
                    randomGen.Next(0, Civilian.SCALE_FACTOR)));
                        break;

                    case 2:
                        civilians.Add(new AdultMaleCharacter(randomGen.Next(0, Civilian.SCALE_FACTOR),
                    randomGen.Next(0, Civilian.SCALE_FACTOR)));
                        break;
                    case 3:
                        civilians.Add(new AdultFemaleCharacter(randomGen.Next(0, Civilian.SCALE_FACTOR),
                    randomGen.Next(0, Civilian.SCALE_FACTOR)));
                        break;

                    case 4:
                        civilians.Add(new OldMaleCharacter(randomGen.Next(0, Civilian.SCALE_FACTOR),
                    randomGen.Next(0, Civilian.SCALE_FACTOR)));
                        break;
                    case 5:
                        civilians.Add(new OldFemaleCharacter(randomGen.Next(0, Civilian.SCALE_FACTOR),
                    randomGen.Next(0, Civilian.SCALE_FACTOR)));
                        break;
                }
                
            }

            civilians.Add(new ChildMaleCharacter(0, 0));
            civilians.Add(new ChildMaleCharacter(1800, 0));
            civilians.Add(new ChildMaleCharacter(0, 1800));
            civilians.Add(new ChildMaleCharacter(1800, 1800));

            foreach (Civilian civilian in civilians)
            {
                civilian.LoadContent(content);
                buckets.addNewCivilian(civilian);
            }
        }

        public void UpdateReal(GameTime gameTime, out bool missionRunning, bool isActive)
        {
            if (realTimeState == RealTimeState.DisplayingListen && isActive)
            {
                realTimeState = RealTimeState.Idle;
            }
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

                List<Civilian> nearbyCivilians = new List<Civilian>();
                ProcessingBucket[] adjacentBuckets = buckets.getNeighbours(gridLocation.X, gridLocation.Y);
                foreach (ProcessingBucket bucket in adjacentBuckets)
                {
                    nearbyCivilians.AddRange(bucket.GetCivilians().Where(member => member.IsDead == false));
                }

                currentResultsScreen= new AudioResultsScreen(nearbyCivilians, gridLocation.Y >= 9);

                parentScreen.ScreenManager.AddScreen(currentResultsScreen, parentScreen.ControllingPlayer);

                realTimeState = RealTimeState.DisplayingListen;

                //Are we done for today? -- NO PEOPLE CAN STILL MOVE
                /*if (actionsRemaining == 0)
                {
                    EndDay();
                }*/
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
                action.Draw(spriteBatch, gameMode == GameMode.REALTIME && realTimeState == RealTimeState.Idle);
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
                    return new Rectangle(uiOffset + 82, 184 + 9, 102, 102);
                case ActionType.DirectAction:
                    return new Rectangle(uiOffset + 246, 184 + 9, 102, 102);
                default:
                    throw new Exception("Unrecognised action type");
            }
        }

    }
}
