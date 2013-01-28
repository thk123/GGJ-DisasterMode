using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
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

        SoundEffectInstance mainBackground;
        SoundEffectInstance background1;
        SoundEffectInstance background2;
        SoundEffectInstance background3;
        SoundEffectInstance background4;

        TimeSpan realTimeMaxDuration = new TimeSpan(0, 0, 30);
        TimeSpan decisionMaxDuration = new TimeSpan(0, 0, 30);

        TimeSpan remainingTime;

        Draggable currentlyDragging;
        
        int dayCount;

        


        enum DragState
        {
            Idle,
            Dragging,
        }
        DragState currentState;

        GameScreen parentScreen;

        public GameplayManager(ContentManager content, GameScreen screen)
        {
            this.gameMode = GameMode.REALTIME;
            this.parentScreen = screen;
            this.missionRunning = true;

            dayCount = 1;

            ConstructReal();

            StartDay();
        }
  
        public void LoadContent(ContentManager content)
        {
            gameFont = content.Load<SpriteFont>("Fonts//gamefont");
            uiTexture = content.Load<Texture2D>("Graphics//UI//GUI_temp");
            backgroundTexture = content.Load<Texture2D>("Graphics//Backgrounds//background");
            Dropoffs.ClockDrawer.LoadContent(content);
            TemperatureManager.LoadTempManager(content);
            LoadContentReal(content);
            LoadContentDecision(content);

            mainBackground = content.Load<SoundEffect>("Audio//loops//loop0_drone_16bit").CreateInstance();
            background1 = content.Load<SoundEffect>("Audio//loops//loop1_drums_16bit").CreateInstance();
            background2 = content.Load<SoundEffect>("Audio//loops//loop2_bass_16bit").CreateInstance();
            background3 = content.Load<SoundEffect>("Audio//loops//loop3_vox_16bit").CreateInstance();
            background4 = content.Load<SoundEffect>("Audio//loops//loop4_strings_16bit").CreateInstance();

            mainBackground.IsLooped = true;
            background1.IsLooped = true;
            background2.IsLooped = true;
            background3.IsLooped = true;
            background4.IsLooped = true;

            mainBackground.Volume = 1.0f;
            background1.Volume = 0.0f;
            background2.Volume = 0.0f;
            background3.Volume = 0.0f;
            background4.Volume = 0.0f;

            mainBackground.Play();
            background1.Play();
            background2.Play();
            background3.Play();
            background4.Play();

            /*mainBackground.Play(1.0f, 0.0f, 0.0f);
            background1.Play(0.0f, 0.0f, 0.0f);
            background2.Play(0.0f, 0.0f, 0.0f);
            background3.Play(0.0f, 0.0f, 0.0f);
            background4.Play(0.0f, 0.0f, 0.0f);*/


        }

        private enum GameMode
        {
            REALTIME, DECISION
        }


        public void Update(GameTime gameTime, bool active, out bool missionRunning)
        {
            if (this.gameMode == GameMode.REALTIME)
            {
                UpdateReal(gameTime, out missionRunning, active);
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
            spriteBatch.Draw(backgroundTexture, new Rectangle(9, 9, backgroundTexture.Width, backgroundTexture.Height), Color.White);
            grid.Draw(spriteBatch);
            spriteBatch.Draw(uiTexture, new Vector2(uiOffset, 9), Color.White);
            TemperatureManager.DrawDiamond(spriteBatch, uiOffset);
            //if (this.gameMode == GameMode.REALTIME)
            {
                DrawReal(gameTime, spriteBatch);
            }
            //else if (this.gameMode == GameMode.DECISION)
            {
                DrawDecision(gameTime, spriteBatch);
            }
            DisplayClock(spriteBatch);
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
                        //Drop the dropoff in this grid#
                        Rectangle cellRect = grid.GetGridRectangleFromGridPoint(gridPoint.Value);
                        currentlyDragging.EndDrag(cellRect);
                        currentState = DragState.Idle;
                        /*this.buckets.addWater(cellRect.X + (cellRect.Width / 2), 
                            cellRect.Y + (cellRect.Height / 2));*/
                        //this.buckets.addWater(cellRect.X + (cellRect.Width / 2), 
                        //    cellRect.Y + (cellRect.Height / 2));

                        if (gameMode == GameMode.REALTIME)
                        {
                            ActionPlaced((Actions.GameAction)currentlyDragging, gridPoint.Value, cellRect);
                        }
                        else
                        {
                            DropoffPlaced((Dropoffs.Dropoff)currentlyDragging, cellRect, gridPoint.Value);
                        }
                    }
                    else
                    {
                        EndDrag();
                    }
                }
            }

            if (gameMode == GameMode.REALTIME)
            {
                HandleInputReal(input);
            }
            else
            {
                HandleInputDecision(input);
            }
            
        }

        private void UpdateTime(GameTime gameTime)
        {
            switch (gameMode)
            {
                case GameMode.REALTIME:
                    remainingTime = remainingTime - gameTime.ElapsedGameTime;
                    if (remainingTime.TotalSeconds <= decisionMaxDuration.TotalSeconds)
                    {
                        EndDay();
                    }
                    break;
                case GameMode.DECISION:
                    remainingTime = remainingTime - gameTime.ElapsedGameTime;
                    if (remainingTime.TotalSeconds <= 0.0)
                    {
                        EndNight();
                    }
                    break;
                default:
                    break;
            }
        }

        private void StartDay()
        {
            remainingTime = decisionMaxDuration + realTimeMaxDuration;

            RealTimeProcessStartDay();
            DecisionProcessStartDay();
        }

        private void EndDay()
        {
            // go from day (real time instruction placing) -> night, placing pick ups
            EndDrag();
           

            RealTimeProcessEndDay();
            DecisionProcessEndDay();

            if (dayCount == 2)
            {
                background1.Volume = 1.0f;
            }
            else if (dayCount == 3)
            {
                background2.Volume = 1.0f;
            }
            else if (dayCount == 4)
            {
                background3.Volume = 1.0f;
            }
            else if (dayCount == 5)
            {
                background4.Volume = 1.0f;
            }

            gameMode = GameMode.DECISION;
            remainingTime = decisionMaxDuration;

            StartNight();
        }

        private void StartNight()
        {
            RealTimeProcessStartNight();
            DecisionProcessStartNight();
        }

        private void EndNight()
        {
            EndDrag();

            DecisionProcessEndNight();
            RealTimeProcessEndNight();

            // go from night (placing pick ups) -> day (real time instruction placing)
            gameMode = GameMode.REALTIME;
           

            ++dayCount;

            StartDay();

        }

        private void EndDrag()
        {
            if (currentState == DragState.Dragging)
            {
                //Return the object to the shop
                currentState = DragState.Idle;
                currentlyDragging.EndDrag();
            }
        }

        private void DisplayClock(SpriteBatch spriteBatch)
        {
            string s;
            if (gameMode == GameMode.REALTIME)
            {
                s = string.Format("DAY {0}", dayCount);
            }
            else
            {
                s = string.Format("NIGHT {0}", dayCount);
            }
            Vector2 stringSize = gameFont.MeasureString(s);
            spriteBatch.DrawString(gameFont, s, new Vector2(uiOffset + 224 - (stringSize.X / 2.0f), 59 - (stringSize.Y / 2.0f)), Color.Black);

            int secondsInADay = 60 * 60 * 24;
            double secondsRemaining = remainingTime.TotalSeconds;
            double percentage = secondsRemaining / (decisionMaxDuration.TotalSeconds + realTimeMaxDuration.TotalSeconds);
            double invervtedPercentage = 1 - percentage;
            double secondsThrough = secondsInADay * invervtedPercentage;
            TimeSpan ofADay = new TimeSpan(0, 0, (int)secondsThrough);

            string timer = string.Format("{0}:{1}", ofADay.Hours, ofADay.Minutes);
            Vector2 stringTimeSize = gameFont.MeasureString(timer);
            spriteBatch.DrawString(gameFont, timer, new Vector2(uiOffset + 224 - (stringTimeSize.X / 2.0f), 109 - (stringTimeSize.Y / 2.0f)), Color.Black);
        }
    }
}
