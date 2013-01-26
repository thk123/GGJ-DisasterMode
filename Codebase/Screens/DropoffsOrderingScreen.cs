using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using GGJ_DisasterMode.Codebase.Dropoffs;

namespace GGJ_DisasterMode.Codebase.Screens
{
    enum DropoffsOderingScreenState
    {
        Idle, 
        Dragging,
    }

    class DropoffsOrderingScreen : GameplayScreen
    {
        DropoffsOderingScreenState currentState;

        List<Dropoff> dropoffs;
        Dropoff currentlyDraggedDropoff;

        ContentManager content;

        Texture2D shopGrid;
        Vector2 shopLocation = new Vector2(10, 100);

        public DropoffsOrderingScreen()
            : base()
        {
            currentState = DropoffsOderingScreenState.Idle;

            dropoffs = new List<Dropoff>();

            //Add all the dropoffs
            dropoffs.Add(new BasicFoodDropoff(GetNextShopRectangle()));

            currentlyDraggedDropoff = null;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            if (content == null)
            {
                content = content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            foreach (Dropoff dropoff in dropoffs)
            {
                dropoff.LoadContent(content);
            }

            shopGrid = content.Load<Texture2D>("Graphics//Dropoffs//ShopGrid");
            
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void HandleInput(InputState input)
        {
            base.HandleInput(input);

            if (currentState == DropoffsOderingScreenState.Idle)
            {
                if (input.GetMouseDown())
                {
                    //Check if mouse is within any of the dropoff boxes
                    foreach (Dropoff dropoff in dropoffs)
                    {
                        if (dropoff.CheckCollisionAgainstShopRectangle(input.GetMousePosition()))
                        {
                            currentlyDraggedDropoff = dropoff;
                            currentState = DropoffsOderingScreenState.Dragging;
                            currentlyDraggedDropoff.BeginDrag(input.GetMousePosition());
                            break;
                        }
                    }
                }
            }
            else if (currentState == DropoffsOderingScreenState.Dragging)
            {
                if (input.GetMouseDown())
                {
                    //Move the one we are dragging
                    currentlyDraggedDropoff.UpdateDrag(input.GetMouseDelta());
                }
                else
                {
                    Point? gridPoint = GetGrid().GetGridPointFromMousePosition(input.GetMousePosition());
                    if(false && gridPoint.HasValue)
                    {
                        //Drop the dropoff in this grid
                        currentlyDraggedDropoff.PlaceDropoff(gridPoint.Value);
                        currentState = DropoffsOderingScreenState.Idle;
                    }
                    else
                    {
                        //Return the object to the shop
                        currentState = DropoffsOderingScreenState.Idle;
                        currentlyDraggedDropoff.PutDropoffBackToStore();
                    }
                }
            }
        }


        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();
            spriteBatch.Draw(shopGrid, shopLocation, Color.White);

            foreach (Dropoff dropoff in dropoffs)
            {
                dropoff.Draw(gameTime, spriteBatch); 
            }

            spriteBatch.End();

        }

        Rectangle startingShopRectangle = new Rectangle(10, 100, 64, 64);
        const int spacing = 10;

        private Rectangle GetNextShopRectangle()
        {
            Rectangle newRectangle = startingShopRectangle;
            startingShopRectangle.X += startingShopRectangle.Width + spacing;
            return newRectangle;
        }

        private Grid GetGrid()
        {
            //TODO: return actual grid
            return new Grid(new Rectangle(0, 0, 500, 500), 10);
        }
    }
}
