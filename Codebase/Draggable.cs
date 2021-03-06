﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GGJ_DisasterMode.Codebase
{
    abstract class Draggable
    {
        enum DragState
        {
            Idle, 
            Dragging,
            Done, 
        }

        DragState dragState;

        Rectangle staticPosition;
        Rectangle currentPosition;
        Rectangle? forcedLocation;

        public Point? Position
        {
            get
            {
                return new Point(currentPosition.X,
                    currentPosition.Y);
            }
        }
        
        protected Texture2D staticTexture;
        protected Texture2D draggingTexture;
        protected Texture2D gridTexture;

        protected bool Redraggable
        {
            get;
            set;
        }


        public Draggable(Rectangle staticPosition)
        {
            currentPosition = staticPosition;
            this.staticPosition = staticPosition;
            dragState = DragState.Idle;

            Redraggable = true;
        }

        public void SetContent(Texture2D staticTexture)
        {
            SetContent(staticTexture);
        }

        public void SetContent(Texture2D staticTexture, Texture2D draggingTexture, Texture2D gridTexture)
        {
            this.staticTexture = staticTexture;
            if (draggingTexture == null)
            {
                this.draggingTexture = staticTexture;
            }
            else
            {
                this.draggingTexture = draggingTexture;
            }
            if (gridTexture != null)
            {
                this.gridTexture = gridTexture;
            }
            else
            {
                if (draggingTexture == null)
                {
                    this.gridTexture = staticTexture;
                }
                else
                {
                    this.gridTexture = draggingTexture;
                }
            }
        }

        

        public bool AttemptBeginDrag(Point mousePosition)
        {
            if (dragState == DragState.Idle)
            {
                if (currentPosition.Contains(mousePosition))
                {
                    dragState = DragState.Dragging;
                    currentPosition.X = mousePosition.X - (int)(draggingTexture.Width / 2.0f);
                    currentPosition.Y = mousePosition.Y - (int)(draggingTexture.Height / 2.0f);
                    currentPosition.Width = draggingTexture.Width;
                    currentPosition.Height = draggingTexture.Height;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void UpdateDrag(Point deltaMouse)
        {
            UpdateDrag(deltaMouse, null);
        }

        public void UpdateDrag(Point deltaMouse, Rectangle? fixedLocation)
        {
            forcedLocation = fixedLocation;
            currentPosition.X += deltaMouse.X;
            currentPosition.Y += deltaMouse.Y;
        }

        public void EndDrag()
        {
            EndDrag(null, null);
        }

        public void EndDrag(Rectangle newStaticLocation)
        {
            EndDrag(newStaticLocation, gridTexture);
        }

        public void EndDrag(Texture2D newStaticTexture)
        {
            EndDrag(null, newStaticTexture);
        }

        public void EndDrag(Rectangle? newStaticLocation, Texture2D newStaticTexture)
        {
            if (newStaticLocation.HasValue)
            {
                staticPosition = newStaticLocation.Value;
                    
                //We have put the entity down
                if (Redraggable)
                {
                    dragState = DragState.Idle;
                }
                else
                {
                    dragState = DragState.Done;
                }
            }
            else
            {
                dragState = DragState.Idle;
            }

            if (newStaticTexture != null)
            {
                staticTexture = newStaticTexture;
            }

            currentPosition = staticPosition;

            
        }

        public void FixLocation()
        {
            dragState = DragState.Done;
            Redraggable = false;
        }

        public void SetRectangle(Rectangle rectangle)
        {
            currentPosition.Width = rectangle.Width;
            currentPosition.Height = rectangle.Height;

            currentPosition.X += rectangle.X;
            currentPosition.Y += rectangle.Y;
        }

        public virtual void Draw(SpriteBatch spriteBatch, bool buttonEnabled)
        {
            switch (dragState)
            {
                case DragState.Idle:
                   
                    if (buttonEnabled)
                    {
                        spriteBatch.Draw(staticTexture, currentPosition, Color.White);
                    }
                    else
                    {
			            spriteBatch.Draw(staticTexture, currentPosition, ColorAdapter.getTransparentColor(Color.White, 0.2f));

                    }
                    break;
                case DragState.Done:
                    spriteBatch.Draw(staticTexture, currentPosition, Color.White);
                    break;
                case DragState.Dragging:
                    if(forcedLocation == null)
                    {
                        spriteBatch.Draw(draggingTexture, currentPosition, Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(draggingTexture, forcedLocation.Value, Color.White);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
