using System;
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
        }

        DragState dragState;

        Rectangle staticPosition;
        Rectangle currentPosition;
        Rectangle? forcedLocation;

        Texture2D staticTexture;
        Texture2D draggingTexture;

        public Draggable(Rectangle staticPosition)
        {
            currentPosition = staticPosition;
            this.staticPosition = staticPosition;
            dragState = DragState.Idle;
        }

        public void SetContent(Texture2D staticTexture, Texture2D draggingTexture)
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
        }

        public bool AttemptBeginDrag(Point mousePosition)
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

        public void UpdateDrag(Point deltaMouse, Rectangle? fixedLocation)
        {
            forcedLocation = fixedLocation;
            currentPosition.X += deltaMouse.X;
            currentPosition.Y += deltaMouse.Y;
        }

        public void EndDrag(Rectangle? newStaticLocation, Texture2D newStaticTexture)
        {
            if (newStaticLocation.HasValue)
            {
                staticPosition = newStaticLocation.Value;
            }

            if (newStaticTexture != null)
            {
                staticTexture = newStaticTexture;
            }

            currentPosition = staticPosition;

            dragState = DragState.Idle;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            switch (dragState)
            {
                case DragState.Idle:
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
